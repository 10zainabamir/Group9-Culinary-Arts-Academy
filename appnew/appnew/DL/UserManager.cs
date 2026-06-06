using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appnew.DL
{
    public class UserManager
    {
        private User[] users;
        private int count;

        // constructor
        public UserManager()
        {
            users = new User[20];
            count = 0;
        }

        public int getCount() { return count; }
        public User getUser(int i) { return users[i]; }

        public void addUser(User u)
        {
            users[count] = u;
            count++;
        }

        public void removeUser(int i)
        {
            for (int j = i; j < count - 1; j++)
            {
                users[j] = users[j + 1];
            }
            count--;
        }

    }
}
