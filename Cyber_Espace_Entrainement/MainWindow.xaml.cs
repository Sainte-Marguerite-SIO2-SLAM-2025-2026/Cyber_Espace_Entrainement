using Cyber_Espace_Entrainement.ViewModels.Accueil;
using Cyber_Espace_Entrainement.Views.Accueil;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Cyber_Espace_Entrainement
{
    /// <summary>
    /// Code-behind pour la fenêtre principale (fenêtre de connexion).
    /// Contient la logique spécifique à l'interface utilisateur (gestion des événements visuels,
    /// synchronisation du PasswordBox vers le ViewModel, navigation vers l'inscription).
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Properties

        /// <summary>
        /// Référence au ViewModel gérant la logique de connexion.
        /// Assigné et exposé via le DataContext pour le binding depuis le XAML.
        /// </summary>
        private ConnexionViewModel _viewModel;
        
        // Brushes chargés depuis les ressources (theme) : évitent les accès répétés aux ressources
        // et garantissent une apparence cohérente avec le thème de l'application.
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

        /// <summary>
        /// Constructeur : initialise le composant, le ViewModel et attache les handlers UI.
        /// - Initialise le DataContext avec une instance de `ConnexionViewModel`.
        /// - Charge les brushes depuis les ressources (ModernTheme / Colors).
        /// - Configure ToolTips et attache les gestionnaires d'événements pour l'UI.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // Crée et associe le ViewModel à la fenêtre (DataContext pour le binding)
            _viewModel = new ConnexionViewModel();
            this.DataContext = _viewModel;

            // Chargement des brushes/thèmes depuis les ressources de l'application
            _defaultTextBoxBorderBrush = (SolidColorBrush)Application.Current.FindResource("BorderDefaultBrush");
            _hoverTextBoxBorderBrush = (SolidColorBrush)Application.Current.FindResource("BorderHoverBrush");
            _focusTextBoxBorderBrush = (SolidColorBrush)Application.Current.FindResource("BorderFocusBrush");

            _defaultButtonBackground = (SolidColorBrush)Application.Current.FindResource("PrimaryBlueBrush");
            _hoverButtonBackground = (SolidColorBrush)Application.Current.FindResource("PrimaryBlueDarkBrush");

            _defaultSecondaryBackground = (SolidColorBrush)Application.Current.FindResource("TransparentBrush");
            _hoverSecondaryBackground = (SolidColorBrush)Application.Current.FindResource("PrimaryBlueLightBrush");

            _defaultQuitBackground = (SolidColorBrush)Application.Current.FindResource("ErrorRedBrush");
            _hoverQuitBackground = (SolidColorBrush)Application.Current.FindResource("ErrorRedDarkBrush"); 

            // Initialisation des comportements et attachement des événements
            InitializeSettings();
            InitializeEventHandlers();
        }

        #endregion

        #region Settings & Init

        /// <summary>
        /// Configure les réglages initiaux :
        /// - Autorise l'affichage des tooltips quand le bouton est désactivé.
        /// - Attache l'événement pour détecter les changements d'état du bouton de connexion.
        /// </summary>
        private void InitializeSettings()
        {
            // Afficher les tooltips même si le bouton est disabled (expérience utilisateur)
            ToolTipService.SetShowOnDisabled(btnConnecter, true);
            btnConnecter.IsEnabledChanged += BtnConnecter_IsEnabledChanged;

            UpdateValiderToolTip();
        }

        /// <summary>
        /// Attache centralement tous les gestionnaires d'événements UI (survol, focus, etc.).
        /// Centraliser l'attachement permet de maintenir plus facilement les handlers.
        /// </summary>
        private void InitializeEventHandlers()
        {
            // TextBox - Login : gestion du survol / focus pour changer la bordure
            tbxLogin.MouseEnter += TextBox_MouseEnter;
            tbxLogin.MouseLeave += TextBox_MouseLeave;
            tbxLogin.GotFocus += TextBox_GotFocus;
            tbxLogin.LostFocus += TextBox_LostFocus;

            // PasswordBox - Mot de passe : events similaires + synchronisation du mot de passe vers le VM
            pbxMotDePasse.MouseEnter += PasswordBox_MouseEnter;
            pbxMotDePasse.MouseLeave += PasswordBox_MouseLeave;
            pbxMotDePasse.GotFocus += PasswordBox_GotFocus;
            pbxMotDePasse.LostFocus += PasswordBox_LostFocus;

            // Bouton Valider : hover visuel + gestion IsEnabledChanged supplémentaire
            btnConnecter.MouseEnter += btnConnecter_MouseEnter;
            btnConnecter.MouseLeave += btnConnecter_MouseLeave;
            btnConnecter.IsEnabledChanged += btnConnecter_IsEnabledChanged;

            // Initialiser le tooltip selon l'état initial
            UpdateValiderToolTip();

            // Bouton Quitter : effet visuel hover
            btnQuitter.MouseEnter += BtnQuitter_MouseEnter;
            btnQuitter.MouseLeave += BtnQuitter_MouseLeave;
        }

        #endregion

        #region Event Handlers

        #region TextBox Events

        // Handlers de TextBox : appliquent des changements visuels (bordure) lors du survol et du focus.
        // Ils sont volontairement UI-only pour séparer la présentation de la logique métier.

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

        // Les PasswordBox n'exposent pas un binding Text simple pour des raisons de sécurité,
        // ici on synchronise manuellement dans PasswordBox_PasswordChanged.
        // Remarque : éviter de stocker des mots de passe en clair en dehors de la durée nécessaire.

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

        /// <summary>
        /// Gestionnaire PasswordChanged :
        /// - met à jour la visibilité du placeholder dans le template du PasswordBox,
        /// - synchronise la valeur côté ViewModel (ConnexionViewModel.MotDePasse).
        /// </summary>
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox pb = (PasswordBox)sender;

            // Recherche du TextBlock placeholder dans le template pour l'afficher/cacher
            var placeholder = (TextBlock)pb.Template.FindName("placeholder", pb);

            if (placeholder != null)
            {
                // Si le mot de passe est vide, on affiche le placeholder, sinon on le cache
                placeholder.Visibility = string.IsNullOrEmpty(pb.Password)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }

            // Synchroniser le mot de passe dans le ViewModel (utilisé pour la validation/connexion)
            if (_viewModel != null)
            {
                _viewModel.MotDePasse = pbxMotDePasse.Password;
            }
        }

        #endregion

        #region Button Events

        // Handlers visuels pour les boutons (hover, état désactivé, etc.)

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

        // Met à jour le tooltip quand le binding IsEnabled change (lié au ViewModel)
        private void btnConnecter_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateValiderToolTip();
        }

        #endregion

        // Ce handler gère l'aspect visuel et le curseur lorsque le bouton est activé ou non.
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
                // Apparence "désactivée"
                btnConnecter.Background = new SolidColorBrush(Color.FromRgb(210, 210, 210));
                btnConnecter.Opacity = 0.6;
                btnConnecter.Cursor = Cursors.No;
            }
            UpdateValiderToolTip();
        }

        /// <summary>
        /// Ouvre la fenêtre d'inscription et ferme la fenêtre de connexion courante.
        /// Navigation simple : ferme la fenêtre actuelle et affiche la suivante.
        /// </summary>
        private void BtnInscription_Click(object sender, RoutedEventArgs e)
        {
            var inscriptionWin = new Inscription();
            this.Close();
            inscriptionWin.Show();
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Met à jour le tooltip du bouton de connexion selon s'il est activé ou non.
        /// Fournit un feedback minimal à l'utilisateur sur l'état requis des champs.
        /// </summary>
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