﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
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

        private const string aliceGithubReleaseLink = "https://api.github.com/repos/Tellilum/.hack-fragment-Definitive-Translation/releases/latest";
        private const string tellipatchDownloadLink = "https://github.com/robby-u/tellipatch/releases/download/Windows/tellipatch.zip";
        private const string JP_ISO_MD5 = "94C82040BF4BB99500EB557A3C0FBB15";

        private const string GOOGLE_API_KEY = "Generate your own API key, please. :-)";

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
            aliceGithubTag = info.Default.aliceGithubVersion.ToString();

            if (Get_TelliPatchVersion() != "Not Installed")
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

            if(Get_TelliPatchVersion() != "Not Installed")
            {
                getTelliPatchToolStripMenuItem.Checked = true;
                getTelliPatchToolStripMenuItem.Enabled = false;
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
            string version = String.Empty;
            try
            {
                version = File.ReadAllText(@telliToolStripMenuItem.ToolTipText + "\\VERSION");
            }
            catch (Exception)
            {

            }

            if (version != String.Empty)
            {
                telliPatchVersion = version;
                return telliPatchVersion;
            }
            else return "Not Installed";
        }

        private void SaveSettings()
        {
            info.Default.isoFilePath = isoFilePath.Text;
            info.Default.telliFolder = telliToolStripMenuItem.ToolTipText;
            info.Default.pcsx2Folder = pcsx2StripMenuItem1.ToolTipText;
            info.Default.lastCheck = lastCheckedForNewVersion;
            info.Default.aliceGithubVersion = aliceGithubTag;
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

        private void DownloadTellipatch()
        {
            using WebClient webClient = new WebClient();
            try
            {
                webClient.Headers.Add("Accept: text/html, application/xhtml+xml, */*");
                webClient.Headers.Add("User-Agent: Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");
                webClient.DownloadFileAsync(new Uri(tellipatchDownloadLink), "tellipatch.zip");
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(CompleteTellipatchDownload);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CompleteTellipatchDownload(object sender, AsyncCompletedEventArgs e)
        {
            ZipFile.ExtractToDirectory("tellipatch.zip", Directory.GetCurrentDirectory() + "\\tellipatch\\");

            string newpath = Directory.GetCurrentDirectory() + "\\tellipatch\\";
            info.Default.telliFolder = newpath;
            telliToolStripMenuItem.ToolTipText = newpath;
            telliToolStripMenuItem.Checked = true;

            if (Get_TelliPatchVersion() != "Not Installed")
            {
                getTelliPatchToolStripMenuItem.Checked = true;
                getTelliPatchToolStripMenuItem.Enabled = false;
            }

            SaveSettings();

            Log("Download complete. TelliPatch folder set to " + newpath);
            statusStrip1.Update();
        }

        private void DeleteTellipatchDirectory()
        {
            var tellipatchDir = Directory.GetCurrentDirectory() + "\\tellipatch\\";

            if (Directory.Exists(tellipatchDir))
            {
                // recursively delete tellipatch directory
                Directory.Delete(tellipatchDir, true);
            }
        }

        private string Get_MD5Hash(string filePath)
        {
            UpdateStatusMessage("Calculating MD5 hash code...");
            reading = true;

            using var md5 = MD5.Create();
            using var stream = File.OpenRead(filePath);
            var hash = md5.ComputeHash(stream);
            string final_checksum = BitConverter.ToString(hash).Replace("-", "");
            reading = false;
            return final_checksum;
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

                    string aliceTagCleaned = Get_GithubTag(aliceGithubReleaseLink)[1..].Replace(".", string.Empty);
                    string aliceLastSavedData = aliceGithubTag[1..].Replace(".", string.Empty);

                    aliceGithubTag = Get_GithubTag(aliceGithubReleaseLink);

                    /*
                     * We need to patch if any of the following criteria are met:
                     * 1. the last check recorded was before a new alice release
                     * 2. the last check recorded was before a change was made in vi's sheet
                     * 3. the iso loaded was created before a new alice release
                     * 4. the iso loaded was created before a change was made in vi's sheet
                     * 5. the saved alice release tag is older than alice's newest release tag
                     * 6. the iso loaded is the original jp disc
                     */
                    if (DateTime.Compare(DateTime.Parse(lastCheckedForNewVersion), GetLastRelease(aliceGithubReleaseLink)) == -1 ||
                        DateTime.Compare(DateTime.Parse(lastCheckedForNewVersion), Vi_LastModified_DateTime()) == -1 ||
                        DateTime.Compare(File.GetLastWriteTime(isoFilePath.Text), GetLastRelease(aliceGithubReleaseLink)) == -1 ||
                        DateTime.Compare(File.GetLastWriteTime(isoFilePath.Text), Vi_LastModified_DateTime()) == -1 ||
                        Convert.ToInt32(aliceLastSavedData) < Convert.ToInt32(aliceTagCleaned) ||
                        md5hash.Text == JP_ISO_MD5)
                    {
                        DialogResult patchNow = MessageBox.Show("A new version was found.\nWould you like to patch now?", ".hack//fragment launcher", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
                            if (!e.Data.Contains("Press any key to continue")) Invoke((MethodInvoker)(() => dialogBox.AppendText(Environment.NewLine + e.Data)));
                        }
                        else
                        {
                            if (!e.Data.Contains("Press any key to continue")) dialogBox.AppendText(Environment.NewLine + e.Data);
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
            string tag = String.Empty;
            if (WebRequest.Create(repo) is HttpWebRequest request)
            {
                request.Method = "GET";
                request.UserAgent = "Anything";
                request.ServicePoint.Expect100Continue = false;

                try
                {
                    using StreamReader response = new StreamReader(request.GetResponse().GetResponseStream());

                    string reader = response.ReadLine();
                    switch (repo)
                    {
                        case aliceGithubReleaseLink:
                            Release.Root aliceRoot = JsonConvert.DeserializeObject<Release.Root>(reader);
                            return aliceRoot.tag_name;
                        default:
                            return tag;
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
            if (WebRequest.Create(repo) is HttpWebRequest request)
            {
                request.Method = "GET";
                request.UserAgent = "Anything";
                request.ServicePoint.Expect100Continue = false;

                try
                {
                    using StreamReader response = new StreamReader(request.GetResponse().GetResponseStream());

                    string reader = response.ReadLine();
                    switch (repo)
                    {
                        case aliceGithubReleaseLink:
                            Release.Root aliceRoot = JsonConvert.DeserializeObject<Release.Root>(reader);
                            return aliceRoot.published_at;
                        default:
                            return DateTime.MinValue;
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

        private DateTime Vi_LastModified_DateTime()
        {
            string request = $"https://www.googleapis.com/drive/v3/files/1vQvaTQXel9jUuUt-GwZZCcGi-JKyhm-LE6MKdIXKxIA?fields=modifiedTime&key={GOOGLE_API_KEY}";
            string data = string.Empty;

            using (WebClient client = new WebClient())
            {
                data = client.DownloadString(request);
            }

            var obj = JObject.Parse(data);
            DateTime lastModified = DateTime.Parse(obj["modifiedTime"].ToString());

            return lastModified;
        }

        private void DialogBox_TextChanged(object sender, EventArgs e)
        {
            dialogBox.ScrollToCaret();
        }

        #endregion

        #region Click Button Functions

        private void CheckNewVersion_Click(object sender, EventArgs e)
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

        private void ChooseIso_Btn_Click(object sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog.Filter = "ISO files (*.iso)|*.iso|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                var filePath = openFileDialog.FileName;

                isoFilePath.Text = filePath.ToString();
                md5hash.Text = Get_MD5Hash(filePath);
                md5hash.BackColor = System.Drawing.Color.White;
            }
        }

        private void LaunchButton_Click(object sender, EventArgs e)
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

        private void MenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            saveToolStripMenuItem.Click += new EventHandler(SaveToolStripMenuItem_Click);
            exitToolStripMenuItem.Click += new EventHandler(ExitToolStripMenuItem_Click);
            telliToolStripMenuItem.Click += new EventHandler(TelliToolStripMenuItem_Click);
            pcsx2StripMenuItem1.Click += new EventHandler(Pcsx2ToolStripMenuItem_Click);
            aboutToolStripMenuItem.Click += new EventHandler(AboutToolStripMenuItem_Click);
        }

        private void TelliToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();

            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                info.Default.telliFolder = fbd.SelectedPath.ToString();
                telliToolStripMenuItem.ToolTipText = fbd.SelectedPath.ToString();
                telliToolStripMenuItem.Checked = true;
            }

            toolStripStatusLabel1.Text = "TelliPatch folder set to " + fbd.SelectedPath.ToString();
            statusStrip1.Update();
        }

        private void Pcsx2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var filePath = string.Empty;

            using OpenFileDialog openFileDialog = new OpenFileDialog();
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

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new About().ShowDialog();
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // save before closing
            SaveSettings();
            Close();
        }

        private void GetTelliPatchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(Get_TelliPatchVersion() == "Not Installed")
            {
                Log("Downloading TelliPatch...");
                DownloadTellipatch();
            } else
            {
                Log($"TelliPatch is already installed. It is located at: {telliToolStripMenuItem.ToolTipText}");
            }
        }

        private void DebugInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log($"Latest Alice Release: {Get_GithubTag(aliceGithubReleaseLink)} (Found: {aliceGithubTag})");
            Log($"Latest Vi Modification: {Vi_LastModified_DateTime().ToString()} (Found: {lastCheckedForNewVersion})");
            Log($"Tellipatch Version {Get_TelliPatchVersion()}");
        }

        #endregion Menu / Status Bar Functions
    }
}
