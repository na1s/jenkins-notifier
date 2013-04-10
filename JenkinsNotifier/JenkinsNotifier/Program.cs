using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using JenkinsNotifier;
using JenkinsRestClient;
using JenkinsRestClient.Data;

namespace MyTrayApp
{
    public class SysTrayApp : Form
    {
        private readonly NotifyIcon trayIcon;
        private readonly ContextMenu trayMenu;

        public SysTrayApp()
        {
            // Create a simple tray menu with only one item.
            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Exit", OnExit);

            // Create a tray icon. In this example we use a
            // standard system icon for simplicity, but you
            // can of course use your own custom icon too.
            trayIcon = new NotifyIcon();
            trayIcon.Text = "MyTrayApp";
            trayIcon.Icon = new Icon(SystemIcons.Application, 40, 40);

            // Add menu to tray icon and show it.
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;
            var timer = new Timer();
            timer.Tick += TimerTick;
            timer.Interval = 1000;
            timer.Start();
        }

        [STAThread]
        public static void Main()
        {
            Application.Run(new SysTrayApp());
        }

        private void TimerTick(object sender, EventArgs e)
        {
            CheckJenkinsState();
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false; // Hide form window.
            ShowInTaskbar = false; // Remove from taskbar.

            base.OnLoad(e);
        }

        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void CheckJenkinsState()
        {
            var server = new JenkinsServerApi(ConfigurationManager.AppSettings["JenkinsUrl"]);
            var client = new JenkinsClient(server);
            List<Job> jobs = client.GetJobs().ToList();
            if (jobs.All(j => j.IsSuccess()))
            {
                trayIcon.Icon = Icons.green_light;
                
            }
            if (jobs.Any(j => j.IsFailed()))
            {
                trayIcon.Icon = Icons.red_light;
            }
            ShowMessage(jobs);

        }

        private void ShowMessage(IEnumerable<Job> jobs)
        {
            trayIcon.BalloonTipIcon = ToolTipIcon.Info;
            trayIcon.BalloonTipTitle = @"Jenkins notifier";
            if (jobs.Any(j => j.IsFailed()))
            {
                trayIcon.BalloonTipText = string.Format("Failed jobs:{0}",
                                                        string.Join(Environment.NewLine,
                                                                    jobs.Where(j => j.IsFailed()).Select(t => t.Name)));
                trayIcon.ShowBalloonTip(5000);
            }
            if (jobs.All(j => j.IsSuccess()))
            {
                trayIcon.BalloonTipText = @"All builds are successful";
                trayIcon.ShowBalloonTip(5000);
            }
            
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                // Release the icon resource.
                trayIcon.Dispose();
            }

            base.Dispose(isDisposing);
        }
    }
}