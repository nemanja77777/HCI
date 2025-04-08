using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace ProjekatA.models
{
    public class NalogModel : OsnovnaModel
    {
        public int Id { get; set; }
        public string KorisnickoIme { get; set; }
        public string Sifra { get; set; }

        // Kreiranje novog naloga
        public override int Create()
        {
            Open();
            int lastInsertId = 0;
            string query = "INSERT INTO nalog (korisnickoIme, sifra) VALUES (@korisnickoIme, @sifra)";
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@korisnickoIme", KorisnickoIme);
                cmd.Parameters.AddWithValue("@sifra", Sifra);
                cmd.ExecuteNonQuery();
                lastInsertId = (int)cmd.LastInsertedId;
            }
            Close();
            return lastInsertId;
        }


        // Čitanje naloga po ID-u
        public override object Read(int id)
        {
            Open();
            string query = "SELECT * FROM nalog WHERE id = @id";
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@id", id);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new NalogModel
                        {
                            Id = reader.GetInt32("id"),
                            KorisnickoIme = reader.GetString("korisnickoIme"),
                            Sifra = reader.GetString("sifra")
                        };
                    }
                }
            }
            Close();
            return null;
        }

        // Ažuriranje naloga
        public override void Update()
        {
            Open();
            string query = "UPDATE nalog SET korisnickoIme = @korisnickoIme, sifra = @sifra WHERE id = @id";
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@korisnickoIme", KorisnickoIme);
                cmd.Parameters.AddWithValue("@sifra", Sifra);
                cmd.Parameters.AddWithValue("@id", Id);
                cmd.ExecuteNonQuery();
            }
            Close();
        }

        // Brisanje naloga po ID-u
        public override void Delete(int id)
        {
            Open();
            string query = "DELETE FROM nalog WHERE id = @id";
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            Close();
        }

        // Čitanje svih naloga
        public override List<object> ReadAll()
        {
            Open();
            var list = new List<object>();
            string query = "SELECT * FROM nalog";
            using (var cmd = new MySqlCommand(query, connection))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new NalogModel
                    {
                        Id = reader.GetInt32("id"),
                        KorisnickoIme = reader.GetString("korisnickoIme"),
                        Sifra = reader.GetString("sifra")
                    });
                }
            }
            Close();
            return list;
        }

        public int VratiId(string korisnickoIme)
        {
            int rezultat = 0;
            foreach (var x in new NalogModel().ReadAll())
            {
                if (korisnickoIme == ((NalogModel)x).KorisnickoIme)
                {
                    rezultat =  ((NalogModel)x).Id;
                }
            }

            return rezultat;
        }
    }
}
