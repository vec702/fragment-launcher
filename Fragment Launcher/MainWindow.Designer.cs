using System.Windows.Forms;

namespace Fragment_Launcher
{
    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.launchButton = new System.Windows.Forms.Button();
            this.checkNewVersion = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.dialogBox = new System.Windows.Forms.RichTextBox();
            this.chooseIso_Btn = new System.Windows.Forms.Button();
            this.isoFilePath = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.md5hash = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pcsx2StripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.telliToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.getTelliPatchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.debugInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // launchButton
            // 
            this.launchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.launchButton.BackColor = System.Drawing.Color.White;
            this.launchButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.launchButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.launchButton.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.launchButton.Location = new System.Drawing.Point(396, 339);
            this.launchButton.Name = "launchButton";
            this.launchButton.Size = new System.Drawing.Size(79, 41);
            this.launchButton.TabIndex = 0;
            this.launchButton.Text = "Launch";
            this.launchButton.UseVisualStyleBackColor = false;
            this.launchButton.Click += new System.EventHandler(this.LaunchButton_Click);
            // 
            // checkNewVersion
            // 
            this.checkNewVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkNewVersion.BackColor = System.Drawing.Color.White;
            this.checkNewVersion.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.checkNewVersion.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.checkNewVersion.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.checkNewVersion.Location = new System.Drawing.Point(312, 339);
            this.checkNewVersion.Name = "checkNewVersion";
            this.checkNewVersion.Size = new System.Drawing.Size(79, 41);
            this.checkNewVersion.TabIndex = 0;
            this.checkNewVersion.Text = "Check for New Version";
            this.checkNewVersion.UseVisualStyleBackColor = false;
            this.checkNewVersion.Click += new System.EventHandler(this.CheckNewVersion_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(10, 339);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(296, 41);
            this.progressBar1.TabIndex = 1;
            // 
            // dialogBox
            // 
            this.dialogBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dialogBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.dialogBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dialogBox.Location = new System.Drawing.Point(10, 222);
            this.dialogBox.Name = "dialogBox";
            this.dialogBox.ReadOnly = true;
            this.dialogBox.Size = new System.Drawing.Size(464, 112);
            this.dialogBox.TabIndex = 2;
            this.dialogBox.Text = "";
            this.dialogBox.TextChanged += new System.EventHandler(this.DialogBox_TextChanged);
            // 
            // chooseIso_Btn
            // 
            this.chooseIso_Btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chooseIso_Btn.BackColor = System.Drawing.Color.White;
            this.chooseIso_Btn.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.chooseIso_Btn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chooseIso_Btn.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.chooseIso_Btn.Location = new System.Drawing.Point(371, 167);
            this.chooseIso_Btn.Name = "chooseIso_Btn";
            this.chooseIso_Btn.Size = new System.Drawing.Size(103, 21);
            this.chooseIso_Btn.TabIndex = 0;
            this.chooseIso_Btn.Text = "Select ISO";
            this.chooseIso_Btn.UseVisualStyleBackColor = false;
            this.chooseIso_Btn.Click += new System.EventHandler(this.ChooseIso_Btn_Click);
            // 
            // isoFilePath
            // 
            this.isoFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.isoFilePath.BackColor = System.Drawing.Color.White;
            this.isoFilePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.isoFilePath.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.isoFilePath.Location = new System.Drawing.Point(10, 167);
            this.isoFilePath.Name = "isoFilePath";
            this.isoFilePath.ReadOnly = true;
            this.isoFilePath.Size = new System.Drawing.Size(356, 23);
            this.isoFilePath.TabIndex = 4;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 196);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "MD5 Hash";
            // 
            // md5hash
            // 
            this.md5hash.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.md5hash.BackColor = System.Drawing.Color.White;
            this.md5hash.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.md5hash.Location = new System.Drawing.Point(81, 193);
            this.md5hash.Name = "md5hash";
            this.md5hash.ReadOnly = true;
            this.md5hash.Size = new System.Drawing.Size(393, 25);
            this.md5hash.TabIndex = 4;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(485, 25);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.MenuStrip1_ItemClicked);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(39, 21);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
            this.saveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShowShortcutKeys = false;
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(96, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(93, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(96, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pcsx2StripMenuItem1,
            this.telliToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(51, 21);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // pcsx2StripMenuItem1
            // 
            this.pcsx2StripMenuItem1.Name = "pcsx2StripMenuItem1";
            this.pcsx2StripMenuItem1.Size = new System.Drawing.Size(208, 22);
            this.pcsx2StripMenuItem1.Text = "Select PCSX2 EXE";
            // 
            // telliToolStripMenuItem
            // 
            this.telliToolStripMenuItem.Name = "telliToolStripMenuItem";
            this.telliToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.telliToolStripMenuItem.Text = "Select TelliPatch Folder";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.debugInfoToolStripMenuItem,
            this.toolStripSeparator2,
            this.getTelliPatchToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(47, 21);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.aboutToolStripMenuItem.Text = "&About...";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(150, 6);
            // 
            // getTelliPatchToolStripMenuItem
            // 
            this.getTelliPatchToolStripMenuItem.Name = "getTelliPatchToolStripMenuItem";
            this.getTelliPatchToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.getTelliPatchToolStripMenuItem.Text = "Get TelliPatch";
            this.getTelliPatchToolStripMenuItem.Click += new System.EventHandler(this.GetTelliPatchToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.statusStrip1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(96, 16);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.statusStrip1.Location = new System.Drawing.Point(0, 400);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 12, 0);
            this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.statusStrip1.Size = new System.Drawing.Size(13, 5);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 0);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.ErrorImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.ErrorImage")));
            this.pictureBox1.Location = new System.Drawing.Point(-11, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(503, 347);
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // debugInfoToolStripMenuItem
            // 
            this.debugInfoToolStripMenuItem.Name = "debugInfoToolStripMenuItem";
            this.debugInfoToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.debugInfoToolStripMenuItem.Text = "Debug Info";
            this.debugInfoToolStripMenuItem.Click += new System.EventHandler(this.DebugInfoToolStripMenuItem_Click);
            // 
            // MainWindow
            // 
            this.AcceptButton = this.launchButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(485, 403);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.md5hash);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.isoFilePath);
            this.Controls.Add(this.chooseIso_Btn);
            this.Controls.Add(this.dialogBox);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.checkNewVersion);
            this.Controls.Add(this.launchButton);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.pictureBox1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(501, 442);
            this.Name = "MainWindow";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = ".hack//fragment Launcher";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button launchButton;
        private System.Windows.Forms.Button checkNewVersion;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.RichTextBox dialogBox;
        private System.Windows.Forms.Button chooseIso_Btn;
        private System.Windows.Forms.TextBox isoFilePath;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox md5hash;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem telliToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripMenuItem pcsx2StripMenuItem1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem getTelliPatchToolStripMenuItem;
        private PictureBox pictureBox1;
        private ToolStripMenuItem debugInfoToolStripMenuItem;
    }
}

