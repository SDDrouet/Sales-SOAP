using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLC
{
    public interface IUserService
    {
        Users Create(Users user);
        Users RetrieveById(int id);
        bool Update(Users userToUpdate);
        bool ChangeStatus(int id);
        string Login(LoginRequest loginRequest);
        bool Logout(string token);
        List<Users> Filter(string filterUsername);
        bool ActivateAccount(string token);
    }


}
