using System.Collections.Generic;
using TheBank.Model;

namespace BLL
{
    public interface IAdminBLL
    {
        bool deactivateAccount(string accountNumber);
        bool deleteUser(string pID);
        bool editAccount(string accountNumber, string accountName);
        bool editUser(Customer inCustomer);
        string executeTransaction(int transactionID);
        List<Customer> getAllCustomers();
        List<TransactionPresentation> listTransactions(bool isTransferred);
        string[] getUserInfo(string pID);
    }
}