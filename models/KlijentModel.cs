using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace ProjekatA.models
{
    public class KlijentModel : OsnovnaModel
    {
        public string KorisnickoIme { get; set; }
        public int NalogId { get; set; }
        public string OpisKlijenta { get; set; }

        // Kreiranje novog klijenta
        public override int Create()
        {
            Open();
            int lastInsertId = 0;
            string query = "INSERT INTO klijent (nalog_id, opisKlijenta) VALUES (@nalogId, @opisKlijenta)";
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@nalogId", NalogId);
                cmd.Parameters.AddWithValue("@opisKlijenta", OpisKlijenta);
                cmd.ExecuteNonQuery();
                lastInsertId = (int)cmd.LastInsertedId;
            }
            Close();
            return lastInsertId;
        }


        // Čitanje klijenta po nalog_id
        public override object Read(int id)
        {
            Open();
            string query = "SELECT * FROM klijent WHERE nalog_id = @id";
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@id", id);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new KlijentModel
                        {
                            NalogId = reader.GetInt32("nalog_id"),
                            OpisKlijenta = reader.GetString("opisKlijenta")
                        };
                    }
                }
            }
            Close();
            return null;
        }

        // Ažuriranje klijenta
        public override void Update()
        {
            Open();
            string query = "UPDATE klijent SET opisKlijenta = @opisKlijenta WHERE nalog_id = @nalogId";
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@opisKlijenta", OpisKlijenta);
                cmd.Parameters.AddWithValue("@nalogId", NalogId);
                cmd.ExecuteNonQuery();
            }
            Close();
        }

        // Brisanje klijenta
        public override void Delete(int id)
        {
            Open();
            string query = "DELETE FROM klijent WHERE nalog_id = @id";
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            Close();
        }

        // Čitanje svih klijenata
        public override List<object> ReadAll()
        {
            Open();
            var list = new List<object>();
            string query = "SELECT * FROM klijent";
            using (var cmd = new MySqlCommand(query, connection))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new KlijentModel
                    {
                        NalogId = reader.GetInt32("nalog_id"),
                        OpisKlijenta = reader.GetString("opisKlijenta")
                    });
                }
            }
            Close();
            return list;
        }
    }
}
