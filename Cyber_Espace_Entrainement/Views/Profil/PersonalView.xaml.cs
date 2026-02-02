using Cyber_Espace_Entrainement.Services;
using Cyber_Espace_Entrainement.ViewModels.Profil;
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

namespace Cyber_Espace_Entrainement.Views.Profil
{
    /// <summary>
    /// Logique d'interaction pour PersonalView.xaml
    /// </summary>
    public partial class PersonalView : Window
    {
        private ProfilViewModel viewModel => (ProfilViewModel)DataContext;
        public PersonalView()
        {
            InitializeComponent();
            var vm = (ProfilViewModel)DataContext;
            vm.PasswordChangedSuccessfully += OnPasswordChangedSuccessfully;
        }

        private void OldPasswordChanged(object sender, RoutedEventArgs e)
            => ((ProfilViewModel)DataContext).OldPassword = ((PasswordBox)sender).Password;
        private void NewPasswordChanged(object sender, RoutedEventArgs e)
            => ((ProfilViewModel)DataContext).NewPassword = ((PasswordBox)sender).Password;
        private void ConfirmPasswordChanged(object sender, RoutedEventArgs e)
            => ((ProfilViewModel)DataContext).ConfirmPassword = ((PasswordBox)sender).Password;

        /// <summary>
        /// Permet de retourner à la page d'accueil
        /// </summary>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Demander confirmation si l'utilisateur est en train de modifier
            if (viewModel.IsEditMode)
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

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBoxService.ShowQuestion(
                "Voulez-vous vraiment quitter l'application ?",
                "Confirmation"
            );

            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

            private void OnPasswordChangedSuccessfully()
        {
            OldPasswordBox.Password = "";
            NewPasswordBox.Password = "";
            ConfirmPasswordBox.Password = "";
        }
        
        private void RetourMdp_Click(object sender, RoutedEventArgs e)
        {
            OldPasswordBox.Password = "";
            NewPasswordBox.Password = "";
            ConfirmPasswordBox.Password = "";
        }
        // adapter profil au 1920x1080
        // faire l'icone pour voir mdp
    }
}
