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
        
        // Récupération des couleurs depuis les ressources
        private readonly SolidColorBrush _defaultTextBoxBorderBrush;
        private readonly SolidColorBrush _hoverTextBoxBorderBrush;
        private readonly SolidColorBrush _focusTextBoxBorderBrush;

        private readonly SolidColorBrush _defaultButtonBackground;
        private readonly SolidColorBrush _hoverButtonBackground;

        private readonly SolidColorBrush _defaultSecondaryBackground;
        private readonly SolidColorBrush _hoverSecondaryBackground;

        private readonly SolidColorBrush _defaultQuitBackground;
        private readonly SolidColorBrush _hoverQuitBackground;


        #endregion

        #region Constructor

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new ConnexionViewModel();
            this.DataContext = _viewModel;

            // Chargement des couleurs depuis les ressources de l'application
            _defaultTextBoxBorderBrush = (SolidColorBrush)Application.Current.FindResource("BorderDefaultBrush");
            _hoverTextBoxBorderBrush = (SolidColorBrush)Application.Current.FindResource("BorderHoverBrush");
            _focusTextBoxBorderBrush = (SolidColorBrush)Application.Current.FindResource("BorderFocusBrush");

            _defaultButtonBackground = (SolidColorBrush)Application.Current.FindResource("PrimaryBlueBrush");
            _hoverButtonBackground = (SolidColorBrush)Application.Current.FindResource("PrimaryBlueDarkBrush");

            _defaultSecondaryBackground = (SolidColorBrush)Application.Current.FindResource("TransparentBrush");
            _hoverSecondaryBackground = (SolidColorBrush)Application.Current.FindResource("PrimaryBlueLightBrush");

            _defaultQuitBackground = (SolidColorBrush)Application.Current.FindResource("ErrorRedBrush");
            _hoverQuitBackground = (SolidColorBrush)Application.Current.FindResource("ErrorRedDarkBrush"); 

            InitializeSettings();
            InitializeEventHandlers();
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

        private void InitializeEventHandlers()
        {
            // TextBox - Login
            tbxLogin.MouseEnter += TextBox_MouseEnter;
            tbxLogin.MouseLeave += TextBox_MouseLeave;
            tbxLogin.GotFocus += TextBox_GotFocus;
            tbxLogin.LostFocus += TextBox_LostFocus;

            // PasswordBox - Mot de passe
            pbxMotDePasse.MouseEnter += PasswordBox_MouseEnter;
            pbxMotDePasse.MouseLeave += PasswordBox_MouseLeave;
            pbxMotDePasse.GotFocus += PasswordBox_GotFocus;
            pbxMotDePasse.LostFocus += PasswordBox_LostFocus;

            // Bouton Valider
            btnConnecter.MouseEnter += btnConnecter_MouseEnter;
            btnConnecter.MouseLeave += btnConnecter_MouseLeave;
            btnConnecter.IsEnabledChanged += btnConnecter_IsEnabledChanged;

            // Initialiser le premier message
            UpdateValiderToolTip();

            // Bouton Quitter
            btnQuitter.MouseEnter += BtnQuitter_MouseEnter;
            btnQuitter.MouseLeave += BtnQuitter_MouseLeave;
        }

        #endregion

        #region Event Handlers

        #region TextBox Events

        private void TextBox_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is TextBox textBox && !textBox.IsFocused)
            {
                textBox.BorderBrush = _hoverTextBoxBorderBrush;
            }
        }

        private void TextBox_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is TextBox textBox && !textBox.IsFocused)
            {
                textBox.BorderBrush = _defaultTextBoxBorderBrush;
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.BorderBrush = _focusTextBoxBorderBrush;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.BorderBrush = _defaultTextBoxBorderBrush;
            }
        }

        #endregion

        #region PasswordBox Events

        private void PasswordBox_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is PasswordBox passwordBox && !passwordBox.IsFocused)
            {
                passwordBox.BorderBrush = _hoverTextBoxBorderBrush;
            }
        }

        private void PasswordBox_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is PasswordBox passwordBox && !passwordBox.IsFocused)
            {
                passwordBox.BorderBrush = _defaultTextBoxBorderBrush;
            }
        }

        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                passwordBox.BorderBrush = _focusTextBoxBorderBrush;
            }
        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                passwordBox.BorderBrush = _defaultTextBoxBorderBrush;
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox pb = (PasswordBox)sender;

            // On va chercher le TextBlock "placeholder" dans le template
            var placeholder = (TextBlock)pb.Template.FindName("placeholder", pb);

            if (placeholder != null)
            {
                // Si le mot de passe est vide, on affiche le placeholder, sinon on le cache
                placeholder.Visibility = string.IsNullOrEmpty(pb.Password)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }

            if (_viewModel != null)
            {
                _viewModel.MotDePasse = pbxMotDePasse.Password;
            }
        }

        #endregion

        #region Button Events

        private void btnConnecter_MouseEnter(object sender, MouseEventArgs e)
        {
            btnConnecter.Background = _hoverButtonBackground;
        }

        private void btnConnecter_MouseLeave(object sender, MouseEventArgs e)
        {
            btnConnecter.Background = _defaultButtonBackground;
        }


        private void BtnQuitter_MouseEnter(object sender, MouseEventArgs e)
        {
            btnQuitter.Background = _hoverQuitBackground;
        }

        private void BtnQuitter_MouseLeave(object sender, MouseEventArgs e)
        {
            btnQuitter.Background = _defaultQuitBackground;
        }

        private void btnConnecter_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateValiderToolTip();
        }

        #endregion

       

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
            this.Close();
            inscriptionWin.Show();
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