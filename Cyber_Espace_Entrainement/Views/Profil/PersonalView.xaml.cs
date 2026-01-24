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
        private ProfilViewModel profil = new ProfilViewModel();
        public PersonalView()
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

        // faire un vos infos avec btn modif qui va sur stackpannel modifiervosinfos (sinon de base affiche pas modifiable) voir gestion user 
        // augmenter auteur et largeur de la view, meilleure disposition aussi a faire
        // au lieu de la zone mdp faire un boutton modif mdp en bas
    }
}
