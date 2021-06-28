using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Fragment_Launcher
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void About_Load(object sender, EventArgs e)
        {
            CenterToParent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            open_netslum();
        }

        private void open_netslum()
        {
            linkLabel1.LinkVisited = true;
            Process.Start(new ProcessStartInfo
            {
                FileName = "http://fragment.dothackers.org",
                UseShellExecute = true
            });
        }
    }
}
