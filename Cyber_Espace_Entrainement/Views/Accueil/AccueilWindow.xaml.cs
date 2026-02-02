using Cyber_Espace_Entrainement.Services;
using Cyber_Espace_Entrainement.Views.Activite;
using Cyber_Espace_Entrainement.Views.Profil;
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
    /// Code-behind pour la fenêtre d'accueil.
    /// Contient la logique UI spécifique (animations, gestion des clics/survols,
    /// navigation vers d'autres fenêtres). 
    /// </summary>
    public partial class AccueilWindow : Window
    {
        // Couleur originale du bouton Profile (utilisée pour restaurer la couleur après animation)
        private readonly Color _profileOriginalColor = (Color)ColorConverter.ConvertFromString("#1565C0");

        // Brushes chargés depuis les ressources (theme) pour garder la cohérence visuelle.
        // Ces champs permettent d'éviter des recherches répétées dans les ressources.
        private readonly SolidColorBrush _defaultTextBoxBorderBrush;
        private readonly SolidColorBrush _hoverTextBoxBorderBrush;
        private readonly SolidColorBrush _focusTextBoxBorderBrush;

        private readonly SolidColorBrush _defaultButtonBackground;
        private readonly SolidColorBrush _hoverButtonBackground;

        private readonly SolidColorBrush _defaultSecondaryBackground;
        private readonly SolidColorBrush _hoverSecondaryBackground;

        private readonly SolidColorBrush _defaultQuitBackground;
        private readonly SolidColorBrush _hoverQuitBackground;

        private readonly SolidColorBrush _defaultDecoBackground;
        private readonly SolidColorBrush _hoverDecoBackground;

        /// <summary>
        /// Constructeur : initialise les composants WPF et charge les brushes depuis les ressources.
        /// Attache également des gestionnaires d'événements pour certains boutons.
        /// </summary>
        public AccueilWindow()
        {
            InitializeComponent();

            // Chargement des couleurs depuis les ressources de l'application (theme centralisé)
            _defaultTextBoxBorderBrush = (SolidColorBrush)Application.Current.FindResource("BorderDefaultBrush");
            _hoverTextBoxBorderBrush = (SolidColorBrush)Application.Current.FindResource("BorderHoverBrush");
            _focusTextBoxBorderBrush = (SolidColorBrush)Application.Current.FindResource("BorderFocusBrush");

            _defaultButtonBackground = (SolidColorBrush)Application.Current.FindResource("PrimaryBlueBrush");
            _hoverButtonBackground = (SolidColorBrush)Application.Current.FindResource("PrimaryBlueDarkBrush");

            _defaultSecondaryBackground = (SolidColorBrush)Application.Current.FindResource("TransparentBrush");
            _hoverSecondaryBackground = (SolidColorBrush)Application.Current.FindResource("PrimaryBlueLightBrush");

            _defaultQuitBackground = (SolidColorBrush)Application.Current.FindResource("ErrorRedBrush");
            _hoverQuitBackground = (SolidColorBrush)Application.Current.FindResource("ErrorRedDarkBrush");

            _defaultDecoBackground = (SolidColorBrush)Application.Current.FindResource("DeconnexionSlateBrush");
            _hoverDecoBackground = (SolidColorBrush)Application.Current.FindResource("DeconnexionSlateDarkBrush");

            // Après que la fenêtre soit chargée, configuration additionnelle (ex : mise en place du bouton Profile)
            Loaded += AccueilWindow_Loaded;

            // Attacher les événements de survol au bouton Quitter (visuels)
            BtnQuitter.MouseEnter += BtnQuitter_MouseEnter;
            BtnQuitter.MouseLeave += BtnQuitter_MouseLeave;

            // Attacher les événements de survol au bouton Déconnexion (visuels)
            BtnDeco.MouseEnter += BtnDeco_MouseEnter;
            BtnDeco.MouseLeave += BtnDeco_MouseLeave;
        }

        /// <summary>
        /// Handler appelé lorsque la fenêtre est entièrement chargée.
        /// Utilisé pour des initialisations qui nécessitent que l'arbre visuel soit prêt.
        /// </summary>
        private void AccueilWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Rendre le bouton Profile rond en lui appliquant un template personnalisé
            MakeProfileButtonRound();

            // Attacher les événements de survol et de clic au bouton Profile
            // Ces événements remplacent des triggers XAML et permettent des animations programmatiques.
            Profile.MouseEnter += ProfileButton_MouseEnter;
            Profile.MouseLeave += ProfileButton_MouseLeave;
            Profile.MouseDown += ProfileButton_MouseDown;
            Profile.MouseUp += ProfileButton_MouseUp;
            Profile.Click += ProfileButton_Click;
        }

        #region Gestion du bouton Profile

        /// <summary>
        /// Crée et applique dynamiquement un ControlTemplate qui affiche un bouton circulaire
        /// (ellipse de fond + ContentPresenter centré). Utilise FrameworkElementFactory pour générer
        /// l'arbre visuel en code.
        /// Remarque : la création de templates en code est utile pour de la personnalisation dynamique,
        /// mais pour des styles statiques préférer le XAML.
        /// </summary>
        private void MakeProfileButtonRound()
        {
            // Créer le template personnalisé pour le bouton
            var template = new ControlTemplate(typeof(Button));

            // Créer la structure visuelle : Grid contenant une Ellipse (fond) et un ContentPresenter.
            var grid = new FrameworkElementFactory(typeof(Grid));

            // Ellipse de fond (sera liée au Background/BorderBrush du bouton)
            var ellipse = new FrameworkElementFactory(typeof(Ellipse));
            ellipse.Name = "BackgroundEllipse";

            // Binding sur Background du TemplatedParent (bouton) pour remplir l'ellipse
            var backgroundBinding = new Binding("Background")
            {
                RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent)
            };
            ellipse.SetBinding(System.Windows.Shapes.Shape.FillProperty, backgroundBinding);

            // Binding sur BorderBrush du TemplatedParent pour la bordure de l'ellipse
            var borderBrushBinding = new Binding("BorderBrush")
            {
                RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent)
            };
            ellipse.SetBinding(System.Windows.Shapes.Shape.StrokeProperty, borderBrushBinding);

            // Épaisseur de la bordure de l'ellipse
            ellipse.SetValue(System.Windows.Shapes.Shape.StrokeThicknessProperty, 3.0);

            // Ombre portée pour donner de la profondeur
            var dropShadow = new DropShadowEffect
            {
                BlurRadius = 15,
                ShadowDepth = 4,
                Opacity = 0.3,
                Color = Colors.Black
            };
            ellipse.SetValue(System.Windows.Shapes.Shape.EffectProperty, dropShadow);

            // ContentPresenter centré pour afficher l'image/icone du profil
            var contentPresenter = new FrameworkElementFactory(typeof(ContentPresenter));
            contentPresenter.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            contentPresenter.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);

            // Assemblage des éléments dans le template
            grid.AppendChild(ellipse);
            grid.AppendChild(contentPresenter);
            template.VisualTree = grid;

            // Application du template au bouton Profile défini dans le XAML
            Profile.Template = template;
        }

        /// <summary>
        /// MouseEnter : effet visuel combiné (zoom + éclaircissement de la bordure).
        /// Cette logique remplace des triggers XAML et réalise des animations programmatiques.
        /// </summary>
        private void ProfileButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Button button)
            {
                // Mise en place d'une ScaleTransform pour l'effet de zoom
                var scaleTransform = new ScaleTransform(1, 1);
                button.RenderTransform = scaleTransform;
                button.RenderTransformOrigin = new Point(0.5, 0.5);

                var scaleAnimation = new DoubleAnimation
                {
                    To = 1.1,
                    Duration = TimeSpan.FromMilliseconds(200),
                    EasingFunction = new QuadraticEase()
                };

                // Animation X et Y simultanées
                scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
                scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);

                // Animation de couleur de la bordure (si la bordure est un SolidColorBrush)
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
        /// MouseLeave : restaure l'apparence originale (taille + couleur de bordure).
        /// </summary>
        private void ProfileButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Button button)
            {
                // Retour à l'échelle normale
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

                // Restauration de la couleur de bordure d'origine
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
        /// MouseDown : petit effet d'enfoncement pour donner du feedback tactile.
        /// </summary>
        private void ProfileButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Button button)
            {
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
        /// MouseUp : ramène l'échelle au niveau de survol (si applicable).
        /// </summary>
        private void ProfileButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Button button)
            {
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
        /// Click : ouvre la fenêtre de profil (PersonalView).
        /// Utilise ShowDialog pour modalité ; adapter selon besoin (Show si modeless).
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
        /// Éclaircit une couleur en appliquant un pourcentage.
        /// Utilisé par les animations pour obtenir une couleur "plus claire".
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
        /// Assombrit une couleur en appliquant un pourcentage.
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
        /// Ouverture de l'espace Tests / Activités.
        /// Méthode reliée à l'événement MouseLeftButtonDown sur la "card" correspondante.
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
            var accueilTestsWindow = new AccueilActivite();
            accueilTestsWindow.ShowDialog();
        }

        /// <summary>
        /// Affiche un message d'information pour l'espace Cours (non implémenté).
        /// Laisser en MessageBoxService pour consistance visuelle.
        /// </summary>
        private void CoursCard_Click(object sender, MouseButtonEventArgs e)
        {
            MessageBoxService.ShowInformation(
                "ESPACE COURS\n\n" +
                "Cette section contiendra :\n" +
                "• Cours de cybersécurité\n" +
                "• Tutoriels .... vidéo\n" +
                "• Documentation\n" +
                "• Ressources pédagogiques\n\n" +
                "À développer...",
                "Espace Cours"
            );

            // TODO : Créer CoursView.xaml et décommenter :
            // var coursWindow = new CoursView();
            // coursWindow.ShowDialog();
        }

        #endregion

        #region Effets visuels des cartes

        /// <summary>
        /// Survol d'une "card" : léger zoom, modification de la bordure et renforcement de l'ombre.
        /// Les cards sont des Border dans le XAML et peuvent utiliser la propriété Tag pour stocker une couleur.
        /// </summary>
        private void Card_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Border card)
            {
                // Application d'un ScaleTransform pour l'effet de zoom
                card.RenderTransform = new ScaleTransform(1.05, 1.05);
                card.RenderTransformOrigin = new Point(0.5, 0.5);

                // Si la carte contient une couleur dans Tag, appliquer une bordure accentuée
                if (card.Tag is string couleur)
                {
                    var color = (Color)ColorConverter.ConvertFromString(couleur);
                    card.BorderBrush = new SolidColorBrush(color);
                    card.BorderThickness = new Thickness(2);
                }

                // Augmenter l'ombre portée si présente
                if (card.Effect is DropShadowEffect shadow)
                {
                    shadow.BlurRadius = 20;
                    shadow.ShadowDepth = 5;
                }
            }
        }

        /// <summary>
        /// Lorsque la souris quitte la card : restauration de l'apparence par défaut.
        /// </summary>
        private void Card_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Border card)
            {
                // Restauration de l'échelle
                card.RenderTransform = new ScaleTransform(1.0, 1.0);

                // Restauration de la bordure par défaut (gris clair)
                card.BorderBrush = new SolidColorBrush(Color.FromRgb(224, 224, 224));
                card.BorderThickness = new Thickness(1);

                // Restauration de l'ombre si présente
                if (card.Effect is DropShadowEffect shadow)
                {
                    shadow.BlurRadius = 10;
                    shadow.ShadowDepth = 2;
                }
            }
        }

        #endregion

        #region Bouton Quitter

        /// <summary>
        /// Clic sur le bouton "Quitter" : confirme puis ferme l'application si l'utilisateur confirme.
        /// </summary>
        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBoxService.ShowQuestion(
                "Voulez-vous vraiment quitter l'application ?",
                "Confirmation"
            );

            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        /// <summary>
        /// MouseEnter du bouton Quitter : changer le background
        /// </summary>
        private void BtnQuitter_MouseEnter(object sender, MouseEventArgs e)
        {
            BtnQuitter.Background = _hoverQuitBackground;
        }

        /// <summary>
        /// MouseLeave du bouton Quitter : restauration du background.
        /// </summary>
        private void BtnQuitter_MouseLeave(object sender, MouseEventArgs e)
        {
            BtnQuitter.Background = _defaultQuitBackground;
        }

        #endregion

        #region Bouton Déconnexion

        /// <summary>
        /// Click du bouton Déconnexion : confirme, appelle le service de session pour logout,
        /// ferme la fenêtre courante et ouvre la fenêtre de connexion.
        /// </summary>
        private void BtnDeco_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBoxService.ShowQuestion(
                "Voulez-vous vraiment vous déconnecter ?",
                "Confirmation de déconnexion"
            );

            if (result == MessageBoxResult.Yes)
            {
                // Appeler la méthode de déconnexion du service de session (singleton)
                SessionService.Instance.Logout();

                // Ouvrir la fenêtre de connexion (MainWindow)
                var connexionWindow = new MainWindow();

                // Fermer la fenêtre actuelle puis afficher la connexion
                this.Close();
                connexionWindow.Show();
            }
        }

        /// <summary>
        /// MouseEnter du bouton Déconnexion : appliquer la couleur hover définie.
        /// </summary>
        private void BtnDeco_MouseEnter(object sender, MouseEventArgs e)
        {
            BtnDeco.Background = _hoverDecoBackground;
        }

        /// <summary>
        /// MouseLeave du bouton Déconnexion : restauration du background.
        /// </summary>
        private void BtnDeco_MouseLeave(object sender, MouseEventArgs e)
        {
            BtnDeco.Background = _defaultDecoBackground;
        }
        #endregion
    }
}