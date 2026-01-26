using Cyber_Espace_Entrainement.ViewModels;
using Cyber_Espace_Entrainement.ViewModels.Accueil;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Cyber_Espace_Entrainement.Views.Accueil
{
    public partial class Inscription : Window
    {
        private InscriptionViewModel _viewModel;

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

        public Inscription()
        {
            InitializeComponent();
            _viewModel = this.DataContext as InscriptionViewModel;

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

            // Autorise l'info-bulle même quand le bouton est IsEnabled = false
            ToolTipService.SetShowOnDisabled(btnValider, true);
            InitializeEventHandlers();
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

            // TextBox - Nom
            tbxNom.MouseEnter += TextBox_MouseEnter;
            tbxNom.MouseLeave += TextBox_MouseLeave;
            tbxNom.GotFocus += TextBox_GotFocus;
            tbxNom.LostFocus += TextBox_LostFocus;

            // TextBox - Prénom
            tbxPrenom.MouseEnter += TextBox_MouseEnter;
            tbxPrenom.MouseLeave += TextBox_MouseLeave;
            tbxPrenom.GotFocus += TextBox_GotFocus;
            tbxPrenom.LostFocus += TextBox_LostFocus;

            // ComboBox - Section
            cbxSection.MouseEnter += ComboBox_MouseEnter;
            cbxSection.MouseLeave += ComboBox_MouseLeave;
            cbxSection.GotFocus += ComboBox_GotFocus;
            cbxSection.LostFocus += ComboBox_LostFocus;

            // TextBox - Mail
            tbxMail.MouseEnter += TextBox_MouseEnter;
            tbxMail.MouseLeave += TextBox_MouseLeave;
            tbxMail.GotFocus += TextBox_GotFocus;
            tbxMail.LostFocus += TextBox_LostFocus;

            // Bouton Valider
            btnValider.MouseEnter += BtnValider_MouseEnter;
            btnValider.MouseLeave += BtnValider_MouseLeave;
            btnValider.IsEnabledChanged += BtnValider_IsEnabledChanged;

            // Bouton Connexion
            btnConnexion.MouseEnter += BtnConnexion_MouseEnter;
            btnConnexion.MouseLeave += BtnConnexion_MouseLeave;
            

            // Initialiser le premier message
            UpdateValiderToolTip();

            // Bouton Quitter
            btnQuitter.MouseEnter += BtnQuitter_MouseEnter;
            btnQuitter.MouseLeave += BtnQuitter_MouseLeave;
        }

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

        #region ComboBox Events

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

        private void BtnValider_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateValiderToolTip();
        }

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

        private void Connexion_Click(object sender, RoutedEventArgs e)
        {
            var connexionWindow = new MainWindow();
            this.Close();
            connexionWindow.ShowDialog();
        }

        #endregion
    }
}