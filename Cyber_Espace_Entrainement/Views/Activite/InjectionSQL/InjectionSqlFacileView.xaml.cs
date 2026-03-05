using System.Windows;

namespace Cyber_Espace_Entrainement.Views.Activite
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

        private void HintButton_Click(object sender, RoutedEventArgs e)
        {
            HintOverlay.Visibility = Visibility.Visible;
        }

        private void HintClose_Click(object sender, RoutedEventArgs e)
        {
            HintOverlay.Visibility = Visibility.Collapsed;
        }
    }
}