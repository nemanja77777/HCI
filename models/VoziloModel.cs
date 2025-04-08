using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace ProjekatA.models
{
    public class VoziloModel : OsnovnaModel
    {
        public string KorisnickoIme { get; set; }
        public int Id { get; set; }
        public string VrstaRegistracije { get; set; }
        public string Model { get; set; }
        public DateTime DatumProizvodnje { get; set; }
        public string DatumProizvodnjeString { get; set; }
        public int KlijentNalogId { get; set; }


        public override int Create()
        {
            Open();
            int lastInsertId = 0;

            string query = "INSERT INTO vozilo (vrstaRegistracije, model, datumProizvodnje, klijent_nalog_id) " +
                           "VALUES (@vrstaRegistracije, @model, @datumProizvodnje, @klijent_nalog_id);";

            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@vrstaRegistracije", VrstaRegistracije);
                cmd.Parameters.AddWithValue("@model", Model);
                cmd.Parameters.AddWithValue("@datumProizvodnje", DatumProizvodnje);
                cmd.Parameters.AddWithValue("@klijent_nalog_id", KlijentNalogId);

                cmd.ExecuteNonQuery();
                lastInsertId = (int)cmd.LastInsertedId;
            }

            Close();
            return lastInsertId;
        }



        // Čitanje vozila po ID-u
        public override object Read(int id)
        {
            Open();
            string query = "SELECT * FROM vozilo WHERE id = @id";
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@id", id);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new VoziloModel
                        {
                            Id = reader.GetInt32("id"),
                            VrstaRegistracije = reader.GetString("vrstaRegistracije"),
                            Model = reader.GetString("model"),
                            DatumProizvodnje = reader.GetDateTime("datumProizvodnje"),
                            KlijentNalogId = reader.GetInt32("klijent_nalog_id")
                        };
                    }
                }
            }
            Close();
            return null;
        }

        // Ažuriranje vozila
        public override void Update()
        {
            Open();
            string query = "UPDATE vozilo SET vrstaRegistracije = @vrstaRegistracije, model = @model, datumProizvodnje = @datumProizvodnje, klijent_nalog_id = @klijent_nalog_id WHERE id = @id";
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@vrstaRegistracije", VrstaRegistracije);
                cmd.Parameters.AddWithValue("@model", Model);
                cmd.Parameters.AddWithValue("@datumProizvodnje", DatumProizvodnje);
                cmd.Parameters.AddWithValue("@klijent_nalog_id", KlijentNalogId);
                cmd.Parameters.AddWithValue("@id", Id);
                cmd.ExecuteNonQuery();
            }
            Close();
        }

        // Brisanje vozila
        public override void Delete(int id)
        {
            Open();
            string query = "DELETE FROM vozilo WHERE id = @id";
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            Close();
        }

        // Čitanje svih vozila
        public override List<object> ReadAll()
        {
            Open();
            var list = new List<object>();
            string query = "SELECT * FROM vozilo";
            using (var cmd = new MySqlCommand(query, connection))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new VoziloModel
                    {
                        Id = reader.GetInt32("id"),
                        VrstaRegistracije = reader.GetString("vrstaRegistracije"),
                        Model = reader.GetString("model"),
                        DatumProizvodnje = reader.GetDateTime("datumProizvodnje"),
                        KlijentNalogId = reader.GetInt32("klijent_nalog_id")
                    });
                }
            }
            Close();
            return list;
        }
    }
}
