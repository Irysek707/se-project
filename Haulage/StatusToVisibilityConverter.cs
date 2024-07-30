using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using Haulage.Model.Constants;

namespace Haulage.Converters
{
    public class StatusToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Status status)
            {
                // Check if the status is AWAITING_PICKUP
                return status == Status.AWAITING_PICKUP;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
