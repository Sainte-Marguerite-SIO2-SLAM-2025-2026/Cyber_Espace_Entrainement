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
using Cyber_Espace_Entrainement.ViewModels.Activite;

namespace Cyber_Espace_Entrainement.Views.Activite
{
    public partial class PhishingFacileView : Window
    {
        public PhishingFacileView()
        {
            InitializeComponent();
            
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
