using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ProjectWatch.Converters
{
	public class CurrentTimeDisplay : IValueConverter
	{

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return "Unknown";
			DateTime _currentTime = new DateTime();
			if (value.GetType() == typeof(DateTime))
			{
				_currentTime = (DateTime)value;
			}
			else
			{
				throw new ArgumentException("Value", "Must be of type DateTime");
			}
			string ampm = "am";
			string hour = _currentTime.Hour.ToString();
			string minute = _currentTime.Minute.ToString();
			string second = _currentTime.Second.ToString();
			if (_currentTime.Hour >= 12)
			{
				ampm = "pm";
				if (_currentTime.Hour > 12)
				{
					hour = (_currentTime.Hour - 12).ToString();
				}
			}
			if (_currentTime.Minute < 10)
			{
				minute = "0" + minute;
			}
			if (_currentTime.Second < 10)
			{
				second = "0" + second;
			}
//			return $"{hour}:{minute}:{second}{ampm}";
			return $"{hour}:{minute}{ampm}";
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
