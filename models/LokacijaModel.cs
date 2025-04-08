using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace ProjekatA.models
{
    public class LokacijaModel : OsnovnaModel
    {
        public int Id { get; set; }
        public string Adresa { get; set; }
        public string Grad { get; set; }
        public string Drzava { get; set; }

        // Kreiranje nove lokacije
        public override int Create()
        {
            Open();
            int lastInsertId = 0;
            string query = "INSERT INTO lokacija (adresa, grad, drzava) VALUES (@adresa, @grad, @drzava)";
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@adresa", Adresa);
                cmd.Parameters.AddWithValue("@grad", Grad);
                cmd.Parameters.AddWithValue("@drzava", Drzava);
                cmd.ExecuteNonQuery();
                lastInsertId = (int)cmd.LastInsertedId;
            }
            Close();
            return lastInsertId;
        }


        // Čitanje lokacije po ID-u
        public override object Read(int id)
        {
            Open();
            string query = "SELECT * FROM lokacija WHERE id = @id";
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@id", id);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new LokacijaModel
                        {
                            Id = reader.GetInt32("id"),
                            Adresa = reader.GetString("adresa"),
                            Grad = reader.GetString("grad"),
                            Drzava = reader.GetString("drzava")
                        };
                    }
                }
            }
            Close();
            return null;
        }

        // Ažuriranje lokacije
        public override void Update()
        {
            Open();
            string query = "UPDATE lokacija SET adresa = @adresa, grad = @grad, drzava = @drzava WHERE id = @id";
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@adresa", Adresa);
                cmd.Parameters.AddWithValue("@grad", Grad);
                cmd.Parameters.AddWithValue("@drzava", Drzava);
                cmd.Parameters.AddWithValue("@id", Id);
                cmd.ExecuteNonQuery();
            }
            Close();
        }

        // Brisanje lokacije
        public override void Delete(int id)
        {
            Open();
            string query = "DELETE FROM lokacija WHERE id = @id";
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            Close();
        }

        // Čitanje svih lokacija
        public override List<object> ReadAll()
        {
            Open();
            var list = new List<object>();
            string query = "SELECT * FROM lokacija";
            using (var cmd = new MySqlCommand(query, connection))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new LokacijaModel
                    {
                        Id = reader.GetInt32("id"),
                        Adresa = reader.GetString("adresa"),
                        Grad = reader.GetString("grad"),
                        Drzava = reader.GetString("drzava")
                    });
                }
            }
            Close();
            return list;
        }
        public int VratiId(string adresa)
        {
            int rezultat = 0;
            foreach (var x in new LokacijaModel().ReadAll())
            {
                if (adresa == ((LokacijaModel)x).Adresa)
                {
                    rezultat = ((LokacijaModel)x).Id;
                }
            }

            return rezultat;
        }

        public override string ToString()
        {
            return $"{Adresa} {Grad} {Drzava}";
        }
    }
}
