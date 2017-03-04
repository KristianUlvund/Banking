using System.Collections.Generic;
using TheBank.Model;
using TheBank.Model.DataContainer.Account;

namespace BLL
{
    public interface IAccountBLL
    {
        AccountPresentation getAccountData(string inAccountNumber, string inPersonalIdentification);
        List<AccountPrimitive> listAccountPrimitive(string PID);
    }
}