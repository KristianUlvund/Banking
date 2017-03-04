using System.Collections.Generic;
using TheBank.Model;
using TheBank.Model.DataContainer.Account;
using TheBank.DAL;

namespace BLL
{
    public class AccountBLL : IAccountBLL
    {
        IDB _DB;
        // Actual database.
        public AccountBLL()
        {
            _DB = new DB();
        }

        // The stub
        public AccountBLL(IDB stub)
        {
            _DB = stub;
        }

        public List<AccountPrimitive> listAccountPrimitive(string PID)
        {
            return _DB.listAccountPrimitive(PID);
        }

        public AccountPresentation getAccountData(string inAccountNumber, string inPersonalIdentification)
        {
            return _DB.getAccountData(inAccountNumber, inPersonalIdentification);
        }

    }
}
