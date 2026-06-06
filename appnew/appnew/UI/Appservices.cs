using appnew.BL;
using appnew.DL;
using System.IO;

namespace appnew
{
    // Shared state across all forms
    internal static class AppServices
    {
        public static Hotel Hotel { get; private set; }

        public static void Init()
        {
            UserManager um = new UserManager();
            LoadUsers(um);
            Hotel = new Hotel(um);
            LoadRooms();
        }

        public static string CheckLogin(string u, string p)
        {
            UserManager um = Hotel.getUserManager();
            for (int i = 0; i < um.getCount(); i++)
                if (um.getUser(i).getUsername() == u && um.getUser(i).getPassword() == p)
                    return um.getUser(i).getRole();
            return "wrong";
        }

        public static bool UserExists(string u, string p, string r)
        {
            UserManager um = Hotel.getUserManager();
            for (int i = 0; i < um.getCount(); i++)
                if (um.getUser(i).getUsername() == u &&
                    um.getUser(i).getPassword() == p &&
                    um.getUser(i).getRole() == r)
                    return true;
            return false;
        }

        public static void SaveUsers()
        {
            UserManager um = Hotel.getUserManager();
            StreamWriter f = new StreamWriter("user.txt");
            for (int i = 0; i < um.getCount(); i++)
                f.WriteLine(um.getUser(i).getUsername() + "," +
                            um.getUser(i).getPassword() + "," +
                            um.getUser(i).getRole());
            f.Close();
        }

        public static void SaveRooms()
        {
            StreamWriter f = new StreamWriter("booking.txt");
            for (int i = 0; i < 35; i++)
            {
                if (Hotel.getRoom(i).getIsBooked())
                    f.WriteLine(i + "," +
                        Hotel.getRoom(i).getGuestName() + "," +
                        Hotel.getRoom(i).getRoomType() + "," +
                        Hotel.getRoom(i).getStayDays() + "," +
                        Hotel.getRoom(i).getCheckInDate() + "," +
                        Hotel.getRoom(i).getCheckOutDate());
            }
            f.Close();
        }

        private static void LoadUsers(UserManager um)
        {
            if (!File.Exists("user.txt")) return;
            foreach (string line in File.ReadAllLines("user.txt"))
            {
                string[] p = line.Split(',');
                if (p.Length == 3)
                {
                    User u = p[2].ToUpper() == "ADMIN"
                        ? (User)new Admin(p[0], p[1])
                        : new Employee(p[0], p[1]);
                    um.addUser(u);
                }
            }
        }

        private static void LoadRooms()
        {
            if (!File.Exists("booking.txt")) return;
            foreach (string line in File.ReadAllLines("booking.txt"))
            {
                string[] p = line.Split(',');
                if (p.Length == 6)
                {
                    int n = int.Parse(p[0]);
                    Hotel.getRoom(n).setGuestName(p[1]);
                    Hotel.getRoom(n).setRoomType(p[2]);
                    Hotel.getRoom(n).setStayDays(int.Parse(p[3]));
                    Hotel.getRoom(n).setCheckInDate(p[4]);
                    Hotel.getRoom(n).setCheckOutDate(p[5]);
                    Hotel.getRoom(n).setIsBooked(true);
                }
            }
        }
    }
}
