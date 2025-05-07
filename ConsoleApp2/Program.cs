using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    public class BankAccount
    {
        public string Name { get; set; }
        public int Balance { get; set; }

        public BankAccount(string name, int balance)
        {
            Name = name;
            Balance = balance;
        }

        public void Deposit(int amount)
        {
            Balance += amount;
            Console.WriteLine("Balance successfully updated");
        }

        public void Withdraw(int amount)
        {
            if (Balance >= amount)
            {
                Balance -= amount;
                Console.WriteLine("Withdrawal successful");
            }
            else
            {
                Console.WriteLine("Insufficient balance");
            }
        }

        public void ViewBalance()
        {
            Console.WriteLine($"Account Holder: {Name}");
            Console.WriteLine($"Current balance: {Balance}");
        }
    }

    class Bank
    {
        public List<BankAccount> Accounts = new List<BankAccount>();
        private const string filePath = "accounts.txt";  // File path to store accounts data

        public Bank()
        {
            LoadAccounts();  // Load accounts from file when the program starts
        }

        // Create an account and save to the file
        public void CreateAccount()
        {
            Console.WriteLine("Enter your name:");
            string nameInput = Console.ReadLine();

            Console.WriteLine("Enter initial balance:");
            int balanceInput = int.Parse(Console.ReadLine());

            BankAccount newAccount = new BankAccount(nameInput, balanceInput);
            Accounts.Add(newAccount);

            // Save account to file
            SaveAccountToFile(newAccount);

            Console.WriteLine($"Account created for {nameInput} with ${balanceInput}");
        }

        // Access an account by name
        public void AccessAccount()
        {
            Console.WriteLine("Enter your name:");
            string inputName = Console.ReadLine();

            BankAccount currentAccount = null;

            foreach (var account in Accounts)
            {
                if (account.Name == inputName)
                {
                    currentAccount = account;
                    Console.WriteLine($"Welcome {inputName}");
                    break;
                }
            }

            if (currentAccount == null)
            {
                Console.WriteLine("Account not found");
                return;
            }

            // Sub-menu for managing the account
            while (true)
            {
                Console.WriteLine("\n1: Deposit");
                Console.WriteLine("2: Withdraw");
                Console.WriteLine("3: View Balance");
                Console.WriteLine("4: Exit to main menu");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Enter deposit amount:");
                        int depositAmount = int.Parse(Console.ReadLine());
                        currentAccount.Deposit(depositAmount);
                        break;
                    case "2":
                        Console.WriteLine("Enter withdrawal amount:");
                        int withdrawAmount = int.Parse(Console.ReadLine());
                        currentAccount.Withdraw(withdrawAmount);
                        break;
                    case "3":
                        currentAccount.ViewBalance();
                        break;
                    case "4":
                        return;  // Exit to main menu
                    default:
                        Console.WriteLine("Invalid choice");
                        break;
                }
            }
        }

        // Save a new account to the file
        private void SaveAccountToFile(BankAccount account)
        {
            string accountData = $"{account.Name},{account.Balance}\n";
            File.AppendAllText(filePath, accountData);
        }

        // Load accounts from the file
        private void LoadAccounts()
        {
            if (File.Exists(filePath))
            {
                string[] accountLines = File.ReadAllLines(filePath);
                foreach (string line in accountLines)
                {
                    string[] accountInfo = line.Split(',');
                    string name = accountInfo[0];
                    int balance = int.Parse(accountInfo[1]);

                    BankAccount loadedAccount = new BankAccount(name, balance);
                    Accounts.Add(loadedAccount);
                }
            }
            else
            {
                Console.WriteLine("No previous data found, starting fresh.");
            }
        }

        static void Main()
        {
            Bank bank = new Bank();

            while (true)
            {
                Console.WriteLine("\n1: Create Account");
                Console.WriteLine("2: Access Account");
                Console.WriteLine("3: Exit");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        bank.CreateAccount();
                        break;
                    case "2":
                        bank.AccessAccount();
                        break;
                    case "3":
                        Console.WriteLine("Exiting...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice");
                        break;
                }
            }
        }
    }
}
