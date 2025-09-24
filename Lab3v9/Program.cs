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

    public virtual void Deposit(decimal amount)
    {
        Balance += amount;
        Console.WriteLine($"{Owner}: Deposit {amount:C}. New Balance: {Balance:C}");
    }

    public virtual void Withdraw(decimal amount)
    {
        if (amount <= Balance)
        {
            Balance -= amount;
            Console.WriteLine($"{Owner}: Withdraw {amount:C}. New Balance: {Balance:C}");
        }
        else
        {
            Console.WriteLine($"{Owner}: Withdraw {amount:C} FAILED. Not enough funds.");
        }
    }

    public virtual void DisplayInfo()
    {
        Console.WriteLine($"{Owner}: Account Balance: {Balance:C}");
    }
}

// Похідний клас CreditAccount
public class CreditAccount : Account
{
    public decimal CreditLimit { get; set; }

    public CreditAccount(string owner, decimal balance, decimal creditLimit)
        : base(owner, balance)
    {
        CreditLimit = creditLimit;
    }

    public override void Withdraw(decimal amount)
    {
        if (amount <= Balance + CreditLimit)
        {
            Balance -= amount;
            Console.WriteLine($"{Owner}: Withdraw {amount:C} from CreditAccount. New Balance: {Balance:C}");
        }
        else
        {
            Console.WriteLine($"{Owner}: Withdraw {amount:C} FAILED. Over credit limit.");
        }
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"{Owner}: Credit Account Balance: {Balance:C}, Credit Limit: {CreditLimit:C}");
    }
}

// Похідний клас SavingsAccount
public class SavingsAccount : Account
{
    public decimal InterestRate { get; set; }

    public SavingsAccount(string owner, decimal balance, decimal interestRate)
        : base(owner, balance)
    {
        InterestRate = interestRate;
    }

    public void ApplyInterest()
    {
        decimal interest = Balance * InterestRate;
        Balance += interest;
        Console.WriteLine($"{Owner}: Interest {interest:C} applied. New Balance: {Balance:C}");
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"{Owner}: Savings Account Balance: {Balance:C}, Interest Rate: {InterestRate:P}");
    }
}

class Program
{
    static void Main(string[] args)
    {
        List<Account> accounts = new List<Account>
        {
            new CreditAccount("Ivan", 500, 200),
            new SavingsAccount("Olena", 1000, 0.05m),
            new Account("Petro", 300)
        };

        foreach (var acc in accounts)
        {
            acc.DisplayInfo();
            acc.Deposit(200);
            acc.Withdraw(400);
            acc.DisplayInfo();
            Console.WriteLine();
        }

        SavingsAccount savings = new SavingsAccount("Marta", 2000, 0.1m);
        savings.ApplyInterest();
    }
}
