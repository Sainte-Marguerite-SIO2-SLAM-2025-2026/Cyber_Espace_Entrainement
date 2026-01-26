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

namespace Cyber_Espace_Entrainement.Views.Profil
{
    /// <summary>
    /// Logique d'interaction pour PersonalView.xaml
    /// </summary>
    public partial class PersonalView : Window
    {
        public PersonalView()
        {
            InitializeComponent();
            PasswordBox.Password = "monmotdepasse";
        }

        /// <summary>
        /// Permet de retourner à la page d'accueil
        /// </summary>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
