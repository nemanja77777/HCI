using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Windows;

namespace ProjekatA.models
{
    public class ZaposleniModel : OsnovnaModel
    {
        public string KorisnickoIme { get; set; }
        public int NalogId { get; set; }
        public string Zvanje { get; set; }

        // Kreiranje novog zaposlenog
        public override int Create()
        {
            Open();
            int lastInsertId = 0;
            string query = "INSERT INTO zaposleni (nalog_id, zvanje) VALUES (@nalog_id, @zvanje)";
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@nalog_id", NalogId);
                cmd.Parameters.AddWithValue("@zvanje", Zvanje);
                cmd.ExecuteNonQuery();
                lastInsertId = (int)cmd.LastInsertedId;
            }
            Close();
            return lastInsertId;
        }


        // Čitanje zaposlenog po ID-u
        public override object Read(int id)
        {
            Open();
            string query = "SELECT * FROM zaposleni WHERE nalog_id = @id";
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@id", id);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new ZaposleniModel
                        {
                            NalogId = reader.GetInt32("nalog_id"),
                            Zvanje = reader.GetString("zvanje")
                        };
                    }
                }
            }
            Close();
            return null;
        }

        // Ažuriranje zaposlenog
        public override void Update()
        {
            Open();
            string query = "UPDATE zaposleni SET zvanje = @zvanje WHERE nalog_id = @nalog_id";
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@zvanje", Zvanje);
                cmd.Parameters.AddWithValue("@nalog_id", NalogId);
                cmd.ExecuteNonQuery();
            }
            Close();
        }

        // Brisanje zaposlenog
        public override void Delete(int id)
        {
            Open();
            string query = "DELETE FROM zaposleni WHERE nalog_id = @id";
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            Close();
        }

        // Čitanje svih zaposlenih
        public override List<object> ReadAll()
        {
            Open();
            var list = new List<object>();
            string query = "SELECT * FROM zaposleni";
            using (var cmd = new MySqlCommand(query, connection))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new ZaposleniModel
                    {
                        NalogId = reader.GetInt32("nalog_id"),
                        Zvanje = reader.GetString("zvanje")
                    });
                }
            }
            Close();
            return list;
        }
    }
}
