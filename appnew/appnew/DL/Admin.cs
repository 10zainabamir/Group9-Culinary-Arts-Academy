using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appnew.DL
{
    public class Admin : User
    {
        public Admin(string username, string password)
            : base(username, password, "ADMIN")
        {

        }
        public override void showRole()
        {
            Console.WriteLine("I am an Admin");
        }
    }
}
