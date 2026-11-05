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
                        Console.WriteLine("Поки не зроблено.");
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
    }
}
