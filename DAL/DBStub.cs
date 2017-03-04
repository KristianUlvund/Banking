using System;
using System.Collections.Generic;
using TheBank.DAL;
using TheBank.Model;
using TheBank.Model.DataContainer.Account;

namespace DAL
{
    public class DBStub : IDB
    {
        /*
         * Not part of obligatory #2
         */ 
        public bool changeTransaction(TransactionPresentation tp)
        {
            throw new NotImplementedException();
        }

        /*
         * Not part of obligatory #2
         */
        public byte[] Create_Hash(string inString)
        {
            throw new NotImplementedException();
        }

        /*
         * Not part of obligatory #2
         */
        public string Create_Salt()
        {
            throw new NotImplementedException();
        }

        /*
         * Not part of obligatory #2
         */
        public int deleteTransaction(int tID)
        {
            throw new NotImplementedException();
        }

        /*
         * This stub will return a boolean value to simulate the success or lack of success regarding
         * the deletion of a user in the database.
         */ 
        public bool deleteUser(string pID)
        {
            if(pID == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /*
         * This stub will return a boolean value to stimulate the success of lack of success regarding
         * the deletion of an account in the database.
         */
        public bool deactivateAccount(string accountName)
        {
            if(accountName == null)
            {
                return false;
            }
            else if(accountName == "no balance")
            {
                return false;
            }
            else
            {
                return true;
            }
        } 

        /*
         * This stub returns a boolean value to emulate a successful or unsuccessful edit
         * of account information.
         */ 
         public bool editAccount (string accountNumber, string accountName)
        {
            if(accountNumber == null || accountName == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /*
         * This stub returns a boolean value to emulate a successful or unsuccessfull edit
         * of a users information.
         */ 
        public bool editUser(Customer inCustomer)
        {
            if(inCustomer == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /*
         * This stub will return one of three strings depending on what the input is.
         */ 
        public string executeTransaction(int transactionID)
        {
            if(transactionID == 0)
            {
                return "Already Transferred!";
            }
            else if(transactionID == 1)
            {
                return "Not enough money";
            }
            else
            {
                return "ok";
            }
        }

        /*
         * Not part of obligatory #2
         */
        public AccountPresentation getAccountData(string inAccountNumber, string inPersonalIdentification)
        {
            throw new NotImplementedException();
        }

        /*
         * Making a list consisting of three customers
         */
        public List<Customer> getAllCustomers()
        {
            List<Customer> custList = new List<Customer>();
            Customer cust1 = new Customer();
            Customer cust2 = new Customer();
            Customer cust3 = new Customer();

            // Customer 1
            cust1.personalIdentification = "11223344551";
            cust1.firstname = "Knut";
            cust1.lastname = "Iversen";
            cust1.address = "Trondheimgate 3";
            cust1.zipCode = "0101";
            cust1.city = "Trondheim";
            cust1.phoneNumber = "11223344";
            cust1.email = "knut.iversen@gmail.com";
            custList.Add(cust1);

            // Customer 2
            cust2.personalIdentification = "22334455667";
            cust2.firstname = "Lise";
            cust2.lastname = "Iversen";
            cust2.address = "Oslogate 3";
            cust2.zipCode = "0202";
            cust2.city = "Oslo";
            cust2.phoneNumber = "22334455";
            cust2.email = "lise.iversen@gmail.com";
            custList.Add(cust2);

            // Customer 3
            cust3.personalIdentification = "33445566778";
            cust3.firstname = "Arild";
            cust3.lastname = "Iversen";
            cust3.address = "Stavangergate 3";
            cust3.zipCode = "0303";
            cust3.city = "Stavanger";
            cust3.phoneNumber = "33445566";
            cust3.email = "arild.iversen@gmail.com";
            custList.Add(cust3);

            return custList;
        }

        /*
         * Returning a list of TransactionPresentation with three Transactions.
         *
         */
        public List<TransactionPresentation> listTransactions(bool isTransferred)
        {
            List <TransactionPresentation> tpList   = new List<TransactionPresentation>();
            TransactionPresentation tp1             = new TransactionPresentation();
            TransactionPresentation tp2             = new TransactionPresentation();
            TransactionPresentation tp3             = new TransactionPresentation();

            // First transaction
            tp1.ID = 1;
            tp1.date = "07.11.2016";
            tp1.amount = 100.00;
            tp1.message = "This is the first transaction";
            tp1.toAccount = "22222222222";
            tp1.fromAccount = "11111111111";
            tpList.Add(tp1);

            // Second transaction
            tp2.ID = 2;
            tp2.date = "07.11.2016";
            tp2.amount = 200.00;
            tp2.message = "This is the second transaction";
            tp2.toAccount = "33333333333";
            tp2.fromAccount = "22222222222";
            tpList.Add(tp2);

            // Thirs transaction
            tp3.ID = 3;
            tp3.date = "07.11.2016";
            tp3.amount = 300.00;
            tp3.message = "This is the third transaction";
            tp3.toAccount = "44444444444";
            tp3.fromAccount = "33333333333";
            tpList.Add(tp3);

            return tpList;
        }

        /*
         * Recives a string and uses that string to return an array of strings with user information.
         * 
         */
        public string[] getUserInfo(string pID)
        {
            // Check if the personal identification is empty (not existing).
            if (pID == null)
            {
                return null;
            }
            // Creates an array of strings with user info.
            else
            {
                string[] userInfo = new string[8];

                userInfo[0] = pID;
                userInfo[1] = "Synne";
                userInfo[2] = "Sørensen";
                userInfo[3] = "synne.sorensen@gmail.com";
                userInfo[4] = "Bodøgate 3";
                userInfo[5] = "999887766";
                userInfo[6] = "8909";
                userInfo[7] = "Bodø";

                return userInfo;
            }
        }

        /*
         * Not part of obligatory #2
         */
        public List<AccountPrimitive> listAccountPrimitive(string pID)
        {
            List<AccountPrimitive> userAccounts = new List<AccountPrimitive>();
            userAccounts.Add(new AccountPrimitive("11111111111", "Konto 1", 100, true));
            userAccounts.Add(new AccountPrimitive("22222222222", "Konto 2", 2100, true));
            userAccounts.Add(new AccountPrimitive("33333333333", "Konto 3", 3100, true));
            return userAccounts;
        }

        /*
         * Not part of obligatory #2
         */
        public List<TransactionPresentation> listTransactions(string accountNumber, int inMonth, int inYear, bool isTransferred)
        {
            throw new NotImplementedException();
        }


        public void logErrorToFile(Exception e)
        {
            throw new NotImplementedException();
        }
        

        /*
         * Not part of obligatory #2
         */
        public int produceRandomNumber()
        {
            throw new NotImplementedException();
        }

        /*
         * Not part of obligatory #2
         */
        public bool registerTransaction(TransactionPresentation inTransaction)
        {
            throw new NotImplementedException();
        }

        /*
         * Not part of obligatory #2
         */
        public string[] validateUser(string pID, string password)
        {
            if(pID == "11111111111" && password == "pass")
            {
                return new string[1];
            }
            else
            {
                return null;
            }
        }
    }
}
