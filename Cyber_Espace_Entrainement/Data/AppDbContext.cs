using Cyber_Espace_Entrainement.Models;
// Data/AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using System.IO;


namespace Cyber_Espace_Entrainement.Data
{
    /// <summary>
    /// Contexte de base de données Entity Framework
    /// Gère la connexion et les opérations sur la BDD
    /// MODIFICATION : Adapté pour la nouvelle base de données bdd_cyberespace.db
    /// </summary>
    public class AppDbContext : DbContext
    {
        // DbSet représente la table Utilisateur
        // MODIFIÉ : Table renommée de 'users' à 'Utilisateur'
        public DbSet<Utilisateurs> Users { get; set; }

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

        }

        // Méthode simple de hashage (à améliorer avec BCrypt) - INCHANGÉE
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