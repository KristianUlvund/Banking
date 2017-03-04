using System;
using System.Collections.Generic;
using TheBank.Model;
using TheBank.Model.DataContainer.Account;

namespace TheBank.DAL
{
    public interface IDB
    {
        bool changeTransaction(TransactionPresentation tp);
        byte[] Create_Hash(string inString);
        string Create_Salt();
        bool deactivateAccount(string accountNumber);
        int deleteTransaction(int tID);
        bool deleteUser(string pID);
        bool editAccount(string accountNumber, string accountName);
        bool editUser(Customer inCustomer);
        string executeTransaction(int transactionID);
        AccountPresentation getAccountData(string inAccountNumber, string inPersonalIdentification);
        List<Customer> getAllCustomers();
        string[] getUserInfo(string pID);
        List<AccountPrimitive> listAccountPrimitive(string PID);
        List<TransactionPresentation> listTransactions(bool isTransferred);
        List<TransactionPresentation> listTransactions(string accountNumber, int inMonth, int inYear, bool isTransferred);
        void logErrorToFile(Exception e);
        int produceRandomNumber();
        bool registerTransaction(TransactionPresentation inTransaction);
        string[] validateUser(string pID, string password);
    }
}