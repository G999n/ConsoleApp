using System;
using System.Collections.Generic;

class Program
{
    class User
    {
        public string Username;
        public string Password;
        public List<Account> Accounts = new List<Account>();
    }

    class Account
    {
        public string AccountNumber;
        public string AccountHolder;
        public string AccountType;
        public double Balance;
        public List<Transaction> Transactions = new List<Transaction>();
    }

    class Transaction
    {
        public string Id;
        public DateTime Date;
        public string Type;
        public double Amount;
    }

    static List<User> users = new List<User>();
    static User loggedInUser;

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\n1. Register\n2. Login\n3. Exit");
            string choice = Console.ReadLine();

            if (choice == "1") Register();
            else if (choice == "2") Login();
            else if (choice == "3") break;
            else Console.WriteLine("Invalid option. Try again.");
        }
    }

    static void Register()
    {
        Console.Write("Enter Username: ");
        string username = Console.ReadLine();
        Console.Write("Enter Password: ");
        string password = Console.ReadLine();

        if (users.Exists(u => u.Username == username))
        {
            Console.WriteLine("Username already exists.");
            return;
        }

        users.Add(new User { Username = username, Password = password });
        Console.WriteLine("Registration successful.");
    }

    static void Login()
    {
        Console.Write("Enter Username: ");
        string username = Console.ReadLine();
        Console.Write("Enter Password: ");
        string password = Console.ReadLine();

        loggedInUser = users.Find(u => u.Username == username && u.Password == password);

        if (loggedInUser == null)
        {
            Console.WriteLine("Invalid credentials.");
            return;
        }

        Console.WriteLine("Login successful.");
        UserMenu();
    }

    static void UserMenu()
    {
        while (true)
        {
            Console.WriteLine("\n1. Open Account\n2. Deposit\n3. Withdraw\n4. Statement\n5. Check Balance\n6. Calculate Interest\n7. Logout");
            string choice = Console.ReadLine();

            if (choice == "1") OpenAccount();
            else if (choice == "2") Deposit();
            else if (choice == "3") Withdraw();
            else if (choice == "4") GenerateStatement();
            else if (choice == "5") CheckBalance();
            else if (choice == "6") CalculateInterest();
            else if (choice == "7") break;
            else Console.WriteLine("Invalid option. Try again.");
        }
    }

    static void OpenAccount()
    {
        Console.Write("Enter Account Holder Name: ");
        string name = Console.ReadLine();
        Console.Write("Enter Account Type (savings/checking): ");
        string type = Console.ReadLine().ToLower();
        Console.Write("Enter Initial Deposit: ");
        double deposit = double.Parse(Console.ReadLine());

        string accountNumber = "AC" + (loggedInUser.Accounts.Count + 1);

        loggedInUser.Accounts.Add(new Account
        {
            AccountNumber = accountNumber,
            AccountHolder = name,
            AccountType = type,
            Balance = deposit
        });

        Console.WriteLine($"Account opened successfully. Account Number: {accountNumber}");
    }

    static void Deposit()
    {
        Console.Write("Enter Account Number: ");
        string accNum = Console.ReadLine();
        var account = loggedInUser.Accounts.Find(a => a.AccountNumber == accNum);

        if (account == null)
        {
            Console.WriteLine("Account not found.");
            return;
        }

        Console.Write("Enter Deposit Amount: ");
        double amount = double.Parse(Console.ReadLine());

        account.Balance += amount;
        LogTransaction(account, "Deposit", amount);
        Console.WriteLine("Deposit successful.");
    }

    static void Withdraw()
    {
        Console.Write("Enter Account Number: ");
        string accNum = Console.ReadLine();
        var account = loggedInUser.Accounts.Find(a => a.AccountNumber == accNum);

        if (account == null)
        {
            Console.WriteLine("Account not found.");
            return;
        }

        Console.Write("Enter Withdrawal Amount: ");
        double amount = double.Parse(Console.ReadLine());

        if (amount > account.Balance)
        {
            Console.WriteLine("Insufficient funds.");
            return;
        }

        account.Balance -= amount;
        LogTransaction(account, "Withdrawal", amount);
        Console.WriteLine("Withdrawal successful.");
    }

    static void LogTransaction(Account account, string type, double amount)
    {
        account.Transactions.Add(new Transaction
        {
            Id = Guid.NewGuid().ToString(),
            Date = DateTime.Now,
            Type = type,
            Amount = amount
        });
    }

    static void GenerateStatement()
    {
        Console.Write("Enter Account Number: ");
        string accNum = Console.ReadLine();
        var account = loggedInUser.Accounts.Find(a => a.AccountNumber == accNum);

        if (account == null)
        {
            Console.WriteLine("Account not found.");
            return;
        }

        Console.WriteLine("\nTransaction History:");
        foreach (var t in account.Transactions)
        {
            Console.WriteLine($"{t.Date} - {t.Type} - ${t.Amount}");
        }
    }

    static void CheckBalance()
    {
        Console.Write("Enter Account Number: ");
        string accNum = Console.ReadLine();
        var account = loggedInUser.Accounts.Find(a => a.AccountNumber == accNum);

        if (account == null)
        {
            Console.WriteLine("Account not found.");
            return;
        }

        Console.WriteLine($"Current Balance: ${account.Balance}");
    }

    static void CalculateInterest()
    {
        Console.Write("Enter Account Number: ");
        string accNum = Console.ReadLine();
        var account = loggedInUser.Accounts.Find(a => a.AccountNumber == accNum);

        if (account == null)
        {
            Console.WriteLine("Account not found.");
            return;
        }

        if (account.AccountType != "savings")
        {
            Console.WriteLine("Interest calculation is only for savings accounts.");
            return;
        }

        double interestRate = 0.01; // 1% monthly interest
        double interest = account.Balance * interestRate;
        account.Balance += interest;
        LogTransaction(account, "Interest", interest);

        Console.WriteLine($"Interest added. New Balance: ${account.Balance}");
    }
}
