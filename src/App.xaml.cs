using System;
using System.Windows;

namespace HueLock {
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application {

		private HueLockManager manager;

		protected override void OnStartup(StartupEventArgs e) {
			base.OnStartup(e);

			var minimized = Array.Exists(e.Args, arg => arg == "/minimized");

			manager = new HueLockManager();
			var MainWindow = new MainWindow(manager);
			if (minimized)
				MainWindow.WindowState = WindowState.Minimized;
			MainWindow.Show();
		}
	}
}
