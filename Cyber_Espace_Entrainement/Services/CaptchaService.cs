using Cyber_Espace_Entrainement.Data;
using Cyber_Espace_Entrainement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Cyber_Espace_Entrainement.Services
{
    /// <summary>
    /// Service pour gérer les opérations sur les captchas
    /// </summary>
    public class CaptchaService : IDisposable
    {
        private readonly AppDbContext _context;

        public CaptchaService()
        {
            _context = new AppDbContext();
            _context.Database.EnsureCreated();
        }

        // Récupérer tous les captchas
        public List<Captchas> GetAllCaptchas()
        {
            try
            {
                return _context.Captcha
                    .OrderBy(c => c.CaptchaId)
                    .ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erreur GetAllCaptchas : {ex.Message}");
                return new List<Captchas>();
            }
        }

        public Captchas? GetCaptchasTest()
        {
            try
            {
                return _context.Captcha.Find(1);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erreur test de recup captcha : {ex.Message}");
                return null;
            }
        }

        // Récupérer un captcha par id
        public Captchas? GetCaptchaById(int id)
        {
            try
            {
                return _context.Captcha.Find(id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erreur GetCaptchaById : {ex.Message}");
                return null;
            }
        }

        // Récupérer les captchas d'une activité
        public List<Captchas> GetCaptchasByActivite(int activiteId)
        {
            try
            {
                return _context.Captcha
                    .Where(c => c.ActiviteId == activiteId)
                    .OrderBy(c => c.CaptchaId)
                    .ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erreur GetCaptchasByActivite : {ex.Message}");
                return new List<Captchas>();
            }
        }

        // Récupérer les captchas d'un cours
        public List<Captchas> GetCaptchasByCours(int coursId)
        {
            try
            {
                return _context.Captcha
                    .Where(c => c.CourdId == coursId)
                    .OrderBy(c => c.CaptchaId)
                    .ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erreur GetCaptchasByCours : {ex.Message}");
                return new List<Captchas>();
            }
        }

        // Mettre à jour un captcha
        public bool UpdateCaptcha(Captchas captcha)
        {
            try
            {
                var existing = _context.Captcha.Find(captcha.CaptchaId);
                if (existing == null) return false;

                // Mettre à jour les champs
                existing.ActiviteId = captcha.ActiviteId;
                existing.CourdId = captcha.CourdId;
                existing.Explication = captcha.Explication;
                existing.Zone = captcha.Zone;
                existing.Image = captcha.Image;
                existing.Valide = captcha.Valide;

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erreur UpdateCaptcha : {ex.Message}");
                return false;
            }
        }

        // Supprimer un captcha
        public bool DeleteCaptcha(int id)
        {
            try
            {
                var entity = _context.Captcha.Find(id);
                if (entity == null) return false;

                _context.Captcha.Remove(entity);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erreur DeleteCaptcha : {ex.Message}");
                return false;
            }
        }

        // Dispose du contexte
        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
