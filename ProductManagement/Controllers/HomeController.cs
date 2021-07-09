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
using ProductManagement.Service;
using AutoMapper;
using ProductManagement.Helpers;
using Microsoft.Extensions.Options;
using ProductManagement.Models;
using ProductManagement.Entities;

namespace ProductManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private IUserService _userService;
        private ICateService _cateService;
        private IProductService _productService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly IConfiguration _configuration;
        public HomeController(IUserService userService,IMapper mapper,
            IOptions<AppSettings> appSettings, IConfiguration configuration,
            ICateService cateService, IProductService productService
            )
        {
            _cateService = cateService;
            _productService = productService;
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _configuration = configuration;
        }
        [Route("getallUser")]
        [HttpGet]
        public IActionResult GetAllUser()
        {
            var users = _userService.GetAll();
            var model = _mapper.Map<IList<UserInfo>>(users);
            return Ok(model);
        }
        [Route("getallCate")]
        [HttpGet]
        public IActionResult GetAllCate()
        {
            var cates = _cateService.GetAll();
            var model = _mapper.Map<IList<Category>>(cates);
            return Ok(model);
        }
        [Route("getallProduct")]
        [HttpGet]
        public IActionResult GetAllProduct()
        {
            var products = _productService.GetAll();
            var model = _mapper.Map<IList<Product>>(products);
            return Ok(model);
        }

        [Route("login")]
        [HttpPost]
        public ActionResult PostLogin([FromBody] Login model)
        {
            var user = _userService.Authenticate(model.Email, model.Password);
            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });
             var claim = new[] {
               new Claim(JwtRegisteredClaimNames.Sub, user.Email)
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
                  UserId = user.UserId,
                  FirstName = user.FirstName,
                  LastName = user.LastName,
                  Username = user.UserName,
                  Email = user.Email,
                  token = new JwtSecurityTokenHandler().WriteToken(token),
                  expiration = token.ValidTo,
              });

          }




        [Route("register")]
        [HttpPost]        
        public ActionResult Register([FromForm] RegisterModel model)
        {
            var user = _mapper.Map<UserInfo>(model);

            try
            {
            // create user
                _userService.Create(user, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
        [Route("addProduct")]
        [HttpPost]
        public ActionResult AddProduct([FromForm] ProductModel model)
        {
            var product = _mapper.Map<Product>(model);

            try
            {
                // create user
                _productService.Create(product);
                return Ok(product);
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [Route("updateProduct")]
        [HttpPut]
        public ActionResult UpdateProduct([FromBody] ProductModel model, int id)
        {
            var product = _mapper.Map<Product>(model);
            product.ProductId = id;
            try
            {
                // create user
                _productService.Update(product);
                return Ok(product);
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
        [Route("deleteProduct")]
        [HttpDelete]
        public ActionResult DeleteProduct(int id)
        {
            _productService.Delete(id);
            return Ok();
        }
        [Route("addCate")]
        [HttpPost]
        public ActionResult AddCate([FromForm] CategoryModel model)
        {
            var cate = _mapper.Map<Category>(model);

            try
            {
                // create user
                _cateService.Create(cate);
                return Ok(cate);
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [Route("updateCate")]
        [HttpPut]
        public ActionResult UpdateCate([FromBody] CategoryModel model, int id)
        {
            var cate = _mapper.Map<Category>(model);
            cate.CategoryId = id;
            try
            {
                // create user
                _cateService.Update(cate);
                return Ok(cate);
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
        [Route("deleteCate")]
        [HttpDelete]
        public ActionResult DeleteCate(int id)
        {
            _cateService.Delete(id);
            return Ok();
        }

    }
}
