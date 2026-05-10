using Cyber_Espace_Entrainement.ViewModels.Admin;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cyber_Espace_Entrainement.Views.Admin
{
    /// <summary>
    /// Logique d'interaction pour UserEnum.xaml
    /// </summary>
    public partial class UserEnum : Window
    {
        private UserEnumViewModel viewModel => (UserEnumViewModel)DataContext;

        public UserEnum()
        {
            InitializeComponent();
        }

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
    }
}
