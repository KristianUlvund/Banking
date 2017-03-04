using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using TheBank.Model;
using TheBank.Model.DataContainer.Account;
using System.IO;
using static TheBank.DAL.ModelContext;

namespace TheBank.DAL
{
    public class DB : IDB
    {

        private static string fileName = "db_error_log.txt";
        private static string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

        public List<TransactionPresentation> listTransactions(bool isTransferred)
        {
            try
            {
                DateTime today = DateTime.Today;
                List<TransactionPresentation> allTransactions = new List<TransactionPresentation>();
                List<Transaction> someTransactions;
                using (var db = new ModelContext())
                {
                    someTransactions = db.Transactions
                        .Where(t => t.date <= today && t.isTransferred == isTransferred)
                        .ToList();

                    if (someTransactions.Count == 0)
                    {
                        return null;
                    }

                    foreach(var t in someTransactions)
                    {
                        TransactionPresentation tp = new TransactionPresentation()
                        {
                            ID = t.tID,
                            date = t.date.ToShortDateString(),
                            amount = t.amount,
                            message = t.message,
                            toAccount = readableAccNmBr(t.toAccount.accountNumber),
                            fromAccount = readableAccNmBr(t.fromAccount.accountNumber)
                        };
                        allTransactions.Add(tp);
                    }
                    return allTransactions;
                }
            }
            catch(Exception error)
            {
                logErrorToFile(error);
                return null;
            }
        }

        public List<AccountPrimitive> listAccountPrimitive(string PID)
        {
            try
            {
                List<AccountPrimitive> userAccounts = new List<AccountPrimitive>();
                using (var db = new ModelContext())
                {
                    foreach (var a in db.Users.FirstOrDefault
                        (u => u.personalIdentification == PID).accounts)
                    {
                        userAccounts.Add(new AccountPrimitive(readableAccNmBr(a.accountNumber), a.name, a.balance, a.active));
                    }
                }
                return userAccounts;
            }
            catch (Exception error)
            {
                logErrorToFile(error);
                return null;   
            }
        }

        public bool registerTransaction(TransactionPresentation inTransaction)
        {
            var thisAcc = inTransaction.fromAccount;
            var toAcc = inTransaction.toAccount;

            thisAcc = workableAccNmBr(thisAcc);
            toAcc = workableAccNmBr(toAcc);

            try
            {
                using (var db = new ModelContext())
                {
                    Account fromThisAccount = db.Accounts.FirstOrDefault
                        (a => a.accountNumber == thisAcc);

                    if (fromThisAccount == null)
                    {
                        return false;
                    }

                    Account toThisAccount = db.Accounts.FirstOrDefault
                        (a => a.accountNumber == toAcc);

                    if(toThisAccount.active != true)
                    {
                        return false;
                    }

                    Transaction newTransaction = new Transaction()
                    {
                        date = Convert.ToDateTime(inTransaction.date),
                        toAccount = toThisAccount,
                        amount = inTransaction.amount,
                        message = inTransaction.message,
                        isTransferred = false,
                        fromAccount = fromThisAccount
                    };

                    //fromAccount.transaction = new List<Transaction>();
                    db.Transactions.Add(newTransaction);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception error)
            {
                logErrorToFile(error);
                return false;
            }
        }

        public List<TransactionPresentation> listTransactions(string accountNumber, int inMonth, int inYear, bool isTransferred)
        {
            accountNumber = workableAccNmBr(accountNumber);
            DateTime startDate, endDate;
            startDate = new DateTime(inYear, inMonth, 1);
            endDate = new DateTime(inYear, inMonth, DateTime.DaysInMonth(inYear, inMonth));

            List<Transaction> allTransactions;
            List<TransactionPresentation> someTransactions = new List<TransactionPresentation>();

            try
            {
                using (var db = new ModelContext())
                {
                    var query = from t in db.Transactions
                                where t.fromAccount.accountNumber == accountNumber
                                && t.isTransferred == isTransferred
                                && t.date <= endDate && t.date >= startDate
                                select t;
                    allTransactions = query.ToList();

                    foreach (Transaction t in allTransactions)
                    {
                        TransactionPresentation transactionPresentation = new TransactionPresentation()
                        {
                            ID = t.tID,
                            date = t.date.ToShortDateString(),
                            amount = t.amount,
                            message = t.message,
                            toAccount = readableAccNmBr(t.toAccount.accountNumber)
                        };
                        someTransactions.Add(transactionPresentation);
                    }
                }
                return someTransactions;
            }
            catch(Exception error)
            {
                logErrorToFile(error);
                return null;
            }
        }

        public string executeTransaction(int transactionID)
        {
            try
            {
                Account fromAccount, toAccount;
                double sum;
                using (var db = new ModelContext())
                {
                     Transaction transaction = db.Transactions.FirstOrDefault
                        (t => t.tID == transactionID);

                    sum = transaction.amount;
                    fromAccount = transaction.fromAccount;
                    toAccount = transaction.toAccount;

                    if(transaction.isTransferred == true)
                    {
                        return "Already transferred!";
                    }

                    if (fromAccount.balance < sum)
                    {
                        return "Not enough money";
                    }
                    else
                    {
                        fromAccount.balance -= sum;
                        toAccount.balance += sum;
                        transaction.isTransferred = true;
                        db.SaveChanges();
                        return "ok";
                    }
                }
            }
            catch (Exception error)
            {
                logErrorToFile(error);
                return "Something went wrong";
            }
        }

        // Spør Tor om man kan ha denne funksjonen i DAL!
        public byte[] Create_Hash(string inString)
        {
            byte[] input, output;
            SHA256 algorithm = SHA256.Create();
            input = Encoding.UTF8.GetBytes(inString);
            output = algorithm.ComputeHash(input);
            return output;
        }

        public string[] validateUser(string pID, string password)
        {
            string[] userInfo = new string[4];

            using (var db = new ModelContext())
            {
                User user = db.Users.FirstOrDefault
                    (u => u.personalIdentification == pID);

                if (!(user == null))
                {
                    string salt = user.salt;

                    byte[] hashedPassword = Create_Hash(password + salt);
                    if (hashedPassword.SequenceEqual(user.password))
                    {
                        userInfo[0] = pID;
                        userInfo[1] = user.firstname;
                        userInfo[2] = user.lastname;

                        Admin isAdmin = db.Admins.FirstOrDefault
                            (a => a.personalIdentification == pID);

                        if (isAdmin != null)
                        {
                            userInfo[3] = "true";
                        }
                        else
                        {
                            userInfo[3] = "false";
                        }

                        return userInfo;
                    }
                    else
                    {
                        return null;
                    }

                }
                else
                {
                    return null;
                }
            }
        }

        public string[] getUserInfo(string pID)
        {
            string[] userInfo = new string[8];

            using (var db = new ModelContext())
            {
                User user = db.Users.FirstOrDefault(u => u.personalIdentification == pID);

                if (!(user == null))
                {
                    userInfo[0] = pID;
                    userInfo[1] = user.firstname;
                    userInfo[2] = user.lastname;
                    userInfo[3] = user.email;
                    userInfo[4] = user.address;
                    userInfo[5] = user.phoneNumber;
                    userInfo[6] = user.postalCode.postalCode;
                    userInfo[7] = user.postalCode.city;

                    return userInfo;
                }
                else
                {
                    return null;
                }
            }
        }

        public  AccountPresentation getAccountData(string inAccountNumber, string inPersonalIdentification)
        {
            inAccountNumber = workableAccNmBr(inAccountNumber);
            AccountPresentation accountPresentation = new AccountPresentation();
            using (var db = new ModelContext())
            {
                User user = db.Users.FirstOrDefault(u => u.personalIdentification == inPersonalIdentification);
                accountPresentation.firstname = user.firstname;
                accountPresentation.lastname = user.lastname;

                foreach(Account a in user.accounts)
                {
                    if(a.accountNumber == inAccountNumber)
                    {
                        accountPresentation.accountNumber = readableAccNmBr(a.accountNumber);
                        accountPresentation.name = a.name;
                        accountPresentation.balance = a.balance;
                        accountPresentation.active = a.active;
                        accountPresentation.card = a.accountType.card;
                        accountPresentation.interest = a.accountType.interest;
                        accountPresentation.limit = a.accountType.limit;
                        accountPresentation.yearlyFee = a.accountType.yearlyFee;
                        return accountPresentation;
                    }
                }                    
            }
            return null;
        }

        public int deleteTransaction(int tID)
        {
            int result = 0;
            using (var db = new ModelContext())
            {
                Transaction delete = (from t in db.Transactions
                                where t.tID == tID
                                select t).FirstOrDefault();
                if(delete == null)
                {
                    return result;
                }

                db.Transactions.Remove(delete);
                result = db.SaveChanges();
            }
            return result;
        }

        public bool changeTransaction(TransactionPresentation tp)
        {
            var thisAcc = workableAccNmBr(tp.fromAccount);
            var toAcc = workableAccNmBr(tp.toAccount);
            try
            {
                using (var db = new ModelContext())
                {

                    Account toThisAccount = db.Accounts.FirstOrDefault
                        (a => a.accountNumber == toAcc);

                    Account fromThisAccount = db.Accounts.FirstOrDefault
                        (a => a.accountNumber == thisAcc);

                    Transaction updatedTransaction = db.Transactions.FirstOrDefault(t => t.tID == tp.ID);
                    updatedTransaction.fromAccount = fromThisAccount;
                    updatedTransaction.toAccount = toThisAccount;
                    updatedTransaction.amount = tp.amount;
                    updatedTransaction.message = tp.message;
                    updatedTransaction.date = Convert.ToDateTime(tp.date);

                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception error)
            {
                logErrorToFile(error);
                return false;
            }
        }

        /*
         * This function will search for and collect all the customers in the database.
         */
        public List<Customer> getAllCustomers()
        {
            try
            {
                using (var db = new ModelContext())
                {
                    // The list of Customers that are to be returned.
                    List<Customer> allCustomers = new List<Customer>();

                    // The query to the database.
                    var query = from c in db.Users select c;

                    // Looping through the query to add each customer in the final list.
                    foreach (User c in query)
                    {
                        Customer oneCustomer = new Customer();
                        oneCustomer.personalIdentification  = c.personalIdentification;
                        oneCustomer.firstname               = c.firstname;
                        oneCustomer.lastname                = c.lastname;
                        oneCustomer.address                 = c.address;
                        oneCustomer.zipCode                 = c.postalCode.postalCode;
                        oneCustomer.phoneNumber             = c.phoneNumber;
                        oneCustomer.email                   = c.email;
                        allCustomers.Add(oneCustomer);
                    }
                    return allCustomers;
                } 
            }
            catch(Exception error)
            {
                logErrorToFile(error);
                return null;
            }
            
        }

        /*
         * Function to change a customers information.
         * This function will recieve a customer (a mix model). First the function will check if the customer
         * exists in the database for security reasons. If it exists the user data will be overwritten.
         * This function will also check if the new zipcode entered by the user exists or not. If it does not
         * exists it will be added to the database.
         */
        public bool editUser(Customer inCustomer)
        {
            try
            {
                using (var db = new ModelContext())
                {
                    // Find the user
                    User selectUser = db.Users.FirstOrDefault
                        (u => u.personalIdentification == inCustomer.personalIdentification);

                    // User does not exists or something went wrong.
                    if (selectUser == null)
                    {
                        return false;
                    }
                    else
                    {
                        // Overwrite old data with new data.
                        selectUser.firstname    = inCustomer.firstname;
                        selectUser.lastname     = inCustomer.lastname;
                        selectUser.address      = inCustomer.address;
                        selectUser.phoneNumber  = inCustomer.phoneNumber;
                        selectUser.email        = inCustomer.email;

                        // Find the postal.
                        Postal verifyPostal = db.Postals.FirstOrDefault
                            (p => p.postalCode == inCustomer.zipCode);

                        // Postal does not exist.
                        if (verifyPostal == null)
                        {
                            // Adding new posta to database and merge with customer.
                            Postal newPostal = new Postal();
                            newPostal.postalCode    = inCustomer.zipCode;
                            newPostal.city          = inCustomer.city;
                            db.Postals.Add(newPostal);
                            selectUser.postalCode   = newPostal;
                        }
                        else
                        {
                            // Postal exists. Merge with customer.
                            selectUser.postalCode = verifyPostal;
                        }
                        db.SaveChanges();
                        return true;
                    }
                }
            }
            catch (Exception error)
            {
                logErrorToFile(error);
                return false;
            }
        }

        /*
         * This function will delete a user and all relevant information.
         * It will remove the information inside-out, beginning from transaction, then
         * continuing to account and then the user itself.
         */ 
        public bool deleteUser(string pID)
        {
            using (var db = new ModelContext())
            {
                User user = db.Users.FirstOrDefault
                        (c => c.personalIdentification == pID);

                if (user == null)
                {
                    return false;
                }
                else
                {
                    var accounts = db.Users
                    .Where(u => u.personalIdentification == pID)
                    .Select(u => u.accounts).FirstOrDefault();


                    foreach (Account a in accounts)
                    {
                        var transactions = db.Transactions
                            .Where(t => t.fromAccount.accountNumber == a.accountNumber)
                            .ToList();

                        foreach(Transaction t in transactions)
                        {
                            db.Transactions.Remove(t);
                        }
                        db.Accounts.Remove(a);
                    }

                    Admin isAdmin = db.Admins.FirstOrDefault
                            (a => a.personalIdentification == pID);

                    if(isAdmin != null)
                    {
                        db.Admins.Remove(isAdmin);
                    }

                    db.Users.Remove(user);
                    db.SaveChanges();
                    return true;
                }
            }
        }

        public bool editAccount(string accountNumber, string accountName)
        {
            accountNumber = workableAccNmBr(accountNumber);
            try
            {
                using (var db = new ModelContext())
                {
                    // Find the user
                    Account selectedAccount = db.Accounts.FirstOrDefault
                        (u => u.accountNumber == accountNumber);

                    // User does not exists or something went wrong.
                    if (selectedAccount == null)
                    {
                        return false;
                    }
                    else
                    {
                        // Overwrite old data with new data.
                        selectedAccount.name = accountName;

                        db.SaveChanges();
                        return true;
                    }
                }
            }
            catch (Exception error)
            {
                logErrorToFile(error);
                return false;
            }
        }

        public bool deactivateAccount(string accountNumber)
        {
            accountNumber = workableAccNmBr(accountNumber);
            try
            {
                using (var db = new ModelContext())
                {
                    var account = db.Accounts.FirstOrDefault(a => a.accountNumber == accountNumber);
                    if(!account.active)
                    {
                        account.active = true;
                        db.SaveChanges();
                        return true;
                    }
                    else
                    {
                        if (account.balance != 0.00)
                        {
                            return false;
                        }
                        else
                        {
                            account.active = false;
                            db.SaveChanges();
                            return true;
                        }
                    }
                }
            }
            catch(Exception error)
            {
                logErrorToFile(error);
                return false;
            }

        }

        public int produceRandomNumber()
        {
            int randomGeneratedNumber;
            StringBuilder strBld = new StringBuilder();
            Random rnd = new Random();

            for (int i = 0; i < 6; i++)
            {
                strBld.Append(rnd.Next(1, 9));
            }
            randomGeneratedNumber = Convert.ToInt32(strBld.ToString());
            return randomGeneratedNumber;
        }

        public string Create_Salt()
        {
            byte[] randomArray = new byte[10];
            string randomString;

            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomArray);
            randomString = Convert.ToBase64String(randomArray);
            return randomString;
        }

        public void logErrorToFile(Exception e)
        {
            // Gets the last string of the strackTrace
            int line = Convert.ToInt32(e.StackTrace.Substring(e.StackTrace.LastIndexOf(' ')));
            string newline = Environment.NewLine;

            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine("Date: " + DateTime.Now.ToString()
                                                        + newline
                                                        + newline
                                + "Line: "              + line
                                                        + newline
                                                        + newline
                                + "Message: "           + e.Message
                                                        + newline
                                                        + newline
                                + "StackTrace: "        + e.StackTrace
                                                        + newline
                                                        + newline
                                );
                writer.WriteLine("-----------------------------------------------------------------------------" 
                                 + newline);
            }
        }

        public string readableAccNmBr(string accountNumber)
        {
            return accountNumber.Insert(4, ".").Insert(7, ".");
        }

        public string workableAccNmBr(string accountNumber)
        {
            var test = accountNumber.Remove(4, 1).Remove(6, 1);
            return test;
        }
    }
}