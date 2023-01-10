using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OdlotoweCyferki
{/// <summary>
 /// Klasa odpowiedzialna za wygenerowanie tablicy cyfr
 /// </summary>

    public class GenerowanieTablicy : Form
    {/// <summary>
     /// Metoda tworząca tablicę wypełnioną cyframi
     /// </summary>
     /// <param name="min_liczba">Najmniejsza liczba występująca w tablicy</param>
     /// <param name="max_liczba">Największa liczba występująca w tablicy</param>
     /// <param name="ILOSC_LICZB">Ilość liczb w tablicy</param>
     /// <param name="SZEROKOSC_OBIEKTU">Szerokość obiektu</param>
     /// <param name="WYSOKOSC_OBIEKTU">Wysokość obiektu</param>
     /// <returns>Zwraca wygenerowaną tablicę</returns>
 
        public List<int[]> generujTablice(int min_liczba, int max_liczba, int ILOSC_LICZB,
        int SZEROKOSC_OBIEKTU, int WYSOKOSC_OBIEKTU)
        { 


            //wypelnienie
            var liczby = new List<int[]>(); // x, y, w, h, 0/1 czy wybrana

            Random generator = new Random();

            for (int i = 0; i < ILOSC_LICZB; i++)
            {
                liczby.Add(new int[] { i % 10 * SZEROKOSC_OBIEKTU, i / 10 * WYSOKOSC_OBIEKTU, SZEROKOSC_OBIEKTU, WYSOKOSC_OBIEKTU, generator.Next(min_liczba, max_liczba), 0 });
            }

            return liczby;
        }


    }
}
