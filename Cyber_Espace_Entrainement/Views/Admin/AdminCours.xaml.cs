using Cyber_Espace_Entrainement.ViewModels.Admin;
using System.Windows;
using System.Windows.Controls;

namespace Cyber_Espace_Entrainement.Views.Admin
{
    /// <summary>
    /// Code-behind de la page AdminCoursView.
    /// Responsabilités limitées au strict minimum MVVM :
    /// - Gestion du bouton Retour avec confirmation si édition en cours.
    /// - Formatage des colonnes auto-générées du DataGrid (largeur + troncature).
    /// </summary>
    public partial class AdminCours : Window
    {
        /// <summary>Raccourci typé vers le ViewModel pour éviter les casts répétés.</summary>
        private AdminCoursViewModel ViewModel => (AdminCoursViewModel)DataContext;

        /// <param name="viewModel">ViewModel injecté depuis la navigation.</param>
        public AdminCours(AdminCoursViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        /// <summary>
        /// Gère le clic sur le bouton Retour.
        /// Si une modification est en cours, demande confirmation avant de fermer.
        /// </summary>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel?.IsEditMode == true)
            {
                var result = MessageBox.Show(
                    "Vous êtes en train de modifier un cours.\n\nVoulez-vous quitter sans enregistrer ?",
                    "Modifications non sauvegardées",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );

                if (result == MessageBoxResult.No) return;
            }

            this.Close();
        }

        /// <summary>
        /// Événement déclenché par le DataGrid pour chaque colonne auto-générée.
        /// Applique une largeur maximale et la troncature du texte avec "…".
        /// </summary>
        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column is DataGridTextColumn textCol)
            {
                // Largeur maximale pour éviter des colonnes trop larges
                textCol.MaxWidth = 220;

                // Troncature du texte avec ellipsis si le contenu dépasse
                textCol.ElementStyle = new Style(typeof(TextBlock))
                {
                    Setters =
                    {
                        new Setter(TextBlock.TextTrimmingProperty, TextTrimming.CharacterEllipsis),
                        new Setter(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center)
                    }
                };
            }
        }
    }
}
