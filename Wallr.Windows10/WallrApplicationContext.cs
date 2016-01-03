using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Wallr.Windows10
{
    public class WallrApplicationContext : ApplicationContext
    {
        private NotifyIcon _notifyIcon;

        public WallrApplicationContext()
        {
            InitializeNotifyIcon();
        }

        private void InitializeNotifyIcon()
        {
            var components = new Container();
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.Items.Add(new ToolStripButton("Exit", null, exitItem_Click));
            _notifyIcon = new NotifyIcon(components)
            {
                ContextMenuStrip = contextMenuStrip,
                Visible = true,
                Icon = new Icon(Assembly.GetEntryAssembly().GetManifestResourceStream("Wallr.Windows10.assets.wallr.ico")), // TODO: Update icon
                Text = "Wallr"
            };
        }

        private void exitItem_Click(object sender, EventArgs e)
        {
            ExitThread();
        }

        protected override void ExitThreadCore()
        {
            _notifyIcon.Visible = false;
            base.ExitThreadCore();
        }
    }
}
