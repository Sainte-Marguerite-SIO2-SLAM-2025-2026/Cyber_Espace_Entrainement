using Cyber_Espace_Entrainement.Models.UserEnumeration;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace Cyber_Espace_Entrainement.Converters
{
    public class ResultColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var item = values[0] as UserEnumeration;
            var estValide = values[1] as bool? ?? false;

            if (item == null)
                return Brushes.White;

            //  Avant validation = toujours blanc
            if (!estValide)
                return Brushes.White;

            //  Après validation
            if (item.ReponseUtilisateur == item.Reponse)
                return Brushes.LightGreen;

            return Brushes.LightCoral;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
