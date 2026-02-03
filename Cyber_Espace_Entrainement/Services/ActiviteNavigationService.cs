using Cyber_Espace_Entrainement.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Cyber_Espace_Entrainement.Services
{
    public class ActiviteNavigationService
    {
        public void OuvrirVueParActivite(Activites activite)
        {
            string libelle = NormaliserNom(activite.Libelle);

            string viewClassName;

            if (!string.IsNullOrWhiteSpace(activite.Niveau))
            {
                string niveau = NormaliserNom(activite.Niveau);
                viewClassName = $"{libelle}{niveau}View";
            }
            else
            {
                viewClassName = $"{libelle}View";
            }

            string fullName = $"Cyber_Espace_Entrainement.Views.Activite.{viewClassName}.xaml";

            var type = typeof(App).Assembly.GetType($"Cyber_Espace_Entrainement.Views.Activite.{viewClassName}");

            if (type == null)
            {
                MessageBox.Show($"Vue introuvable : {viewClassName}.xaml");
                return;
            }

            var window = (Window)Activator.CreateInstance(type)!;
            window.Show();
        }

        private string NormaliserNom(string texte)
        {
            return texte
                .Replace(" ", "")
                .Replace("-", "")
                .Replace("é", "e")
                .Replace("è", "e")
                .Replace("ê", "e")
                .Replace("à", "a")
                .Replace("ù", "u");
        }
    }
}
