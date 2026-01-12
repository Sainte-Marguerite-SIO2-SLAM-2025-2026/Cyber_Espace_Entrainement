using Cyber_Espace_Entrainement.Views;
using Cyber_Espace_Entrainement.Views.Accueil;
using Cyber_Espace_Entrainement.Views.Tests;
using Cyber_Espace_Entrainement.Views.Users;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cyber_Espace_Entrainement
{
        /// <summary>
        /// Page d'accueil avec menu de navigation c
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
                accueilWindow.ShowDialog();
            }

        }
    }