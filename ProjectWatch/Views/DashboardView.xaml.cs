using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Core.Common.UI.Core;

namespace ProjectWatch.Views
{
	/// <summary>
	/// Interaction logic for DashboardView.xaml
	/// </summary>
	public partial class DashboardView : UserControlViewBase
	{
		public DashboardView()
		{
			InitializeComponent();
			
		}

		public static readonly DependencyProperty IsCollapsedProperty = DependencyProperty.Register(
			"IsCollapsed", typeof(bool), typeof(DashboardView), new PropertyMetadata(false));

		public bool IsCollapsed
		{
			get { return (bool) GetValue(IsCollapsedProperty); }
			set { SetValue(IsCollapsedProperty, value); }
		}

		public void ToggleCollapsed()
		{
			IsCollapsed = !IsCollapsed;
		}

		private void PlayButton_Click(object sender, RoutedEventArgs e)
		{
			if (!IsCollapsed)
				IsCollapsed = true;
		}

		private void PauseButton_Click(object sender, RoutedEventArgs e)
		{
			if (!IsCollapsed)
				IsCollapsed = true;

		}

		private void StopButton_Click(object sender, RoutedEventArgs e)
		{
			if (!IsCollapsed)
				IsCollapsed = true;

		}
	}
}
