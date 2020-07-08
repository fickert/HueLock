using Q42.HueApi.Models.Bridge;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace HueLock {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged {

		private HueLockManager Manager;
		public event PropertyChangedEventHandler PropertyChanged;

		enum Status {
			DISCONNECTED,
			CONNECTED,
			CONNECTING
		}

		private Status status;
		public string ConnectionStatus {
			get {
				switch (status) {
					case Status.DISCONNECTED:
						return "not connected";
					case Status.CONNECTED:
						return "connected";
					case Status.CONNECTING:
						return "connecting...";
					default:
						return "UNKNOWN";
				}
			}
		}

		public string BridgeIpAddress {
			get {
				var bridgeIpAddress = Properties.Settings.Default.BridgeIpAddress;
				return string.IsNullOrEmpty(bridgeIpAddress) ? "not configured" : bridgeIpAddress;
			}
		}

		protected void OnPropertyChanged(string propertyName) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public MainWindow() {
			InitializeComponent();
			connectionStatus.DataContext = this;
			bridgeStatus.DataContext = this;
			Manager = new HueLockManager();
			status = Status.DISCONNECTED;
			OnPropertyChanged(nameof(ConnectionStatus));
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			Properties.Settings.Default.Save();
		}

		private async Task InitializeConnectionAsync() {
			status = Status.CONNECTING;
			OnPropertyChanged(nameof(ConnectionStatus));
			var initializationResult = await Manager.InitializeClient();
			status = initializationResult ? Status.CONNECTED : Status.DISCONNECTED;
			OnPropertyChanged(nameof(ConnectionStatus));
		}

		private async void Button_Click(object sender, RoutedEventArgs e) {
			var bridgeDiscoveryWindow = new Hue.BridgeDiscovery();
			if (!(bridgeDiscoveryWindow.ShowDialog() ?? false))
				return;
			var selectedBridge = (LocatedBridge) bridgeDiscoveryWindow.bridgesView.SelectedItem;
			if (selectedBridge is null)
				return;
			Properties.Settings.Default.BridgeId = selectedBridge.BridgeId;
			Properties.Settings.Default.BridgeIpAddress = selectedBridge.IpAddress;
			OnPropertyChanged(nameof(BridgeIpAddress));
			await InitializeConnectionAsync();
		}

		private async void Window_Loaded(object sender, RoutedEventArgs e) {
			if (!string.IsNullOrEmpty(Properties.Settings.Default.BridgeIpAddress))
				await InitializeConnectionAsync();
		}
	}
}
