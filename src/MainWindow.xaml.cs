using Q42.HueApi.Models.Bridge;
using System.ComponentModel;
using System.Windows;

namespace HueLock {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged {

		private readonly HueLockManager manager;

		public string BridgeIpAddress {
			get {
				var bridgeIpAddress = Properties.Settings.Default.BridgeIpAddress;
				return string.IsNullOrEmpty(bridgeIpAddress) ? "not configured" : bridgeIpAddress;
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string propertyName) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public MainWindow(HueLockManager Manager) {
			InitializeComponent();
			manager = Manager;
			bridgeStatus.DataContext = this;
			connectionStatus.DataContext = manager;
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			Properties.Settings.Default.Save();
		}

		private async void Button_Click(object sender, RoutedEventArgs e) {
			var bridgeDiscoveryWindow = new Hue.BridgeDiscovery();
			if (!(bridgeDiscoveryWindow.ShowDialog() ?? false))
				return;
			var selectedBridge = (LocatedBridge)bridgeDiscoveryWindow.bridgesView.SelectedItem;
			if (selectedBridge is null)
				return;
			Properties.Settings.Default.BridgeId = selectedBridge.BridgeId;
			Properties.Settings.Default.BridgeIpAddress = selectedBridge.IpAddress;
			OnPropertyChanged(nameof(BridgeIpAddress));
			await manager.InitializeConnectionAsync();
		}

		private async void Window_Loaded(object sender, RoutedEventArgs e) {
			if (!string.IsNullOrEmpty(Properties.Settings.Default.BridgeIpAddress))
				await manager.InitializeConnectionAsync();
		}
	}
}
