using System;
using System.Collections.Generic;

namespace BankAccountsHierarchy
{
    // Абстрактный базовый класс
    public abstract class BankAccount
    {
        public string AccountNumber { get; protected set; }
        public string OwnerName { get; set; }
        public decimal Balance { get; protected set; }
        public DateTime OpenDate { get; protected set; }

        public BankAccount(string accountNumber, string ownerName, decimal initialBalance)
        {
            AccountNumber = accountNumber;
            OwnerName = ownerName;
            Balance = initialBalance;
            OpenDate = DateTime.Now;
        }

        // Минимум 2 абстрактных метода
        public abstract void Deposit(decimal amount);
        public abstract bool Withdraw(decimal amount);

        // Минимум 2 виртуальных метода
        public virtual void DisplayAccountInfo()
        {
            Console.WriteLine($"Account #: {AccountNumber}");
            Console.WriteLine($"Owner: {OwnerName}");
            Console.WriteLine($"Balance: {Balance:C}");
            Console.WriteLine($"Opened: {OpenDate:d}");
        }

        public virtual void AddInterest()
        {
            // Базовая реализация — ничего не делает, переопределяется в дочерних классах
        }

        // Общий метод для получения информации
        public abstract string GetAccountInfo();
    }

    // Производный класс - Сберегательный счет
    public class SavingsAccount : BankAccount
    {
        public decimal InterestRate { get; set; } // Процентная ставка

        public SavingsAccount(string accountNumber, string ownerName, decimal initialBalance, decimal interestRate)
            : base(accountNumber, ownerName, initialBalance)
        {
            InterestRate = interestRate;
        }

        public override void Deposit(decimal amount)
        {
            if (amount > 0)
            {
                Balance += amount;
                Console.WriteLine($"Deposited {amount:C} to Savings Account {AccountNumber}");
            }
            else
            {
                Console.WriteLine("Invalid deposit amount");
            }
        }

        public override bool Withdraw(decimal amount)
        {
            if (amount > 0 && amount <= Balance)
            {
                Balance -= amount;
                Console.WriteLine($"Withdrew {amount:C} from Savings Account {AccountNumber}");
                return true;
            }
            Console.WriteLine("Insufficient funds or invalid amount");
            return false;
        }

        // Переопределение виртуального метода
        public override void AddInterest()
        {
            decimal interest = Balance * InterestRate / 100;
            Balance += interest;
            Console.WriteLine($"Interest of {interest:C} added to Savings Account {AccountNumber}");
        }

        public override string GetAccountInfo()
        {
            return $"Savings Account #{AccountNumber} - Owner: {OwnerName} - Balance: {Balance:C} - Interest Rate: {InterestRate}%";
        }
    }

    // Производный класс - Текущий счет
    public class CheckingAccount : BankAccount
    {
        public decimal OverdraftLimit { get; set; } // Лимит перерасхода

        public CheckingAccount(string accountNumber, string ownerName, decimal initialBalance, decimal overdraftLimit)
            : base(accountNumber, ownerName, initialBalance)
        {
            OverdraftLimit = overdraftLimit;
        }

        public override void Deposit(decimal amount)
        {
            if (amount > 0)
            {
                Balance += amount;
                Console.WriteLine($"Deposited {amount:C} to Checking Account {AccountNumber}");
            }
        }

        public override bool Withdraw(decimal amount)
        {
            if (amount > 0 && (Balance + OverdraftLimit) >= amount)
            {
                Balance -= amount;
                Console.WriteLine($"Withdrew {amount:C} from Checking Account {AccountNumber}");
                return true;
            }
            Console.WriteLine("Insufficient funds or exceeds overdraft limit");
            return false;
        }

        public override string GetAccountInfo()
        {
            return $"Checking Account #{AccountNumber} - Owner: {OwnerName} - Balance: {Balance:C} - Overdraft Limit: {OverdraftLimit:C}";
        }
    }

    // Производный класс - Кредитный счет
    public class CreditAccount : BankAccount
    {
        public decimal CreditLimit { get; set; }
        public decimal InterestRate { get; set; } // Процент по кредиту

        public CreditAccount(string accountNumber, string ownerName, decimal initialBalance, decimal creditLimit, decimal interestRate)
            : base(accountNumber, ownerName, initialBalance)
        {
            CreditLimit = creditLimit;
            InterestRate = interestRate;
        }

        public override void Deposit(decimal amount)
        {
            if (amount > 0)
            {
                Balance += amount;
                Console.WriteLine($"Deposited {amount:C} to Credit Account {AccountNumber}");
            }
        }

        public override bool Withdraw(decimal amount)
        {
            if (amount > 0 && (Balance - amount) >= -CreditLimit)
            {
                Balance -= amount;
                Console.WriteLine($"Withdrew {amount:C} from Credit Account {AccountNumber}");
                return true;
            }
            Console.WriteLine("Exceeds credit limit");
            return false;
        }

        public override void AddInterest()
        {
            if (Balance < 0)
            {
                decimal interest = Math.Abs(Balance) * InterestRate / 100;
                Balance -= interest;
                Console.WriteLine($"Interest of {interest:C} applied to Credit Account {AccountNumber}");
            }
        }

        public override string GetAccountInfo()
        {
            return $"Credit Account #{AccountNumber} - Owner: {OwnerName} - Balance: {Balance:C} - Credit Limit: {CreditLimit:C}";
        }
    }

    // Производный класс - Депозитный счет
    public class DepositAccount : BankAccount
    {
        public DateTime MaturityDate { get; set; }
        public decimal FixedInterestRate { get; set; }

        public DepositAccount(string accountNumber, string ownerName, decimal initialBalance, DateTime maturityDate, decimal interestRate)
            : base(accountNumber, ownerName, initialBalance)
        {
            MaturityDate = maturityDate;
            FixedInterestRate = interestRate;
        }

        public override void Deposit(decimal amount)
        {
            if (amount > 0)
            {
                Balance += amount;
                Console.WriteLine($"Deposited {amount:C} to Deposit Account {AccountNumber}");
            }
        }

        public override bool Withdraw(decimal amount)
        {
            if (DateTime.Now >= MaturityDate)
            {
                if (amount > 0 && amount <= Balance)
                {
                    Balance -= amount;
                    Console.WriteLine($"Withdrew {amount:C} from Deposit Account {AccountNumber}");
                    return true;
                }
            }
            else
            {
                Console.WriteLine("Cannot withdraw before maturity date");
            }
            return false;
        }

        public override void AddInterest()
        {
            // Рост процентов если срок истек
            if (DateTime.Now >= MaturityDate)
            {
                decimal interest = Balance * FixedInterestRate / 100;
                Balance += interest;
                Console.WriteLine($"Interest of {interest:C} added to Deposit Account {AccountNumber}");
            }
        }

        public override string GetAccountInfo()
        {
            return $"Deposit Account #{AccountNumber} - Owner: {OwnerName} - Balance: {Balance:C} - Maturity Date: {MaturityDate:d}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Коллекция базового типа
            List<BankAccount> accounts = new List<BankAccount>
            {
                new SavingsAccount("SA001", "Alice", 5000, 5),
                new CheckingAccount("CA001", "Bob", 2000, 500),
                new CreditAccount("CR001", "Charlie", 0, 10000, 12),
                new DepositAccount("DA001", "Diana", 10000, DateTime.Now.AddMonths(6), 3)
            };

            // Полиморфизм: вызов методов через базовый тип
            foreach (var account in accounts)
            {
                account.DisplayAccountInfo(); // виртуальный метод
                Console.WriteLine();

                // Демонстрация методов
                account.Deposit(100);
                account.Withdraw(50);
                account.AddInterest();
                Console.WriteLine(account.GetAccountInfo());
                Console.WriteLine("------------------------");
            }
        }
    }
}