using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appnew.DL
{
    public class Room
    {
        private int roomNumber;
        private bool isBooked;
        private string guestName;
        private string roomType;
        private int stayDays;
        private string checkInDate;
        private string checkOutDate;

        // constructor
        public Room(int roomNumber)
        {
            this.roomNumber = roomNumber;
            this.isBooked = false;
            this.guestName = "";
            this.roomType = "";
            this.stayDays = 0;
            this.checkInDate = "";
            this.checkOutDate = "";
        }
        public int getRoomNumber() { return roomNumber; }
        public bool getIsBooked() { return isBooked; }
        public string getGuestName() { return guestName; }
        public string getRoomType() { return roomType; }
        public int getStayDays() { return stayDays; }
        public string getCheckInDate() { return checkInDate; }
        public string getCheckOutDate() { return checkOutDate; }


        public void setGuestName(string g) { guestName = g; }
        public void setRoomType(string t) { roomType = t; }
        public void setStayDays(int d) { stayDays = d; }
        public void setIsBooked(bool b) { isBooked = b; }
        public void setCheckInDate(string d) { checkInDate = d; }
        public void setCheckOutDate(string d) { checkOutDate = d; }

    }
}
