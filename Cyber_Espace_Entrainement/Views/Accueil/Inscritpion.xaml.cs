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

namespace Cyber_Espace_Entrainement.Views
{
    /// <summary>
    /// Logique d'interaction pour Inscritpion.xaml
    /// </summary>
    public partial class Inscritpion : Window
    {
        public Inscritpion()
        {
            InitializeComponent();
        }

        private void Connexion_Click(object sender, RoutedEventArgs e)
        {
            var ConnexionWindow = new MainWindow();
            this.Hide();
            ConnexionWindow.ShowDialog();
        }

        //
        // BOUTON QUITTER
        //

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

