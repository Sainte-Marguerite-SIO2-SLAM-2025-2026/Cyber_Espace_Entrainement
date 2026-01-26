using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cyber_Espace_Entrainement.ViewModels.Activite.Captcha
{
    public partial class CaptchaFacileViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool intro = true;

        [ObservableProperty]
        private bool jeuCaptcha = false;

        [ObservableProperty]
        private bool jeuCaptcha2 = false;

        [ObservableProperty]
        private bool outro = false;



        [ObservableProperty]
        private int scoreJeu = 0;

        [ObservableProperty]
        private string couleur;

        private bool _isCaptchaValid;


        public CaptchaFacileViewModel()
        {
            Intro = true;
        }

        [RelayCommand]
        private void JeuCa()
        {
            // Logique pour jouer au Captcha
            // Par exemple, afficher une image Captcha et vérifier la réponse de l'utilisateur
            if (_isCaptchaValid)
            {
                
            }
        }

        [RelayCommand]
        private void Refresh()
        {
            Intro = false;
            JeuCaptcha = true;
            
        }
    }
}
