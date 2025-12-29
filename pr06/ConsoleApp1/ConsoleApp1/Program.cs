using System;

namespace VehicleManagement
{
    class Program
    {
        static void Main()
        {
            // Создаем 4 объекта автомобилей
            Car car1 = new Car("Toyota", "Camry", 2018, "Красный", 30000, 20000m);
            Car car2 = new Car("BMW", "X5", 2020, "Черный", 15000, 45000m);
            Car car3 = new Car("Honda", "Civic", 2015, "Белый", 50000, 12000m);
            Car car4 = new Car("Ford", "Focus", 2012, "Синий", 80000, 8000m);

            // Вывод информации
            car1.DisplayInfo();
            Console.WriteLine();
            car2.DisplayInfo();
            Console.WriteLine();

            // Обновление пробега
            car3.UpdateMileage(52000);
            // Расчет амортизации
            decimal depreciatedValue = car2.CalculateDepreciation();
            Console.WriteLine($"Амортизированная стоимость {car2.Brand} {car2.Model}: ${depreciatedValue:F2}");

            // Изменение цены
            car4.ChangePrice(10); // увеличение на 10%
            Console.WriteLine("После повышения цены:");
            car4.DisplayInfo();
        }
    }
    public class Car
    {
        // Поля
        private string brand;
        private string model;
        private int year;
        private string color;
        private double mileage;
        private decimal price;

        // Свойства (с валидацией)
        public string Brand
        {
            get { return brand; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Brand cannot be empty");
                brand = value;
            }
        }

        public string Model
        {
            get { return model; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Model cannot be empty");
                model = value;
            }
        }

        public int Year
        {
            get { return year; }
            set
            {
                if (value < 1886 || value > DateTime.Now.Year) // Год первого автомобиля - 1886
                    throw new ArgumentException("Invalid year");
                year = value;
            }
        }

        public string Color
        {
            get { return color; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Color cannot be empty");
                color = value;
            }
        }

        public double Mileage
        {
            get { return mileage; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Mileage cannot be negative");
                mileage = value;
            }
        }

        public decimal Price
        {
            get { return price; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Price cannot be negative");
                price = value;
            }
        }

        // Конструктор по умолчанию
        public Car() { }

        // Конструктор с параметрами
        public Car(string brand, string model, int year, string color, double mileage, decimal price)
        {
            Brand = brand;
            Model = model;
            Year = year;
            Color = color;
            Mileage = mileage;
            Price = price;
        }

        // Методы

        // Вывод информации
        public void DisplayInfo()
        {
            Console.WriteLine($"Машина: {Brand} {Model}");
            Console.WriteLine($"Год: {Year}");
            Console.WriteLine($"Цвет: {Color}");
            Console.WriteLine($"Пробег: {Mileage} km");
            Console.WriteLine($"Цена: ${Price}");
            Console.WriteLine($"Возраст: {CalculateAge()} лет");
        }

        // Обновление пробега
        public void UpdateMileage(double newMileage)
        {
            if (newMileage < Mileage)
                Console.WriteLine("Новій пробег не может быть меньше текущего");
            else
                Mileage = newMileage;
        }

        // Расчет амортизации (например, снижение стоимости на 10% за каждый год использования)
        public decimal CalculateDepreciation()
        {
            int age = CalculateAge();
            decimal depreciationRate = 0.10m * age;
            decimal depreciatedValue = Price * (1 - depreciationRate);
            return depreciatedValue > 0 ? depreciatedValue : 0;
        }

        // Изменение цены
        public void ChangePrice(decimal percentage)
        {
            if (percentage < -100 || percentage > 100)
            {
                Console.WriteLine("Некорректный процент изменения цены");
                return;
            }
            Price = Price + (Price * (decimal)percentage / 100);
        }

        // Расчет возраста автомобиля
        public int CalculateAge()
        {
            return DateTime.Now.Year - Year;
        }
    }
}