using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OdlotoweCyferki
{   /// <summary>
/// Klasa przypisująca parametry dla poziomu
/// </summary>
    public class Poziom
    {   /// <summary>
        /// Klasa przypisująca parametry dla poziomu
        /// </summary>

        /// <summary>
        /// Metoda zwracająca parametry przydzielone poszczególnym poziomom
        /// </summary>
        /// <param name="poziom">Poziom gry</param>
        /// <param name="int czas">Czas na ukończenie poziomu</param>
        /// <param name="int liczba">Wygenerowana losowo liczba do wyklikania</param>
        /// <param name="int suma">Suma wyklikanych cyfr</param>
        /// <param name="int min_liczba">Najmniejsza liczba występująca w tablicy</param>
        /// <param name="int max_liczba">Największa liczba występująca w tablicy</param>
        /// <returns>
        /// Czas na ukończenie poziomu,
        /// wygenerowana losowo liczba do wyklikania,
        /// suma wyklikanych cyfr,
        /// najmniejsza liczba występująca w tablicy,
        /// największa liczba występująca w tablicy
        /// </returns>
        public Tuple<int, int, int, int, int> getPoziom(int poziom)
        {
            int czas = 0, liczba = 0, suma = 0, min_liczba = 0, max_liczba = 0;
            Random generator = new Random();

            switch (poziom) // dane dla poziomów
            {
                case 1:
                    czas = 40;
                    min_liczba = 0;
                    max_liczba = 9;
                    liczba = generator.Next(14, 99);
                    suma = 0; break;
                case 2:
                    czas = 30;
                    min_liczba = 0; 
                    max_liczba = 9;
                    liczba = generator.Next(14, 99);
                    suma = 0; break;

                case 3:
                    czas = 40;
                    min_liczba = -9;
                    max_liczba = 9;
                    liczba = generator.Next(-35, 35);
                    suma = 0; break;

                case 4:
                    czas = 35;
                    min_liczba = -9;
                    max_liczba = 9;
                    liczba = generator.Next(-40, 40);
                    suma = 0; break;

                case 5:
                    czas = 40;
                    min_liczba = -9;
                    max_liczba = 9;
                    liczba = generator.Next(-45, 50);
                    suma = 0; break;

               
            }
            
            return Tuple.Create(czas,  liczba,  suma,  min_liczba,  max_liczba);

        }
    }
}
