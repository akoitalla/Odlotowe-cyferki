using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OdlotoweCyferki
{/// <summary>
 /// Klasa umożliwiająca grywalność
 /// </summary>
    public partial class OdlotoweCyferki : Form
    {   /// <summary>
        /// Klasa umożliwiająca grywalność
        /// </summary>


        Poziom poziom = new Poziom();
        GenerowanieTablicy tablica = new GenerowanieTablicy();

        /// <summary>
        /// Konstruktor, w którym tworzona, rysowana i wywołana jest bitmapa o rozmiarze głównego panelu oraz obiekt grafika utowrzony z obrazu bitmapy. Dodatkowo ustawiane jest kilka stylów dla panelu głównego i dodana obsługa przycisku replay. 
        /// </summary>
        public OdlotoweCyferki()
        {
            InitializeComponent();
            
            var bitmapa = new Bitmap(panelGlowny.Width, panelGlowny.Height);
            var grafika = Graphics.FromImage(bitmapa);  

            // Ustawienie kilku stylów dla obiektu panelGlowny
            panelGlowny.GetType().GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(panelGlowny, 
            new object[] { ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true }); 

            //osługa zdarzenia Paint - rysowanie bitmapy na panelu Głównym
            panelGlowny.Paint += (src, args) => args.Graphics.DrawImage(bitmapa, 0, 0);

            // przycisniecie przycisku buttonReplay spowoduje ponowne rozpoczęcie gry
            buttonReplay.Click += (src, ars) =>
            {
                buttonReplay.Visible = true;
                gra(grafika, 1);
            };

            //Dodanie do obslugi zdarzenia Shown nowej akcji o parametrach src i args  -> wywołanie metody "gra"
            Shown +=  (src, args) => gra(grafika, 1);

        }

        /// <summary>
        /// Sprawia, że:
        /// 1) wygenerowana tablica porusza się ku górze
        /// 2) kliknięte przez użytkownika cyfry są podświetlane oraz dodawane do sumy
        /// 3) wyświetlany jest czas pozostąły do ukończenia poziomu
        /// 4) użytkownik jest informowany o stanie gry
        /// </summary>
        /// <param name="grafika">Utworzony obiekt grafika, który jest bitmapą </param>
        /// <param name="poziom">Poziom gry</param>
        private void gra(Graphics grafika, int poziom)
        {
            // KONFIGURACJA
            Random generator = new Random();
            var ILOSC_LICZB = 1300;
            var MAX_LICZB_NA_WIERSZ = 5;
            var MAX_LICZB_NA_KOLUMNE = 6;
            var SZEROKOSC_OBIEKTU = Height / MAX_LICZB_NA_WIERSZ;
            var WYSOKOSC_OBIEKTU = Width / MAX_LICZB_NA_KOLUMNE;
            var CZESTOTLIWOSC_ODSWIEZANIA_MS    = 10;


            (int czas, int liczba, int suma, int min_liczba, int max_liczba) = this.poziom.getPoziom(poziom);;

            if (poziom == 1 || poziom == 2 || poziom == 3 || poziom == 4 || poziom == 5)
            {
                MessageBox.Show($"Przygotuj się do poziomu {poziom}\n\n Powodzenia!", "Odlotowe Cyferki");
            }

            /// <summary>
            /// Tablica liczb
            /// </summary>
            var liczby = tablica.generujTablice(min_liczba, max_liczba, ILOSC_LICZB, SZEROKOSC_OBIEKTU, WYSOKOSC_OBIEKTU);

            var blokada = new object();

            // Możliwość kliknięcia w cyfry, podświetlenie wybranej i dodanie do sumy
            panelGlowny.MouseClick += (p, o) =>
            {
                if (o.Button == MouseButtons.Left)
                    lock (blokada)
                    {
                        foreach (var item in liczby)
                            if (new Rectangle(item[0], item[1], item[2], item[3]).Contains(o.Location))
                            {
                                item[5] = 1;
                                suma += item[4];
                                Invoke(new Action(() => labelSuma.Text = $"∑\n" + (suma >= -9 && suma <= 9 ? (suma > 0 ? " 0" + suma : "-  " + Math.Abs(suma)) : (suma < 0 ? "- " + suma.ToString().Substring(1) : " " + suma))));
                            }
                    }
            };
            // Aktualizowanie w nieskończonej pętli tekstu pojawiającego się na etykiecie czas
            Task.Run(async () =>
            {
                while (true)
                {
                    Invoke(new Action(() => labelCzas.Text = $"🕑\n{czas.ToString("00")}"));
                    await Task.Delay(1000);
                    lock (blokada)
                        if (czas == 0) return;
                        else czas--;
                }
            });

            ///<summary>
            ///Sprawienie, żeby liczby poruszały się w górę
            ///</summary>
            var watek = Task.Run(async () =>
            {
                System.Drawing.Font czcionka = new System.Drawing.Font("Arial", 14.0f, System.Drawing.FontStyle.Bold);
                var szerokosc_czcionki = grafika.MeasureString($"00", czcionka);

                while (true)
                {
                    lock (blokada)
                    {
                        if (suma == liczba) return;
                        if (czas <= 0) return;
                        foreach (var obiekt in liczby)
                        {
                            obiekt[1] = obiekt[1] - 2;
                            grafika.FillRectangle(Brushes.Plum, obiekt[0], obiekt[1], obiekt[2], obiekt[3]);
                            
                            if (obiekt[5] == 1) 
                            grafika.DrawRectangle(Pens.White, obiekt[0] - 1, obiekt[1] - 1, obiekt[2] - 1, obiekt[3] - 1);
                           
                            grafika.DrawString($" {obiekt[4]}", czcionka, Brushes.White, obiekt[0] + (obiekt[2] / 2) 
                            - (szerokosc_czcionki.Width / 2), obiekt[1] + (obiekt[3] / 2) - (szerokosc_czcionki.Height / 2));
                        }
                    }
                    await Task.Delay(CZESTOTLIWOSC_ODSWIEZANIA_MS);
                    Invoke(new Action(() => { panelGlowny.Refresh(); }));
                }
            });

            // Wyswietlenie uzytkownikowi informacji o stanie gry
            Task.Run(async () =>
            {
                await watek;

               
                if (poziom == 6)
                {
                    MessageBox.Show($"Ukończyłeś gre z poziomem {poziom - 1} :)\n\nMożesz rozpocząć grę ponownie klikając przycisk 'jeszcze raz'' ", "Liczby");
                   
                }
                else if (liczba == suma)
                {
                    MessageBox.Show($"Gratulacje!\n\nUkończyłeś poziom {poziom}", "Odlotowe Cyferki");
                    gra(grafika, ++poziom);
                }
                else if (czas == 0)
                {
                    MessageBox.Show("Niestety, koniec czasu. Spróbuj jeszcze raz!", "Odlotowe Cyferki");
                }

            });
            Invoke(new Action(() =>
            {
                labelPoziom.Text = $"\U0001f9e9\r\n{poziom.ToString("00")}";
                labelCel.Text = $"🎯\n{liczba.ToString("00")}";
                labelSuma.Text = $"∑\n{suma.ToString("00")}";
            }));

        }

        private void panelCzas_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
