using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using kolisale.DBContext;
using kolisale.Models;
using kolisale.Repositories;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using System.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace kolisale.Controllers
{
    public class UserController : Controller
    {
        // private readonly ILogger<UserController> _logger;

        // public UserController(ILogger<UserController> logger)
        // {
        //     _logger = logger;
        // }
        // user implemention
        private readonly DapperContext context;

        public UserController(DapperContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var query = "SELECT * FROM Users";

            using (var connection = context.CreateConnection())
            {
                var users = await connection.QueryAsync<User>(query);
                return users.ToList();
            }
        }
        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            if(Request.Method=="GET")
            {
                var user = new User();
                var username = HttpContext.User.Identity.Name;
                var query = $"SELECT * FROM Users WHERE UserName = '{username}'";
                using (var connection = context.CreateConnection())
                {
                  user = await connection.QuerySingleOrDefaultAsync<User>(query);
                }

                return View(user);
            }
            return null;
        }
        public async Task<IActionResult> Login(User user)
        {
            var tr = new User();
            var query = $"SELECT * FROM Users WHERE UserName='{user.UserName}' AND Password='{user.Password}'";
                 using (var connection = context.CreateConnection())
                {
                  tr = await connection.QuerySingleOrDefaultAsync<User>(query);
                }

            if(tr==null)
            {
                ViewBag.Error="User does not exist";
                return View();
            }
            ClaimsIdentity identity = null;
            identity = new ClaimsIdentity(new[] {
            new Claim(ClaimTypes.Name, tr.UserName),
            }, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(principal);
            return RedirectToAction("Dashboard");
        }

        public async Task<IActionResult> RegisterUser(User user)
        {
            if(Request.Method=="POST")
            {
            var query = "INSERT INTO Users (Email, UserName, Password,FirstName, LastName, Address, City) VALUES (@Email, @UserName,@Password, @FirstName, @LastName, @Address, @City)";

            // var parameters = new DynamicParameters();
            // parameters.Add("Name", user.UserName, DbType.String);
            // parameters.Add("Address", user.Address, DbType.String);
            // parameters.Add("Country", user.City, DbType.String);
            // parameters.Add("Country", user.GlassdoorRating, DbType.Int32);

            using (var connection = context.CreateConnection())
            {
                await connection.ExecuteAsync(query, user);
            }
            return RedirectToAction("Dashboard");
            }
            else
            return View();
        }

        public async Task UpdateUserInfo(int id, User user)
        {
            var query = "INSERT INTO Users (Email, UserName, Password,FirstName, LastName,Address, City) VALUES (@Email, @UserName,@Password, @FirstName, @LastName @Address, @City WHERE Id = @Id)";
            // var parameters = new DynamicParameters();
            // parameters.Add("Name", user.UserName, DbType.String);
            // parameters.Add("Address", user.Address, DbType.String);
            // parameters.Add("Country", user.Country, DbType.String);
            // parameters.Add("Country", user.GlassdoorRating, DbType.Int32);
            using (var connection = context.CreateConnection())
            {
                await connection.ExecuteAsync(query, user);
            }
        }
        public async Task DeleteUser(int id)
        {

            var query = "DELETE FROM Users WHERE Id = @Id";
            using (var connection = context.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { id });
            }
        }
        public IActionResult Index()
        {
            return View();
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }

       
    }
}