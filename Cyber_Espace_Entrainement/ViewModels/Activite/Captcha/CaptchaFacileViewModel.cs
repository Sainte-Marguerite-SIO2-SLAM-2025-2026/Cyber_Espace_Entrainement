using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Cyber_Espace_Entrainement.Commands;
using System.Windows.Input;

namespace Cyber_Espace_Entrainement.ViewModels.Activite.Captcha
{
    public partial class CaptchaFacileViewModel : ObservableObject
    {
        [ObservableProperty]
        private Dictionary<string, string> dicoPrecedent = new()
        {
            { "jeuCaptcha", "intro" },
            { "jeuCaptcha2", "jeuCaptcha" },
            { "outro", "jeuCaptcha2" }
        };


        [ObservableProperty]
        private bool intro = true;

        [ObservableProperty]
        private bool jeuCaptcha = false;

        [ObservableProperty]
        private bool jeuCaptcha2 = false;

        [ObservableProperty]
        private bool outro = false;

        public ICommand RetourCommand { get; }


        [ObservableProperty]
        private int scoreJeu = 0;

        [ObservableProperty]
        private string couleur;



        public CaptchaFacileViewModel()
        {
            Intro = true;
        }

        [RelayCommand]
        private void JeuCa()
        {
            // Logique pour jouer au Captcha
            // Par exemple, afficher une image Captcha et vérifier la réponse de l'utilisateur
        }

        [RelayCommand]
        private void Niveau1()
        {
            Intro = false;
            JeuCaptcha = true;

        }

        [RelayCommand]
        private void Niveau2()
        {
            JeuCaptcha2 = true;
            JeuCaptcha = false;

        }

        [RelayCommand]
        private void Niveau3()
        {
            Outro = true;
            JeuCaptcha2 = false;

        }



        //[RelayCommand]
        //private void Retour(string pageActuelle, string pageAvant)
        //{
        //    if (pageActuelle == "intro" && pageAvant == "outro")
        //    {
        //        Intro = true;
        //        JeuCaptcha = false;
        //    }
        //}
    }
}
