using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace ThreadPractice
{
    class Program
    {
        // Объект для синхронизации вывода в консоль
        private static readonly object consoleLock = new object();

        // Объект для синхронизации доступа к общим данным
        private static readonly object dataLock = new object();

        // Общие данные для потоков
        private static int sharedCounter = 0;
        private static List<int> allNumbers = new List<int>();
        private static bool isRunning = true;

        static void Main(string[] args)
        {
            Console.WriteLine("=== Практическая работа 9: Создание и управление потоками в .NET ===\n");

            try
            {
                // 1. БАЗОВЫЙ ПРИМЕР: Один поток считает от 1 до 100
                Console.WriteLine("1. БАЗОВЫЙ ПРИМЕР: Один поток считает от 1 до 100");
                Console.WriteLine(new string('=', 60));

                BasicThreadExample();
                Thread.Sleep(1000); // Небольшая пауза между примерами

                // 2. НЕСКОЛЬКО ПОТОКОВ С СИНХРОНИЗАЦИЕЙ
                Console.WriteLine("\n\n2. НЕСКОЛЬКО ПОТОКОВ С СИНХРОНИЗАЦИЕЙ");
                Console.WriteLine(new string('=', 60));

                MultipleThreadsWithSynchronization();
                Thread.Sleep(1000);

                // 3. РАБОТА С ОБЩИМИ ДАННЫМИ
                Console.WriteLine("\n\n3. РАБОТА С ОБЩИМИ ДАННЫМИ");
                Console.WriteLine(new string('=', 60));

                SharedDataExample();
                Thread.Sleep(1000);

                // 4. УПРАВЛЕНИЕ ПОТОКАМИ
                Console.WriteLine("\n\n4. УПРАВЛЕНИЕ ПОТОКАМИ (ПРИОСТАНОВКА, ВОЗОБНОВЛЕНИЕ)");
                Console.WriteLine(new string('=', 60));

                ThreadManagementExample();
                Thread.Sleep(1000);

                // 5. POOL ПОТОКОВ
                Console.WriteLine("\n\n5. POOL ПОТОКОВ");
                Console.WriteLine(new string('=', 60));

                ThreadPoolExample();
                Thread.Sleep(1000);

                // 6. СРАВНЕНИЕ ПРОИЗВОДИТЕЛЬНОСТИ
                Console.WriteLine("\n\n6. СРАВНЕНИЕ ПРОИЗВОДИТЕЛЬНОСТИ");
                Console.WriteLine(new string('=', 60));

                PerformanceComparison();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Ошибка: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("\n" + new string('=', 60));
                Console.WriteLine("Программа завершена. Нажмите любую клавишу для выхода...");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// 1. Базовый пример: один поток считает от 1 до 100
        /// </summary>
        static void BasicThreadExample()
        {
            Console.WriteLine("\nЗапуск потока, который считает от 1 до 100...\n");

            // Создаем новый поток
            Thread countingThread = new Thread(CountNumbers)
            {
                Name = "Счетный поток" // Устанавливаем имя потока
            };

            // Запускаем поток
            countingThread.Start();

            // Ждем завершения потока
            countingThread.Join();

            Console.WriteLine("\n✅ Поток завершил работу!");
        }

        /// <summary>
        /// Метод, который выполняется в потоке - считает числа от 1 до 100
        /// </summary>
        static void CountNumbers()
        {
            lock (consoleLock)
            {
                Console.WriteLine($"Поток '{Thread.CurrentThread.Name}' (ID: {Thread.CurrentThread.ManagedThreadId}) начал работу");
                Console.WriteLine("Считаем числа от 1 до 100:\n");
            }

            for (int i = 1; i <= 100; i++)
            {
                lock (consoleLock)
                {
                    // Выводим число с цветом в зависимости от четности
                    ConsoleColor originalColor = Console.ForegroundColor;

                    if (i % 2 == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write($"{i,3} ");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write($"{i,3} ");
                    }

                    Console.ForegroundColor = originalColor;

                    // Переход на новую строку каждые 10 чисел
                    if (i % 10 == 0)
                    {
                        Console.WriteLine();
                    }
                }

                // Имитация работы - небольшая задержка
                Thread.Sleep(50);
            }

            lock (consoleLock)
            {
                Console.WriteLine($"\nПоток '{Thread.CurrentThread.Name}' завершил работу");
            }
        }

        /// <summary>
        /// 2. Пример с несколькими потоками и синхронизацией
        /// </summary>
        static void MultipleThreadsWithSynchronization()
        {
            Console.WriteLine("\nЗапуск 3 потоков, которые считают числа в разных диапазонах...\n");

            // Создаем несколько потоков
            Thread thread1 = new Thread(() => CountRange(1, 33))
            {
                Name = "Поток 1 (1-33)"
            };

            Thread thread2 = new Thread(() => CountRange(34, 66))
            {
                Name = "Поток 2 (34-66)"
            };

            Thread thread3 = new Thread(() => CountRange(67, 100))
            {
                Name = "Поток 3 (67-100)"
            };

            // Запускаем все потоки
            thread1.Start();
            thread2.Start();
            thread3.Start();

            // Ждем завершения всех потоков
            thread1.Join();
            thread2.Join();
            thread3.Join();

            Console.WriteLine("\n✅ Все потоки завершили работу!");
        }

        /// <summary>
        /// Метод для подсчета чисел в заданном диапазоне
        /// </summary>
        static void CountRange(int start, int end)
        {
            lock (consoleLock)
            {
                Console.WriteLine($"Поток '{Thread.CurrentThread.Name}' начал считать от {start} до {end}");
            }

            for (int i = start; i <= end; i++)
            {
                lock (consoleLock)
                {
                    // Каждый поток выводит своим цветом
                    ConsoleColor originalColor = Console.ForegroundColor;

                    switch (Thread.CurrentThread.Name)
                    {
                        case "Поток 1 (1-33)":
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;
                        case "Поток 2 (34-66)":
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            break;
                        case "Поток 3 (67-100)":
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            break;
                    }

                    Console.Write($"{i,3} ");
                    Console.ForegroundColor = originalColor;

                    if ((i - start + 1) % 10 == 0)
                    {
                        Console.WriteLine();
                    }
                }

                Thread.Sleep(30); // Разная скорость работы потоков
            }

            lock (consoleLock)
            {
                Console.WriteLine($"\nПоток '{Thread.CurrentThread.Name}' завершил работу\n");
            }
        }

        /// <summary>
        /// 3. Работа с общими данными и синхронизацией
        /// </summary>
        static void SharedDataExample()
        {
            Console.WriteLine("\nЗапуск потоков, которые работают с общим счетчиком...\n");

            // Сбрасываем общие данные
            sharedCounter = 0;
            allNumbers.Clear();

            // Создаем несколько потоков, которые увеличивают общий счетчик
            Thread[] threads = new Thread[5];

            for (int i = 0; i < threads.Length; i++)
            {
                int threadNumber = i + 1;
                threads[i] = new Thread(() => IncrementSharedCounter(threadNumber))
                {
                    Name = $"Поток-счетчик {threadNumber}"
                };
            }

            // Запускаем все потоки
            foreach (var thread in threads)
            {
                thread.Start();
            }

            // Ждем завершения всех потоков
            foreach (var thread in threads)
            {
                thread.Join();
            }

            // Выводим результаты
            lock (consoleLock)
            {
                Console.WriteLine("\n📊 Результаты работы с общими данными:");
                Console.WriteLine($"Финальное значение счетчика: {sharedCounter}");
                Console.WriteLine($"Количество сохраненных чисел: {allNumbers.Count}");

                // Проверяем, нет ли дубликатов (при правильной синхронизации их быть не должно)
                var duplicates = allNumbers.GroupBy(x => x).Where(g => g.Count() > 1).ToList();
                if (duplicates.Any())
                {
                    Console.WriteLine("⚠️  Обнаружены дубликаты - проблема синхронизации!");
                }
                else
                {
                    Console.WriteLine("✅ Дубликатов нет - синхронизация работает правильно");
                }
            }
        }

        /// <summary>
        /// Метод для увеличения общего счетчика
        /// </summary>
        static void IncrementSharedCounter(int threadNumber)
        {
            Random rnd = new Random();

            for (int i = 0; i < 20; i++) // Каждый поток выполнит 20 итераций
            {
                // Безопасное увеличение счетчика с использованием lock
                int currentValue;

                lock (dataLock)
                {
                    sharedCounter++;
                    currentValue = sharedCounter;
                    allNumbers.Add(currentValue);
                }

                lock (consoleLock)
                {
                    ConsoleColor originalColor = Console.ForegroundColor;
                    Console.ForegroundColor = threadNumber switch
                    {
                        1 => ConsoleColor.Red,
                        2 => ConsoleColor.Green,
                        3 => ConsoleColor.Blue,
                        4 => ConsoleColor.Yellow,
                        5 => ConsoleColor.Magenta,
                        _ => ConsoleColor.White
                    };

                    Console.WriteLine($"[{Thread.CurrentThread.Name}] Увеличил счетчик до: {currentValue}");
                    Console.ForegroundColor = originalColor;
                }

                // Случайная задержка для имитации разной скорости работы
                Thread.Sleep(rnd.Next(50, 200));
            }
        }

        /// <summary>
        /// 4. Управление потоками (приостановка, возобновление)
        /// </summary>
        static void ThreadManagementExample()
        {
            Console.WriteLine("\nДемонстрация управления потоками...\n");

            // Флаги для управления потоком
            bool isPaused = false;
            bool shouldStop = false;

            // Создаем управляемый поток
            Thread managedThread = new Thread(() =>
            {
                int count = 0;

                while (!shouldStop && count < 50)
                {
                    // Проверяем, не поставлен ли поток на паузу
                    while (isPaused && !shouldStop)
                    {
                        Thread.Sleep(100); // Ждем, пока пауза не снимется
                    }

                    if (shouldStop) break;

                    count++;

                    lock (consoleLock)
                    {
                        Console.WriteLine($"[Управляемый поток] Счет: {count}");
                    }

                    Thread.Sleep(100);
                }

                lock (consoleLock)
                {
                    Console.WriteLine("[Управляемый поток] Завершил работу");
                }
            })
            {
                Name = "Управляемый поток"
            };

            // Запускаем поток
            managedThread.Start();

            // Даем потоку поработать немного
            Thread.Sleep(500);

            // Ставим на паузу
            lock (consoleLock)
            {
                Console.WriteLine("\n⏸️  Ставим поток на паузу на 2 секунды...");
            }
            isPaused = true;
            Thread.Sleep(2000);

            // Возобновляем работу
            lock (consoleLock)
            {
                Console.WriteLine("▶️  Возобновляем работу потока...");
            }
            isPaused = false;
            Thread.Sleep(1000);

            // Снова ставим на паузу
            lock (consoleLock)
            {
                Console.WriteLine("\n⏸️  Снова ставим на паузу на 1 секунду...");
            }
            isPaused = true;
            Thread.Sleep(1000);

            // Возобновляем и даем поработать
            isPaused = false;
            Thread.Sleep(1500);

            // Останавливаем поток
            lock (consoleLock)
            {
                Console.WriteLine("\n🛑 Останавливаем поток...");
            }
            shouldStop = true;

            // Ждем завершения потока
            managedThread.Join();

            Console.WriteLine("\n✅ Управление потоком завершено!");
        }

        /// <summary>
        /// 5. Пример использования ThreadPool
        /// </summary>
        static void ThreadPoolExample()
        {
            Console.WriteLine("\nИспользование ThreadPool для подсчета чисел...\n");

            // Используем ManualResetEvent для ожидания завершения всех задач
            ManualResetEvent[] doneEvents = new ManualResetEvent[10];
            ThreadPoolResult[] results = new ThreadPoolResult[10];

            // Запускаем 10 задач через ThreadPool
            for (int i = 0; i < 10; i++)
            {
                doneEvents[i] = new ManualResetEvent(false);
                results[i] = new ThreadPoolResult(i * 10 + 1, (i + 1) * 10);

                ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadPoolTask),
                    new Tuple<int, ManualResetEvent, ThreadPoolResult>(i, doneEvents[i], results[i]));
            }

            // Ждем завершения всех задач
            WaitHandle.WaitAll(doneEvents);

            // Выводим результаты
            lock (consoleLock)
            {
                Console.WriteLine("\n📊 Результаты ThreadPool:");
                foreach (var result in results)
                {
                    Console.WriteLine($"Задача {result.TaskId}: {result.Start}-{result.End} = {result.Sum}");
                }

                int totalSum = results.Sum(r => r.Sum);
                Console.WriteLine($"\nОбщая сумма всех чисел от 1 до 100: {totalSum}");
            }
        }

        /// <summary>
        /// Задача для ThreadPool
        /// </summary>
        static void ThreadPoolTask(object state)
        {
            var (taskId, doneEvent, result) = (Tuple<int, ManualResetEvent, ThreadPoolResult>)state;

            int sum = 0;
            for (int i = result.Start; i <= result.End; i++)
            {
                sum += i;
                Thread.Sleep(10); // Имитация работы
            }

            result.Sum = sum;

            lock (consoleLock)
            {
                Console.WriteLine($"[ThreadPool задача {taskId}] Завершена: {result.Start}-{result.End} = {sum}");
            }

            doneEvent.Set();
        }

        /// <summary>
        /// 6. Сравнение производительности
        /// </summary>
        static void PerformanceComparison()
        {
            Console.WriteLine("\nСравнение производительности разных подходов...\n");

            Stopwatch stopwatch = new Stopwatch();
            int numberOfIterations = 1000000; // 1 миллион итераций

            // 1. Последовательное выполнение
            Console.WriteLine("1. Последовательное выполнение:");
            stopwatch.Start();

            long sequentialSum = 0;
            for (int i = 1; i <= numberOfIterations; i++)
            {
                sequentialSum += i;
            }

            stopwatch.Stop();
            Console.WriteLine($"   Сумма: {sequentialSum}");
            Console.WriteLine($"   Время: {stopwatch.ElapsedMilliseconds} мс");

            // 2. Многопоточное выполнение
            Console.WriteLine("\n2. Многопоточное выполнение (4 потока):");
            stopwatch.Restart();

            long threadSum = 0;
            object sumLock = new object();

            Thread[] calcThreads = new Thread[4];
            for (int i = 0; i < 4; i++)
            {
                int threadNum = i;
                calcThreads[i] = new Thread(() =>
                {
                    long localSum = 0;
                    int start = threadNum * (numberOfIterations / 4) + 1;
                    int end = (threadNum == 3) ? numberOfIterations : (threadNum + 1) * (numberOfIterations / 4);

                    for (int j = start; j <= end; j++)
                    {
                        localSum += j;
                    }

                    lock (sumLock)
                    {
                        threadSum += localSum;
                    }
                });

                calcThreads[i].Start();
            }

            foreach (var thread in calcThreads)
            {
                thread.Join();
            }

            stopwatch.Stop();
            Console.WriteLine($"   Сумма: {threadSum}");
            Console.WriteLine($"   Время: {stopwatch.ElapsedMilliseconds} мс");

            // 3. ThreadPool
            Console.WriteLine("\n3. ThreadPool (4 задачи):");
            stopwatch.Restart();

            long poolSum = 0;
            object poolLock = new object();
            ManualResetEvent[] poolEvents = new ManualResetEvent[4];

            for (int i = 0; i < 4; i++)
            {
                poolEvents[i] = new ManualResetEvent(false);
                int taskNum = i;

                ThreadPool.QueueUserWorkItem(_ =>
                {
                    long localSum = 0;
                    int start = taskNum * (numberOfIterations / 4) + 1;
                    int end = (taskNum == 3) ? numberOfIterations : (taskNum + 1) * (numberOfIterations / 4);

                    for (int j = start; j <= end; j++)
                    {
                        localSum += j;
                    }

                    lock (poolLock)
                    {
                        poolSum += localSum;
                    }

                    poolEvents[taskNum].Set();
                });
            }

            WaitHandle.WaitAll(poolEvents);
            stopwatch.Stop();
            Console.WriteLine($"   Сумма: {poolSum}");
            Console.WriteLine($"   Время: {stopwatch.ElapsedMilliseconds} мс");

            // Сравнение результатов
            Console.WriteLine("\n📈 Выводы:");
            Console.WriteLine($"Все три метода дали одинаковую сумму: {(sequentialSum == threadSum && threadSum == poolSum ? "Да" : "Нет")}");

            if (sequentialSum == threadSum && threadSum == poolSum)
            {
                Console.WriteLine("✅ Все методы вычислений корректны");
            }
        }
    }

    /// <summary>
    /// Вспомогательный класс для хранения результатов ThreadPool
    /// </summary>
    class ThreadPoolResult
    {
        public int TaskId { get; set; }
        public int Start { get; }
        public int End { get; }
        public int Sum { get; set; }

        public ThreadPoolResult(int start, int end)
        {
            Start = start;
            End = end;
        }
    }
}