using System;
using System.Collections.Generic;

// Базовий клас Account
public class Account
{
    public string Owner { get; set; }
    public decimal Balance { get; protected set; }

    public Account(string owner, decimal balance)
    {
        Owner = owner;
        Balance = balance;
    }

    // Віртуальний метод депозиту
    public virtual void Deposit(decimal amount)
    {
        Balance += amount;
        Console.WriteLine($"{Owner}: Deposit {amount:C}. New Balance: {Balance:C}");
    }

    // Віртуальний метод зняття коштів
    public virtual void Withdraw(decimal amount)
    {
        if (amount <= Balance)
        {
            Balance -= amount;
            Console.WriteLine($"{Owner}: Withdraw {amount:C}. New Balance: {Balance:C}");
        }
        else
        {
            Console.WriteLine($"{Owner}: Insufficient funds to withdraw {amount:C}.");
        }
    }

    // Вивід інформації про баланс
    public virtual void Display()
    {
        Console.WriteLine($"{Owner}: Account Balance: {Balance:C}");
    }
}

// Похідний клас CreditAccount із кредитним лімітом
public class CreditAccount : Account
{
    public decimal CreditLimit { get; private set; }

    public CreditAccount(string owner, decimal balance, decimal creditLimit)
        : base(owner, balance)
    {
        CreditLimit = creditLimit;
    }

    // Перевизначення методу зняття коштів із урахуванням кредитного ліміту
    public override void Withdraw(decimal amount)
    {
        if (amount <= Balance + CreditLimit)
        {
            Balance -= amount;
            Console.WriteLine($"{Owner}: Withdraw {amount:C} from CreditAccount. New Balance: {Balance:C}");
        }
        else
        {
            Console.WriteLine($"{Owner}: Credit limit exceeded. Cannot withdraw {amount:C}.");
        }
    }

    public override void Display()
    {
        Console.WriteLine($"{Owner}: Credit Account Balance: {Balance:C}, Credit Limit: {CreditLimit:C}");
    }
}

// Похідний клас SavingsAccount із процентною ставкою
public class SavingsAccount : Account
{
    public decimal InterestRate { get; private set; } // у відсотках

    public SavingsAccount(string owner, decimal balance, decimal interestRate)
        : base(owner, balance)
    {
        InterestRate = interestRate;
    }

    // Метод нарахування відсотків
    public void ApplyInterest()
    {
        decimal interest = Balance * InterestRate / 100;
        Balance += interest;
        Console.WriteLine($"{Owner}: Interest {interest:C} applied. New Balance: {Balance:C}");
    }

    public override void Display()
    {
        Console.WriteLine($"{Owner}: Savings Account Balance: {Balance:C}, Interest Rate: {InterestRate:F2}%");
    }
}

// Головна програма
class Program
{
    static void Main()
    {
        // Створюємо список рахунків різних типів
        List<Account> accounts = new List<Account>
        {
            new CreditAccount("Ivan", 500m, 200m),
            new SavingsAccount("Olena", 1000m, 5m),
            new Account("Petro", 300m)
        };

        // Виводимо початкові баланси
        foreach (var account in accounts)
        {
            account.Display();
        }
        Console.WriteLine();

        // Робимо операції депозита
        accounts[0].Deposit(200m);
        accounts[1].Deposit(200m);
        accounts[2].Deposit(200m);
        Console.WriteLine();

        // Робимо операції зняття коштів
        accounts[0].Withdraw(400m);
        accounts[1].Withdraw(400m);
        accounts[2].Withdraw(400m);
        Console.WriteLine();

        // Виводимо баланси після операцій
        foreach (var account in accounts)
        {
            account.Display();
        }
        Console.WriteLine();

        // Демонструємо додатковий метод нарахування відсотків у SavingsAccount
        if (accounts[1] is SavingsAccount savingsAccount)
        {
            savingsAccount.ApplyInterest();
        }
    }
}
