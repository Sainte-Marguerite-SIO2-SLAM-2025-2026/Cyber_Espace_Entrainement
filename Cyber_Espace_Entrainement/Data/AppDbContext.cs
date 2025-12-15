using Cyber_Espace_Entrainement.Models;
// Data/AppDbContext.cs
using Microsoft.EntityFrameworkCore;


namespace Cyber_Espace_Entrainement.Data
{
        /// <summary>
        /// Contexte de base de données Entity Framework
        /// Gère la connexion et les opérations sur la BDD
        /// </summary>
        public class AppDbContext : DbContext
        {
            // DbSet représente la table users
            public DbSet<User> Users { get; set; }

            // Configuration de la connexion
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                // OPTION 1 : SQLite (pour tester pour le moment en local mais vous devrez le faire en ligne ensuite !)
                optionsBuilder.UseSqlite("Data Source=cyberentrainement.db");

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
                modelBuilder.Entity<User>(entity =>
                {
                    // Index unique sur login
                    entity.HasIndex(u => u.Login).IsUnique();

                    // Index unique sur email
                    entity.HasIndex(u => u.Email).IsUnique();

                    // Conversion de l'enum en string pour MySQL
                    entity.Property(u => u.Role)
                        .HasConversion<string>()
                        .HasMaxLength(20);

                    // Valeur par défaut pour dateCreation
                    entity.Property(u => u.DateCreation)
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");
                });

                // Données de test 
                modelBuilder.Entity<User>().HasData(
                    new User
                    {
                        UserId = 1,
                        Login = "adminProf",
                        MotPasse = HashPassword("admin123"), // devinez !!!! 
                        Email = "prof.admin@sfda37.fr",
                        Role = UserRole.Admin,
                        DateCreation = DateTime.Now
                    },
                    new User
                    {
                        UserId = 2,
                        Login = "Achille.Talon",
                        MotPasse = HashPassword("prof123"),// devinez !!!!
                        Email = "ach.Talon.prof@gmail.com",
                        Role = UserRole.Prof,
                        DateCreation = DateTime.Now
                    },
                    new User
                    {
                        UserId = 3,
                        Login = "gaston",
                        MotPasse = HashPassword("gaston123"),// devinez !!!!
                        Email = "gaston@gmail.com",
                        Role = UserRole.Etudiant,
                        DateCreation = DateTime.Now
                    }
                );
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