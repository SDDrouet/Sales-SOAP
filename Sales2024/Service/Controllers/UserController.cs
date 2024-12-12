using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Entities;
using SLC;
using BLL;
using Security;

namespace Service.Controllers
{
    public class UserController : ApiController, IUserService
    {
        [HttpPost]
        [PublicRoute]
        public Users Create(Users user)
        {
            var userLogic = new UserLogic();
            var newUser = userLogic.Create(user);
            return newUser;
        }

        [HttpGet]
        [AuthorizeRoles("ADMIN", "EDITOR", "VIEWER")]
        public Users RetrieveById(int id)
        {
            var userLogic = new UserLogic();
            var user = userLogic.RetrieveById(id);
            return user;
        }

        [HttpPut]
        [AuthorizeRoles("ADMIN", "EDITOR", "VIEWER")]
        public bool Update(Users userToUpdate)
        {
            var userLogic = new UserLogic();
            var isUpdated = userLogic.Update(userToUpdate);
            return isUpdated;
        }

        [HttpPut]
        [AuthorizeRoles("ADMIN")]
        public bool ChangeStatus(int id)
        {
            var userLogic = new UserLogic();
            var isStatusChanged = userLogic.ChangeStatus(id);
            return isStatusChanged;
        }

        [HttpPost]
        [PublicRoute]
        public string Login(LoginRequest loginRequest)
        {
            var userLogic = new UserLogic();
            var token = userLogic.Login(loginRequest.Username, loginRequest.Password);
            return token;
        }

        [HttpPost]
        [PublicRoute]
        public bool Logout([FromBody] string token)
        {
            var userLogic = new UserLogic();
            var isLoggedOut = userLogic.Logout(token);
            return isLoggedOut;
        }

        [HttpGet]
        [AuthorizeRoles("ADMIN")]
        public List<Users> Filter(string filterUsername)
        {
            var userLogic = new UserLogic();
            var filteredUsers = userLogic.Filter(filterUsername);
            return filteredUsers;
        }

        [HttpGet]
        [PublicRoute]
        public bool ActivateAccount(string token)
        {
            var userLogic = new UserLogic();
            var isActive = userLogic.ActivateAccountEmail(token);
            return isActive;
        }
    }
}
