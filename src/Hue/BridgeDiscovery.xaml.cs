using Q42.HueApi;
using Q42.HueApi.Models.Bridge;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
