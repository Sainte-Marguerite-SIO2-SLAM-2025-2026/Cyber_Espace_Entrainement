using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Cyber_Espace_Entrainement.Views.Activite
{
    /// <summary>
    /// Logique d'interaction pour AccueilActivite.xaml
    /// </summary>
    public partial class AccueilActivite : Window
    {
        public AccueilActivite()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Bouton Retour - Fermer cette fenêtre et retourner au menu
        /// </summary>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Fermer la fenêtre (retour au menu principal)
            this.Close();
        }

        // BOUTON QUITTER
        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Voulez-vous vraiment quitter l'application ?",
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        #region Effets visuels des boutons

        /// <summary>
        /// Survol d'une "card" : léger zoom, modification de la bordure et renforcement de l'ombre.
        /// Les cards sont des Border dans le XAML et peuvent utiliser la propriété Tag pour stocker une couleur.
        /// </summary>
        private void Card_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Border card)
            {
                // Application d'un ScaleTransform pour l'effet de zoom
                card.RenderTransform = new ScaleTransform(1.05, 1.05);
                card.RenderTransformOrigin = new Point(0.5, 0.5);

                // Si la carte contient une couleur dans Tag, appliquer une bordure accentuée
                if (card.Tag is string couleur)
                {
                    var color = (Color)ColorConverter.ConvertFromString(couleur);
                    card.BorderBrush = new SolidColorBrush(color);
                    card.BorderThickness = new Thickness(2);
                }

                // Augmenter l'ombre portée si présente
                if (card.Effect is DropShadowEffect shadow)
                {
                    shadow.BlurRadius = 20;
                    shadow.ShadowDepth = 5;
                }
            }
        }

        /// <summary>
        /// Lorsque la souris quitte la card : restauration de l'apparence par défaut.
        /// </summary>
        private void Card_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Border card)
            {
                // Restauration de l'échelle
                card.RenderTransform = new ScaleTransform(1.0, 1.0);

                // Restauration de la bordure par défaut (gris clair)
                card.BorderBrush = new SolidColorBrush(Color.FromRgb(224, 224, 224));
                card.BorderThickness = new Thickness(1);

                // Restauration de l'ombre si présente
                if (card.Effect is DropShadowEffect shadow)
                {
                    shadow.BlurRadius = 10;
                    shadow.ShadowDepth = 2;
                }
            }
        }

        #endregion
    }
}
