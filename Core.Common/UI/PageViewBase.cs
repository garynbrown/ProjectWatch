using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using GalaSoft.MvvmLight;

namespace Core.Common.UI
{
	public class PageViewBase : Page
	{
		public PageViewBase()
		{
			BindingOperations.SetBinding(this, ViewLoadedProperty, new Binding("ViewLoaded"));

			DataContextChanged += OnDataContextChanged;
		}
		public static readonly DependencyProperty ViewLoadedProperty =
			DependencyProperty.Register("ViewLoaded", typeof(object), typeof(PageViewBase),
			new PropertyMetadata(null));
		protected virtual void OnUnwireViewModelEvents(ViewModelBase viewModel) { }

		protected virtual void OnWireViewModelEvents(ViewModelBase viewModel) { }
		void OnDataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue == null)
			{
				if (e.OldValue != null)
				{
					// view going out of scope and view-model disconnected (but still around in the parent)
					// unwire events to allow view to dispose

					OnUnwireViewModelEvents(e.OldValue as ViewModelBase);
				}
			}
			else
			{
				OnWireViewModelEvents(e.NewValue as ViewModelBase);
			}
		}
	}
}
