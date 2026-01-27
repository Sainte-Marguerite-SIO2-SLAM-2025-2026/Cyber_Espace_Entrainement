using Cyber_Espace_Entrainement.Services;
using Cyber_Espace_Entrainement.Views.Profil;
using Cyber_Espace_Entrainement.Views.Tests;
using Cyber_Espace_Entrainement.Views.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Cyber_Espace_Entrainement.Views.Accueil
{
    /// <summary>
    /// Logique d'interaction pour Accueil.xaml
    /// </summary>
    public partial class AccueilWindow : Window
    {
        // Couleur originale du bouton Profile pour les animations
        private readonly Color _profileOriginalColor = (Color)ColorConverter.ConvertFromString("#1565C0");

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

        public AccueilWindow()
        {
            InitializeComponent();

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

            // Configuration du bouton Profile après le chargement
            Loaded += AccueilWindow_Loaded;

            // Bouton Quitter
            BtnQuitter.MouseEnter += BtnQuitter_MouseEnter;
            BtnQuitter.MouseLeave += BtnQuitter_MouseLeave;
        }

        /// <summary>
        /// Configuration initiale du bouton Profile
        /// </summary>
        private void AccueilWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Rendre le bouton Profile rond avec template Ellipse
            MakeProfileButtonRound();

            // Attacher les événements de survol et clic au bouton Profile
            Profile.MouseEnter += ProfileButton_MouseEnter;
            Profile.MouseLeave += ProfileButton_MouseLeave;
            Profile.MouseDown += ProfileButton_MouseDown;
            Profile.MouseUp += ProfileButton_MouseUp;
            Profile.Click += ProfileButton_Click;
        }

        #region Gestion du bouton Profile

        /// <summary>
        /// Transforme le bouton Profile en bouton rond avec template Ellipse
        /// </summary>
        private void MakeProfileButtonRound()
        {
            // Créer le template personnalisé
            var template = new ControlTemplate(typeof(Button));

            // Créer la structure avec FrameworkElementFactory
            var grid = new FrameworkElementFactory(typeof(Grid));

            // Créer l'ellipse de fond
            var ellipse = new FrameworkElementFactory(typeof(Ellipse));
            ellipse.Name = "BackgroundEllipse";

            // CORRECTION : Utiliser Binding avec RelativeSource au lieu de TemplateBinding
            // TemplateBinding n'existe pas en C#, c'est uniquement pour XAML
            var backgroundBinding = new Binding("Background")
            {
                RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent)
            };
            ellipse.SetBinding(System.Windows.Shapes.Shape.FillProperty, backgroundBinding);

            var borderBrushBinding = new Binding("BorderBrush")
            {
                RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent)
            };
            ellipse.SetBinding(System.Windows.Shapes.Shape.StrokeProperty, borderBrushBinding);

            ellipse.SetValue(System.Windows.Shapes.Shape.StrokeThicknessProperty, 3.0);

            // Ajouter une ombre portée
            var dropShadow = new DropShadowEffect
            {
                BlurRadius = 15,
                ShadowDepth = 4,
                Opacity = 0.3,
                Color = Colors.Black
            };
            ellipse.SetValue(System.Windows.Shapes.Shape.EffectProperty, dropShadow);

            // Créer le ContentPresenter pour afficher l'image
            var contentPresenter = new FrameworkElementFactory(typeof(ContentPresenter));
            contentPresenter.SetValue(ContentPresenter.HorizontalAlignmentProperty,
                HorizontalAlignment.Center);
            contentPresenter.SetValue(ContentPresenter.VerticalAlignmentProperty,
                VerticalAlignment.Center);

            // Assembler la structure
            grid.AppendChild(ellipse);
            grid.AppendChild(contentPresenter);
            template.VisualTree = grid;

            // Appliquer le template au bouton Profile
            Profile.Template = template;
        }

        /// <summary>
        /// Événement MouseEnter - Effet de zoom et éclaircissement au survol
        /// Remplace le trigger IsMouseOver en XAML
        /// </summary>
        private void ProfileButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Button button)
            {
                // Animation de zoom (1.0 -> 1.1)
                var scaleTransform = new ScaleTransform(1, 1);
                button.RenderTransform = scaleTransform;
                button.RenderTransformOrigin = new Point(0.5, 0.5);

                var scaleAnimation = new DoubleAnimation
                {
                    To = 1.1,
                    Duration = TimeSpan.FromMilliseconds(200),
                    EasingFunction = new QuadraticEase()
                };

                scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
                scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);

                // Animation de couleur de la bordure (plus claire)
                if (button.BorderBrush is SolidColorBrush borderBrush)
                {
                    var lighterColor = LightenColor(_profileOriginalColor, 0.3f);
                    var colorAnimation = new ColorAnimation
                    {
                        To = lighterColor,
                        Duration = TimeSpan.FromMilliseconds(200)
                    };
                    borderBrush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
                }
            }
        }

        /// <summary>
        /// Événement MouseLeave - Retour à la normale
        /// Remplace le trigger IsMouseOver=False en XAML
        /// </summary>
        private void ProfileButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Button button)
            {
                // Retour à la taille normale (1.1 -> 1.0)
                if (button.RenderTransform is ScaleTransform scaleTransform)
                {
                    var scaleAnimation = new DoubleAnimation
                    {
                        To = 1.0,
                        Duration = TimeSpan.FromMilliseconds(200),
                        EasingFunction = new QuadraticEase()
                    };

                    scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
                    scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
                }

                // Retour à la couleur de bordure originale
                if (button.BorderBrush is SolidColorBrush borderBrush)
                {
                    var colorAnimation = new ColorAnimation
                    {
                        To = _profileOriginalColor,
                        Duration = TimeSpan.FromMilliseconds(200)
                    };
                    borderBrush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
                }
            }
        }

        /// <summary>
        /// Événement MouseDown - Effet d'enfoncement au clic
        /// Remplace le trigger IsPressed en XAML
        /// </summary>
        private void ProfileButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Button button)
            {
                // Effet d'enfoncement (1.1 -> 0.95)
                if (button.RenderTransform is ScaleTransform scaleTransform)
                {
                    var scaleAnimation = new DoubleAnimation
                    {
                        To = 0.95,
                        Duration = TimeSpan.FromMilliseconds(100)
                    };

                    scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
                    scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
                }
            }
        }

        /// <summary>
        /// Événement MouseUp - Retour à la taille de survol après le clic
        /// </summary>
        private void ProfileButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Button button)
            {
                // Retour à la taille de survol (0.95 -> 1.1)
                if (button.RenderTransform is ScaleTransform scaleTransform)
                {
                    var scaleAnimation = new DoubleAnimation
                    {
                        To = 1.1,
                        Duration = TimeSpan.FromMilliseconds(100)
                    };

                    scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
                    scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
                }
            }
        }

        /// <summary>
        /// Événement Click - Ouvrir le profil utilisateur
        /// </summary>
        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(
            //    "ESPACE PERSONNEL\n\n" +
            //    "Cette section contiendra :\n" +
            //    "• Profil utilisateur\n" +
            //    "• Statistiques personnelles\n" +
            //    "• Historique des tests\n" +
            //    "• Badges et achievements\n\n" +
            //    "À développer...",
            //    "Espace Personnel",
            //    MessageBoxButton.OK,
            //    MessageBoxImage.Information
            //);

            // TODO : Créer PersonalView.xaml par exemple et décommenter la suite !
            // Ouvrir la fenêtre de profil
            var profileWindow = new PersonalView();
            profileWindow.ShowDialog();
        }

        #endregion

        #region Utilitaires de couleurs

        /// <summary>
        /// Éclaircir une couleur d'un certain pourcentage
        /// </summary>
        private Color LightenColor(Color color, float amount)
        {
            return Color.FromArgb(
                color.A,
                (byte)Math.Min(255, color.R + (255 - color.R) * amount),
                (byte)Math.Min(255, color.G + (255 - color.G) * amount),
                (byte)Math.Min(255, color.B + (255 - color.B) * amount)
            );
        }

        /// <summary>
        /// Assombrir une couleur d'un certain pourcentage
        /// </summary>
        private Color DarkenColor(Color color, float amount)
        {
            return Color.FromArgb(
                color.A,
                (byte)(color.R * (1 - amount)),
                (byte)(color.G * (1 - amount)),
                (byte)(color.B * (1 - amount))
            );
        }

        #endregion

        #region Événements des cartes

        /// <summary>
        /// Ouvrir l'espace Tests (à créer)
        /// </summary>
        private void TestsCard_Click(object sender, MouseButtonEventArgs e)
        {
            //MessageBox.Show(
            //    "ESPACE TESTS\n\n" +
            //    "Cette section contiendra :\n" +
            //    "• Quiz de cybersécurité\n" +
            //    "• Exercices pratiques\n" +
            //    "• Évaluations\n" +
            //    "• Scores et classements\n\n" +
            //    "À développer...à vous de trouver",
            //    "Espace Tests",
            //    MessageBoxButton.OK,
            //    MessageBoxImage.Information
            //);
            var morseWindow = new MorseTestView();
            morseWindow.ShowDialog();
        }

        /// <summary>
        /// Ouvrir l'espace Cours (à créer)
        /// </summary>
        private void CoursCard_Click(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show(
                "ESPACE COURS\n\n" +
                "Cette section contiendra :\n" +
                "• Cours de cybersécurité\n" +
                "• Tutoriels .... vidéo\n" +
                "• Documentation\n" +
                "• Ressources pédagogiques\n\n" +
                "À développer...",
                "Espace Cours",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );

            // TODO : Créer CoursView.xaml et décommenter :
            // var coursWindow = new CoursView();
            // coursWindow.ShowDialog();
        }

        #endregion

        #region Effets visuels des cartes

        /// <summary>
        /// Effet de zoom au survol de la carte
        /// </summary>
        private void Card_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Border card)
            {
                // Animation de zoom léger
                card.RenderTransform = new ScaleTransform(1.05, 1.05);
                card.RenderTransformOrigin = new Point(0.5, 0.5);

                //
                // couleur de la card à modifier
                //
                if (card.Tag is string couleur)
                {
                    // Bordure plus visible
                    var color = (Color)ColorConverter.ConvertFromString(couleur);
                    card.BorderBrush = new SolidColorBrush(color);
                    card.BorderThickness = new Thickness(2);
                }

                // Ombre plus marquée
                if (card.Effect is DropShadowEffect shadow)
                {
                    shadow.BlurRadius = 20;
                    shadow.ShadowDepth = 5;
                }
            }
        }

        /// <summary>
        /// Retour à la normale quand la souris quitte
        /// </summary>
        private void Card_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Border card)
            {
                // Retour à la taille normale
                card.RenderTransform = new ScaleTransform(1.0, 1.0);

                // Bordure normale
                card.BorderBrush = new SolidColorBrush(Color.FromRgb(224, 224, 224));
                card.BorderThickness = new Thickness(1);

                // Ombre normale
                if (card.Effect is DropShadowEffect shadow)
                {
                    shadow.BlurRadius = 10;
                    shadow.ShadowDepth = 2;
                }
            }
        }

        #endregion

        #region Bouton Quitter

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Voulez-vous vraiment quitter l'application ?",
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
        private void BtnQuitter_MouseEnter(object sender, MouseEventArgs e)
        {
            BtnQuitter.Background = _hoverQuitBackground;
        }

        private void BtnQuitter_MouseLeave(object sender, MouseEventArgs e)
        {
            BtnQuitter.Background = _defaultQuitBackground;
        }

        #endregion
    }
}