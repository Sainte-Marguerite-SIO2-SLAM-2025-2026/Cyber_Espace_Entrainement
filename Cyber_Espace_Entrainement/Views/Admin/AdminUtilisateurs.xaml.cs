using Cyber_Espace_Entrainement.ViewModels.Admin;
using System.Windows;
using System.Windows.Controls;

namespace Cyber_Espace_Entrainement.Views.Admin
{
    /// <summary>
    /// Code-behind de la page AdminUtilisateursView.
    /// 
    /// Responsabilités :
    /// - Gestion du bouton Retour avec confirmation si édition en cours.
    /// - Synchronisation de la PasswordBox avec le ViewModel (impossible en binding pur WPF).
    /// - Nettoyage de la PasswordBox quand le formulaire est réinitialisé (event FormCleared).
    /// - Formatage des colonnes auto-générées du DataGrid.
    /// </summary>
    public partial class AdminUtilisateurs : Window
    {
        /// <summary>Raccourci typé vers le ViewModel pour éviter les casts répétés.</summary>
        private AdminUtilisateursViewModel ViewModel => (AdminUtilisateursViewModel)DataContext;

        /// <param name="viewModel">ViewModel injecté depuis la navigation.</param>
        public AdminUtilisateurs(AdminUtilisateursViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            // On s'abonne à l'événement FormCleared pour vider la PasswordBox
            // quand le formulaire est réinitialisé (après sauvegarde, suppression, annulation)
            viewModel.FormCleared += OnFormCleared;
        }

        /// <summary>
        /// Appelé quand le ViewModel demande la réinitialisation du formulaire.
        /// Vide la PasswordBox (impossible via binding, donc géré ici).
        /// </summary>
        private void OnFormCleared()
        {
            PasswordBoxField.Clear();
        }

        /// <summary>
        /// Appelé à chaque frappe dans la PasswordBox.
        /// Synchronise la valeur vers la propriété MotPasse du ViewModel.
        /// 
        /// Note : WPF ne permet pas de binder PasswordBox.Password directement
        /// (par sécurité, le mot de passe ne circule pas dans les bindings).
        /// On utilise donc cet événement comme passerelle.
        /// </summary>
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is AdminUtilisateursViewModel vm)
                vm.MotPasse = PasswordBoxField.Password;
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
                    "Vous êtes en train de modifier un utilisateur.\n\nVoulez-vous quitter sans enregistrer ?",
                    "Modifications non sauvegardées",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );

                if (result == MessageBoxResult.No) return;
            }

            this.Close();
        }

        /// <summary>
        /// Événement déclenché pour chaque colonne auto-générée du DataGrid.
        /// Applique une largeur maximale et la troncature du texte avec "…".
        /// </summary>
        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column is DataGridTextColumn textCol)
            {
                textCol.MaxWidth = 200;

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
