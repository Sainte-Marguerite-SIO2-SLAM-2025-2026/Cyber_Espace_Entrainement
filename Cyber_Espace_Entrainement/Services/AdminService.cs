using Cyber_Espace_Entrainement.Data;
using Cyber_Espace_Entrainement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Cyber_Espace_Entrainement.Services
{
    public class AdminService
    {
        private readonly AppDbContext _context;

        public AdminService()
        {
            _context = new AppDbContext();
            // S'assurer que la base existe
            _context.Database.EnsureCreated();
        }

        public List<Admin> GetAllAdmin()
        {
            try
            {
                return _context.Admin
                    .Where(a => a.Icon != null) // ne pas prendre les tables vides (celles sans icon)
                    .OrderBy(a => a.Table)
                    .ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur : {ex.Message}");
                return new List<Admin>();
            }
        }


        public void AjouterLigne(string nomTable, System.Collections.Generic.Dictionary<string, object?> valeurs)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nomTable)) return;

                var colonnes = string.Join(", ", valeurs.Keys.Select(k => $"[{k}]"));
                var parametres = string.Join(", ", valeurs.Keys.Select(k => $"@{k}"));
                var sql = $"INSERT INTO [{nomTable}] ({colonnes}) VALUES ({parametres})";

                var conn = _context.Database.GetDbConnection();
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = sql;

                foreach (var kvp in valeurs)
                {
                    var param = cmd.CreateParameter();
                    param.ParameterName = $"@{kvp.Key}";
                    param.Value = kvp.Value ?? DBNull.Value;
                    cmd.Parameters.Add(param);
                }

                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur AjouterLigne : {ex.Message}");
                throw;
            }
        }

        public void ModifierLigne(string nomTable, string nomColonnePrimaire, object cléValeur, System.Collections.Generic.Dictionary<string, object?> valeurs)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nomTable)) return;

                var setClause = string.Join(", ", valeurs.Keys.Select(k => $"[{k}] = @{k}"));
                var sql = $"UPDATE [{nomTable}] SET {setClause} WHERE [{nomColonnePrimaire}] = @clePrimaire";

                var conn = _context.Database.GetDbConnection();
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = sql;

                var paramCle = cmd.CreateParameter();
                paramCle.ParameterName = "@clePrimaire";
                paramCle.Value = cléValeur;
                cmd.Parameters.Add(paramCle);

                foreach (var kvp in valeurs)
                {
                    var param = cmd.CreateParameter();
                    param.ParameterName = $"@{kvp.Key}";
                    param.Value = kvp.Value ?? DBNull.Value;
                    cmd.Parameters.Add(param);
                }

                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur ModifierLigne : {ex.Message}");
                throw;
            }
        }

        public void SupprimerLigne(string nomTable, string nomColonnePrimaire, object cléValeur)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nomTable)) return;

                var sql = $"DELETE FROM [{nomTable}] WHERE [{nomColonnePrimaire}] = @clePrimaire";
                var conn = _context.Database.GetDbConnection();

                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = sql;

                var param = cmd.CreateParameter();
                param.ParameterName = "@clePrimaire";
                param.Value = cléValeur;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur SupprimerLigne : {ex.Message}");
                throw;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public DataTable? ChargerTable(string nomTable)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nomTable))
                    throw new ArgumentException("Le nom de la table est invalide.", nameof(nomTable));

                // Remplacement de l'appel manquant GetConfigurationOuException
                // On sélectionne toutes les colonnes de la table demandée
                var sql = $"SELECT * FROM [{nomTable}]";

                var dt = new DataTable();
                var conn = _context.Database.GetDbConnection();

                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                using var reader = cmd.ExecuteReader();
                dt.Load(reader);
                conn.Close();

                return dt;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur ChargerTable : {ex.Message}");
                return null;
            }
        }
    }
}