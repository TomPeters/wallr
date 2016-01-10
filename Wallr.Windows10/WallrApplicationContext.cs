using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Serilog;
using Wallr.Common;

namespace Wallr.Windows10
{
    public class WallrApplicationContext : ApplicationContext
    {
        private NotifyIcon _notifyIcon;

        public void InitializeNotifyIcon(IEnumerable<ToolStripItem> systemTrayOptions)
        {
            var components = new Container();
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            systemTrayOptions.ForEach(o => contextMenuStrip.Items.Add(o));
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
            Log.Logger.Information("Application exiting");
            _notifyIcon.Visible = false;
            base.ExitThreadCore();
        }
    }
}
