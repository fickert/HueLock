﻿using Q42.HueApi;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HueLock {
	class HueLockManager {

		private LocalHueClient hueClient;
		private readonly IDictionary<string, bool> lastState;

		private async Task TurnOffLightsAsync(bool storeState) {
			lastState.Clear();
			if (storeState) {
				var lights = hueClient.GetLightsAsync();
				foreach (var light in await lights)
					lastState[light.Id] = light.State.On;
			}
			var command = new LightCommand().TurnOff();
			await hueClient.SendCommandAsync(command);
		}

		private async Task RestoreLightsAsync() {
			var lightsToTurnOn = new List<string>();
			foreach (var entry in lastState)
				if (entry.Value)
					lightsToTurnOn.Add(entry.Key);
			var command = new LightCommand().TurnOn();
			await hueClient.SendCommandAsync(command, lightsToTurnOn);
		}

		public HueLockManager() {
			Microsoft.Win32.SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;
			Microsoft.Win32.SystemEvents.SessionEnded += SystemEvents_SessionEnded;
			lastState = new Dictionary<string, bool>();
		}

		public async Task<bool> InitializeClient() {
			hueClient = new LocalHueClient(Properties.Settings.Default.BridgeIpAddress);
			if (string.IsNullOrEmpty(Properties.Settings.Default.BridgeKey)) {
				var key = await Hue.HueClient.PairBridgeAsync(Properties.Settings.Default.BridgeIpAddress);
				if (string.IsNullOrEmpty(key))
					return false;
				Properties.Settings.Default.BridgeKey = key;
			}
			hueClient.Initialize(Properties.Settings.Default.BridgeKey);
			return await hueClient.CheckConnection();
		}

		private async void SystemEvents_SessionSwitch(object sender, Microsoft.Win32.SessionSwitchEventArgs e) {
			if (!Properties.Settings.Default.TurnOffOnLock)
				return;
			if (hueClient is null || !hueClient.IsInitialized)
				return;
			if (e.Reason == Microsoft.Win32.SessionSwitchReason.SessionLock) {
				await TurnOffLightsAsync(true);
			} else if (e.Reason == Microsoft.Win32.SessionSwitchReason.SessionUnlock) {
				await RestoreLightsAsync();
			}
		}

		private async void SystemEvents_SessionEnded(object sender, Microsoft.Win32.SessionEndedEventArgs e) {
			Microsoft.Win32.SystemEvents.SessionEnded -= SystemEvents_SessionEnded;
			if (!Properties.Settings.Default.TurnOffOnShutdown)
				return;
			if (hueClient is null || !hueClient.IsInitialized)
				return;
			await TurnOffLightsAsync(false);
		}
	}
}
