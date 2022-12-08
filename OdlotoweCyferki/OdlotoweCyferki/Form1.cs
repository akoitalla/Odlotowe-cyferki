using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OdlotoweCyferki
{
    public partial class OdlotoweCyferki : Form
    {
        static readonly Random generator = new Random();

        public OdlotoweCyferki()
        {
            InitializeComponent();

            var bitmapa = new Bitmap(panelGlowny.Width, panelGlowny.Height);
            var grafika = Graphics.FromImage(bitmapa);

            panelGlowny.GetType().GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(panelGlowny, new object[] { ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true });
            panelGlowny.Paint += (src, args) => args.Graphics.DrawImage(bitmapa, 0, 0);
           
            Shown +=  (src, args) => gra(grafika, 1);
        }

        private void gra(Graphics grafika, int poziom)
        {
            int czas, liczba, suma, min_liczba, max_liczba;

            // KONFIGURACJA

            var MAX_LICZB_NA_WIERSZ              = 6;
            var MAX_LICZB_NA_KOLUMNE            = 6;
            var SZEROKOSC_OBIEKTU               = Height / MAX_LICZB_NA_WIERSZ;
            var WYSOKOSC_OBIEKTU                = Width / MAX_LICZB_NA_KOLUMNE;
            var ILOSC_LICZB                  = 1300;
            var CZESTOTLIWOSC_ODSWIEZANIA_MS    = 10;

            switch (poziom) // dane dla poziomów
            {
                case 1: 
                    czas = 40; 
                    min_liczba = 0; 
                    max_liczba = 10; 
                    liczba = generator.Next(14, 28); 
                    suma = 0; break;
                case 2: 
                    czas = 30; 
                    min_liczba = 0; 
                    max_liczba = 9; 
                    liczba = generator.Next(14, 48); 
                    suma = 0; break;

                case 3: 
                    czas = 40; 
                    min_liczba = -10; 
                    max_liczba = 10; 
                    liczba = generator.Next(-35, 35); 
                    suma = 0; break;
               
                case 4: 
                    czas = 35; 
                    min_liczba = -10; 
                    max_liczba = 10; 
                    liczba = generator.Next(-30, 30);
                    suma = 0; break;
               
                case 5: 
                    czas = 40; 
                    min_liczba = -10; 
                    max_liczba = 10; 
                    liczba = generator.Next(-99, 99); 
                    suma = 0; break;

                default:
                    
                    MessageBox.Show($"Ukończyłeś gre z poziomem {poziom - 1} :)", "Liczby");

                    return;
            }

            // generowanie ruchomej tablicy cyfr

            var obiekty = new List<int[]>(); // x, y, w, h, 0/1 czy wybrana

            for (int i = 0; i < ILOSC_LICZB; i++)
            {
                obiekty.Add(new int[] { i % 10 * SZEROKOSC_OBIEKTU, i /  10 * WYSOKOSC_OBIEKTU, SZEROKOSC_OBIEKTU, WYSOKOSC_OBIEKTU, generator.Next(min_liczba, max_liczba), 0 });
            }
            var watek = Task.Run(async () =>
            {
                var szerokosc_czcionki = grafika.MeasureString($"00", this.Font);
                while (true)
                {
                    foreach (var obiekt in obiekty)
                    {
                        obiekt[1] = obiekt[1] - 3 ;
                        grafika.FillRectangle(Brushes.MediumOrchid, obiekt[0], obiekt[1], obiekt[2], obiekt[3]); //kolor tła
                        grafika.DrawString($" {obiekt[4]}", this.Font, Brushes.White, obiekt[0] + (obiekt[2]) - (szerokosc_czcionki.Width), obiekt[1] + (obiekt[3]) - (szerokosc_czcionki.Height)); 
                    }

                    await Task.Delay(CZESTOTLIWOSC_ODSWIEZANIA_MS);
                    Invoke(new Action(() => { panelGlowny.Refresh(); }));
                }
            });

        }
    }
}
