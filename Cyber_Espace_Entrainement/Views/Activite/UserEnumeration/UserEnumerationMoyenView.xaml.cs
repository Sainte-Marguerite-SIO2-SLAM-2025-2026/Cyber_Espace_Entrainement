using Cyber_Espace_Entrainement.Services;
using Cyber_Espace_Entrainement.ViewModels.Activite;
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

namespace Cyber_Espace_Entrainement.Views.Activite
{
    /// <summary>
    /// Logique d'interaction pour UserEnumerationMoyenView.xaml
    /// </summary>
    public partial class UserEnumerationMoyenView : Window
    {
        // Couleurs utilisées pour l'état normal et survol du bouton Quitter.
        private readonly SolidColorBrush _defaultQuitBackground;
        private readonly SolidColorBrush _hoverQuitBackground;
        public UserEnumerationMoyenView()
        {
            InitializeComponent();
            _defaultQuitBackground = (SolidColorBrush)Application.Current.FindResource("ErrorRedBrush");
            _hoverQuitBackground = (SolidColorBrush)Application.Current.FindResource("ErrorRedDarkBrush");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Regles(); // s’exécute dès l’arrivée sur la page
        }

        /// <summary>
        /// Fermeture de la fenêtre (retour au menu).
        /// </summary>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Clic sur le bouton "Quitter" : confirme puis ferme l'application si l'utilisateur confirme.
        /// </summary>
        private void ButtonQuitter_Click(object sender, RoutedEventArgs e)
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

        /// <summary>
        /// affiche une MessageBox avec les règles du jeu lorsque l'utilisateur clique sur le bouton "Règles".
        /// </summary>
        private void Regles_Click(object sender, RoutedEventArgs e)
        {
            Regles();
        }

        private void Regles()
        {
            MessageBoxService.ShowInformation(
    "1) Remplissez le formulaire affiché (connexion, inscription ou reset de mot de passe).\n" +
    "2) Cliquez sur Valider pour confirmer votre saisie.\n" +
    "3) Après validation, un message apparait, répondez par Oui ou Non à la question posée.\n" +
    "4) Votre réponse influencera votre score.\n" +
    "5) Passez automatiquement au niveau suivant après chaque réponse.\n" +
    "6) À la fin des activités, votre score final sera affiché.\n\n" +
    "Bonne chance !",
    "RÈGLES DU JEU :"
);

        }

        /// <summary>
        /// MouseEnter du bouton Quitter : changer le background
        /// </summary>
        private void ButtonQuitter_MouseEnter(object sender, MouseEventArgs e)
        {
            BtnQuitter.Background = _hoverQuitBackground;
        }

        /// <summary>
        /// MouseLeave du bouton Quitter : restauration du background.
        /// </summary>
        private void ButtonQuitter_MouseLeave(object sender, MouseEventArgs e)
        {
            BtnQuitter.Background = _defaultQuitBackground;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is UserEnumerationMoyenViewModel vm)
                vm.MotDePasse = ((PasswordBox)sender).Password;
        }

        private void PasswordBoxConfirm_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is UserEnumerationMoyenViewModel vm)
                vm.ConfirmationMotDePasse = ((PasswordBox)sender).Password;
        }

        private void Recommencer_Click(object sender, RoutedEventArgs e)
        {
            PasswordBox.Clear();
            PasswordBoxCreation.Clear();
            PasswordBoxCreationConfirm.Clear();
        }
    }
}
