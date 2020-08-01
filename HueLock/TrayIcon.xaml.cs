using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Windows;
using System.Windows.Input;

namespace HueLock {
	/// <summary>
	/// Interaction logic for TrayIcon.xaml
	/// </summary>
	public partial class TrayIcon : TaskbarIcon {
		public HueLockManager Manager { get; }

		public ICommand ShowWindowCommand {
			get {
				return new DelegateCommand {
					CanExecuteFunc = () => Application.Current.MainWindow == null,
					CommandAction = () => {
						Application.Current.MainWindow = new MainWindow(Manager);
						Application.Current.MainWindow.Show();
					}
				};
			}
		}

		public ICommand ExitApplicationCommand {
			get {
				return new DelegateCommand { CommandAction = () => Application.Current.Shutdown() };
			}
		}

		private class DelegateCommand : ICommand {
			public Action CommandAction { get; set; }
			public Func<bool> CanExecuteFunc { get; set; }

			public void Execute(object parameter) {
				CommandAction();
			}

			public bool CanExecute(object parameter) {
				return CanExecuteFunc == null || CanExecuteFunc();
			}

			public event EventHandler CanExecuteChanged {
				add { CommandManager.RequerySuggested += value; }
				remove { CommandManager.RequerySuggested -= value; }
			}
		}

		public TrayIcon(HueLockManager Manager) {
			InitializeComponent();
			this.Manager = Manager;
			DataContext = this;
		}
	}
}