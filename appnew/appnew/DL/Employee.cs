using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appnew.DL
{
    public class Employee : User
    {
        public Employee(string username, string password)
            : base(username, password, "EMPLOYEE")
        {

        }
        public override void showRole()
        {
            Console.WriteLine("I am an Employee");
        }
    }
}
