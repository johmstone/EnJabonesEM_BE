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

        public List<User> List()
        {
            return UDAL.List();
        }

        public List<string> AdminUserList()
        {
            return UDAL.AdminUserList();
        }
        
        public bool Update(User user, string insertuser)
        {
            return UDAL.Update(user, insertuser);
        }

        public bool AdminResetPassword(int UserID, string InsertUser)
        {
            return UDAL.AdminResetPassword(UserID, InsertUser);
        }

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
