using System.Collections.Generic;
using TheBank.DAL;
using TheBank.Model;

namespace BLL
{
    public class AdminBLL : IAdminBLL
    {
        // Creating the variabel for the repository.
        private IDB _DB;
        
        // Actual database.
        public AdminBLL()
        {
            _DB = new DB();
        }

        // The stub
        public AdminBLL(IDB stub)
        {
            _DB = stub;
        }
        /*
         ********************************************************
         */

        /*
         * Stub is constructed.
         */ 
        public List<TransactionPresentation> listTransactions(bool isTranseferred)
        {
            return _DB.listTransactions(isTranseferred);
        }

        /*
         * Stub is constructed.
         */ 
        public List<Customer> getAllCustomers()
        {
            return _DB.getAllCustomers();
        }

        /*
         * Stub is constructed.
         */ 
        public string[] getUserInfo(string pID)
        {
            return _DB.getUserInfo(pID);
        }
        
        /*
         * Stub is constructed.
         */ 
        public bool editUser(Customer inCustomer)
        {
            return _DB.editUser(inCustomer);
        }

        /*
         * Stub is constructed.
         */ 
        public bool deleteUser(string pID)
        {
            return _DB.deleteUser(pID);
        }

        /*
         * Stub is constructed.
         */ 
        public string executeTransaction(int transactionID)
        {
            return _DB.executeTransaction(transactionID);
        }

        /*
         * Stub is constructed.
         */ 
        public bool editAccount(string accountNumber, string accountName)
        {
            return _DB.editAccount(accountNumber, accountName);
        }

        /*
         * Stub is constructed.
         */ 
        public bool deactivateAccount(string accountNumber)
        {
            return _DB.deactivateAccount(accountNumber);
        }
    }
}
