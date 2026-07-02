using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace KnockServerWV
{
    public partial class Form1 : Form
    {        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Logger.Init();
            Server.LoadConfig("config.txt", "rules.txt");
            RefreshDisplay();
            timer1.Enabled = true;
            Server.StartFilterEngine();
            Server.Start();
            startToolStripMenuItem.Enabled = !Server.isRunning;
            stopToolStripMenuItem.Enabled = Server.isRunning;
        }

        public void RefreshDisplay()
        {
            StringBuilder sb = new StringBuilder();
            lock (Server._sync)
            {
                List<LogEntry> log = Logger.LogEntries();
                foreach (LogEntry e in log)
                    sb.AppendLine(e.time.ToLongTimeString() + " " + e.text);
                log.Clear();
            }
            rtb1.BeginUpdate();
            rtb1.Text = sb.ToString();
            rtb1.SelectionLength = 0;
            rtb1.SelectionStart = rtb1.Text.Length;
            rtb1.ScrollToCaret();
            rtb1.EndUpdate();
            sb = new StringBuilder();
            int[] seq = Server.MakeCurrentKnockSequence();
            sb.Append("Current Sequence:");
            foreach (int port in seq)
                sb.Append(" " + port);
            sb.AppendLine();
            rtb2.Text = sb.ToString();
            sb = new StringBuilder();
            foreach (PortWatcher pw in Server.watchers)
                sb.AppendLine("Port " + pw.port + " : " + (pw.IsRunning() ? "LISTENING": "STOPPED"));
            rtb3.Text = sb.ToString();
            sb = new StringBuilder();
            foreach (KeyValuePair<uint, Server.KnockUser> kvp in Server.knockUsers)
            {
                uint ip = kvp.Key;
                Server.KnockUser user = kvp.Value;
                sb.Append(Server.Ip4ToString(ip) + " : " + user.Authenticated + " ");
                TimeSpan t = DateTime.Now - user.Updated;
                t = TimeSpan.FromSeconds(Server.maxUserTimeout - t.TotalSeconds);
                sb.AppendLine(t.ToString(@"hh\:mm\:ss"));
            }
            rtb4.Text = sb.ToString();
            Server.Update();
            GC.Collect();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            RefreshDisplay();
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Server.Start();
            startToolStripMenuItem.Enabled = !Server.isRunning;
            stopToolStripMenuItem.Enabled = Server.isRunning;
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Server.Stop();
            startToolStripMenuItem.Enabled = !Server.isRunning;
            stopToolStripMenuItem.Enabled = Server.isRunning;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Server.Stop();
        }
    }
}
