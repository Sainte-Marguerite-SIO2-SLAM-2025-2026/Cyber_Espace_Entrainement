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
        public CaptchaFacileView()
        {
            InitializeComponent();
        }

        public void CaptchaCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // Logique lorsque la case est cochée
        }

        private void ButtonRetour_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
