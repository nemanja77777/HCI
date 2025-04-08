using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace ProjekatA.models
{
    public class TehnickiPregledModel : OsnovnaModel
    {
        public string KorisnickoIme { get; set; }
        public LokacijaModel Lokacija { get; set; }

        public string Grad { get; set; }
        public string Adresa { get; set; }

        public int Id { get; set; }
        public string Vrsta { get; set; }
        public int LokacijaId { get; set; }
        public DateTime Datum { get; set; }
        public string DatumString { get; set; }
        public string SatiString { get; set; }
        public string MinutiString { get; set; }



        public int KlijentNalogId { get; set; }

        // Kreiranje novog tehničkog pregleda
        public override int Create()
        {
            Open();
            int lastInsertId = 0;
            string query = "INSERT INTO tehnickipregled (vrsta, lokacija_id, datum, klijent_nalog_id) VALUES (@vrsta, @lokacija_id, @datum, @klijent_nalog_id)";
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@vrsta", Vrsta);
                cmd.Parameters.AddWithValue("@lokacija_id", LokacijaId);
                cmd.Parameters.AddWithValue("@datum", Datum);
                cmd.Parameters.AddWithValue("@klijent_nalog_id", KlijentNalogId);
                cmd.ExecuteNonQuery();
                lastInsertId = (int)cmd.LastInsertedId;
            }
            Close();
            return lastInsertId;
        }


        // Čitanje tehničkog pregleda po ID-u
        public override object Read(int id)
        {
            Open();
            string query = "SELECT * FROM tehnickipregled WHERE id = @id";
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@id", id);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new TehnickiPregledModel
                        {
                            Id = reader.GetInt32("id"),
                            Vrsta = reader.GetString("vrsta"),
                            LokacijaId = reader.GetInt32("lokacija_id"),
                            Datum = reader.GetDateTime("datum"),
                            KlijentNalogId = reader.GetInt32("klijent_nalog_id")
                        };
                    }
                }
            }
            Close();
            return null;
        }

        // Ažuriranje tehničkog pregleda
        public override void Update()
        {
            Open();
            string query = "UPDATE tehnickipregled SET vrsta = @vrsta, lokacija_id = @lokacija_id, datum = @datum, klijent_nalog_id = @klijent_nalog_id WHERE id = @id";
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@vrsta", Vrsta);
                cmd.Parameters.AddWithValue("@lokacija_id", LokacijaId);
                cmd.Parameters.AddWithValue("@datum", Datum);
                cmd.Parameters.AddWithValue("@klijent_nalog_id", KlijentNalogId);
                cmd.Parameters.AddWithValue("@id", Id);
                cmd.ExecuteNonQuery();
            }
            Close();
        }

        // Brisanje tehničkog pregleda
        public override void Delete(int id)
        {
            Open();
            string query = "DELETE FROM tehnickipregled WHERE id = @id";
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            Close();
        }

        // Čitanje svih tehničkih pregleda
        public override List<object> ReadAll()
        {
            Open();
            var list = new List<object>();
            string query = "SELECT * FROM tehnickipregled";
            using (var cmd = new MySqlCommand(query, connection))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new TehnickiPregledModel
                    {
                        Id = reader.GetInt32("id"),
                        Vrsta = reader.GetString("vrsta"),
                        LokacijaId = reader.GetInt32("lokacija_id"),
                        Datum = reader.GetDateTime("datum"),
                        KlijentNalogId = reader.GetInt32("klijent_nalog_id")
                    });
                }
            }
            Close();
            return list;
        }
        public override string ToString()
        {
            // Proveri svaki element i ako je null, postavi "NULL"
            string vrsta = Vrsta ?? "NULL";
            string datum = Datum != DateTime.MinValue ? Datum.ToString("yyyy-MM-dd") : "NULL"; // Formatiraj datum po želji
            string lokacijaAdresa = ((LokacijaModel)(new LokacijaModel().Read(LokacijaId)))?.Adresa ?? "NULL";
            string lokacijaGrad = ((LokacijaModel)(new LokacijaModel().Read(LokacijaId)))?.Grad ?? "NULL";
            string korisnickoIme = ((NalogModel)(new NalogModel().Read(KlijentNalogId)))?.KorisnickoIme ?? "NULL";

            // Sastavi string sa proverenim vrednostima
            return $"{vrsta} {datum} {lokacijaAdresa} {lokacijaGrad} {korisnickoIme}";
        }

    }
}
