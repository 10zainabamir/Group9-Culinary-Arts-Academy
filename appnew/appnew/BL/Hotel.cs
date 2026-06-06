using appnew.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appnew.BL
{
    public class Hotel
    {
        private Room[] rooms;
        private UserManager userManager;
        public Hotel(UserManager userManager)
        {
            this.userManager = userManager;

            rooms = new Room[35];
            for (int i = 0; i < 35; i++)
            {
                rooms[i] = new Room(i);
            }
        }

        public Room getRoom(int i) { return rooms[i]; }
        public UserManager getUserManager() { return userManager; }

    }
}
