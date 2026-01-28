using Cyber_Espace_Entrainement.Views.Commun;
using System.Windows;

namespace Cyber_Espace_Entrainement.Services
{
    /// <summary>
    /// Service pour afficher des MessageBox personnalisées avec le thème de l'application
    /// </summary>
    public static class MessageBoxService
    {
        /// <summary>
        /// Affiche une MessageBox personnalisée
        /// </summary>
        /// <param name="message">Le message à afficher</param>
        /// <param name="title">Le titre de la fenêtre</param>
        /// <param name="button">Les boutons à afficher</param>
        /// <param name="icon">L'icône à afficher</param>
        /// <returns>Le résultat du bouton cliqué</returns>
        public static MessageBoxResult Show(
            string message,
            string title = "Information",
            MessageBoxButton button = MessageBoxButton.OK,
            MessageBoxImage icon = MessageBoxImage.Information)
        {
            var messageBox = new CustomMessageBox();
            messageBox.Configure(message, title, button, icon);

            // Définir le propriétaire si possible
            if (Application.Current.MainWindow != null && Application.Current.MainWindow.IsActive)
            {
                messageBox.Owner = Application.Current.MainWindow;
            }
            else
            {
                // Trouver la fenêtre active
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.IsActive)
                    {
                        messageBox.Owner = window;
                        break;
                    }
                }
            }

            messageBox.ShowDialog();
            return messageBox.Result;
        }

        /// <summary>
        /// Affiche une MessageBox d'information
        /// </summary>
        public static MessageBoxResult ShowInformation(string message, string title = "Information")
        {
            return Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Affiche une MessageBox d'erreur
        /// </summary>
        public static MessageBoxResult ShowError(string message, string title = "Erreur")
        {
            return Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Affiche une MessageBox d'avertissement
        /// </summary>
        public static MessageBoxResult ShowWarning(string message, string title = "Avertissement")
        {
            return Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        /// <summary>
        /// Affiche une MessageBox de question (Oui/Non)
        /// </summary>
        public static MessageBoxResult ShowQuestion(string message, string title = "Question")
        {
            return Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question);
        }

        /// <summary>
        /// Affiche une MessageBox de confirmation (OK/Annuler)
        /// </summary>
        public static MessageBoxResult ShowConfirmation(string message, string title = "Confirmation")
        {
            return Show(message, title, MessageBoxButton.OKCancel, MessageBoxImage.Question);
        }
    }
}