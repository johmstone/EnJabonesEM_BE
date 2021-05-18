using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ET;
using DAL;

namespace BL
{
    public class UserBL
    {
        private UserDAL UDAL = new UserDAL();

        public bool CheckUserEmailAvailability(string Email)
        {
            return UDAL.CheckUserEmailAvailability(Email);
        }

        public bool AddUser(User newuser, string InsertUser)
        {
            return UDAL.AddUser(newuser, InsertUser);
        }

        public User Login(UserLogin user)
        {
            return UDAL.Login(user);
        }

        public bool AddLogin(LoginRecord login)
        {
            return UDAL.AddLogin(login);
        }

        public User Details(int UserID)
        {
            return UDAL.Details(UserID);
        }
    }
}
