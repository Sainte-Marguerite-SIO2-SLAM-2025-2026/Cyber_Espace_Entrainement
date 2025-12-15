using Cyber_Espace_Entrainement.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Cyber_Espace_Entrainement.Views.Users
{
    /// <summary>
    /// Logique d'interaction pour UserGestion.xaml
    /// </summary>
    public partial class UserGestion : Window
    {
        private UserGestionViewModel ViewModel => (UserGestionViewModel)DataContext;

        public UserGestion()
        {
            InitializeComponent();
            // S'abonner à l'événement de réinitialisation du formulaire
            if (DataContext is UserGestionViewModel vm)
            {
                vm.FormCleared += OnFormCleared;
            }
        }
        /// <summary>
        /// Gestion du PasswordBox (pas de binding direct en XAML pour sécurité)
        /// </summary>
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is UserGestionViewModel viewModel)
            {
                var passwordBox = sender as PasswordBox;
                viewModel.MotPasse = passwordBox?.Password ?? string.Empty;

                viewModel.SaveUserCommand.NotifyCanExecuteChanged();
            }
        }
        /// <summary>
        /// Vider le PasswordBox quand le formulaire est réinitialisé
        /// </summary>
        private void OnFormCleared()
        {
            PasswordBox.Clear(); // Vider visuellement le PasswordBox
        }

        protected override void OnClosed(EventArgs e)
        {
            // "Nettoyer" l'événement
            if (DataContext is UserGestionViewModel vm)
            {
                vm.FormCleared -= OnFormCleared;
            }
            base.OnClosed(e);
        }

        /// <summary>
        /// Bouton Retour - Fermer cette fenêtre et retourner au menu
        /// </summary>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Demander confirmation si l'utilisateur est en train de modifier
            if (ViewModel.IsEditMode)
            {
                var result = MessageBox.Show(
                    "Vous êtes en train de modifier un utilisateur.\n\n" +
                    "Voulez-vous vraiment quitter sans enregistrer ?",
                    "Confirmation",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );

                if (result == MessageBoxResult.No)
                {
                    return; // Ne pas fermer
                }
            }

            // Fermer la fenêtre (retour au menu principal)
            this.Close();
        }

    }
}
