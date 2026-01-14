using Cyber_Espace_Entrainement.Views;
using Cyber_Espace_Entrainement.Views.Accueil;
using Cyber_Espace_Entrainement.Views.Tests;
using Cyber_Espace_Entrainement.Views.Users;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using Color = System.Windows.Media.Color;
using ColorConverter = System.Windows.Media.ColorConverter;

namespace Cyber_Espace_Entrainement
{
    /// <summary>
    /// Page d'accueil avec menu de navigation
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Valider_Click(object sender, RoutedEventArgs e)
        {
            var accueilWindow = new Accueil();
            this.Hide();
            accueilWindow.ShowDialog();
        }

        private void Inscription_Click(object sender, RoutedEventArgs e)
        {
            var inscriptionlWindow = new Inscription();
            this.Hide();
            inscriptionlWindow.ShowDialog();
        }

    }
}