using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appnew.DL
{
    public class User
    {

        private string username;
        private string password;
        private string role;
        public User(string username, string password, string role)
        {
            this.username = username;
            this.password = password;
            this.role = role;
        }
        public virtual void showRole()
        {
            Console.WriteLine("General User");
        }
        public string getUsername() { return username; }
        public string getPassword() { return password; }
        public string getRole() { return role; }


    }
}
