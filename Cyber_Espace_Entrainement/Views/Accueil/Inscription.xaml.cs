using Cyber_Espace_Entrainement.ViewModels;
using Cyber_Espace_Entrainement.ViewModels.Accueil;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Cyber_Espace_Entrainement.Views.Accueil
{
    /// <summary>
    /// Code-behind pour la fenêtre d'inscription.
    /// Contient la logique UI spécifique (gestion des événements visuels, placeholder du PasswordBox,
    /// tooltips et navigation).
    /// </summary>
    public partial class Inscription : Window
    {
        // Référence au ViewModel associé (cast depuis DataContext).
        private InscriptionViewModel _viewModel;

        // Brushes chargés depuis les ressources pour conserver le thème et éviter les recherches répétées.
        private readonly SolidColorBrush _defaultTextBoxBorderBrush;
        private readonly SolidColorBrush _hoverTextBoxBorderBrush;
        private readonly SolidColorBrush _focusTextBoxBorderBrush;

        private readonly SolidColorBrush _defaultButtonBackground;
        private readonly SolidColorBrush _hoverButtonBackground;

        private readonly SolidColorBrush _defaultSecondaryBackground;
        private readonly SolidColorBrush _hoverSecondaryBackground;

        private readonly SolidColorBrush _defaultQuitBackground;
        private readonly SolidColorBrush _hoverQuitBackground;

        /// <summary>
        /// Constructeur : initialise les composants WPF, récupère les ressources de thème,
        /// configure le DataContext (viewmodel) et attache les gestionnaires d'événements UI.
        /// </summary>
        public Inscription()
        {
            InitializeComponent();

            // Récupération du ViewModel à partir du DataContext (lié dans le XAML ou en code).
            _viewModel = this.DataContext as InscriptionViewModel;

            // Chargement des brushes/thèmes depuis les ressources de l'application.
            _defaultTextBoxBorderBrush = (SolidColorBrush)Application.Current.FindResource("BorderDefaultBrush");
            _hoverTextBoxBorderBrush = (SolidColorBrush)Application.Current.FindResource("BorderHoverBrush");
            _focusTextBoxBorderBrush = (SolidColorBrush)Application.Current.FindResource("BorderFocusBrush");

            _defaultButtonBackground = (SolidColorBrush)Application.Current.FindResource("PrimaryBlueBrush");
            _hoverButtonBackground = (SolidColorBrush)Application.Current.FindResource("PrimaryBlueDarkBrush");

            _defaultSecondaryBackground = (SolidColorBrush)Application.Current.FindResource("TransparentBrush");
            _hoverSecondaryBackground = (SolidColorBrush)Application.Current.FindResource("PrimaryBlueLightBrush");

            _defaultQuitBackground = (SolidColorBrush)Application.Current.FindResource("ErrorRedBrush");
            _hoverQuitBackground = (SolidColorBrush)Application.Current.FindResource("ErrorRedDarkBrush");

            // Autoriser l'affichage des tooltips même lorsque le bouton est désactivé.
            ToolTipService.SetShowOnDisabled(btnValider, true);

            // Attacher tous les gestionnaires d'événements pour l'UI.
            InitializeEventHandlers();
        }

        /// <summary>
        /// Attache les événements (MouseEnter/Leave, GotFocus/LostFocus, etc.) pour les contrôles.
        /// Centraliser l'attachement facilite la lecture et la maintenance.
        /// </summary>
        private void InitializeEventHandlers()
        {
            // Login (TextBox)
            tbxLogin.MouseEnter += TextBox_MouseEnter;
            tbxLogin.MouseLeave += TextBox_MouseLeave;
            tbxLogin.GotFocus += TextBox_GotFocus;
            tbxLogin.LostFocus += TextBox_LostFocus;

            // Mot de passe (PasswordBox) - events spécifiques pour placeholder et synchronisation ViewModel
            pbxMotDePasse.MouseEnter += PasswordBox_MouseEnter;
            pbxMotDePasse.MouseLeave += PasswordBox_MouseLeave;
            pbxMotDePasse.GotFocus += PasswordBox_GotFocus;
            pbxMotDePasse.LostFocus += PasswordBox_LostFocus;

            // Nom
            tbxNom.MouseEnter += TextBox_MouseEnter;
            tbxNom.MouseLeave += TextBox_MouseLeave;
            tbxNom.GotFocus += TextBox_GotFocus;
            tbxNom.LostFocus += TextBox_LostFocus;

            // Prénom
            tbxPrenom.MouseEnter += TextBox_MouseEnter;
            tbxPrenom.MouseLeave += TextBox_MouseLeave;
            tbxPrenom.GotFocus += TextBox_GotFocus;
            tbxPrenom.LostFocus += TextBox_LostFocus;

            // Section (ComboBox)
            cbxSection.MouseEnter += ComboBox_MouseEnter;
            cbxSection.MouseLeave += ComboBox_MouseLeave;
            cbxSection.GotFocus += ComboBox_GotFocus;
            cbxSection.LostFocus += ComboBox_LostFocus;

            // Mail
            tbxMail.MouseEnter += TextBox_MouseEnter;
            tbxMail.MouseLeave += TextBox_MouseLeave;
            tbxMail.GotFocus += TextBox_GotFocus;
            tbxMail.LostFocus += TextBox_LostFocus;

            // Bouton Valider - visuels et changement d'état
            btnValider.MouseEnter += BtnValider_MouseEnter;
            btnValider.MouseLeave += BtnValider_MouseLeave;
            btnValider.IsEnabledChanged += BtnValider_IsEnabledChanged;

            // Bouton Connexion (lien vers la fenêtre de connexion)
            btnConnexion.MouseEnter += BtnConnexion_MouseEnter;
            btnConnexion.MouseLeave += BtnConnexion_MouseLeave;

            // Initialiser le tooltip du bouton Valider selon son état initial
            UpdateValiderToolTip();

            // Bouton Quitter (visuels)
            btnQuitter.MouseEnter += BtnQuitter_MouseEnter;
            btnQuitter.MouseLeave += BtnQuitter_MouseLeave;
        }

        #region TextBox Events

        // Les handlers ci-dessous appliquent des changements visuels (bordure) lors du survol
        // et du focus. Ils n'affectent pas la logique métier — c'est volontaire : UI-only.

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

        // Les PasswordBox n'exposent pas la propriété Text : on gère le placeholder via le template
        // et on synchronise la valeur vers le ViewModel dans PasswordBox_PasswordChanged.

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
        /// Gestionnaire de l'événement PasswordChanged :
        /// - met à jour la visibilité du placeholder contenu dans le template du PasswordBox,
        /// - synchronise la valeur vers le ViewModel (propriété MotDePasse).
        /// Maybe TO DO : pour la sécurité, évitez de persister le mot de passe en clair ; ici on le conserve temporairement
        /// dans le ViewModel pour la validation/inscription.
        /// </summary>
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox pb = (PasswordBox)sender;

            // Cherche le TextBlock nommé "placeholder" dans le template du PasswordBox.
            var placeholder = (TextBlock)pb.Template.FindName("placeholder", pb);

            if (placeholder != null)
            {
                // Affiche le placeholder si le mot de passe est vide, sinon le cache.
                placeholder.Visibility = string.IsNullOrEmpty(pb.Password)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }

            // Synchroniser la valeur du PasswordBox vers le ViewModel
            if (_viewModel != null)
            {
                _viewModel.MotDePasse = pbxMotDePasse.Password;
            }
        }

        #endregion

        #region ComboBox Events

        // Comportement similaire aux TextBox : bordure changeante selon survol / focus.

        private void ComboBox_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is ComboBox comboBox && !comboBox.IsFocused)
            {
                comboBox.BorderBrush = _hoverTextBoxBorderBrush;
            }
        }

        private void ComboBox_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is ComboBox comboBox && !comboBox.IsFocused)
            {
                comboBox.BorderBrush = _defaultTextBoxBorderBrush;
            }
        }

        private void ComboBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                comboBox.BorderBrush = _focusTextBoxBorderBrush;
            }
        }

        private void ComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                comboBox.BorderBrush = _defaultTextBoxBorderBrush;
            }
        }

        #endregion

        #region Button Events

        // Handlers pour changement visuel des boutons (hover) et mise à jour du tooltip.

        private void BtnValider_MouseEnter(object sender, MouseEventArgs e)
        {
            btnValider.Background = _hoverButtonBackground;
        }

        private void BtnValider_MouseLeave(object sender, MouseEventArgs e)
        {
            btnValider.Background = _defaultButtonBackground;
        }

        private void BtnConnexion_MouseEnter(object sender, MouseEventArgs e)
        {
            btnConnexion.Background = _hoverSecondaryBackground;
        }

        private void BtnConnexion_MouseLeave(object sender, MouseEventArgs e)
        {
            btnConnexion.Background = _defaultSecondaryBackground;
        }

        private void BtnQuitter_MouseEnter(object sender, MouseEventArgs e)
        {
            btnQuitter.Background = _hoverQuitBackground;
        }

        private void BtnQuitter_MouseLeave(object sender, MouseEventArgs e)
        {
            btnQuitter.Background = _defaultQuitBackground;
        }

        /// <summary>
        /// Lorsque l'état IsEnabled du bouton Valider change, on met à jour son tooltip
        /// pour donner un retour utilisateur sur les champs requis.
        /// </summary>
        private void BtnValider_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateValiderToolTip();
        }

        /// <summary>
        /// Met à jour le tooltip du bouton Valider en fonction de s'il est activé ou non.
        /// Fournit les règles minimales attendues quand le bouton est désactivé.
        /// </summary>
        private void UpdateValiderToolTip()
        {
            if (btnValider.IsEnabled)
            {
                btnValider.ToolTip = "Tout est correct. Cliquez pour valider l'inscription.";
            }
            else
            {
                btnValider.ToolTip = "Veuillez remplir correctement tous les champs :\n" +
                                     "- Login (3 car. min)\n" +
                                     "- Mot de passe (6 car. min)\n" +
                                     "- Nom et Prénom\n" +
                                     "- Section sélectionnée\n" +
                                     "- Email valide";
            }
        }

        #endregion

        #region Navigation

        /// <summary>
        /// Handler du bouton Connexion : ferme la fenêtre d'inscription et ouvre la fenêtre de connexion.
        /// Utilise ShowDialog pour ouvrir la fenêtre de connexion modalement (comportement actuel).
        /// </summary>
        private void Connexion_Click(object sender, RoutedEventArgs e)
        {
            var connexionWindow = new MainWindow();
            this.Close();
            connexionWindow.ShowDialog();
        }

        #endregion
    }
}