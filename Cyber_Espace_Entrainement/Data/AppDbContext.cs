using Cyber_Espace_Entrainement.Models;
using Cyber_Espace_Entrainement.Models.InjectionSQL;

using Cyber_Espace_Entrainement.Models.UserEnumeration;

// Data/AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Windows;
using System.Windows.Media.Animation;


namespace Cyber_Espace_Entrainement.Data
{
    /// <summary>
    /// Contexte de base de données Entity Framework
    /// Gère la connexion et les opérations sur la BDD
    /// </summary>
    public class AppDbContext : DbContext
    {
        // DbSet représente la table Utilisateur
        public DbSet<Utilisateurs> Users { get; set; }

        //Représente la table Activite
        public DbSet<Activites> Activites { get; set; }

        //Représente la table Cours
        public DbSet<Cours> Cours { get; set; }

        // Représente la table Phishing
        public DbSet<Phishing> Phishing { get; set; }

        public DbSet<LogConnexion> logConnexion { get; set; }

        public DbSet<Captchas> Captcha { get; set; }
        //Représente la table UserEnumeration
        public DbSet<UserEnumeration> userEnumeration { get; set; }


        // Configuration de la connexion
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Permet d'aller chercher la BDD dans le répertoire du projet
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo? projectDir = Directory.GetParent(baseDir)?.Parent?.Parent?.Parent?.Parent;

            string dbPath = projectDir != null
                ? Path.Combine(projectDir.FullName, "bdd_cyberespace.db")
                : "bdd_cyberespace.db"; // Fallback

            optionsBuilder.UseSqlite($"Data Source={dbPath}");

            // OPTION 2 : MySQL (je l'ai préparé, au cas où !)
            // optionsBuilder.UseMySql(
            //     "Server=localhost;Database=cyberentrainement;User=cyberentraineur;Password=aVousDeVoir;",
            //     ServerVersion.AutoDetect("Server=localhost;Database=cyberentrainement;User=cyberentraineur;Password=aVousDeVoir;")
            // );
        }

        // Configuration du modèle
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration de l'entité User
            modelBuilder.Entity<Utilisateurs>(entity =>
            {
                // Index unique sur login - INCHANGÉ
                entity.HasIndex(u => u.Login).IsUnique();

                // Index unique sur email - INCHANGÉ
                entity.HasIndex(u => u.Email).IsUnique();

                // MODIFIÉ : Conversion de l'enum en string avec longueur de 14 (longueur max dans la BDD)
                // Ancienne valeur : HasMaxLength(20)
                entity.Property(u => u.Role)
                    .HasConversion<string>()
                    .HasMaxLength(14);

                // Valeur par défaut pour dateCreation - INCHANGÉ
                entity.Property(u => u.DateCreation)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                // AJOUTÉ : Configuration pour les nouveaux champs optionnels
                entity.Property(u => u.Nom).IsRequired(false);
                entity.Property(u => u.Prenom).IsRequired(false);
                entity.Property(u => u.Section).IsRequired(false);
                entity.Property(u => u.ScoreTotal).IsRequired(false).HasDefaultValue(0);
            });

            // COMMENTÉ : Données de test désactivées pour ne pas modifier la base existante
            // Si vous souhaitez ajouter des utilisateurs de test, décommentez cette section

            modelBuilder.Entity<Utilisateurs>().HasData(
                new Utilisateurs
                {
                    UserId = 1,
                    Login = "adminProf",
                    MotPasse = HashPassword("admin123"),
                    Email = "prof.admin@sfda37.fr",
                    Role = UserRole.Admin,
                    DateCreation = DateTime.Now
                },
                new Utilisateurs
                {
                    UserId = 2,
                    Login = "Achille.Talon",
                    MotPasse = HashPassword("prof123"),
                    Email = "ach.Talon.prof@gmail.com",
                    Role = UserRole.Prof,
                    DateCreation = DateTime.Now
                },
                new Utilisateurs
                {
                    UserId = 3,
                    Login = "gaston",
                    MotPasse = HashPassword("gaston123"),
                    Email = "gaston@gmail.com",
                    Role = UserRole.Etudiant,
                    DateCreation = DateTime.Now
                }
            );

            modelBuilder.Entity<Activites>(entity =>
            {
                // 1. Correction du nom de la table (doit correspondre à l'attribut [Table("Activite")] du modèle)
                entity.ToTable("Activite");

                // 2. IMPORTANT : Définir la clé primaire composée ici aussi pour éviter les conflits
                entity.HasKey(a => new { a.Id, a.CoursId });

                // 3. Configuration des colonnes (Assurez-vous que les noms correspondent à la BDD)
                entity.Property(a => a.Id).HasColumnName("ID");
                entity.Property(a => a.CoursId).HasColumnName("CoursID");

                // Propriétés nullable (vos réglages actuels sont corrects)
                entity.Property(a => a.Libelle).IsRequired(false);
                entity.Property(a => a.Image).IsRequired(false);
                entity.Property(a => a.Contenu).IsRequired(false);
                entity.Property(a => a.Explication).IsRequired(false);
                entity.Property(a => a.Type).IsRequired(false);
                entity.Property(a => a.Niveau).IsRequired(false);
            });

            modelBuilder.Entity<Cours>(entity =>
            {
                entity.ToTable("Cours");
                entity.HasKey(c => c.ID);
                entity.Property(c => c.ID).HasColumnName("ID");
                entity.Property(c => c.Titre).IsRequired(false);
                entity.Property(c => c.Definition).IsRequired(false);
                entity.Property(c => c.Explication).IsRequired(false);
                entity.Property(c => c.Exemple).IsRequired(false);
                entity.Property(c => c.Image1).IsRequired(false);
                entity.Property(c => c.Image2).IsRequired(false);
                entity.Property(c => c.Image3).IsRequired(false);
                entity.Property(c => c.Lien).IsRequired(false);
                entity.Property(c => c.Theme).IsRequired(false);
                entity.Property(c => c.ImageBouton).IsRequired(false);

            });

            modelBuilder.Entity<Phishing>(entity =>
            {
                entity.ToTable("Phishing");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).HasColumnName("ID");
                entity.Property(p => p.ActiviteId).IsRequired(true);
                entity.Property(p => p.CoursId).IsRequired(true);
                entity.Property(p => p.Type).IsRequired(false);
                entity.Property(p => p.Image).IsRequired(false);
                entity.Property(p => p.Expediteur).IsRequired(false);
                entity.Property(p => p.Objet).IsRequired(false);
                entity.Property(p => p.Contenu).IsRequired(false);

            });

            // MAPPING DE L'ENTITÉ Captcha : correspondance explicite entre propriétés et colonnes
            modelBuilder.Entity<Captchas>(entity =>
            {
                entity.ToTable("Captcha");

                // On définit la clé primaire sur la propriété du modèle (CaptchaId)
                entity.HasKey(c => c.CaptchaId);

                // Correspondances colonne <-> propriété (selon votre schéma demandé)
                entity.Property(c => c.CaptchaId).HasColumnName("CaptchaID");
                entity.Property(c => c.ActiviteId).HasColumnName("ActiviteID");
                entity.Property(c => c.CourdId).HasColumnName("CoursID");

                entity.Property(c => c.Explication)
                      .HasColumnName("Explication")
                      .IsRequired(false);

                entity.Property(c => c.Zone)
                      .HasColumnName("Zone")
                      .IsRequired(false);

                entity.Property(c => c.Image)
                      .HasColumnName("Image")
                      .IsRequired(false);

                entity.Property(c => c.Valide)
                      .HasColumnName("Valide")
                      .IsRequired();
                modelBuilder.Entity<UserEnumeration>(entity =>
                {
                    entity.ToTable("UserEnumeration");
                    entity.HasKey(c => new { c.Id, c.CoursId, c.ActiviteId });
                    entity.Property(c => c.Id).HasColumnName("ID");
                    entity.Property(c => c.CoursId).HasColumnName("CoursID");
                    entity.Property(c => c.ActiviteId).HasColumnName("ActiviteID");
                    entity.Property(c => c.Image).IsRequired(false);
                    entity.Property(c => c.Reponse);
                    entity.Property(c => c.Message).IsRequired(false);
                });
            });

            // MAPPING DE L'ENTITÉ InjectionSQL : correspondance explicite entre propriétés et colonnes
            modelBuilder.Entity<InjectionSQL>(entity =>
            {
                entity.ToTable("InjectionSQL");
                // Clé primaire
                entity.HasKey(i => i.Id);
                // Correspondances colonne <-> propriété
                entity.Property(i => i.Id).HasColumnName("ID");
                entity.Property(i => i.CoursId).HasColumnName("CoursID");
                entity.Property(i => i.Login).HasColumnName("Login").IsRequired(false);
                entity.Property(i => i.Password).HasColumnName("Password").IsRequired(false);
                entity.Property(i => i.SoldeCompte).HasColumnName("SoldeCompte");
                entity.Property(i => i.Nom).HasColumnName("Nom").IsRequired(false);
                entity.Property(i => i.Prenom).HasColumnName("Prenom").IsRequired(false);
            });
        
        }

        

        
        // Méthode simple de hashage (à améliorer avec BCrypt)
        private static string HashPassword(string password)
        {
            // Pour l'exemple : utilisation simple (PAS SÉCURISÉ, donc il faudra la modifier )
            // Utiliser BCrypt.Net
            return Convert.ToBase64String(
                System.Text.Encoding.UTF8.GetBytes(password)
            );
        }
    }
}