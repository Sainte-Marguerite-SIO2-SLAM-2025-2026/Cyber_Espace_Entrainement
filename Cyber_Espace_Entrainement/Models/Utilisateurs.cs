using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cyber_Espace_Entrainement.Models
{
    /// <summary>
    /// Modèle User correspondant à la table Utilisateur de la BDD
    /// MODIFICATION : Table renommée de 'users' à 'Utilisateur'
    /// </summary>
    [Table("Utilisateur")]
    public class Utilisateurs
    {
        [Key]
        [Column("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)] 
        [Column("Login")]
        public string Login { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [Column("Password")]
        public string MotPasse { get; set; } = string.Empty;

        
        [MaxLength(50)]
        [Column("Nom")]
        public string? Nom { get; set; }

        
        [MaxLength(50)]
        [Column("Prenom")]
        public string? Prenom { get; set; }


        [Required]
        [MaxLength(255)]
        [Column("Email")]
        public string Email { get; set; } = string.Empty;


        [MaxLength(15)]
        [Column("Section")]
        public string? Section { get; set; }

        [Required]
        [Column("Role")] 
        public UserRole Role { get; set; } = UserRole.Prof;

        
        [Column("ScoreTotal")]
        public int? ScoreTotal { get; set; }

        [Column("DateCreation")]
        public DateTime DateCreation { get; set; } = DateTime.Now;

        [Column("DerniereConnexion")]
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
    /// Énumération des rôles (correspondant aux valeurs VARCHAR dans SQLite)
    /// </summary>
    public enum UserRole
    {
        Etudiant = 0,
        Prof = 1,
        Admin = 2
    }
}