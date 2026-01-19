using Cyber_Espace_Entrainement.ViewModels.Accueil;
using Cyber_Espace_Entrainement.Views.Accueil;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Cyber_Espace_Entrainement
{
    public partial class MainWindow : Window
    {
        #region Properties

        private ConnexionViewModel _viewModel;
        private Brush _defaultButtonBackground;

        #endregion

        #region Constructor

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new ConnexionViewModel();
            this.DataContext = _viewModel;

            // Sauvegarde de la couleur initiale du bouton (Bleu)
            _defaultButtonBackground = new SolidColorBrush(Color.FromRgb(21, 101, 192));

            InitializeSettings();
        }

        #endregion

        #region Settings & Init

        private void InitializeSettings()
        {
            // Configuration de l'info-bulle sur bouton désactivé
            ToolTipService.SetShowOnDisabled(btnConnecter, true);
            btnConnecter.IsEnabledChanged += BtnConnecter_IsEnabledChanged;

            UpdateValiderToolTip();
        }

        #endregion

        #region Event Handlers

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox pb = (PasswordBox)sender;

            // 1. Gestion du Placeholder
            var placeholder = (TextBlock)pb.Template.FindName("placeholder", pb);
            if (placeholder != null)
            {
                placeholder.Visibility = string.IsNullOrEmpty(pb.Password)
                    ? Visibility.Visible : Visibility.Collapsed;
            }

            // 2. Mise à jour du ViewModel
            if (_viewModel != null)
            {
                _viewModel.MotDePasse = pb.Password;
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private void BtnConnecter_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (btnConnecter.IsEnabled)
            {
                btnConnecter.Background = _defaultButtonBackground;
                btnConnecter.Opacity = 1.0;
                btnConnecter.Cursor = Cursors.Hand;
            }
            else
            {
                btnConnecter.Background = new SolidColorBrush(Color.FromRgb(210, 210, 210));
                btnConnecter.Opacity = 0.6;
                btnConnecter.Cursor = Cursors.No;
            }
            UpdateValiderToolTip();
        }

        private void BtnInscription_Click(object sender, RoutedEventArgs e)
        {
            var inscriptionWin = new Inscription();
            inscriptionWin.Show();
            this.Close();
        }

        #endregion

        #region Helper Methods

        private void UpdateValiderToolTip()
        {
            if (btnConnecter.IsEnabled)
            {
                btnConnecter.ToolTip = "Cliquer pour vous connecter.";
            }
            else
            {
                btnConnecter.ToolTip = "Veuillez saisir votre login et votre mot de passe.";
            }
        }

        #endregion
    }
}