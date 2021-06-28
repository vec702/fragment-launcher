using Microsoft.VisualBasic.FileIO;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace Fragment_Launcher
{
    public partial class MainWindow : Form
    {

        private string patchReleaseDate = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            CenterToScreen();

            isoFilePath.Text = info.Default.isoFilePath.ToString();
            telliToolStripMenuItem.ToolTipText = info.Default.telliFolder.ToString();
            pcsx2StripMenuItem1.ToolTipText = info.Default.pcsx2Folder.ToString();

            if (telliToolStripMenuItem.ToolTipText != string.Empty)
            {
                telliToolStripMenuItem.Checked = true;
            }

            if (pcsx2StripMenuItem1.ToolTipText != string.Empty)
            {
                pcsx2StripMenuItem1.Checked = true;
            }

            if (isoFilePath.Text != string.Empty)
            {
                md5hash.Text = get_MD5Hash(isoFilePath.Text);
            }
        }

        #region General Functions
        private bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }

            catch (IOException)
            {
                return true;
            }

            finally
            {
                if (stream != null)
                    stream.Close();
            }

            return false;
        }

        private string get_MD5Hash(string filePath)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    var hash = md5.ComputeHash(stream);
                    string final_checksum = BitConverter.ToString(hash).Replace("-", "");
                    return final_checksum;
                }
            }
        }

        private void saveSettings()
        {
            info.Default.isoFilePath = isoFilePath.Text;
            info.Default.telliFolder = telliToolStripMenuItem.ToolTipText;
            info.Default.pcsx2Folder = pcsx2StripMenuItem1.ToolTipText;
            info.Default.Save();

            updateStatusMessage("Settings saved.");
        }

        private void updateProgress(int number)
        {
            progressBar1.Value = number;
            progressBar1.Update();
        }

        private void updateStatusMessage(string message)
        {
            toolStripStatusLabel1.Text = message;
            statusStrip1.Update();
        }

        private bool needToUpdatePatch()
        {
            DateTime lastModified = File.GetLastWriteTime(isoFilePath.Text);
            DateTime patchUploaded = DateTime.Parse(patchReleaseDate);

            if(lastModified.CompareTo(patchUploaded) == -1)
            {
                return true;
            } else
            {
                return false;
            }
        }
        #endregion

        #region Click Button Functions

        private void checkNewVersion_Click(object sender, EventArgs e)
        {
            updateProgress(0);

            if (isoFilePath.Text == string.Empty)
            {
                MessageBox.Show("Please select a .hack//fragment ISO before checking for a new version.", ".hack//fragment launcher - error");

            } else if (telliToolStripMenuItem.ToolTipText == string.Empty) {
                MessageBox.Show("No TelliPatch folder found.\nUnder the Tools menu, please select the folder where TelliPatch's 'Apply Patch.bat' is stored.", ".hack//fragment launcher - error");

            } else
            {
                dialogBox.Text = ""; // clear out textbox.
                dialogBox.Text += "Checking for new version...\n";

                updateProgress(20);

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://docs.google.com/spreadsheets/d/e/2PACX-1vTz5vYBFwyBfDUb5mLfq0OoLjuUw3pnybi7I-2uyFkGppveCe_fVz0pTXWRtGbc7Si_-F-xM4H89sU-/pub?gid=0&single=true&output=csv");
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

                using (TextFieldParser csvParser = new TextFieldParser(resp.GetResponseStream()))
                {
                    csvParser.SetDelimiters(new string[] { "," });

                    updateProgress(40);

                    while (!csvParser.EndOfData)
                    {
                        string[] fields = csvParser.ReadFields();
                        string checksum = fields[0];
                        string releaseVersion = fields[1];
                        string releaseDate = fields[2];
                        string vi_checksum = fields[3];

                        patchReleaseDate = releaseDate;

                        dialogBox.Text += "Found version " + releaseVersion + ", released on " + releaseDate + ".\n";

                        // check bin\vis_patcher.exe & whether or not we need to update that first.
                        if (!File.Exists(@telliToolStripMenuItem.ToolTipText + "\\bin\\vis_patcher.exe") || vi_checksum != get_MD5Hash(@telliToolStripMenuItem.ToolTipText + "\\bin\\vis_patcher.exe"))
                        {
                            dialogBox.Text += "Your version of TelliPatch is too far out-of-date to use with this launcher!\nYou will need download a newer version from the Netslum BBS. Link: https://bbs.dothackers.org/viewtopic.php?f=6&t=80 \n";
                            updateProgress(0);
                        }

                        else
                        {

                            dialogBox.Text += "Comparing loaded ISO's last modified time to see if an update is required...\n";

                            if (!needToUpdatePatch())
                            {
                                dialogBox.Text += "The latest version is installed. Please enjoy \"THE WORLD.\"\n";
                            }
                            else
                            {
                                if (!File.Exists(@telliToolStripMenuItem.ToolTipText + "\\fragment.ISO"))
                                {
                                    // if it's the unmodified JP ISO
                                    if (md5hash.Text == "94C82040BF4BB99500EB557A3C0FBB15")
                                    {
                                        dialogBox.Text += "No ISO detected in TelliPatch folder. Raw JP ISO found in Launcher. Copying it over...\n";
                                        File.Copy(isoFilePath.Text, @telliToolStripMenuItem.ToolTipText + "\\fragment.ISO");

                                        updateProgress(60);
                                    }
                                }

                                while (IsFileLocked(new FileInfo(@telliToolStripMenuItem.ToolTipText + "\\fragment.ISO")))
                                {
                                    updateProgress(80);
                                }

                                updateProgress(80);

                                dialogBox.Text += "A new version has been found. Now running TelliPatch...\n";

                                updateStatusMessage("TelliPatch is running... Please wait.");

                                var process = new Process
                                {
                                    StartInfo = new ProcessStartInfo
                                    {
                                        FileName = @telliToolStripMenuItem.ToolTipText + "\\Apply Patch.bat"
                                    }
                                };

                                process.Start();
                                process.WaitForExit();

                                dialogBox.Text += "The latest version of TelliPatch has been applied to your .hack//fragment ISO.\n";
                                dialogBox.Text += "Please enjoy \"THE WORLD.\"";

                                isoFilePath.Text = @telliToolStripMenuItem.ToolTipText + "\\dotHack Fragment (1.0).iSO";
                                md5hash.Text = get_MD5Hash(@telliToolStripMenuItem.ToolTipText + "\\dotHack Fragment (1.0).iSO");

                                DateTime lastModified = File.GetLastWriteTime(isoFilePath.Text);
                                updateStatusMessage("Last Checked: " + lastModified.ToString());
                            }
                        }
                        updateProgress(100);
                    }
                        
                }

            }
        }

        private void chooseIso_Btn_Click(object sender, EventArgs e)
        {
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                openFileDialog.Filter = "ISO files (*.iso)|*.iso|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    isoFilePath.Text = filePath.ToString();
                    md5hash.Text = get_MD5Hash(filePath);
                }
            }
        }

        private void launchButton_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Update();

            if (pcsx2StripMenuItem1.ToolTipText == string.Empty)
            {
                MessageBox.Show("No PCSX2 folder found.\nUnder the Tools menu, please select the folder where PCSX2.exe is stored.", ".hack//fragment launcher - error");
            }
            else
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = pcsx2StripMenuItem1.ToolTipText,
                        Arguments = "\"" + isoFilePath.Text + "\""
                    }
                };

                process.Start();
                Close();
            }

        }

        #endregion Click Button Functions

        #region Menu / Status Bar Functions
        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            saveToolStripMenuItem.Click += new EventHandler(saveToolStripMenuItem_Click);
            exitToolStripMenuItem.Click += new EventHandler(exitToolStripMenuItem_Click);
            telliToolStripMenuItem.Click += new EventHandler(telliToolStripMenuItem_Click);
            pcsx2StripMenuItem1.Click += new EventHandler(pcsx2ToolStripMenuItem_Click);
            aboutToolStripMenuItem.Click += new EventHandler(aboutToolStripMenuItem_Click);
        }

        private void telliToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    info.Default.telliFolder = fbd.SelectedPath.ToString();
                    telliToolStripMenuItem.ToolTipText = fbd.SelectedPath.ToString();
                    telliToolStripMenuItem.Checked = true;
                }

                toolStripStatusLabel1.Text = "TelliPatch Folder set to " + fbd.SelectedPath.ToString();
                statusStrip1.Update();
            }
        }

        private void pcsx2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                openFileDialog.Filter = "pcsx2.exe file (*.exe)|*.exe|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;

                    info.Default.pcsx2Folder = filePath.ToString();
                    pcsx2StripMenuItem1.ToolTipText = filePath.ToString();
                    pcsx2StripMenuItem1.Checked = true;
                }

                toolStripStatusLabel1.Text = "PCSX2 Folder set to " + filePath.ToString();
                statusStrip1.Update();
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About a = new About();
            a.Show();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveSettings();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // save before closing
            saveSettings();
            Close();
        }

        #endregion Menu / Status Bar Functions
    }
}
