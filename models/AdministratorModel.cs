using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace ProjekatA.models
{
    public class AdministratorModel : OsnovnaModel
    {
        public string KorisnickoIme { get; set; }
        public LokacijaModel Lokacija { get; set; }


        public string Adresa { get; set; }
        public int NalogId { get; set; }
        public int LokacijaId { get; set; }

        // Kreiranje novog administratora
        public override int Create()
        {
            Open();
            int lastInsertId = 0;
            string query = "INSERT INTO administrator (nalog_id, lokacija_id) VALUES (@nalogId, @lokacijaId)";
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@nalogId", NalogId);
                cmd.Parameters.AddWithValue("@lokacijaId", LokacijaId);
                cmd.ExecuteNonQuery();
                lastInsertId = (int)cmd.LastInsertedId;
            }
            Close();
            return lastInsertId;
        }


        // Čitanje administratora po ID-u naloga
        public override object Read(int id)
        {
            Open();
            string query = "SELECT * FROM administrator WHERE nalog_id = @id";
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@id", id);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new AdministratorModel
                        {
                            NalogId = reader.GetInt32("nalog_id"),
                            LokacijaId = reader.GetInt32("lokacija_id")
                        };
                    }
                }
            }
            Close();
            return null;
        }

        // Ažuriranje administratora
        public override void Update()
        {
            Open();
            string query = "UPDATE administrator SET lokacija_id = @lokacijaId WHERE nalog_id = @nalogId";
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@lokacijaId", LokacijaId);
                cmd.Parameters.AddWithValue("@nalogId", NalogId);
                cmd.ExecuteNonQuery();
            }
            Close();
        }

        // Brisanje administratora po ID-u naloga
        public override void Delete(int id)
        {
            Open();
            string query = "DELETE FROM administrator WHERE nalog_id = @id";
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            Close();
        }

        // Čitanje svih administratora
        public override List<object> ReadAll()
        {
            Open();
            var list = new List<object>();
            string query = "SELECT * FROM administrator";
            using (var cmd = new MySqlCommand(query, connection))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new AdministratorModel
                    {
                        NalogId = reader.GetInt32("nalog_id"),
                        LokacijaId = reader.GetInt32("lokacija_id")
                    });
                }
            }
            Close();
            return list;
        }
    }
}