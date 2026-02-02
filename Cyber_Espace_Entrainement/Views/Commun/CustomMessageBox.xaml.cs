using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Cyber_Espace_Entrainement.Views.Commun
{
    public partial class CustomMessageBox : Window
    {
        public MessageBoxResult Result { get; private set; }

        // Récupération des couleurs depuis les ressources
        private readonly SolidColorBrush _primaryBlueBrush;
        private readonly SolidColorBrush _primaryBlueDarkBrush;
        private readonly SolidColorBrush _errorRedBrush;
        private readonly SolidColorBrush _errorRedDarkBrush;
        private readonly SolidColorBrush _backgroundWhiteBrush;
        private readonly SolidColorBrush _popupBackgroundBrush;
        private readonly Color           _popupMainInfoQuest;
        private readonly Color           _popupMainWarning;
        private readonly Color           _popupMainError;



        public CustomMessageBox()
        {
            InitializeComponent();
            Result = MessageBoxResult.None;

            // Chargement des couleurs depuis les ressources de l'application
            _primaryBlueBrush = (SolidColorBrush)Application.Current.FindResource("PrimaryBlueBrush");
            _primaryBlueDarkBrush = (SolidColorBrush)Application.Current.FindResource("PrimaryBlueDarkBrush");
            _errorRedBrush = (SolidColorBrush)Application.Current.FindResource("ErrorRedBrush");
            _errorRedDarkBrush = (SolidColorBrush)Application.Current.FindResource("ErrorRedDarkBrush");
            _backgroundWhiteBrush = (SolidColorBrush)Application.Current.FindResource("BackgroundWhiteBrush");
            _popupBackgroundBrush = (SolidColorBrush)Application.Current.FindResource("PopupBackgroundBrush");
            _popupMainInfoQuest = (Color)Application.Current.FindResource("PopupMainInfoQuest");
            _popupMainWarning = (Color)Application.Current.FindResource("PopupMainWarning");
            _popupMainError = (Color)Application.Current.FindResource("PopupMainError");
        }

        /// <summary>
        /// Configure la MessageBox avec les paramètres spécifiés
        /// </summary>
        public void Configure(string message, string title, MessageBoxButton buttons, MessageBoxImage icon)
        {
            var viewModel = new CustomMessageBoxViewModel
            {
                Title = title,
                Message = message
            };
            this.DataContext = viewModel;

            // Configurer l'icône
            ConfigureIcon(icon);

            // Configurer les boutons
            ConfigureButtons(buttons);
        }

        /// <summary>
        /// Configure l'icône selon le type de message
        /// </summary>
        private void ConfigureIcon(MessageBoxImage icon)
        {
            string iconText = "";
            Color iconColor = Colors.White;
            Color headerColor;
            Color borderColor;

            switch (icon)
            {
                case MessageBoxImage.Information:
                    iconText = "ℹ";
                    iconColor = Colors.White;
                    headerColor = _popupMainInfoQuest; // Bleu clair
                    borderColor = _popupMainInfoQuest;
                    break;
                case MessageBoxImage.Question:
                    iconText = "?";
                    iconColor = Colors.White;
                    headerColor = _popupMainInfoQuest; // Bleu clair
                    borderColor = _popupMainInfoQuest;
                    break;
                case MessageBoxImage.Warning:
                    iconText = "⚠";
                    iconColor = Colors.White;
                    headerColor = _popupMainWarning; // Orange clair
                    borderColor = _popupMainWarning;
                    break;
                case MessageBoxImage.Error:
                    iconText = "✖";
                    iconColor = Colors.White;
                    headerColor = _popupMainError; // Rouge clair
                    borderColor = _popupMainError;
                    break;
                default:
                    iconText = "ℹ";
                    iconColor = Colors.White;
                    headerColor = _popupMainInfoQuest; // Bleu clair par défaut
                    borderColor = _popupMainInfoQuest;
                    break;
            }

            IconText.Text = iconText;
            IconText.Foreground = new SolidColorBrush(iconColor);

            // Appliquer les couleurs au header et au contour
            HeaderBorder.Background = new SolidColorBrush(headerColor);
            MainBorder.BorderBrush = new SolidColorBrush(borderColor);
        }

        /// <summary>
        /// Configure les boutons selon le type demandé
        /// </summary>
        private void ConfigureButtons(MessageBoxButton buttons)
        {
            ButtonPanel.Children.Clear();

            switch (buttons)
            {
                case MessageBoxButton.OK:
                    AddButton("OK", MessageBoxResult.OK, _primaryBlueBrush, _primaryBlueDarkBrush, true);
                    break;

                case MessageBoxButton.OKCancel:
                    AddButton("OK", MessageBoxResult.OK, _primaryBlueBrush, _primaryBlueDarkBrush, true);
                    AddButton("Annuler", MessageBoxResult.Cancel, _errorRedBrush, _errorRedDarkBrush, false);
                    break;

                case MessageBoxButton.YesNo:
                    AddButton("Oui", MessageBoxResult.Yes, _primaryBlueBrush, _primaryBlueDarkBrush, true);
                    AddButton("Non", MessageBoxResult.No, _errorRedBrush, _errorRedDarkBrush, false);
                    break;

                case MessageBoxButton.YesNoCancel:
                    AddButton("Oui", MessageBoxResult.Yes, _primaryBlueBrush, _primaryBlueDarkBrush, true);
                    AddButton("Non", MessageBoxResult.No, _errorRedBrush, _errorRedDarkBrush, false);
                    AddButton("Annuler", MessageBoxResult.Cancel, _errorRedBrush, _errorRedDarkBrush, false);
                    break;
            }
        }

        /// <summary>
        /// Ajoute un bouton au panel avec les styles appropriés
        /// </summary>
        private void AddButton(string content, MessageBoxResult result,
            SolidColorBrush defaultBackground, SolidColorBrush hoverBackground, bool isDefault)
        {
            var button = new Button
            {
                Content = content,
                Width = 100,
                Height = 38,
                Margin = new Thickness(5, 0, 5, 0),
                FontFamily = new FontFamily("Verdana"),
                FontSize = 14,
                FontWeight = FontWeights.SemiBold,
                Background = defaultBackground,
                Foreground = _backgroundWhiteBrush,
                BorderThickness = new Thickness(0),
                Cursor = Cursors.Hand,
                IsDefault = isDefault,
                IsCancel = (result == MessageBoxResult.Cancel || result == MessageBoxResult.No)
            };

            // Template personnalisé avec coins arrondis
            var template = new ControlTemplate(typeof(Button));
            var border = new FrameworkElementFactory(typeof(Border));
            border.Name = "border";
            border.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Button.BackgroundProperty));
            border.SetValue(Border.BorderBrushProperty, new TemplateBindingExtension(Button.BorderBrushProperty));
            border.SetValue(Border.BorderThicknessProperty, new TemplateBindingExtension(Button.BorderThicknessProperty));
            border.SetValue(Border.CornerRadiusProperty, new CornerRadius(6));
            border.SetValue(Border.PaddingProperty, new TemplateBindingExtension(Button.PaddingProperty));

            var contentPresenter = new FrameworkElementFactory(typeof(ContentPresenter));
            contentPresenter.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            contentPresenter.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);

            border.AppendChild(contentPresenter);
            template.VisualTree = border;
            button.Template = template;

            // Gestionnaires d'événements pour le survol
            button.MouseEnter += (s, e) =>
            {
                button.Background = hoverBackground;
            };

            button.MouseLeave += (s, e) =>
            {
                button.Background = defaultBackground;
            };

            // Gestionnaire de clic
            button.Click += (s, e) =>
            {
                Result = result;
                DialogResult = (result == MessageBoxResult.OK || result == MessageBoxResult.Yes);
                Close();
            };

            ButtonPanel.Children.Add(button);
        }
    }
}