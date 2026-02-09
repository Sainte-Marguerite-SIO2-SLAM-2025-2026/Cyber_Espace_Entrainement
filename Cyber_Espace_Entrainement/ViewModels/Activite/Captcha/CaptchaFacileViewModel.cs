using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cyber_Espace_Entrainement.Commands;
using Cyber_Espace_Entrainement.Models;
using Cyber_Espace_Entrainement.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Cyber_Espace_Entrainement.ViewModels.Activite.Captcha
{
    public partial class CaptchaFacileViewModel : ObservableObject
    {
        private readonly CaptchaService _captchaService;

        [ObservableProperty]
        private ObservableCollection<Captchas> captchas = new();


        #region Property tests
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

        [ObservableProperty]
        private string description;

        #endregion

        public CaptchaFacileViewModel()
        {
            Intro = true;
            _captchaService = new CaptchaService();
            Captchas = new ObservableCollection<Captchas>();

            RecupCaptcha();
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

        private void RecupCaptcha()
        {
            try
            {
                // GetCaptchasTest() retourne un seul objet Captchas (ou null)
                var data = _captchaService.GetCaptchasTest();

                Captchas.Clear();

                if (data is not null)
                {
                    Captchas.Add(data);
                    Description = data.Explication;
                }
                else
                {
                    Description = string.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur de chargement : {ex.Message}");
            }
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
