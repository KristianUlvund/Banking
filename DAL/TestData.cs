using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static TheBank.DAL.ModelContext;

namespace TheBank.DAL
{
    public static class TestData
    {
        //
        // Oppsummering:
        //     Representerer testdata til prosjektet. Brukere, kontoer og transaksjoner opprettes her.
        //
        public static void initialize_test_data()
        {
            DB daldb = new DB();
            // Open connection to Database
            using (var db = new ModelContext())
            {
                // Get user
                var exist = from a in db.Users where a.firstname == "Ola" select a;
                DateTime setDate = DateTime.Now;

                // Check if user exist
                if (!exist.Any())
                {
                    try
                    {
                        string[] salt = { daldb.Create_Salt(), daldb.Create_Salt(), daldb.Create_Salt() };

                        Postal newPostal = null;
                        // Postalcode
                        newPostal = new Postal
                        {
                            postalCode = "0707",
                            city = "Oslo"
                        };
                        

                        // accountType
                        AccountType newAccountType = new AccountType
                        {
                            card = true,
                            interest = 3.00,
                            limit = 10000,
                            yearlyFee = 0
                        };

                        // User1 account
                        Account account1 = new Account
                        {
                            accountNumber = "44444444444",
                            name = "Savings account",
                            balance = 10000,
                            accountType = newAccountType,
                            active = true
                        };
   
                        // User2 account1
                        Account account2_1 = new Account
                        {
                            accountNumber = "11111111111",
                            name = "Savings account",
                            balance = 4100.50,
                            accountType = newAccountType,
                            active = true
                        };
                        
                        // User2 account2
                        Account account2_2 = new Account
                        {
                            accountNumber = "22222222222",
                            name = "Checkings account",
                            balance = 1533.40,
                            accountType = newAccountType,
                            active = true
                        };

                        // User2 account3
                        Account account2_3 = new Account
                        {
                            accountNumber = "33333333333",
                            name = "Loan account",
                            balance = 0.00,
                            accountType = newAccountType,
                            active = false
                        };

                        // User3 account
                        Account account3 = new Account
                        {
                            accountNumber = "55555555555",
                            name = "Savings account",
                            balance = 2500,
                            accountType = newAccountType,
                            active = true
                        };

                        // Transaction from user 1 account to user 2 account
                        Transaction transaction1 = new Transaction
                        {
                            date = setDate.Date,
                            fromAccount = account1,
                            toAccount = account2_1,
                            amount = 500,
                            message = "Vi er skuls",
                            isTransferred = true
                        };
                        
                        // Transaction from user 2 account too user 3 account
                        Transaction transaction2 = new Transaction
                        {
                            date = setDate.Date,
                            fromAccount = account2_1,
                            toAccount = account3,
                            amount = 250,
                            message = "Mat greier",
                            isTransferred = true
                        };

                        // Transaction from user 3 account too user 1 account
                        Transaction transaction3 = new Transaction
                        {
                            date = setDate.Date,
                            fromAccount = account3,
                            toAccount = account1,
                            amount = 1500,
                            message = "Nasty stuff",
                            isTransferred = false
                        };

                        // Add user 1
                        User user1 = new User
                        {
                            personalIdentification = "11111111111",
                            firstname = "Ola",
                            lastname = "Nordmann",
                            address = "Nordmannveien 32",
                            postalCode = newPostal,
                            phoneNumber = "07070707",
                            email = "OlaNordmann@example.com",
                            salt = salt[0],
                            password = daldb.Create_Hash("password" + salt[0]),
                            active = true,
                            accounts = new List<Account> { account1 }
                        };

                        // Add user 2
                        User user2 = new User
                        {
                            personalIdentification = "22222222222",
                            firstname = "Kari",
                            lastname = "Nordmann",
                            address = "Nordmannveien 32",
                            postalCode = newPostal,
                            phoneNumber = "06060606",
                            email = "KariNordmann@example.com",
                            salt = salt[1],
                            password = daldb.Create_Hash("password" + salt[1]),
                            active = true,
                            accounts = new List<Account> { account2_1, account2_2, account2_3 }
                        };

                        // Add user 3
                        User user3 = new User
                        {
                            personalIdentification = "33333333333",
                            firstname = "Nils",
                            lastname = "Nordmann",
                            address = "Nordmannveien 32",
                            postalCode = newPostal,
                            phoneNumber = "05050505",
                            email = "NilsNordmann@example.com",
                            salt = salt[2],
                            password = daldb.Create_Hash("password3" + salt[2]),
                            active = true,
                            accounts = new List<Account> { account3 }
                        };

                        Admin admin1 = new Admin
                        {
                            personalIdentification = "22222222222"
                        };
                        db.Admins.Add(admin1);

                        // Add Transanctions
                        db.Transactions.Add(transaction1);
                        db.Transactions.Add(transaction2);
                        db.Transactions.Add(transaction3);

                        // Add Accounts
                        db.Accounts.Add(account1);
                        db.Accounts.Add(account2_1);
                        db.Accounts.Add(account2_2);
                        db.Accounts.Add(account2_3);
                        db.Accounts.Add(account3);

                        // Add Users
                        db.Users.Add(user1);
                        db.Users.Add(user2);
                        db.Users.Add(user3);
                        
                        // Save data to Database
                        db.SaveChanges();

                    }
                    catch (Exception error)
                    {
                        Console.Write(error);
                    }
                }
            }
        }
    }
}