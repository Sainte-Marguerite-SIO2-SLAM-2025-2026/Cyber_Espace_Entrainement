using System;
using System.Globalization;
using System.Windows.Data;

namespace Cyber_Espace_Entrainement.Converters
{
    /// <summary>
    /// Convertisseur qui ajoute automatiquement le préfixe du répertoire d'images.
    /// Permet de stocker uniquement le nom du fichier dans la base de données. 
    /// </summary>
    public class ImagePathConverter : IValueConverter
    {
        // Chemin de base pour toutes les images d'icônes
        private const string IMAGE_BASE_PATH = "/Resources/Images/Icons/";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return null;

            string fileName = value.ToString();

            // Si le chemin complet est déjà présent, le retourner tel quel
            if (fileName.StartsWith("/") || fileName.StartsWith("pack://"))
                return fileName;

            // Sinon, ajouter le préfixe
            return IMAGE_BASE_PATH + fileName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Pour le ConvertBack, on extrait juste le nom du fichier
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return null;

            string fullPath = value.ToString();

            // Retirer le préfixe si présent
            if (fullPath.StartsWith(IMAGE_BASE_PATH))
                return fullPath.Substring(IMAGE_BASE_PATH.Length);

            return fullPath;
        }
    }
}