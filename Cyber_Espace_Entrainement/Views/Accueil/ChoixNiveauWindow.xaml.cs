using Cyber_Espace_Entrainement.Models;
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

namespace Cyber_Espace_Entrainement.Views.Accueil
{
    /// <summary>
    /// Logique d'interaction pour ChoixNiveauWindow.xaml
    /// </summary>
    public partial class ChoixNiveauWindow : Window
    {
        public ChoixNiveauWindow(List<Activites> activites)
        {
            InitializeComponent();
            DataContext = new ChoixNiveauViewModel(activites);
        }

        /// <summary>
        /// Bouton Retour - Fermer cette fenêtre et retourner à l'accueil des tests
        /// </summary>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Fermer la fenêtre (retour au menu principal)
            this.Close();
        }
    }
}
