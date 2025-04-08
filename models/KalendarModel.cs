using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjekatA.models
{

   public  class KalendarModel
    {
        public string KorisnickoIme { get; set; }
        public LokacijaModel Lokacija { get; set; }
        public string Adresa { get; set; }
        public string Grad { get; set; }
        public string Vrsta { get; set; }
        public string Vrijeme { get; set; }

        public List<KalendarModel> ReadAll()
        {
            List<KalendarModel> list = new List<KalendarModel>();
            var tpm = new ZaposleniTehnickiPregledModel().ReadAll();

            foreach (ZaposleniTehnickiPregledModel x in tpm)
            {
                var nalog = new NalogModel().Read(x.ZaposleniNalogId) as NalogModel;
                var tehnicki = new TehnickiPregledModel().Read(x.TehnickiPregledId) as TehnickiPregledModel;

                if (tehnicki == null || nalog == null)
                    continue;

                var lokacija = new LokacijaModel().Read(tehnicki.LokacijaId) as LokacijaModel;

                if (lokacija == null)
                    continue;

                list.Add(new KalendarModel()
                {
                    KorisnickoIme = nalog.KorisnickoIme,
                    Grad = lokacija.Grad,
                    Adresa = lokacija.Adresa,
                    Lokacija = lokacija,
                    Vrijeme = tehnicki.Datum.ToString("dd/MM/HH-mm"),
                    Vrsta = tehnicki.Vrsta
                });
            }

            return list;


        }


    }
}
