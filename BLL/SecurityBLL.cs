using TheBank.DAL;

namespace BLL
{
    public class SecurityBLL
    {
        IDB _DB;

        public SecurityBLL()
        {
            _DB = new DB();
        }

        public SecurityBLL(IDB stub)
        {
            _DB = stub;
        }

        public string[] validateUser(string pID, string password)
        {
            return _DB.validateUser(pID, password);
        }

        public int produceRandomNumber()
        {
            return _DB.produceRandomNumber();
        }
    }
}
