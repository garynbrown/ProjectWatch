﻿using Core.Common.Core;
using System.ComponentModel;
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
//using MahApps.Metro.Controls;
using ProjectWatch.ViewModel;

namespace ProjectWatch
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow 
	{
		#region Constructors
		public MainWindow()
		{
			InitializeComponent();
			//			main.DataContext = ClientEntityBase.Container.GetExportedValue<MainViewModel>();
			Dashboard.DataContext = ClientEntityBase.Container.GetExportedValue<DashboardViewModel>();
		}
		#endregion
	}
}
