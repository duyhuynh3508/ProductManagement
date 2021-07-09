using ProductManagement.Entities;
using ProductManagement.Helpers;
using ProductManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Service
{
    public interface IUserService
    {
        UserInfo Authenticate(string username, string password);
        IEnumerable<UserInfo> GetAll();
        UserInfo GetById(int id);
        UserInfo Create(UserInfo user, string password);
        void Update(UserInfo user, string password = null);
        void Delete(int id);
    }
    public class UserService : IUserService
    {
        private UserDBContext _context;
        public UserService(UserDBContext context)
        {
            _context = context;
        }
        public UserInfo Authenticate(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;

            var user = _context.UserInfo.SingleOrDefault(x => x.Email == email);

            // check if username exists
            if (user == null)
                return null;

            // check if password is correct
            if (!(user.Password == GetMD5(password)))
                return null;

            // authentication successful
            return user;
        }
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }

        public UserInfo Create(UserInfo user, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (_context.UserInfo.Any(x => x.Email == user.Email))
                throw new AppException("Email \"" + user.Email + "\" is already taken");
            string pass = GetMD5(password);
           

            user.Password = pass;
            

            _context.UserInfo.Add(user);
            _context.SaveChanges();

            return user;
        }

        public void Delete(int id)
        {
            var cate = _context.Category.Find(id);
            if (cate != null)
            {
                _context.Category.Remove(cate);
                _context.SaveChanges();
            }
        }

        public IEnumerable<UserInfo> GetAll()
        {
            return _context.UserInfo;
        }

        public UserInfo GetById(int id)
        {
            return _context.UserInfo.Find(id);
        }

        public void Update(UserInfo user, string password = null)
        {
            throw new NotImplementedException();
        }
    }
}
