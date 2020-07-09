using System.Windows;

namespace HueLock.Hue {
	/// <summary>
	/// Interaction logic for ConfirmLink.xaml
	/// </summary>
	public partial class ConfirmLink : Window {
		public ConfirmLink() {
			InitializeComponent();
		}

		private void Confirm_Click(object sender, RoutedEventArgs e) {
			DialogResult = true;
			Close();
		}

		private void Cancel_Click(object sender, RoutedEventArgs e) {
			Close();
		}
	}
}
