# Projekat_A-Zakazivanje_Tehnickog_Pregleda

WPF desktop aplikacija za zakazivanje tehničkog pregleda vozila. Aplikacija koristi **MySQL** bazu podataka za čuvanje informacija i sadrži organizovanu strukturu foldera radi lakšeg održavanja i proširenja funkcionalnosti.

## Tehnologije

- WPF (.NET)
- MySQL
- MVVM arhitektura
- JSON lokalizacija i podešavanja

## Struktura Projekta

- `messages/` – XAML forme za prilagodjene poruke  
- `updates/` – XAML forme za update stavki u meniju  
- `creates/` – XAML forme za create stavki u meniju  
- `themes/` – podešavanja tema (svetla, tamna, kontrastna)  
- `views/` – XAML forme stavki u meniju  
- `modelViews/` – view modeli za MVVM arhitekturu  
- `models/` – za entitete iz baze podataka  
- `resources/` – sr.xaml, en.xaml resource dictionary 
- `images/` – screenshotovi i vizuelni prikazi aplikacije  

---

## Screenshotovi
# Logi Page

Pocetni izgled aplikacije

![LoginPage](images/1%20LoginPage.png)  

## Izmjena jezika

![LanguageChanged](images/2%20LanguageChanged.png)  

U slucaju da je korisnik uspjesno ulogovan i da je izabrao neki od dva dostupna jezika, koji je razlicit od izbora prethodnog korisnika.

## Izmjena Stila

![StyleChanged](images/3%20StyleChanged.png)  

Ova forma se ispisuje kada se korisnik sistema uspjesno uloguje i izabaere prilikom logovanja temu, koja do tada nije bila izabrana tehe prehhodnog korisnika.

## Neuspjesno Logovanje

![LoginFail](images/4%20LoginFail.png) 

Ako su korisnicko ime i ili korisnicka sifra podresni.

# Admin Page

![AdminMeniPage](images/5%20AdminMeniPage.png)  

Ako su korisnicko ime i sifra vezani za administratorski nalog

# Employee Page

![EmployeeMeniPage](images/6%20EmployeeMeniPage.png)  

Ako su korisnicko ime i sifra vezani za nalog korisnika koji je zaposleni

## Employee Page Stavke

### Calendar Stavka

![EmployeeCalendar](images/7%20EmployeeCalendar.png)  

![CalendarClick](images/7.1%20CalendarClick.png)  

Zaposleni u kalendaru vidi tehnicke preglede vezano za svoje korisnicko ime.
Klikom na odredjeni datum vidi dostupne termine za svoje korisnicko ime i za taj datum.

### Pdf Stavka

![PdfStavka](images/8%20PdfStavka.png)  

![Pdf Stavka](images/8.1%20Pdf%20Stavka.png) 

Izgled forme koji popunjavaju zaposleni ili administrator. Forma ce posluziti kao sablon za kreiranje PDF izvjestaja tehnickog pregleda.

![Pdf Create Success](images/8.2%20Pdf%20Create%20Success.png) 

![PdfContent](images/9%20PdfContent.png)  

Takodje sadrzi automatski korisnicko ime i datum kreiranja. Odnosno sadrzai time stamp i potps korisnickom imenom.

### Employee Stavka

![EmployeeReport](images/10%20EmployeeReport.png)  

![ReportContextMenu](images/11%20ReportContextMenu.png)  

Employee Stavka je specificna po tome da u kontekst meniju ima opiciju "Read Content"

![ReportCreate](images/12%20ReportCreate.png)   

Za svaki izvjestaj bira se zaposleni koji je prisustvovao tom izvjestaju i tehnicki pregled.

![SuccessfullyCreated](images/13%20SuccessfullyCreated.png)   

![ErrorWhileCreating](images/14%20ErrorWhileCreating.png)  

Nastaje ako neka od opcija nije izabrana

![PrimaryKey](images/15%20PrimaryKey.png)  

Ako se kreira izvjestaj koji vec postoji, odnosno ako dodje do toga da primarni kljuc izvjestaja ne bude jedinstven ispisuje se ova poruka. To znaci da taj izvjestaj vec postoji.

![ReportUpdate](images/16%20ReportUpdate.png)  

Prilikom svake izmjene stavke u tabelama update forma se popuni podacima na koji se red odnosi, odnosno na koji red je kliknut desni klik.

![ErrorWhileUpdating](images/17%20ErrorWhileUpdating.png)  

Takodje ne dozvoljava da dodje do promjene u vec postojevi unos u tabeli.

![SuccessFullyUpdated](images/17.1%20SuccessFullyUpdated.png) 

![DeleteQuestion](images/18%20DeleteQuestion.png)  

Prilikom svakog brisanja korisnik dobija ovu formu, da se smanji sansa da korisnik nesto obrise slucajno

![DeleteSuccess](images/18.1%20DeleteSuccess.png)  

![Referencijalni](images/19%20Referencijalni.png)  

Ako je korisnik pokusao da obrise red u tabli koji je referenciran iz neke druge tabele dobija ovu formu. Posljedica je da se taj red nije obrisao i da se treba prvo obrisati unos u tabeli koja referencira ovaj red.

![ReadContentMenuContext](images/19.1%20ReadContentMenuContext.png) 

Ova stavka sluzi da korisnik procita potrebni fajl iz baze podataka. Otvara mu forma na operativnom sistemu da odabere gdje ce sacuvati fajl.

![FileCreated](images/20%20FileCreated.png)  

Ako korisnik ima potrebne privilegije, dovoljno memorije na sistemu i nije odabrao vec postojeci fajl na odredjenoj putanji.

# Administrator Page

## Admin Page Stavke

### Vozilo Stavka

![VoziloMeniStavka](images/21%20VoziloMeniStavka.png)  

![VoziloMeniContext](images/22%20VoziloMeniContext.png)  

![VoziloCreate](images/23%20VoziloCreate.png)  

![VoziloCreateWarning](images/24%20VoziloCreateWarning.png)  

![VoziloCreateSuccess](images/25%20VoziloCreateSuccess.png)  

![UpdateContextMeni](images/26%20UpdateContextMeni.png)  

![UpdateWarning](images/27%20UpdateWarning.png)  

![UpdateSuccess](images/28%20UpdateSuccess.png)  

![DeleteContextMeni](images/29%20DeleteContextMeni.png)  

![Delete question](images/30%20Delete%20question.png)  

![DeleteSUccess](images/31%20DeleteSUccess.png)  

### Account Stavka

Sadrzi pregled korisnicki imena i hesirahih lozinki.

![AccountMeniStavka](images/32%20AccountMeniStavka.png)  

### Kalendar Stavka

![AdminKalendarClick](images/32%20AdminKalendarClick.png) 

![AdminKalendar](images/33%20AdminKalendar.png)  

Admin Za razliku od zaposlenog vidi sve tehnicke preglede. I na odabir datuma u kalendaru vidi sve dostupne tehnicke preglede za taj datum.

### Report Stavka

![AdminReportMeniStavka](images/34%20AdminReportMeniStavka.png)  

Admin ima uvid u sve izvjestaje, nezavisno od korisnickog imena zaposlenog koji je ucistvovao na kreiranju tog izvjestaja.

### Vehicle Inspection Stavka

![TPCOntextCreate](images/35%20TPCOntextCreate.png)  

![TPCreateView](images/36%20TPCreateView.png)  

Sistem onemogucuje da se izabere unos koji narusava integritet primarnog kljuca, mehanizmom da vrsi provjeru elementa koji je dostupan u stavkama. Odnosno korisnik nije u stanju da odabere nesto sto ce narusiti pravilno funkcionisanje sistema.

![TpCreateWarning](images/37%20TpCreateWarning.png)  

![TPCreatePopunjen](images/38%20TPCreatePopunjen.png)  

![Created](images/39%20Created.png)  

![TpUpdateCOntext](images/40%20TpUpdateCOntext.png)  

![TpUpdateView](images/41%20TpUpdateView.png)  

![UpdateError](images/42%20UpdateError.png)  

![Update SUccess](images/43%20Update%20SUccess.png)  

![TpDeleteContextMeni](images/44%20TpDeleteContextMeni.png) 

![TpDeleteQuestion](images/45%20TpDeleteQuestion.png)  

![DeleteSuccess](images/46%20DeleteSuccess.png)  

![NalogMeniStavka](images/47%20NalogMeniStavka.png)  

![Referencial](images/48%20Referencial.png)  

Pokusaj brisanja, koji je sprjecen da se ne bi doslo do narusavanja referencijalnog integriteta.

![LightTheme](images/49%20LightTheme.png)  

Izgled svijetle teme pocetnog Login prozora

![HighContrast](images/50%20HighContrast.png)

Izgled kontrastne teme pocetnog Login prozora

