using System;

namespace ScoreSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Система подсчета очков игры ===");
            Console.WriteLine("Введите очки за три уровня игры:");

            try
            {
                // Ввод и преобразование очков за первый уровень
                Console.Write("Уровень 1: ");
                string input1 = Console.ReadLine();
                int score1 = Convert.ToInt32(input1);

                // Ввод и преобразование очков за второй уровень
                Console.Write("Уровень 2: ");
                string input2 = Console.ReadLine();
                int score2 = Convert.ToInt32(input2);

                // Ввод и преобразование очков за третий уровень
                Console.Write("Уровень 3: ");
                string input3 = Console.ReadLine();
                int score3 = Convert.ToInt32(input3);

                // Вычисление общего количества очков
                int totalScore = score1 + score2 + score3;

                // Вычисление среднего балла (вещественное число)
                // Неявное преобразование int в double при делении
                double averageScore = (double)totalScore / 3;

                // УПАКОВКА (boxing): преобразование int в object
                Console.WriteLine("\n--- Демонстрация упаковки ---");
                object boxedScore = totalScore; // Упаковка
                Console.WriteLine($"Тип boxedScore после упаковки: {boxedScore.GetType()}");
                Console.WriteLine($"Значение boxedScore: {boxedScore}");

                // РАСПАКОВКА (unboxing): преобразование object обратно в int
                Console.WriteLine("\n--- Демонстрация распаковки ---");
                int unboxedScore = (int)boxedScore; // Распаковка с явным приведением
                Console.WriteLine($"Тип unboxedScore после распаковки: {unboxedScore.GetType()}");

                // Вывод результатов с использованием распакованного значения
                Console.WriteLine("\n=== Результаты игры ===");
                Console.WriteLine($"Очки за уровень 1: {score1}");
                Console.WriteLine($"Очки за уровень 2: {score2}");
                Console.WriteLine($"Очки за уровень 3: {score3}");
                Console.WriteLine($"Общее количество очков: {unboxedScore}");
                Console.WriteLine($"Средний балл: {averageScore:F2}");

                // Дополнительная информация
                Console.WriteLine("\n=== Дополнительная информация ===");
                Console.WriteLine($"Общее количество очков (int): {totalScore}");
                Console.WriteLine($"Средний балл (double): {averageScore}");
                Console.WriteLine($"Проверка равенства totalScore и unboxedScore: {totalScore == unboxedScore}");
            }
            catch (FormatException)
            {
                Console.WriteLine("Ошибка: Введено некорректное значение. Пожалуйста, вводите целые числа.");
            }
            catch (OverflowException)
            {
                Console.WriteLine("Ошибка: Введенное число слишком большое или слишком маленькое.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла непредвиденная ошибка: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("\nНажмите любую клавишу для выхода...");
                Console.ReadKey();
            }
        }
    }
}