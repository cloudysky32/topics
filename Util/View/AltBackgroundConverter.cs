using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;

namespace Topics.Util.View
{
    public class AltBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(value is int)) return null;
            int index = (int)value;

            switch (index % 4)
            {
                case 0:
                    return Color.FromArgb(0xff, 0x1f, 0x6e, 0x9b);

                case 1:
                    return Color.FromArgb(0xff, 0x00, 0x7d, 0x21);

                case 2:
                    return Color.FromArgb(0xff, 0xbb, 0x49, 0x25);

                default:
                    return Color.FromArgb(0xff, 0xbd, 0x26, 0x47);
            }
        }

        // No need to implement converting back on a one-way binding
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
