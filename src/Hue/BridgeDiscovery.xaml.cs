using Q42.HueApi.Models.Bridge;
using System.Collections.ObjectModel;
using System.Windows;

namespace HueLock.Hue {
	/// <summary>
	/// Interaction logic for BridgeDiscovery.xaml
	/// </summary>
	public partial class BridgeDiscovery : Window {

		public ObservableCollection<LocatedBridge> DiscoveredBridges { get; }

		public BridgeDiscovery() {
			InitializeComponent();
			DataContext = this;
			DiscoveredBridges = new ObservableCollection<LocatedBridge>();
			RefreshBridges();
		}

		private async void RefreshBridges() {
			DiscoveredBridges.Clear();
			var bridges = await HueClient.DiscoverBridgesAsync();
			foreach (var bridge in bridges)
				DiscoveredBridges.Add(bridge);
		}

		private void Confirm_Click(object sender, RoutedEventArgs e) {
			DialogResult = true;
			Close();
		}

		private void Cancel_Click(object sender, RoutedEventArgs e) {
			DialogResult = false;
			Close();
		}

		private void Refresh_Click(object sender, RoutedEventArgs e) {
			RefreshBridges();
		}
	}
}
