using Cyber_Espace_Entrainement.Data;
using Cyber_Espace_Entrainement.Models.InjectionSQL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cyber_Espace_Entrainement.Services
{
    /// <summary>
    /// Service pour le niveau Facile de l'activité Injection SQL.
    ///
    /// Principe :
    ///  • Charge 3 enregistrements aléatoires depuis la table InjectionSQL,
    ///    filtrés sur l'activité de niveau "Facile" dans la table Activite.
    ///  • La vérification du login/mot de passe est SIMULÉE en C# —
    ///    aucune requête non-paramétrée n'est envoyée à la base.
    ///  • La chaîne SQL affichée à l'écran est construite par concaténation
    ///    (purement pédagogique, jamais exécutée).
    /// </summary>
    public class InjectionSQLFacileService
    {
        // Nombre d'utilisateurs à charger (peut évoluer)
        private const int NbUsers = 3;

        // ──────────────────────────────────────────────────────────────
        // Chargement des utilisateurs
        // ──────────────────────────────────────────────────────────────

        /// <summary>
        /// Retourne <see cref="NbUsers"/> enregistrements aléatoires de la table InjectionSQL
        /// dont l'ActiviteId correspond à l'activité de niveau "Facile" dans la table Activite.
        /// </summary>
        public List<InjectionSQL> GetRandomUsers()
        {
            using AppDbContext db = new();

            // 1. Récupère l'ID de l'activité InjectionSQL niveau Facile
            //    On cherche dans Activite un enregistrement dont le Niveau == "Facile"
            //    et dont le Libelle contient "Injection" (ajustez selon vos données réelles).
            int? activiteId = db.Activites
                .Where(a => a.Niveau != null
                         && a.Niveau.ToLower() == "facile"
                         && a.Libelle != null
                         && a.Libelle.ToLower().Contains("injection"))
                .Select(a => (int?)a.Id)
                .FirstOrDefault();

            if (activiteId is null)
            {
                // Fallback : on prend n'importe quels enregistrements, mélangés côté C#
                return db.Set<InjectionSQL>()
                         .ToList()
                         .OrderBy(_ => Guid.NewGuid())
                         .Take(NbUsers)
                         .ToList();
            }

            // 2. Charge tous les enregistrements liés à l'activité en mémoire,
            //    puis mélange et tronque côté C# (Guid.NewGuid() n'est pas traduisible en SQL)
            var all = db.Set<InjectionSQL>()
                        .Where(u => u.ActiviteId == activiteId.Value)
                        .ToList();

            return all.OrderBy(_ => Guid.NewGuid())
                      .Take(NbUsers)
                      .ToList();
        }

        // ──────────────────────────────────────────────────────────────
        // Construction de la requête SQL (affichage pédagogique)
        // ⚠️  Jamais exécutée — concaténation volontairement vulnérable
        // ──────────────────────────────────────────────────────────────

        /// <summary>
        /// Construit la chaîne SQL telle qu'elle apparaîtrait dans une application
        /// non sécurisée. Utilisée uniquement pour l'affichage en temps réel.
        /// </summary>
        public string BuildSqlQuery(string login, string password)
        {
            return $"SELECT * FROM InjectionSQL\nWHERE login = '{login}'\nAND password = '{password}'";
        }

        // ──────────────────────────────────────────────────────────────
        // Simulation de la tentative de connexion
        // ──────────────────────────────────────────────────────────────

        /// <summary>
        /// Simule le comportement d'une requête SQL vulnérable :
        ///  - Si le mot de passe contient une injection OR-bypass → retourne le premier utilisateur
        ///    de la liste (comme le ferait un vrai moteur SQL sans paramétrage).
        ///  - Sinon, vérifie login + password normalement dans la liste en mémoire.
        /// </summary>
        /// <param name="users">La liste chargée au démarrage (3 enregistrements).</param>
        /// <param name="login">Login affiché / pré-rempli.</param>
        /// <param name="password">Valeur saisie dans le champ mot de passe.</param>
        public InjectionSQL? SimulateLogin(IReadOnlyList<InjectionSQL> users, string login, string password)
        {
            // ── Injection détectée dans le champ mot de passe ────────
            if (ContainsOrBypass(password))
            {
                // Le SQL serait :  ... AND password = '' OR '1'='1' --'
                // La condition devient toujours vraie → premier enregistrement renvoyé
                return users.FirstOrDefault();
            }

            // ── Authentification légitime ─────────────────────────────
            return users.FirstOrDefault(u =>
                u.Login.Equals(login, StringComparison.OrdinalIgnoreCase) &&
                u.Password == password);
        }

        // ──────────────────────────────────────────────────────────────
        // Détection des patterns OR-bypass (champ mot de passe)
        // ──────────────────────────────────────────────────────────────

        private static bool ContainsOrBypass(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return false;

            string normalized = input.ToUpperInvariant()
                                     .Replace(" ", "")
                                     .Replace("\t", "");

            bool hasQuote = input.Contains('\'');
            bool hasOrKeyword = normalized.Contains("OR");
            bool hasComment = input.Contains("--") || input.Contains("#");
            bool hasTautology = normalized.Contains("'1'='1")
                             || normalized.Contains("1=1")
                             || normalized.Contains("'X'='X")
                             || normalized.Contains("'A'='A");

            return hasQuote && hasOrKeyword && (hasComment || hasTautology);
        }
    }
}