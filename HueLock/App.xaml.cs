using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Windows;

namespace HueLock {
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application {

		private HueLockManager manager;
		private TaskbarIcon trayIcon;

		protected override void OnStartup(StartupEventArgs e) {
			base.OnStartup(e);

			manager = new HueLockManager();
			if (!string.IsNullOrEmpty(HueLock.Properties.Settings.Default.BridgeIpAddress))
				_ = manager.InitializeConnectionAsync().ConfigureAwait(false);
			trayIcon = new TrayIcon(manager);

			var minimized = Array.Exists(e.Args, arg => arg == "/minimized");
			if (!minimized) {
				var MainWindow = new MainWindow(manager);
				MainWindow.Show();
			}
		}
	}
}
