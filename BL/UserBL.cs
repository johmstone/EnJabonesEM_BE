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

        public User DetailsbyEmail(string Email)
        {
            return UDAL.DetailsbyEmail(Email);
        }

        public InfoEmailValidation ValidationEmailRequest(string Email)
        {
            return UDAL.ValidationEmailRequest(Email);
        }

        public int ValidateEmail(string EVToken)
        {
            return UDAL.ValidateEmail(EVToken);
        }

        public AuthorizationCode AuthCode(string email)
        {
            return UDAL.AuthCode(email);
        }

        public int ValidateGUID(string guid)
        {
            return UDAL.ValidateGUID(guid);
        }

        public bool ResetPassword(ResetPasswordModel model)
        {
            return UDAL.ResetPassword(model);
        }
    }
}
