using PdfSharp.Pdf;
using PdfSharp.Drawing;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using ProjekatA.messages;
using System.Windows.Input;
using ProjekatA.viewModels;

namespace ProjekatA.creates
{
    public partial class PDFCreate : Window
    {
        public PDFCreate()
        {
            InitializeComponent();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            //WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void TopPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // Metoda za proveru da li je potrebno dodati novu stranicu
        private void CheckPage(ref int yPosition, PdfDocument document, ref PdfPage page, ref XGraphics gfx, XFont font)
        {
            const int margin = 40;
            if (yPosition > page.Height - margin)
            {
                page = document.AddPage();
                gfx = XGraphics.FromPdfPage(page);
                yPosition = margin;
            }
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            // Otvaranje SaveFileDialog za biranje lokacije
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF dokument (*.pdf)|*.pdf",
                Title = "Sačuvaj PDF"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;

                try
                {
                    // Ako fajl već postoji, obriši ga
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }

                    string directory = Path.GetDirectoryName(filePath);
                    if (!Directory.Exists(directory))
                    {
                        new MyPDFCreateWarning().ShowDialog();
                        return;
                    }

                    // Kreiranje PDF dokumenta
                    PdfDocument document = new PdfDocument();
                    document.Info.Title = "Forma PDF";

                    // Dodavanje prve stranice
                    PdfPage page = document.AddPage();
                    XGraphics gfx = XGraphics.FromPdfPage(page);
                    XFont font = new XFont("Verdana", 12);
                    int yPosition = 40; // Početna pozicija

                    // Sekcija: Opšti podaci
                    gfx.DrawString("Opšti podaci", font, XBrushes.Black, new XPoint(40, yPosition));
                    yPosition += 30;
                    gfx.DrawString($"Broj tehničkog pregleda: {TextBoxBrojPregleda.Text}", font, XBrushes.Black, new XPoint(60, yPosition));
                    yPosition += 20;
                    gfx.DrawString($"Broj šasije: {TextBoxBrojSasije.Text}", font, XBrushes.Black, new XPoint(60, yPosition));
                    yPosition += 20;
                    gfx.DrawString($"Datum pregleda: {DatePickerDatumPregleda.SelectedDate?.ToShortDateString()}", font, XBrushes.Black, new XPoint(60, yPosition));
                    yPosition += 30;
                    CheckPage(ref yPosition, document, ref page, ref gfx, font);

                    // Sekcija: Zatezni uređaji
                    gfx.DrawString("Zatezni uređaji", font, XBrushes.Black, new XPoint(40, yPosition));
                    yPosition += 30;
                    gfx.DrawString($"Upravljački zatezni uređaji u ispravnom stanju: {(CheckBoxZatezniUredaji.IsChecked == true ? "DA" : "NE")}", font, XBrushes.Black, new XPoint(60, yPosition));
                    yPosition += 20;
                    gfx.DrawString($"Pojas u dobrom stanju: {(CheckBoxPojas.IsChecked == true ? "DA" : "NE")}", font, XBrushes.Black, new XPoint(60, yPosition));
                    yPosition += 30;
                    CheckPage(ref yPosition, document, ref page, ref gfx, font);

                    // Sekcija: Klimatizacija
                    gfx.DrawString("Klimatizacija", font, XBrushes.Black, new XPoint(40, yPosition));
                    yPosition += 30;
                    gfx.DrawString($"Klimatizacija funkcioniše ispravno: {(CheckBoxKlimatizacija.IsChecked == true ? "DA" : "NE")}", font, XBrushes.Black, new XPoint(60, yPosition));
                    yPosition += 20;
                    gfx.DrawString($"Grejanje funkcioniše ispravno: {(CheckBoxGrejanje.IsChecked == true ? "DA" : "NE")}", font, XBrushes.Black, new XPoint(60, yPosition));
                    yPosition += 30;
                    CheckPage(ref yPosition, document, ref page, ref gfx, font);

                    // Sekcija: Električni sistem
                    gfx.DrawString("Električni sistem", font, XBrushes.Black, new XPoint(40, yPosition));
                    yPosition += 30;
                    gfx.DrawString($"Baterija u dobrom stanju: {(CheckBoxBaterija.IsChecked == true ? "DA" : "NE")}", font, XBrushes.Black, new XPoint(60, yPosition));
                    yPosition += 20;
                    gfx.DrawString($"Alternator ispravan: {(CheckBoxAlternator.IsChecked == true ? "DA" : "NE")}", font, XBrushes.Black, new XPoint(60, yPosition));
                    yPosition += 20;
                    gfx.DrawString($"Osvetljenje interijera funkcioniše: {(CheckBoxOsvetljenje.IsChecked == true ? "DA" : "NE")}", font, XBrushes.Black, new XPoint(60, yPosition));
                    yPosition += 30;
                    CheckPage(ref yPosition, document, ref page, ref gfx, font);

                    // Sekcija: Vazdušni filter
                    gfx.DrawString("Vazdušni filter", font, XBrushes.Black, new XPoint(40, yPosition));
                    yPosition += 30;
                    gfx.DrawString($"Vazdušni filter čist i u dobrom stanju: {(CheckBoxVazdusniFilter.IsChecked == true ? "DA" : "NE")}", font, XBrushes.Black, new XPoint(60, yPosition));
                    yPosition += 30;
                    CheckPage(ref yPosition, document, ref page, ref gfx, font);

                    // Sekcija: Ispravnost brisača i prskalica
                    gfx.DrawString("Ispravnost brisača i prskalica", font, XBrushes.Black, new XPoint(40, yPosition));
                    yPosition += 30;
                    gfx.DrawString($"Brisači u ispravnom stanju: {(CheckBoxBrisaci.IsChecked == true ? "DA" : "NE")}", font, XBrushes.Black, new XPoint(60, yPosition));
                    yPosition += 20;
                    gfx.DrawString($"Prskalica za vetrobran funkcioniše: {(CheckBoxPrskalica.IsChecked == true ? "DA" : "NE")}", font, XBrushes.Black, new XPoint(60, yPosition));
                    yPosition += 30;
                    CheckPage(ref yPosition, document, ref page, ref gfx, font);

                    // Sekcija: Provera izduvnog sistema
                    gfx.DrawString("Provera izduvnog sistema", font, XBrushes.Black, new XPoint(40, yPosition));
                    yPosition += 30;
                    gfx.DrawString($"Izduvni sistem nije oštećen: {(CheckBoxIzduvniSistem.IsChecked == true ? "DA" : "NE")}", font, XBrushes.Black, new XPoint(60, yPosition));
                    yPosition += 20;
                    gfx.DrawString($"Izduvni gasovi unutar dozvoljenih vrednosti: {(CheckBoxIzduvniGasovi.IsChecked == true ? "DA" : "NE")}", font, XBrushes.Black, new XPoint(60, yPosition));
                    yPosition += 30;
                    CheckPage(ref yPosition, document, ref page, ref gfx, font);

                    // Sekcija: Kočioni sistem
                    gfx.DrawString("Kočioni sistem", font, XBrushes.Black, new XPoint(40, yPosition));
                    yPosition += 30;
                    gfx.DrawString($"Kočioni sistem u dobrom stanju: {(CheckBoxKocioniSistem.IsChecked == true ? "DA" : "NE")}", font, XBrushes.Black, new XPoint(60, yPosition));
                    yPosition += 20;
                    gfx.DrawString($"Kočione pločice u dobrom stanju: {(CheckBoxKocionePlocice.IsChecked == true ? "DA" : "NE")}", font, XBrushes.Black, new XPoint(60, yPosition));
                    yPosition += 30;
                    CheckPage(ref yPosition, document, ref page, ref gfx, font);

                    // Sekcija: Pneumatici
                    gfx.DrawString("Pneumatici", font, XBrushes.Black, new XPoint(40, yPosition));
                    yPosition += 30;
                    gfx.DrawString($"Pneumatici imaju dovoljnu dubinu gazećeg sloja: {(CheckBoxPneumaticiDubina.IsChecked == true ? "DA" : "NE")}", font, XBrushes.Black, new XPoint(60, yPosition));
                    yPosition += 20;
                    gfx.DrawString($"Pneumatici nisu oštećeni: {(CheckBoxPneumaticiOsteceni.IsChecked == true ? "DA" : "NE")}", font, XBrushes.Black, new XPoint(60, yPosition));
                    yPosition += 30;
                    CheckPage(ref yPosition, document, ref page, ref gfx, font);

                    // Sekcija: Osvetljenje i signalizacija
                    gfx.DrawString("Osvetljenje i signalizacija", font, XBrushes.Black, new XPoint(40, yPosition));
                    yPosition += 30;
                    gfx.DrawString($"Sve svetlosne funkcije ispravno rade: {(CheckBoxSvetlosneFunkcije.IsChecked == true ? "DA" : "NE")}", font, XBrushes.Black, new XPoint(60, yPosition));
                    yPosition += 20;
                    gfx.DrawString($"Migavci i svetla za signalizaciju rade ispravno: {(CheckBoxMigavci.IsChecked == true ? "DA" : "NE")}", font, XBrushes.Black, new XPoint(60, yPosition));
                    yPosition += 30;
                    CheckPage(ref yPosition, document, ref page, ref gfx, font);

                    // Sekcija: Sigurnosni sistemi
                    gfx.DrawString("Sigurnosni sistemi", font, XBrushes.Black, new XPoint(40, yPosition));
                    yPosition += 30;
                    gfx.DrawString($"Airbagovi funkcionišu ispravno: {(CheckBoxAirbag.IsChecked == true ? "DA" : "NE")}", font, XBrushes.Black, new XPoint(60, yPosition));
                    yPosition += 20;
                    gfx.DrawString($"ABS, ESP i drugi sigurnosni sistemi funkcionišu: {(CheckBoxABS.IsChecked == true ? "DA" : "NE")}", font, XBrushes.Black, new XPoint(60, yPosition));
                    yPosition += 30;
                    CheckPage(ref yPosition, document, ref page, ref gfx, font);

                    // Sekcija: Vrsta goriva
                    gfx.DrawString("Vrsta goriva", font, XBrushes.Black, new XPoint(40, yPosition));
                    yPosition += 30;
                    string fuelType = (ComboBoxVrstaGoriva.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content.ToString() ?? "";
                    gfx.DrawString($"Vrsta goriva: {fuelType}", font, XBrushes.Black, new XPoint(60, yPosition));
                    yPosition += 30;
                    CheckPage(ref yPosition, document, ref page, ref gfx, font);

                    gfx.DrawString($"Nalog je napravio nalog: {LoginViewModel.CurrentUsername} Vrijeme: {DateTime.Now.ToString("dd-MM-yyyy-HH-mm")}", font, XBrushes.Black, new XPoint(40, yPosition));
                    yPosition += 30;

                    // Sekcija: Napomena
                    gfx.DrawString("Napomena", font, XBrushes.Black, new XPoint(40, yPosition));
                    yPosition += 30;
                    gfx.DrawString($"Napomena: {TextBoxNapomena.Text}", font, XBrushes.Black, new XPoint(60, yPosition));

                    // Spremanje dokumenta
                    document.Save(filePath);
                    new MyPDFCreateSuccess().ShowDialog();
                    Close();
                }
                catch (Exception ex)
                {
                    new MyPDFCreateWarning().ShowDialog();
                }
            }
        }

        private void TextBoxNapomena_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}
