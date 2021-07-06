using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Model;

namespace ProductManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserInfoesController : ControllerBase
    {
        private readonly UserDBContext _context;

        public UserInfoesController(UserDBContext context)
        {
            _context = context;
        }
        
        [Route("login")]
        [HttpPost]
        public ActionResult PostLogin(UserInfo _user)
        {
            try
            {
                var check = _context.UserInfo.Where(s => s.Email == _user.Email && s.Password == _user.Password).FirstOrDefault();
                if (check.UserId > 0)
                {
                    return Ok((new Response { Status = "Succsess", Message = "Login Successfully" }));
                }
                return Ok(new Response { Status="Error", Message="Invalid Email Or Password !"});

            }
            catch (Exception e)
            {
                return Ok(new Response { Status = "Error", Message = "Something Wrong !" });
            }

        }
        [Route("register")]
        [HttpPost]        
        public async Task<ActionResult> Post([FromForm] Register rs)
        {
            var check = _context.UserInfo.Where(s => s.Email == rs.Email.ToLower()).ToList();
            if (check.Count() > 0)
            {
                return Ok(new Response { Status = "Error", Message = "Email Is Exist !" });
            }
            var user = new UserInfo
            {
                
                FirstName = rs.FirstName,
                LastName = rs.LastName,
                UserName = rs.UserName,
                Email = rs.Email.ToLower(),                
                Password = rs.Password
             

            };           

            _context.UserInfo.Add(user);
            await _context.SaveChangesAsync();
            int _insertID = user.UserId;
            if (_insertID > 0)
            {
                return Ok(_insertID);
            }
            return Ok(new Response {Status="Success",Message="Register Successfully"});
        }
        //create a string MD5
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


        // GET: api/UserInfoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserInfo>>> GetUsers()
        {
            return await _context.UserInfo.ToListAsync();
        }

        // GET: api/UserInfoes/5
        

        private bool UserInfoExists(int id)
        {
            return _context.UserInfo.Any(e => e.UserId == id);
        }
    }
}
