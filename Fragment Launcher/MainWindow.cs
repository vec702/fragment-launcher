using Newtonsoft.Json;
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

        #region Declarations
        private bool reading = false;

        private string lastCheckedForNewVersion = string.Empty;
        private string telliPatchVersion = string.Empty;
        private string aliceGithubTag = "v0.0.0";
        private string viGithubTag = "v0.0.0";

        private const string aliceGithubReleaseLink = "https://api.github.com/repos/Tellilum/.hack-fragment-Definitive-Translation/releases/latest";
        private const string viGithubReleaseLink = "https://api.github.com/repos/Finzenku/FragmentUpdater/releases/latest";
        private const string JP_ISO_MD5 = "94C82040BF4BB99500EB557A3C0FBB15";
        
        private readonly Timer timer = new Timer();
        #endregion

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
            lastCheckedForNewVersion = info.Default.lastCheck.ToString();
            aliceGithubTag = info.Default.aliceGithubVersion;
            viGithubTag = info.Default.viGithubVersion;

            if (telliToolStripMenuItem.ToolTipText != string.Empty)
            {
                telliToolStripMenuItem.Checked = true;
            }

            if (pcsx2StripMenuItem1.ToolTipText != string.Empty)
            {
                pcsx2StripMenuItem1.Checked = true;
            }

            if (isoFilePath.Text != string.Empty && File.Exists(isoFilePath.Text))
            {
                md5hash.Text = Get_MD5Hash(isoFilePath.Text);
            }
            else
            {
                md5hash.BackColor = System.Drawing.Color.LightCoral;
                md5hash.Text = " error - select another ISO";
            }
            
            timer.Tick += new EventHandler(CheckLauncherStatus);
            timer.Interval = 5000;
            timer.Start();
        }

        #region General Functions

        private bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                reading = true;
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

                reading = false;
            }

            return false;
        }

        private void CheckLauncherStatus(Object sender, EventArgs e)
        {
            if(!reading)
            {
                UpdateStatusMessage("Launcher ready.");
            }
            else
            {
                UpdateStatusMessage("Please wait...");
            }
        }

        private void Log(string message, bool createNewline=true)
        {
            switch(createNewline)
            {
                case true:
                    dialogBox.Text += message + Environment.NewLine;
                    break;
                case false:
                    dialogBox.Text += message;
                    break;
            }
        }

        private string Get_TelliPatchVersion()
        { 
            telliPatchVersion = File.ReadAllText(@telliToolStripMenuItem.ToolTipText + "\\VERSION");
            return telliPatchVersion;
        }

        private void SaveSettings()
        {
            info.Default.isoFilePath = isoFilePath.Text;
            info.Default.telliFolder = telliToolStripMenuItem.ToolTipText;
            info.Default.pcsx2Folder = pcsx2StripMenuItem1.ToolTipText;
            info.Default.lastCheck = lastCheckedForNewVersion;
            info.Default.aliceGithubVersion = aliceGithubTag;
            info.Default.viGithubVersion = viGithubTag;
            info.Default.Save();
        }

        private void UpdateProgress(int number)
        {
            progressBar1.Value = number;
            progressBar1.Update();
        }

        private void UpdateStatusMessage(string message)
        {
            toolStripStatusLabel1.Text = message;
            statusStrip1.Update();
        }

        private string Get_MD5Hash(string filePath)
        {
            UpdateStatusMessage("Calculating MD5 hash code...");
            reading = true;

            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    var hash = md5.ComputeHash(stream);
                    string final_checksum = BitConverter.ToString(hash).Replace("-", "");
                    reading = false;
                    return final_checksum;
                }
            }
        }

        private bool IsFragmentLoaded()
        {
            bool IS_SLPS255_27_FOUND = false;

            Log("Mounting ISO to check for .hack//fragment ID file: \"SLPS_255.27\" ... ", false);

            // mount the iso
            var mountIso = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "PowerShell.exe",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = false,
                    WindowStyle = ProcessWindowStyle.Minimized,
                    Arguments = "-WindowStyle Hidden -Command Mount-DiskImage -ImagePath '" + isoFilePath.Text + "'"
                }
            };

            mountIso.Start();

            while(!mountIso.HasExited)
            {
                Application.DoEvents();
            }
            
            // check all mounted drives for file SLPS_255.27 in the root
            // this is to confirm it's .hack//fragment loaded into the launcher
            DriveInfo[] AllDrives = DriveInfo.GetDrives();
            foreach (DriveInfo d in AllDrives)
            {
                if (d.IsReady == true)
                {
                    if(!File.Exists(d.RootDirectory + "\\SLPS_255.27"))
                    {
                        IS_SLPS255_27_FOUND = false;
                        continue;
                    }
                    else
                    {
                        IS_SLPS255_27_FOUND = true;
                        break;
                    }
                }
            }

            if (IS_SLPS255_27_FOUND) Log("found!");
            else Log("not found!");

            // unmount the iso.
            var unmountIso = new Process
            {
                StartInfo = new ProcessStartInfo   
                {
                    FileName = "PowerShell.exe",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = false,
                    WindowStyle = ProcessWindowStyle.Minimized,
                    Arguments = "-WindowStyle Hidden -Command Dismount-DiskImage -ImagePath '" + isoFilePath.Text + "'"
                }
            };

            unmountIso.Start();

            while(!unmountIso.HasExited)
            {
                Application.DoEvents();
            }

            return IS_SLPS255_27_FOUND;
        }

        private void CheckForUpdate()
        {
            reading = true;
            if (!IsFragmentLoaded())
            {
                Log("Could not determine whether or not .hack//fragment is loaded. Please try another ISO.");
                reading = false;
                return;
            }
            else
            {
                UpdateProgress(0);

                if (!File.Exists(@telliToolStripMenuItem.ToolTipText + "\\VERSION"))
                {
                    Log("Your version of TelliPatch is too far out-of-date to use with this launcher!");
                    Log("Download a newer version from the Netslum BBS. Link: https://bbs.dothackers.org/viewtopic.php?f=6&t=80");
                }
                else
                {
                    string pathToViExe = @telliToolStripMenuItem.ToolTipText + "\\bin\\vis_patcher.exe";

                    DateTime vi_lastModified = File.GetLastWriteTime(pathToViExe);

                    if (lastCheckedForNewVersion == string.Empty)
                    {
                        lastCheckedForNewVersion = DateTime.MinValue.ToString();
                    }

                    string aliceTagCleaned = Get_GithubTag(aliceGithubReleaseLink).Substring(1).Replace(".", string.Empty);
                    string aliceLastSavedData = aliceGithubTag.Substring(1).Replace(".", string.Empty);

                    string viLastSavedData = viGithubTag.Substring(1).Replace(".", string.Empty);
                    string viTagCleaned = Get_GithubTag(viGithubReleaseLink).Substring(1).Replace(".", string.Empty);

                    /*
                     * We need to patch if any of the following criteria are met:
                     * 1. the last time we ran a check was before a new telli release
                     * 2. the last time we ran a check was before a new vi release
                     * 3. the iso loaded was created before a new telli release
                     * 4. the iso loaded was created before a new vi release
                     * 5. saved alice tag is older than alice's new tag_name
                     * 6. saved vi tag is older than vi's new tag_name
                     * 7. the iso loaded is the original jp disc
                     */
                    if (DateTime.Compare(DateTime.Parse(lastCheckedForNewVersion), GetLastRelease(aliceGithubReleaseLink)) == -1 ||
                        DateTime.Compare(File.GetLastWriteTime(isoFilePath.Text), GetLastRelease(aliceGithubReleaseLink)) == -1 ||
                        //DateTime.Compare(vi_lastModified, GetLastRelease(viGithubReleaseLink)) == -1 ||
                        // DateTime.Compare(File.GetCreationTime(isoFilePath.Text), GetLastRelease(viGithubReleaseLink)) == -1 ||
                        Convert.ToInt32(aliceLastSavedData) < Convert.ToInt32(aliceTagCleaned) || /*Convert.ToInt32(viLastSavedData) < Convert.ToInt32(viTagCleaned) || uncomment when vi releases properly */
                        md5hash.Text == JP_ISO_MD5)
                    {
                        DialogResult patchNow = MessageBox.Show("A new version was found. Would you like to patch now?", ".hack//fragment launcher", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (patchNow == DialogResult.Yes)
                        {
                            RunTelliPatch();
                        }
                        else if (patchNow == DialogResult.No)
                        {
                            UpdateProgress(100);
                        }
                    }
                    else
                    {
                        Log("You have the latest version.");
                        UpdateCheckMessage();
                        UpdateProgress(100);
                    }
                }
                reading = false;
            }
        }

        private void RunTelliPatch()
        {
            bool finished_checking = false;
            if(!File.Exists(@telliToolStripMenuItem.ToolTipText + "\\fragment.ISO") && md5hash.Text != JP_ISO_MD5)
            {
                Log("No ISO found in TelliPatch directory. Cancelling patch...");
                return;
            }
            
            if(!File.Exists(@telliToolStripMenuItem.ToolTipText + "\\fragment.ISO") && md5hash.Text == JP_ISO_MD5)
            {
                Log("An original JP ISO is loaded into the launcher. Copying it over for use...");
                File.Copy(@isoFilePath.Text, @telliToolStripMenuItem.ToolTipText + "\\fragment.ISO");
            }

            UpdateProgress(10);

            if (IsFileLocked(new FileInfo(@telliToolStripMenuItem.ToolTipText + "\\fragment.ISO")))
            {
                // wait until it's done copying over...
            }
            else
            {
                Log("A command prompt window will open. Please do not close this until it the patch is complete.");

                var process = new Process
                {
                StartInfo = new ProcessStartInfo
                    {
                        FileName = @telliToolStripMenuItem.ToolTipText + "\\Apply Patch.bat",
                        UseShellExecute = false,
                        RedirectStandardOutput = true
                    }
                };

                var lineCount = 0;
                process.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
                {
                    // Prepend line numbers to each line of the output.
                    if (!String.IsNullOrEmpty(e.Data))
                    {
                        lineCount++;
                        if (dialogBox.InvokeRequired)
                        {
                            Invoke((MethodInvoker)(() => dialogBox.AppendText(Environment.NewLine + e.Data)));
                        }
                        else
                        {
                            dialogBox.AppendText(Environment.NewLine + e.Data);
                        }
                    }
                });

                process.Start();
                process.BeginOutputReadLine();

                while (!process.HasExited)
                {
                    string terminationString = "Your patched ISO is located at:";
                    if (dialogBox.Text.Contains(terminationString, StringComparison.OrdinalIgnoreCase))
                    {
                        process.Kill();
                    }

                    if (Process.GetProcessesByName("bsdtar").Length != 0)
                    {
                        UpdateProgress(20);
                    }
                    if (Process.GetProcessesByName("xdelta").Length != 0)
                    {
                        UpdateProgress(35);
                    }

                    if(Process.GetProcessesByName("fragment_patcher").Length != 0)
                    {
                        UpdateProgress(50);
                    }
                    
                    if(Process.GetProcessesByName("imgburn").Length != 0)
                    {
                        UpdateProgress(65);
                    }

                    if(Process.GetProcessesByName("vis_patcher").Length != 0)
                    {
                        UpdateProgress(80);
                    }

                    reading = true;
                    Application.DoEvents();
                }

                if(process.HasExited)
                {
                    UpdateProgress(100);

                    reading = false;
                    string newIso = @telliToolStripMenuItem.ToolTipText + "\\dotHack Fragment (" + Get_TelliPatchVersion() + ").ISO";
                    isoFilePath.Text = newIso;
                    md5hash.Text = Get_MD5Hash(newIso);
                    finished_checking = true;
                }
            }

            if(finished_checking)
            {
                UpdateCheckMessage();
            }
        }

        private void UpdateCheckMessage()
        {
            UpdateStatusMessage("Last Checked: " + DateTime.Now.ToString());
            lastCheckedForNewVersion = DateTime.Now.ToString();
            SaveSettings();
        }

        private string Get_GithubTag(string repo)
        {
            string tag = "v0.0.0";
            HttpWebRequest request = WebRequest.Create(repo) as HttpWebRequest;
            if (request != null)
            {
                request.Method = "GET";
                request.UserAgent = "Anything";
                request.ServicePoint.Expect100Continue = false;

                try
                {
                    using (StreamReader response = new StreamReader(request.GetResponse().GetResponseStream()))
                    {

                        string reader = response.ReadLine();
                        switch (repo)
                        {
                            case aliceGithubReleaseLink:
                                Release.Root aliceRoot = JsonConvert.DeserializeObject<Release.Root>(reader);
                                aliceGithubTag = aliceRoot.tag_name;
                                return aliceRoot.tag_name;
                            case viGithubReleaseLink:
                                Release.Root viRoot = JsonConvert.DeserializeObject<Release.Root>(reader);
                                viGithubTag = viRoot.tag_name;
                                return viRoot.tag_name;
                            default:
                                return tag;
                        }
                    }
                }
                catch
                {
                    return tag;
                }
            }
            else
            {
                return tag;
            }
        }

        private DateTime GetLastRelease(string repo)
        {
            HttpWebRequest request = WebRequest.Create(repo) as HttpWebRequest;
            if (request != null)
            {
                request.Method = "GET";
                request.UserAgent = "Anything";
                request.ServicePoint.Expect100Continue = false;

                try
                {
                    using (StreamReader response = new StreamReader(request.GetResponse().GetResponseStream()))
                    {

                        string reader = response.ReadLine();
                        switch (repo)
                        {
                            case aliceGithubReleaseLink:
                                Release.Root aliceRoot = JsonConvert.DeserializeObject<Release.Root>(reader);
                                return aliceRoot.published_at;
                            case viGithubReleaseLink:
                                Release.Root viRoot = JsonConvert.DeserializeObject<Release.Root>(reader);
                                return viRoot.published_at;
                            default:
                                return DateTime.MinValue;
                        }
                    }
                }
                catch
                {
                    return DateTime.MinValue;
                }
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        private void dialogBox_TextChanged(object sender, EventArgs e)
        {
            dialogBox.ScrollToCaret();
        }

        #endregion

        #region Click Button Functions

        private void checkNewVersion_Click(object sender, EventArgs e)
        {
            UpdateProgress(0);

            if (isoFilePath.Text == string.Empty)
            {
                MessageBox.Show("Please select a .hack//fragment ISO before checking for a new version.", ".hack//fragment launcher - error");

            }
            else if (telliToolStripMenuItem.ToolTipText == string.Empty) {
                MessageBox.Show("No TelliPatch folder found.\nUnder the Tools menu, please select the folder where TelliPatch's 'Apply Patch.bat' is stored.", ".hack//fragment launcher - error");

            }
            else
            {
                dialogBox.Text = ""; // clear out textbox.
                Log("Checking for new version...");
                CheckForUpdate();
            }
        }

        private void chooseIso_Btn_Click(object sender, EventArgs e)
        {
            var filePath = string.Empty;

            using OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog.Filter = "ISO files (*.iso)|*.iso|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                filePath = openFileDialog.FileName;

                isoFilePath.Text = filePath.ToString();
                md5hash.Text = Get_MD5Hash(filePath);
                md5hash.BackColor = System.Drawing.Color.White;
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
            new About().Show();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // save before closing
            SaveSettings();
            Close();
        }

        private void getTelliPatchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://bbs.dothackers.org/viewtopic.php?f=6&t=80&p=262#p262",
                UseShellExecute = true
            });
        }

        #endregion Menu / Status Bar Functions
    }
}
