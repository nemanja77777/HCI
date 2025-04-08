using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Windows;

namespace ProjekatA.models
{
    public class ZaposleniTehnickiPregledModel : OsnovnaModel
    {
        public string KorisnickoIme { get; set; }
        public LokacijaModel Lokacija { get; set; }
        public TehnickiPregledModel TehnickiPregled { get; set; }
        public string Adresa { get; set; }
        public string Grad { get; set; }
        public string IzvjestajNaslov { get; set; } // Dodata nova kolona
        public int TehnickiPregledId { get; set; }
        public byte[] IzvjestajSadrzaj { get; set; }
        public int ZaposleniNalogId { get; set; }

        // Kreiranje novog zapisa
        public override int Create()
        {
            Open();
            int lastInsertId = 0;
            string query = "INSERT INTO `zaposleni-tehnickipregled` (tehnickiPregled_id, izvjestaj, zaposleni_nalog_id, izvjestaj_naslov) VALUES (@tehnickiPregled_id, @izvjestaj, @zaposleni_nalog_id, @izvjestaj_naslov)";
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@tehnickiPregled_id", TehnickiPregledId);
                cmd.Parameters.AddWithValue("@izvjestaj", IzvjestajSadrzaj);
                cmd.Parameters.AddWithValue("@zaposleni_nalog_id", ZaposleniNalogId);
                cmd.Parameters.AddWithValue("@izvjestaj_naslov", IzvjestajNaslov);
                cmd.ExecuteNonQuery();
                lastInsertId = (int)cmd.LastInsertedId;
            }
            Close();
            return lastInsertId;
        }

        // Čitanje zapisa po ID-u (bez sadržaja fajla)
        public override object Read(int id)
        {
            Open();
            string query = "SELECT tehnickiPregled_id, zaposleni_nalog_id, izvjestaj_naslov FROM `zaposleni-tehnickipregled` WHERE tehnickiPregled_id = @id";
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@id", id);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new ZaposleniTehnickiPregledModel
                        {
                            TehnickiPregledId = reader.GetInt32("tehnickiPregled_id"),
                            IzvjestajNaslov = reader.GetString("izvjestaj_naslov"),
                            ZaposleniNalogId = reader.GetInt32("zaposleni_nalog_id")
                        };
                    }
                }
            }
            Close();
            return null;
        }

        // Čitanje svih zapisa (bez sadržaja fajla)
        public override List<object> ReadAll()
        {
            Open();
            var list = new List<object>();
            string query = "SELECT tehnickiPregled_id, zaposleni_nalog_id, izvjestaj_naslov FROM `zaposleni-tehnickipregled`";

            using (var cmd = new MySqlCommand(query, connection))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(new ZaposleniTehnickiPregledModel
                    {
                        TehnickiPregledId = reader.GetInt32("tehnickiPregled_id"),
                        IzvjestajNaslov = reader.GetString("izvjestaj_naslov"),
                        ZaposleniNalogId = reader.GetInt32("zaposleni_nalog_id")
                    });
                }
            }
            Close();
            return list;
        }

        // Nova metoda za čitanje sadržaja fajla
        public byte[] ReadFileContent(int tehnickiPregledId, int zaposleniNalogId)
        {
            Open();
            byte[] fileContent = null;
            string query = "SELECT izvjestaj FROM `zaposleni-tehnickipregled` WHERE tehnickiPregled_id = @tehnickiPregledId AND zaposleni_nalog_id = @zaposleniNalogId";

            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@tehnickiPregledId", tehnickiPregledId);
                cmd.Parameters.AddWithValue("@zaposleniNalogId", zaposleniNalogId);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        fileContent = (byte[])reader["izvjestaj"];
                    }
                }
            }
            Close();
            return fileContent;
        }

        // Ažuriranje zapisa (naslov i sadržaj fajla)
        public override void Update()
        {
            Open();
            string query = "UPDATE `zaposleni-tehnickipregled` SET izvjestaj = @izvjestaj, izvjestaj_naslov = @izvjestaj_naslov WHERE tehnickiPregled_id = @tehnickiPregled_id AND zaposleni_nalog_id = @zaposleni_nalog_id";
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@izvjestaj", IzvjestajSadrzaj);
                cmd.Parameters.AddWithValue("@izvjestaj_naslov", IzvjestajNaslov);
                cmd.Parameters.AddWithValue("@tehnickiPregled_id", TehnickiPregledId);
                cmd.Parameters.AddWithValue("@zaposleni_nalog_id", ZaposleniNalogId);
                cmd.ExecuteNonQuery();
            }
            Close();
        }

        // Brisanje zapisa
        public override void Delete(int a)
        {
            new NotImplementedException();
        }

        public void Delete(int tehnickiPregledId, int zaposleniNalogId)
        {
            Open();
            string query = "DELETE FROM `zaposleni-tehnickipregled` WHERE tehnickiPregled_id = @tehnickiPregledId AND zaposleni_nalog_id = @zaposleniNalogId";

            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@tehnickiPregledId", tehnickiPregledId);
                cmd.Parameters.AddWithValue("@zaposleniNalogId", zaposleniNalogId);
                cmd.ExecuteNonQuery();
            }

            Close();
        }
    }
}
