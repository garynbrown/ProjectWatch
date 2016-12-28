using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ProjectWatch.Converters
{
	public class TimespanToMinutes : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return "Unknown";
			TimeSpan _timeSpan = new TimeSpan();
			if (value.GetType() == typeof(TimeSpan))
			{
				_timeSpan = (TimeSpan)value;
			}
			else
			{
				throw new ArgumentException("Value", "Must be of type TimeSpan");
			}
			string timePassed = "Unknown";

			if (_timeSpan != null && _timeSpan != new TimeSpan())
			{
				int seconds = (_timeSpan.Seconds - (_timeSpan.Hours*3600))/60;
				timePassed = _timeSpan.Hours + ":"+ seconds;
			}
			return timePassed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
