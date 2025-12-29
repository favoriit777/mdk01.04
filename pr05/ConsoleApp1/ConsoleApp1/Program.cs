using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqPractice
{
    // Класс для представления сотрудника
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Department { get; set; }
        public decimal Salary { get; set; }

        // Метод для удобного вывода информации о сотруднике
        public override string ToString()
        {
            return $"{Id,2}. {LastName} {FirstName,-10} | Возраст: {Age,2} | Отдел: {Department,-10} | Зарплата: {Salary,8:C0}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Практическая работа 5: Основы LINQ to Objects ===");
            Console.WriteLine("=== Вариант 3: Группировка сотрудников по отделам ===\n");

            // Создаем список сотрудников
            List<Employee> employees = CreateEmployeeList();

            // Выводим всех сотрудников для наглядности
            Console.WriteLine("=== ВСЕ СОТРУДНИКИ ===");
            PrintEmployees(employees);

            // Выполняем задание варианта 3
            Console.WriteLine("\n=== РЕШЕНИЕ ВАРИАНТА 3 ===");
            Console.WriteLine("Задание: Сгруппировать сотрудников по отделам и вывести для каждого отдела");
            Console.WriteLine("количество сотрудников и среднюю зарплату.\n");

            // Способ 1: Синтаксис методов расширения
            Console.WriteLine("=== Способ 1: Синтаксис методов расширения ===");
            GroupByDepartmentUsingMethods(employees);

            Console.WriteLine();

            // Способ 2: Синтаксис запросов
            Console.WriteLine("=== Способ 2: Синтаксис запросов ===");
            GroupByDepartmentUsingQuerySyntax(employees);

            // Дополнительные демонстрации LINQ
            Console.WriteLine("\n=== ДОПОЛНИТЕЛЬНЫЕ ПРИМЕРЫ LINQ ===");
            DemonstrateAdditionalLinqQueries(employees);

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        // Метод для создания тестового списка сотрудников
        static List<Employee> CreateEmployeeList()
        {
            return new List<Employee>
            {
                new Employee { Id = 1, FirstName = "Иван", LastName = "Иванов", Age = 30, Department = "IT", Salary = 50000 },
                new Employee { Id = 2, FirstName = "Петр", LastName = "Петров", Age = 25, Department = "HR", Salary = 40000 },
                new Employee { Id = 3, FirstName = "Мария", LastName = "Сидорова", Age = 35, Department = "Финансы", Salary = 60000 },
                new Employee { Id = 4, FirstName = "Анна", LastName = "Кузнецова", Age = 28, Department = "IT", Salary = 55000 },
                new Employee { Id = 5, FirstName = "Алексей", LastName = "Смирнов", Age = 40, Department = "Финансы", Salary = 70000 },
                new Employee { Id = 6, FirstName = "Елена", LastName = "Попова", Age = 32, Department = "Маркетинг", Salary = 45000 },
                new Employee { Id = 7, FirstName = "Дмитрий", LastName = "Васильев", Age = 29, Department = "IT", Salary = 52000 },
                new Employee { Id = 8, FirstName = "Ольга", LastName = "Новикова", Age = 27, Department = "HR", Salary = 38000 },
                new Employee { Id = 9, FirstName = "Сергей", LastName = "Федоров", Age = 45, Department = "Финансы", Salary = 75000 },
                new Employee { Id = 10, FirstName = "Наталья", LastName = "Морозова", Age = 31, Department = "Маркетинг", Salary = 47000 },
                new Employee { Id = 11, FirstName = "Андрей", LastName = "Волков", Age = 26, Department = "IT", Salary = 48000 },
                new Employee { Id = 12, FirstName = "Татьяна", LastName = "Алексеева", Age = 33, Department = "Финансы", Salary = 65000 },
                new Employee { Id = 13, FirstName = "Михаил", LastName = "Лебедев", Age = 38, Department = "Продажи", Salary = 58000 },
                new Employee { Id = 14, FirstName = "Екатерина", LastName = "Семенова", Age = 24, Department = "HR", Salary = 36000 },
                new Employee { Id = 15, FirstName = "Артем", LastName = "Павлов", Age = 41, Department = "Продажи", Salary = 62000 }
            };
        }

        // Метод для вывода списка сотрудников
        static void PrintEmployees(List<Employee> employees)
        {
            foreach (var emp in employees)
            {
                Console.WriteLine(emp);
            }
        }

        // Способ 1: Группировка с использованием синтаксиса методов расширения
        static void GroupByDepartmentUsingMethods(List<Employee> employees)
        {
            // Группируем сотрудников по отделам
            var departmentGroups = employees
                .GroupBy(e => e.Department)  // Группировка по отделу
                .Select(g => new              // Проекция в новый объект
                {
                    Department = g.Key,       // Ключ группы (название отдела)
                    EmployeeCount = g.Count(), // Количество сотрудников в отделе
                    AverageSalary = g.Average(e => e.Salary), // Средняя зарплата
                    MaxSalary = g.Max(e => e.Salary),        // Максимальная зарплата
                    MinSalary = g.Min(e => e.Salary),        // Минимальная зарплата
                    TotalSalary = g.Sum(e => e.Salary)       // Общая зарплата отдела
                })
                .OrderByDescending(d => d.EmployeeCount) // Сортировка по количеству сотрудников
                .ThenBy(d => d.Department);              // Затем по названию отдела

            // Выводим результаты
            Console.WriteLine("┌────────────────────┬────────────────┬────────────────┬────────────────┬────────────────┬────────────────┐");
            Console.WriteLine("│ Отдел              │ Кол-во сотрудн.│ Средняя зарплата│ Макс. зарплата │ Мин. зарплата  │ Общая зарплата │");
            Console.WriteLine("├────────────────────┼────────────────┼────────────────┼────────────────┼────────────────┼────────────────┤");

            foreach (var dept in departmentGroups)
            {
                Console.WriteLine($"│ {dept.Department,-18} │ {dept.EmployeeCount,14} │ {dept.AverageSalary,14:C0} │ {dept.MaxSalary,14:C0} │ {dept.MinSalary,14:C0} │ {dept.TotalSalary,14:C0} │");
            }

            Console.WriteLine("└────────────────────┴────────────────┴────────────────┴────────────────┴────────────────┴────────────────┘");

            // Дополнительная статистика
            Console.WriteLine("\nДополнительная информация:");
            var richestDept = departmentGroups.OrderByDescending(d => d.AverageSalary).First();
            var largestDept = departmentGroups.OrderByDescending(d => d.EmployeeCount).First();

            Console.WriteLine($"Самый высокооплачиваемый отдел: {richestDept.Department} " +
                              $"(средняя зарплата: {richestDept.AverageSalary:C0})");
            Console.WriteLine($"Самый большой отдел: {largestDept.Department} " +
                              $"(количество сотрудников: {largestDept.EmployeeCount})");
        }

        // Способ 2: Группировка с использованием синтаксиса запросов
        static void GroupByDepartmentUsingQuerySyntax(List<Employee> employees)
        {
            // Группируем сотрудников по отделам с использованием синтаксиса запросов
            var departmentGroups = from emp in employees
                                   group emp by emp.Department into deptGroup
                                   orderby deptGroup.Count() descending, deptGroup.Key
                                   select new
                                   {
                                       Department = deptGroup.Key,
                                       EmployeeCount = deptGroup.Count(),
                                       AverageSalary = deptGroup.Average(e => e.Salary),
                                       MaxSalary = deptGroup.Max(e => e.Salary),
                                       MinSalary = deptGroup.Min(e => e.Salary),
                                       TotalSalary = deptGroup.Sum(e => e.Salary),
                                       Employees = deptGroup.OrderBy(e => e.LastName).ToList()
                                   };

            // Выводим результаты
            Console.WriteLine("┌────────────────────┬────────────────┬────────────────┬────────────────┬────────────────┬────────────────┐");
            Console.WriteLine("│ Отдел              │ Кол-во сотрудн.│ Средняя зарплата│ Макс. зарплата │ Мин. зарплата  │ Общая зарплата │");
            Console.WriteLine("├────────────────────┼────────────────┼────────────────┼────────────────┼────────────────┼────────────────┤");

            foreach (var dept in departmentGroups)
            {
                Console.WriteLine($"│ {dept.Department,-18} │ {dept.EmployeeCount,14} │ {dept.AverageSalary,14:C0} │ {dept.MaxSalary,14:C0} │ {dept.MinSalary,14:C0} │ {dept.TotalSalary,14:C0} │");
            }

            Console.WriteLine("└────────────────────┴────────────────┴────────────────┴────────────────┴────────────────┴────────────────┘");

            // Дополнительно выводим сотрудников по отделам
            Console.WriteLine("\nПодробная информация по отделам:");
            foreach (var dept in departmentGroups)
            {
                Console.WriteLine($"\n--- {dept.Department} ({dept.EmployeeCount} сотрудников) ---");
                foreach (var emp in dept.Employees)
                {
                    Console.WriteLine($"  {emp.LastName} {emp.FirstName}, Зарплата: {emp.Salary:C0}");
                }
            }
        }

        // Демонстрация дополнительных возможностей LINQ
        static void DemonstrateAdditionalLinqQueries(List<Employee> employees)
        {
            Console.WriteLine("1. Фильтрация: Сотрудники старше 30 лет:");
            var olderEmployees = employees.Where(e => e.Age > 30).OrderBy(e => e.LastName);
            foreach (var emp in olderEmployees)
            {
                Console.WriteLine($"  {emp.LastName} {emp.FirstName}, Возраст: {emp.Age}");
            }

            Console.WriteLine("\n2. Сортировка: Сотрудники по убыванию зарплаты:");
            var sortedBySalary = from emp in employees
                                 orderby emp.Salary descending
                                 select emp;
            foreach (var emp in sortedBySalary.Take(5)) // Берем только топ-5
            {
                Console.WriteLine($"  {emp.LastName} {emp.FirstName}, Зарплата: {emp.Salary:C0}");
            }

            Console.WriteLine("\n3. Проекция: Имена и зарплаты сотрудников IT отдела:");
            var itEmployees = employees
                .Where(e => e.Department == "IT")
                .Select(e => new { e.LastName, e.FirstName, e.Salary });
            foreach (var emp in itEmployees)
            {
                Console.WriteLine($"  {emp.LastName} {emp.FirstName}, Зарплата: {emp.Salary:C0}");
            }

            Console.WriteLine("\n4. Агрегация: Общая статистика по компании:");
            Console.WriteLine($"  Всего сотрудников: {employees.Count}");
            Console.WriteLine($"  Средний возраст: {employees.Average(e => e.Age):F1} лет");
            Console.WriteLine($"  Средняя зарплата: {employees.Average(e => e.Salary):C0}");
            Console.WriteLine($"  Максимальная зарплата: {employees.Max(e => e.Salary):C0}");
            Console.WriteLine($"  Минимальная зарплата: {employees.Min(e => e.Salary):C0}");
            Console.WriteLine($"  Общий фонд оплаты труда: {employees.Sum(e => e.Salary):C0}");

            Console.WriteLine("\n5. Разница между немедленным и отложенным выполнением:");

            // Отложенное выполнение (deferred execution)
            Console.WriteLine("  Отложенное выполнение:");
            var deferredQuery = employees.Where(e => e.Department == "IT");
            Console.WriteLine("  Запрос создан, но еще не выполнен");

            // Добавляем нового сотрудника
            employees.Add(new Employee { Id = 16, FirstName = "Новый", LastName = "Сотрудник", Age = 28, Department = "IT", Salary = 51000 });

            Console.WriteLine("  Добавили нового сотрудника в IT отдел");
            Console.WriteLine($"  Результат запроса (учитывает нового сотрудника): {deferredQuery.Count()} сотрудников");

            // Немедленное выполнение (immediate execution)
            Console.WriteLine("\n  Немедленное выполнение:");
            var immediateQuery = employees.Where(e => e.Department == "HR").ToList(); // ToList() вызывает немедленное выполнение
            Console.WriteLine("  Запрос выполнен немедленно с помощью ToList()");

            // Добавляем нового сотрудника
            employees.Add(new Employee { Id = 17, FirstName = "Еще", LastName = "Один", Age = 26, Department = "HR", Salary = 39000 });

            Console.WriteLine("  Добавили нового сотрудника в HR отдел");
            Console.WriteLine($"  Результат запроса (не учитывает нового сотрудника): {immediateQuery.Count} сотрудников");

            // Убираем добавленных сотрудников для чистоты данных
            employees.RemoveAll(e => e.Id >= 16);
        }
    }
}