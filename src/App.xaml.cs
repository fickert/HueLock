using System.Windows;

namespace HueLock {
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application {

		private HueLockManager manager;

		protected override void OnStartup(StartupEventArgs e) {
			base.OnStartup(e);

			manager = new HueLockManager();
			var MainWindow = new MainWindow(manager);
			MainWindow.Show();
		}
	}
}
