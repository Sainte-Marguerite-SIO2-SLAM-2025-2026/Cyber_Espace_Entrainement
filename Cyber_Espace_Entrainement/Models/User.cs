using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cyber_Espace_Entrainement.Models
{
    /// <summary>
    /// Modèle User correspondant à la table users de la BDD
    /// </summary>
    [Table("users")]
    public class User
    {
            [Key]
            [Column("user_id")]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int UserId { get; set; }

            [Required]
            [MaxLength(30)]
            [Column("login")]
            public string Login { get; set; } = string.Empty;

            [Required]
            [MaxLength(255)]
            [Column("motPasse")]
            public string MotPasse { get; set; } = string.Empty;

            [Required]
            [MaxLength(100)]
            [Column("email")]
            public string Email { get; set; } = string.Empty;

            [Required]
            [Column("role")]
            public UserRole Role { get; set; } = UserRole.Prof;

            [Column("dateCreation")]
            public DateTime DateCreation { get; set; } = DateTime.Now;

            [Column("derniereConnexion")]
            public DateTime? DerniereConnexion { get; set; }

            // Propriété calculée pour l'affichage
            [NotMapped]
            public string RoleDisplay => Role switch
            {
                UserRole.Etudiant => "Etudiant",
                UserRole.Prof => "Professeur",
                UserRole.Admin => "Administrateur",
                _ => "Inconnu"
            };
        }

        /// <summary>
        /// Énumération des rôles (correspondant à ENUM dans MySQL)
        /// </summary>
        public enum UserRole
        {
            Etudiant = 0,
            Prof = 1,
            Admin = 2
        }
    }