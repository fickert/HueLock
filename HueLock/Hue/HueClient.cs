using Q42.HueApi;
using Q42.HueApi.Models.Bridge;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HueLock.Hue {
	public static class HueClient {

		public static async Task<IEnumerable<LocatedBridge>> DiscoverBridgesAsync(TimeSpan? timeout = null) {
			var locator = new HttpBridgeLocator();
			return await locator.LocateBridgesAsync(timeout ?? TimeSpan.FromSeconds(5));
		}

		public static async Task<string> PairBridgeAsync(string ipAddress) {
			var client = new LocalHueClient(ipAddress);
			var confirmLinkWindow = new ConfirmLink();
			if (!(confirmLinkWindow.ShowDialog() ?? false))
				return null;
			// var registrationResult = await client.RegisterAsync("HueLock", System.Environment.MachineName, true);
			// return registrationResult.StreamingClientKey;
			return await client.RegisterAsync("HueLock", System.Environment.MachineName);
		}
	}
}
