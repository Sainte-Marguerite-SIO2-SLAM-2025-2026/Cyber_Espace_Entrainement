using Cyber_Espace_Entrainement.Services;
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

namespace Cyber_Espace_Entrainement.Views.Activite.Captcha
{
    /// <summary>
    /// Logique d'interaction pour CaptchaFacileView.xaml
    /// </summary>
    public partial class CaptchaFacileView : Window
    {
        private readonly SolidColorBrush _defaultQuitBackground;
        private readonly SolidColorBrush _hoverQuitBackground;

        public CaptchaFacileView()
        {
            InitializeComponent();

            _defaultQuitBackground = (SolidColorBrush)Application.Current.FindResource("ErrorRedBrush");
            _hoverQuitBackground = (SolidColorBrush)Application.Current.FindResource("ErrorRedDarkBrush");
        }

        public void CaptchaCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // Logique lorsque la case est cochée
        }

        private void ButtonRetour_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        #region Bouton Quitter

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

        #endregion
    }
}
