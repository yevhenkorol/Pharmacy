using System;
using System.Globalization;
using System.Text;

namespace Pharmacy
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            double[] prices = new double[100];
            int count = 0;

            bool running = true;
            while (running)
            {
                ShowMenu();
                string choice = (Console.ReadLine() ?? "").Trim();
                Console.WriteLine();

                switch (choice)
                {
                    case "1": ShowAbout(); Pause(); break;
                    case "2": ShowCostTable(); Pause(); break;
                    case "3": ShowWeeklyTable(); Pause(); break;
                    case "4":
                    {
                        double newPrice = ReadDouble("Ціна, грн: ", 0.01, 100000);
                        AddValue(prices, ref count, newPrice, prices.Length);
                        Console.WriteLine($"Значень у списку: {count}");
                        Pause();
                        break;
                    }
                    case "5": PrintValues(prices, count); Pause(); break;
                    case "6":
                    {
                        int removeNumber = ReadInt("Номер для видалення: ", 1, 1000000);
                        RemoveAt(prices, ref count, removeNumber);
                        Console.WriteLine($"Значень у списку: {count}");
                        Pause();
                        break;
                    }
                    case "7": ShowStatistics(prices, count); Pause(); break;
                    case "0": running = false; break;
                    default:
                        Console.WriteLine("Немає такого пункту. Спробуйте ще раз.");
                        Pause();
                        break;
                }
            }

            Console.WriteLine("До побачення.");
        }

        static void ShowMenu()
        {
            Console.WriteLine("=== Аптека ===");
            Console.WriteLine("1 — довідка");
            Console.WriteLine("2 — вартість порошку залежно від маси");
            Console.WriteLine("3 — продажі за тиждень за формами випуску");
            Console.WriteLine("4 — додати ціну");
            Console.WriteLine("5 — показати список");
            Console.WriteLine("6 — видалити за номером");
            Console.WriteLine("7 — статистика (сума, середнє, максимум і його номер)");
            Console.WriteLine("0 — вихід");
            Console.Write("Ваш вибір: ");
        }

        static void Pause()
        {
            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey(true); // не показувати символ
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

        // вартість порошку зі знижкою 5 % за грам, але не більше 15 %
        static double CalculateCost(double grams, double pricePerGram)
        {
            double discount = Math.Min(0.15, 0.05 * grams);
            return grams * pricePerGram * (1 - discount);
        }

        static void ShowCostTable()
        {
            double start = ReadDouble("Початкова маса, г: ", 0.1, 1000);
            double end = ReadDouble("Кінцева маса, г: ", start, 1000);
            double step = ReadDouble("Крок, г: ", 0.01, 1000);
            PrintCostTable(start, end, step);
        }

        static void PrintCostTable(double start, double end, double step)
        {
            const double PricePerGram = 18.5; // ціна за 1 г порошку, грн

            Console.WriteLine("Вартість порошку залежно від маси (знижка 5 % за грам, до 15 %)");
            Console.WriteLine($"{"Маса, г",10}{"Без знижки",14}{"Зі знижкою",14}");
            Console.WriteLine(new string('-', 38));

            // запас на похибку double
            for (double x = start; x <= end + 1e-9; x += step)
            {
                double full = x * PricePerGram;
                Console.WriteLine($"{x,10:F2}{full,14:F2}{CalculateCost(x, PricePerGram),14:F2}");
            }
        }

        static void ShowWeeklyTable()
        {
            string[] forms = { "Таблетки", "Капсули", "Сироп", "Краплі", "Мазь", "Порошок" };
            string[] days = { "Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Нд" };

            int rows = ReadInt($"Скільки форм випуску показати (1-{forms.Length}): ", 1, forms.Length);
            int cols = ReadInt($"Скільки днів показати (1-{days.Length}): ", 1, days.Length);
            PrintWeeklyTable(forms, days, rows, cols);
        }

        static void PrintWeeklyTable(string[] forms, string[] days, int rows, int cols)
        {
            Console.WriteLine();
            Console.WriteLine("Продано упаковок за тиждень");
            Console.Write($"{"Форма",-12}");
            for (int c = 0; c < cols; c++)
            {
                Console.Write($"{days[c],6}");
            }
            Console.WriteLine($"{"Разом",8}");

            int lineWidth = 12 + 6 * cols + 8;
            PrintLine(lineWidth);

            int[] colTotals = new int[cols];
            int grandTotal = 0;

            for (int r = 0; r < rows; r++)
            {
                int rowTotal = 0;
                Console.Write($"{forms[r],-12}");
                for (int c = 0; c < cols; c++)
                {
                    int sold = GetSold(r, c);
                    Console.Write($"{sold,6}");
                    rowTotal += sold;
                    colTotals[c] += sold;
                }
                Console.WriteLine($"{rowTotal,8}");
                grandTotal += rowTotal;
            }

            PrintLine(lineWidth);
            Console.Write($"{"Разом",-12}");
            for (int c = 0; c < cols; c++)
            {
                Console.Write($"{colTotals[c],6}");
            }
            Console.WriteLine($"{grandTotal,8}");
        }

        static int GetSold(int r, int c)
        {
            return 8 + (r * 7 + c * 5) % 13;
        }

        static void PrintLine(int width)
        {
            Console.WriteLine(new string('-', width));
        }

        // масив змінюється на місці, лічильник повертаємо через ref
        static void AddValue(double[] values, ref int count, double newValue, int capacity)
        {
            if (count == capacity)
            {
                Console.WriteLine("Місця більше немає.");
                return;
            }

            values[count] = newValue;
            count++;
        }

        static void PrintValues(double[] values, int count)
        {
            if (count == 0)
            {
                Console.WriteLine("Список порожній.");
                return;
            }

            Console.WriteLine("Список цін");
            Console.WriteLine($"{"№",4}{"Ціна, грн",14}");
            PrintLine(18);

            for (int i = 0; i < count; i++)
            {
                Console.WriteLine($"{i + 1,4}{values[i],14:F2}");
            }
        }

        // номер у таблиці починається з 1
        static void RemoveAt(double[] values, ref int count, int index)
        {
            if (index < 1 || index > count)
            {
                Console.WriteLine("Помилка: немає значення з таким номером.");
                return;
            }

            for (int i = index - 1; i < count - 1; i++)
            {
                values[i] = values[i + 1];
            }

            count--;
            values[count] = 0;
        }

        // для порожнього набору середнє дорівнює 0
        static void Statistics(double[] values, int count, out double sum, out double average)
        {
            sum = 0;
            for (int i = 0; i < count; i++)
            {
                sum += values[i];
            }
            average = count > 0 ? sum / count : 0;
        }

        // для порожнього набору обидва out дорівнюють 0
        static void FindMax(double[] values, int count, out double max, out int maxNumber)
        {
            if (count == 0)
            {
                max = 0;
                maxNumber = 0;
                return;
            }

            max = values[0];
            maxNumber = 1;
            for (int i = 1; i < count; i++)
            {
                if (values[i] > max)
                {
                    max = values[i];
                    maxNumber = i + 1;
                }
            }
        }

        // для порожнього списку статистику не друкуємо
        static void ShowStatistics(double[] values, int count)
        {
            if (count == 0)
            {
                Console.WriteLine("Немає значень.");
                return;
            }

            Statistics(values, count, out double sum, out double average);
            FindMax(values, count, out double max, out int maxNumber);
            Console.WriteLine("Статистика цін");
            Console.WriteLine($"Сума: {sum:F2}");
            Console.WriteLine($"Середнє: {average:F2}");
            Console.WriteLine($"Максимум: {max:F2} (номер {maxNumber})");
        }

        // повторює запит, доки число не в межах
        static double ReadDouble(string prompt, double min, double max)
        {
            while (true)
            {
                Console.Write(prompt);
                string text = Console.ReadLine();

                // приймаємо і кому, і крапку як десятковий роздільник
                if (!double.TryParse((text ?? "").Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
                {
                    Console.WriteLine("Помилка: це не число. Спробуйте ще раз.");
                    continue;
                }
                if (double.IsNaN(value) || value < min || value > max)
                {
                    Console.WriteLine($"Помилка: число має бути від {min} до {max}. Спробуйте ще раз.");
                    continue;
                }
                return value;
            }
        }

        // повторює запит, доки число не в межах
        static int ReadInt(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                string text = Console.ReadLine();

                if (!int.TryParse(text, out int value))
                {
                    Console.WriteLine("Помилка: це не ціле число. Спробуйте ще раз.");
                    continue;
                }
                if (value < min || value > max)
                {
                    Console.WriteLine($"Помилка: число має бути від {min} до {max}. Спробуйте ще раз.");
                    continue;
                }
                return value;
            }
        }
    }
}
