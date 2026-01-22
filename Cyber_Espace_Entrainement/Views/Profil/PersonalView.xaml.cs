using Cyber_Espace_Entrainement.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Cyber_Espace_Entrainement.Views.Profil
{
    /// <summary>
    /// Logique d'interaction pour PersonalView.xaml
    /// </summary>
    public partial class PersonalView : Window
    {
        public PersonalView()
        {
            InitializeComponent();
            InitialiseData();
        }

        /// <summary>
        /// Permet de retourner à la page d'accueil
        /// </summary>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void InitialiseData()
        {
            textBoxPrenom.Text = SessionService.Instance.CurrentPrenom;
            textBoxNom.Text = SessionService.Instance.CurrentNom;
            textBoxPseudo.Text = SessionService.Instance.CurrentLogin;
            passwordBoxPassword.Password = SessionService.Instance.CurrentPassword; //mdp hash -> a changer en modif mdp avec ancien et nv
            textBoxEmail.Text = SessionService.Instance.CurrentEmail;
            textBoxSection.Text = SessionService.Instance.CurrentSection;
            textBoxDateCreation.Text = SessionService.Instance.CurrentDateCrea.ToString();
            textBoxDerniereConnection.Text = SessionService.Instance.CurrentDerniereCo.ToString();
            textBoxScoreTotal.Text = SessionService.Instance.CurrentScore.ToString();
        }
        // faire un vos infos avec btn modif qui va sur stackpannel modifiervosinfos (sinon de base affiche pas modifiable) voir gestion user 
        // augmenter auteur et largeur de la view, meilleure disposition aussi a faire
    }
}
