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

        // Couleurs pour les effets de survol
        private readonly SolidColorBrush _defaultBorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#BDBDBD"));
        private readonly SolidColorBrush _hoverBorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1565C0"));
        private readonly SolidColorBrush _focusBorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1565C0"));

        private readonly SolidColorBrush _defaultButtonBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1565C0"));
        private readonly SolidColorBrush _hoverButtonBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0D47A1"));

        private readonly SolidColorBrush _defaultSecondaryBackground = Brushes.Transparent;
        private readonly SolidColorBrush _hoverSecondaryBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E3F2FD"));

        private readonly SolidColorBrush _defaultQuitBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f44336"));
        private readonly SolidColorBrush _hoverQuitBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#d32f2f"));

        public Inscription()
        {
            InitializeComponent();
            _viewModel = this.DataContext as InscriptionViewModel;
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

            // Bouton Connexion
            btnConnexion.MouseEnter += BtnConnexion_MouseEnter;
            btnConnexion.MouseLeave += BtnConnexion_MouseLeave;

            // Bouton Quitter
            btnQuitter.MouseEnter += BtnQuitter_MouseEnter;
            btnQuitter.MouseLeave += BtnQuitter_MouseLeave;
        }

        #region TextBox Events

        private void TextBox_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is TextBox textBox && !textBox.IsFocused)
            {
                textBox.BorderBrush = _hoverBorderBrush;
            }
        }

        private void TextBox_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is TextBox textBox && !textBox.IsFocused)
            {
                textBox.BorderBrush = _defaultBorderBrush;
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.BorderBrush = _focusBorderBrush;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.BorderBrush = _defaultBorderBrush;
            }
        }

        #endregion

        #region PasswordBox Events

        private void PasswordBox_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is PasswordBox passwordBox && !passwordBox.IsFocused)
            {
                passwordBox.BorderBrush = _hoverBorderBrush;
            }
        }

        private void PasswordBox_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is PasswordBox passwordBox && !passwordBox.IsFocused)
            {
                passwordBox.BorderBrush = _defaultBorderBrush;
            }
        }

        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                passwordBox.BorderBrush = _focusBorderBrush;
            }
        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                passwordBox.BorderBrush = _defaultBorderBrush;
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
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
                comboBox.BorderBrush = _hoverBorderBrush;
            }
        }

        private void ComboBox_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is ComboBox comboBox && !comboBox.IsFocused)
            {
                comboBox.BorderBrush = _defaultBorderBrush;
            }
        }

        private void ComboBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                comboBox.BorderBrush = _focusBorderBrush;
            }
        }

        private void ComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                comboBox.BorderBrush = _defaultBorderBrush;
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

        #endregion

        #region Navigation

        private void Connexion_Click(object sender, RoutedEventArgs e)
        {
            var connexionWindow = new MainWindow();
            this.Hide();
            connexionWindow.ShowDialog();
            this.Close();
        }

        #endregion
    }
}