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
using System.Windows.Shapes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Cyber_Espace_Entrainement.Views.Activite
{
    /// <summary>
    /// Logique d'interaction pour AccueilActivite.xaml
    /// </summary>
    public partial class AccueilActivite : Window
    {

        private SolidColorBrush _defaultQuitBackground;
        private SolidColorBrush _hoverQuitBackground;

        private SolidColorBrush _defaultDecoBackground;
        private SolidColorBrush _hoverDecoBackground;

        // Brushes pour le bouton Retour (couleur complémentaire orange/amber)
        private SolidColorBrush _defaultRetourBackground;
        private SolidColorBrush _hoverRetourBackground;

        private Color _ProfilOriginalColor = (Color)ColorConverter.ConvertFromString("#1565C0");

        public AccueilActivite()
        {
            InitializeComponent();

            InitialiserBrushes();
            
            AttacherEvenements();
        }

        /// <summary>
        /// Handler appelé lorsque la fenêtre est entièrement chargée.
        /// Utilisé pour des initialisations qui nécessitent que l'arbre visuel soit prêt.
        /// </summary>
        private void AccueilActivite_Loaded(object sender, RoutedEventArgs e)
        {
            // Rendre le bouton Profil rond en lui appliquant un template personnalisé
            MakeProfilButtonRound();

            // Attacher les événements de survol et de clic au bouton Profil
            // Ces événements remplacent des triggers XAML et permettent des animations programmatiques.
            Profil.MouseEnter += Profil_MouseEnter;
            Profil.MouseLeave += Profil_MouseLeave;
            Profil.MouseDown += Profil_MouseDown;
            Profil.MouseUp += Profil_MouseUp;
            Profil.Click += ProfilButton_Click;
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

        #region Boutons

        #region Gestion du bouton Profil

        /// <summary>
        /// Crée et applique dynamiquement un ControlTemplate qui affiche un bouton circulaire
        /// (ellipse de fond + ContentPresenter centré). Utilise FrameworkElementFactory pour générer
        /// l'arbre visuel en code.
        /// Remarque : la création de templates en code est utile pour de la personnalisation dynamique,
        /// mais pour des styles statiques préférer le XAML.
        /// </summary>
        private void MakeProfilButtonRound()
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
            Profil.Template = template;
        }

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
            var profilWindow = new PersonalView();
            profilWindow.ShowDialog();
        }

        #endregion

        #region Bouton déconnexion

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

        #region Bouton retour
        /// <summary>
        /// Bouton Retour - Fermer cette fenêtre et retourner au menu
        /// </summary>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Fermer la fenêtre (retour au menu principal)
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

        private void BtnQuitter_MouseEnter(object sender, MouseEventArgs e)
        {
            btnQuitter.Background = _hoverQuitBackground;
        }

        private void BtnQuitter_MouseLeave(object sender, MouseEventArgs e)
        {
            btnQuitter.Background = _defaultQuitBackground;
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

        #endregion

        #region Utilitaires Colors

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

        #endregion

        #region Utilitaires boutons ronds

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
    }
}
