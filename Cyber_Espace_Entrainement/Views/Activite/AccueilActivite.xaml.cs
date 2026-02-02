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
    }
}
