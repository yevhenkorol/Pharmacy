using System;
using System.Text;

namespace Pharmacy
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            bool running = true;
            while (running)
            {
                Console.WriteLine("=== Аптека ===");
                Console.WriteLine("1 — довідка");
                Console.WriteLine("2 — вартість порошку залежно від маси");
                Console.WriteLine("3 — продажі за тиждень за формами випуску");
                Console.WriteLine("0 — вихід");
                Console.Write("Ваш вибір: ");
                string choice = Console.ReadLine();
                if (choice != null)
                {
                    choice = choice.Trim();
                }
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        ShowAbout();
                        Pause();
                        break;
                    case "2":
                        ShowCostTable();
                        Pause();
                        break;
                    case "3":
                        Console.WriteLine("Поки не зроблено.");
                        Pause();
                        break;
                    case "0":
                        running = false; // єдиний вихід із циклу
                        Console.WriteLine("До побачення.");
                        break;
                    default:
                        Console.WriteLine("Немає такого пункту. Спробуйте ще раз.");
                        Pause();
                        break;
                }
            }
        }

        static void Pause()
        {
            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey(true); // true — не показувати символ
            Console.Clear();
        }

        static void ShowAbout()
        {
            Console.WriteLine("Тема проєкту: Аптека (облік лікарських засобів).");
            Console.WriteLine("Сутності: Користувач, Лікарський засіб, Форма випуску (довідник).");
            Console.WriteLine();
            Console.WriteLine("Головна сутність: Лікарський засіб. Полів: 8.");
            Console.WriteLine("  int    код");
            Console.WriteLine("  string назва");
            Console.WriteLine("  string виробник");
            Console.WriteLine("  int    код форми випуску (з довідника)");
            Console.WriteLine("  int    кількість");
            Console.WriteLine("  double ціна");
            Console.WriteLine("  int    термін придатності");
            Console.WriteLine("  bool   чи за рецептом");
            Console.WriteLine();
            Console.WriteLine("Підсумок: ліки, у яких термін придатності спливає за 30 днів.");
        }

        static void ShowCostTable()
        {
            const double PricePerGram = 18.5; // ціна за 1 г порошку, грн
            double from = 0.5;
            double to = 3.0;
            double step = 0.25;

            Console.WriteLine("Вартість порошку залежно від маси (знижка 5 % за грам, до 15 %)");
            Console.WriteLine($"{"Маса, г",10}{"Без знижки",14}{"Зі знижкою",14}");
            Console.WriteLine(new string('-', 38));

            // 1e-9 — запас, щоб остання стрічка не зникла через похибку double
            for (double x = from; x <= to + 1e-9; x += step)
            {
                double full = x * PricePerGram;
                double discount = Math.Min(0.15, 0.05 * x);
                double withDiscount = full * (1 - discount);
                Console.WriteLine($"{x,10:F2}{full,14:F2}{withDiscount,14:F2}");
            }
        }
    }
}
