﻿using System;
using System.Collections.Generic;
using System.Linq;
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
