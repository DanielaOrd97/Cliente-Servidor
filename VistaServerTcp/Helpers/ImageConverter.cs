using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace VistaServerTcp.Helpers
{
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var base64 = value as string;

            Bitmap bmpReturn = null;

            byte[] Buffer = System.Convert.FromBase64String(base64);
            MemoryStream ms = new MemoryStream(Buffer);

            bmpReturn = (Bitmap)Bitmap.FromStream(ms);

            ms.Close();
            ms = null!;
            Buffer = null!;

            return bmpReturn;   
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
