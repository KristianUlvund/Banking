using System;
using System.Collections.Generic;
using TheBank.Model;
using TheBank.DAL;

namespace BLL
{
    public class TransactionBLL
    {
        DB db = new DB();

        public bool registerTransaction(TransactionPresentation inTransaction)
        {
            return db.registerTransaction(inTransaction);
        }

        public List<TransactionPresentation> listTransactions(string accountNumber, int inMonth, int inYear, bool isTransferred)
        {
            return db.listTransactions(accountNumber, inMonth, inYear, isTransferred);
        }

        public int deleteTransaction(int tID)
        {
            return db.deleteTransaction(tID);
        }

        public bool changeTransaction(TransactionPresentation tp)
        {
            return db.changeTransaction(tp);
        }
    }
}
