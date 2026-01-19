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
    [Table("Utilisateur")] // MODIFIÉ : Nom de table changé
    public class User
    {
        [Key]
        [Column("ID")] // MODIFIÉ : Colonne renommée de 'user_id' à 'ID'
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)] // MODIFIÉ : Taille augmentée de 30 à 50
        [Column("Login")] // MODIFIÉ : Première lettre en majuscule
        public string Login { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)] // MODIFIÉ : Taille réduite de 255 à 100
        [Column("Password")] // MODIFIÉ : Colonne renommée de 'motPasse' à 'Password'
        public string MotPasse { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)] // Taille inchangée
        [Column("Email")] // MODIFIÉ : Première lettre en majuscule
        public string Email { get; set; } = string.Empty;

        // AJOUTÉ : Nouveau champ Nom
        [MaxLength(50)]
        [Column("Nom")]
        public string? Nom { get; set; }

        // AJOUTÉ : Nouveau champ Prenom
        [MaxLength(50)]
        [Column("Prenom")]
        public string? Prenom { get; set; }

        // AJOUTÉ : Nouveau champ Section
        [MaxLength(5)]
        [Column("Section")]
        public string? Section { get; set; }

        [Required]
        [Column("Role")] // MODIFIÉ : Première lettre en majuscule
        public UserRole Role { get; set; } = UserRole.Prof;

        // AJOUTÉ : Nouveau champ ScoreTotal
        [Column("ScoreTotal")]
        public int? ScoreTotal { get; set; }

        [Column("DateCreation")] // MODIFIÉ : Première lettre en majuscule
        public DateTime DateCreation { get; set; } = DateTime.Now;

        [Column("DerniereConnexion")] // MODIFIÉ : Première lettre en majuscule
        public DateTime? DerniereConnexion { get; set; }

        // Propriété calculée pour l'affichage - INCHANGÉE
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
    /// MODIFICATION : Les valeurs correspondent maintenant aux valeurs texte de la BDD
    /// </summary>
    public enum UserRole
    {
        Etudiant = 0,
        Prof = 1,
        Admin = 2
    }
}