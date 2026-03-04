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

namespace Cyber_Espace_Entrainement.Views.Activite.InjectionSQL
{
    /// <summary>
    /// Logique d'interaction pour InjectionSQLFacileView.xaml
    /// </summary>
    public partial class InjectionSQLFacileView : Window
    {
        public InjectionSQLFacileView()
        {
            InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
