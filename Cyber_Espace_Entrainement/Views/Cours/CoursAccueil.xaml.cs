using Cyber_Espace_Entrainement.Services;
using Cyber_Espace_Entrainement.Views.Profil;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cyber_Espace_Entrainement.Views.Cours
{
    /// <summary>
    /// Logique d'interaction pour CoursAccueil.xaml
    /// </summary>
    public partial class CoursAccueil : Window
    {
        // Brushes pour le bouton Quitter
        private SolidColorBrush _defaultQuitBackground;
        private SolidColorBrush _hoverQuitBackground;

        // Brushes pour le bouton Retour (couleur complémentaire orange/amber)
        private SolidColorBrush _defaultRetourBackground;
        private SolidColorBrush _hoverRetourBackground;

        // Brushes pour le bouton Déconnexion
        private SolidColorBrush _defaultDecoBackground;
        private SolidColorBrush _hoverDecoBackground;

        private Color _ProfilOriginalColor = (Color)ColorConverter.ConvertFromString("#1565C0"); // TO DO : remplacer par le theme

        public CoursAccueil()
        {
            InitializeComponent();

            // Initialiser les brushes pour les boutons
            InitialiserBrushes();

            // Attacher les événements des boutons
            AttacherEvenements();
        }

        /// <summary>
        /// Initialise les brushes pour les boutons depuis les ressources
        /// </summary>
        private void InitialiserBrushes()
        {
            // Bouton Quitter
            _defaultQuitBackground = (SolidColorBrush)Application.Current.FindResource("ErrorRedBrush");
            _hoverQuitBackground = (SolidColorBrush)Application.Current.FindResource("ErrorRedDarkBrush");

            // Bouton Retour (couleur complémentaire orange/amber)
            _defaultRetourBackground = (SolidColorBrush)Application.Current.FindResource("RetourBackBlueBrush");
            _hoverRetourBackground = (SolidColorBrush)Application.Current.FindResource("RetourBackBlueLightBrush");

            // Bouton Déconnexion
            _defaultDecoBackground = (SolidColorBrush)Application.Current.FindResource("DeconnexionSlateBrush");
            _hoverDecoBackground = (SolidColorBrush)Application.Current.FindResource("DeconnexionSlateDarkBrush");

            Profil.Background = new SolidColorBrush(_ProfilOriginalColor);
        }

        /// <summary>
        /// Attache tous les événements des boutons
        /// </summary>
        private void AttacherEvenements()
        {
            // Bouton Retour
            BackButton.Click += BackButton_Click;
            BackButton.MouseEnter += BackButton_MouseEnter;
            BackButton.MouseLeave += BackButton_MouseLeave;

            // Bouton Quitter
            btnQuitter.Click += BtnQuitter_Click;
            btnQuitter.MouseEnter += BtnQuitter_MouseEnter;
            btnQuitter.MouseLeave += BtnQuitter_MouseLeave;

            /// --- Événements Bouton PROFIL ---
            Profil.MouseEnter += Profil_MouseEnter;
            Profil.MouseLeave += Profil_MouseLeave;
            Profil.MouseDown += Profil_MouseDown;
            Profil.MouseUp += Profil_MouseUp;
            Profil.Click += ProfilButton_Click;

            // Attacher les événements de survol au bouton Déconnexion (visuels)
            BtnDeco.MouseEnter += BtnDeco_MouseEnter;
            BtnDeco.MouseLeave += BtnDeco_MouseLeave;
        }

        #region Bouton Retour

        /// <summary>
        /// Gestionnaire du clic sur le bouton Retour
        /// </summary>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Événement MouseEnter pour le bouton Retour (effet hover)
        /// </summary>
        private void BackButton_MouseEnter(object sender, MouseEventArgs e)
        {
            BackButton.Background = _hoverRetourBackground;
        }

        /// <summary>
        /// Événement MouseLeave pour le bouton Retour (retour à l'état normal)
        /// </summary>
        private void BackButton_MouseLeave(object sender, MouseEventArgs e)
        {
            BackButton.Background = _defaultRetourBackground;
        }

        #endregion

        #region Bouton quitter

        /// <summary>
        /// Gestionnaire du clic sur le bouton Quitter
        /// Affiche une confirmation et ferme l'application si confirmé
        /// </summary>
        private void BtnQuitter_Click(object sender, RoutedEventArgs e)
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
        /// Événement MouseEnter pour le bouton Quitter (effet hover)
        /// </summary>
        private void BtnQuitter_MouseEnter(object sender, MouseEventArgs e)
        {
            btnQuitter.Background = _hoverQuitBackground;
        }

        /// <summary>
        /// Événement MouseLeave pour le bouton Quitter (retour à l'état normal)
        /// </summary>
        private void BtnQuitter_MouseLeave(object sender, MouseEventArgs e)
        {
            btnQuitter.Background = _defaultQuitBackground;
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

        #region Bouton Profil

        private void Profil_MouseEnter(object sender, MouseEventArgs e)
        {
            AnimateButton(Profil, 1.1, LightenColor(_ProfilOriginalColor, 0.3f));
        }

        private void Profil_MouseLeave(object sender, MouseEventArgs e)
        {
            AnimateButton(Profil, 1.0, _ProfilOriginalColor);
        }

        private void Profil_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AnimateScale(Profil, 0.95);
        }

        private void Profil_MouseUp(object sender, MouseButtonEventArgs e)
        {
            AnimateScale(Profil, 1.1);
        }

        /// <summary>
        /// Click : ouvre la fenêtre de profil (PersonalView).
        /// Utilise ShowDialog pour modalité ; adapter selon besoin (Show si modeless).
        /// </summary>
        private void ProfilButton_Click(object sender, RoutedEventArgs e)
        {
            // Ouvrir la fenêtre de profil
            var ProfilWindow = new PersonalView();
            ProfilWindow.ShowDialog();
        }

        #endregion

        #region Utilitaires Bouton Rond
        /// <summary>
        /// Version généralisée de MakeProfilButtonRound
        /// </summary>
        private void ApplyRoundStyle(Button button)
        {
            var template = new ControlTemplate(typeof(Button));
            var grid = new FrameworkElementFactory(typeof(Grid));

            var ellipse = new FrameworkElementFactory(typeof(System.Windows.Shapes.Ellipse));

            // On lie la couleur de fond de l'ellipse au Background du bouton
            ellipse.SetBinding(System.Windows.Shapes.Shape.FillProperty, new Binding("Background") { RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent) });

            // Bordure liée au BorderBrush
            ellipse.SetBinding(System.Windows.Shapes.Shape.StrokeProperty, new Binding("BorderBrush") { RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent) });
            ellipse.SetValue(System.Windows.Shapes.Shape.StrokeThicknessProperty, 3.0);

            var dropShadow = new DropShadowEffect { BlurRadius = 15, ShadowDepth = 4, Opacity = 0.3, Color = Colors.Black };
            ellipse.SetValue(System.Windows.Shapes.Shape.EffectProperty, dropShadow);

            var contentPresenter = new FrameworkElementFactory(typeof(ContentPresenter));
            contentPresenter.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            contentPresenter.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);

            grid.AppendChild(ellipse);
            grid.AppendChild(contentPresenter);
            template.VisualTree = grid;
            button.Template = template;
        }

        // Méthodes d'aide pour éviter la répétition de code d'animation
        private void AnimateButton(Button btn, double scale, Color color)
        {
            AnimateScale(btn, scale);
            if (btn.Background is SolidColorBrush brush)
            {
                brush.BeginAnimation(SolidColorBrush.ColorProperty,
                    new ColorAnimation(color, TimeSpan.FromMilliseconds(200)));
            }
        }

        private void AnimateScale(Button btn, double scale)
        {
            if (!(btn.RenderTransform is ScaleTransform))
            {
                btn.RenderTransform = new ScaleTransform(1, 1);
                btn.RenderTransformOrigin = new Point(0.5, 0.5);
            }

            var st = (ScaleTransform)btn.RenderTransform;
            var anim = new DoubleAnimation(scale, TimeSpan.FromMilliseconds(200))
            {
                EasingFunction = new QuadraticEase()
            };
            st.BeginAnimation(ScaleTransform.ScaleXProperty, anim);
            st.BeginAnimation(ScaleTransform.ScaleYProperty, anim);
        }

        #endregion

        #region Effets visuels des boutons

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
    }
}
