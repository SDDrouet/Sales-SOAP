using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Entities;
using SLC;
using BLL;

namespace Service.Controllers
{
    public class UserController : ApiController, IUserService
    {
        [HttpPost]
        public Users Create(Users user)
        {
            var userLogic = new UserLogic();
            var newUser = userLogic.Create(user);
            return newUser;
        }

        [HttpGet]
        public Users RetrieveById(int id)
        {
            var userLogic = new UserLogic();
            var user = userLogic.RetrieveById(id);
            return user;
        }

        [HttpPut]
        public bool Update(Users userToUpdate)
        {
            var userLogic = new UserLogic();
            var isUpdated = userLogic.Update(userToUpdate);
            return isUpdated;
        }

        [HttpPut]
        public bool ChangeStatus(int id)
        {
            var userLogic = new UserLogic();
            var isStatusChanged = userLogic.ChangeStatus(id);
            return isStatusChanged;
        }

        [HttpPost]
        public string Login(LoginRequest loginRequest)
        {
            var userLogic = new UserLogic();
            var token = userLogic.Login(loginRequest.Username, loginRequest.Password);
            return token;
        }

        [HttpPost]
        public bool Logout([FromBody] string token)
        {
            var userLogic = new UserLogic();
            var isLoggedOut = userLogic.Logout(token);
            return isLoggedOut;
        }

        [HttpGet]
        public List<Users> Filter(string filterUsername)
        {
            var userLogic = new UserLogic();
            var filteredUsers = userLogic.Filter(filterUsername);
            return filteredUsers;
        }
    }
}
