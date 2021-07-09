using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Model;
using Microsoft.Extensions.Configuration;

namespace ProductManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserInfoesController : ControllerBase
    {
        private readonly UserDBContext _context;
        private readonly IConfiguration _configuration;

        public UserInfoesController(UserDBContext context , IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        
        [Route("login")]
        [HttpPost]
        public ActionResult PostLogin([FromBody] UserInfo user)
        {
            try
            {              
                 
                 var checkUser = _context.UserInfo.Where(a => a.Email.Equals(user.Email) && a.Password.Equals(user.Password)).FirstOrDefault();
                 if(checkUser != null)
                     //(_context.UserInfo.Any(s => s.Password == checkUser.Password) && _context.UserInfo.Any(s => s.Email == checkUser.Email))
                 {
                    if(_context.UserInfo.Any(s => s.Password == checkUser.Password) && _context.UserInfo.Any(s => s.Email == checkUser.Email))
                    {
                        var claim = new[] {
                         new Claim(JwtRegisteredClaimNames.Sub, checkUser.Email)
                     };
                        var signinKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(_configuration["Jwt:SigningKey"]));

                        int expiryInMinutes = Convert.ToInt32(_configuration["Jwt:ExpiryInMinutes"]);

                        var token = new JwtSecurityToken(
                          issuer: _configuration["Jwt:Site"],
                          audience: _configuration["Jwt:Site"],
                          expires: DateTime.UtcNow.AddMinutes(expiryInMinutes),
                          signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)
                        );
                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo,
                        });

                    }

                 }
                 else
                 {
                    return Ok(new Response { Status = "Error", Message = "Invalid Email Or Password !" });
                }
                return Ok(new Response { Status = "Error", Message = "Invalid Email Or Password !" });
            }
            catch (Exception e)
            {
                return Ok(e);
                    //Ok(new Response { Status = "Error", Message = "Error" });
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
                Password = GetMD5(rs.Password)
             

            };           

            _context.UserInfo.Add(user);
            await _context.SaveChangesAsync();
            int _insertID = user.UserId;
            if (_insertID > 0)
            {
                return Ok(new Response { Status = "Success", Message = "Register Successfully" });
            }
            return Ok(new Response {Status="Error",Message="Something Wrong"});
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
