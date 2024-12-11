using DAL;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using JWT;
using JWT.Algorithms;
using JWT.Builder;

namespace BLL
{
    public class UserLogic
    {
        private const int MaxLoginAttempts = 3;
        private readonly string SecretKey = "DALJb15cjk_Wq2@sMOZ15"; // Reemplazar con una clave segura

        public Users Create(Users user)
        {
            Users res = null;

            using (var r = RepositoryFactory.CreateRepository())
            {
                Users existingUser = r.Retrieve<Users>(u => u.username == user.username || u.email == user.email);

                if (existingUser == null)
                {
                    user.password = BCrypt.Net.BCrypt.HashPassword(user.password); // Cifrar el password
                    user.loginAttempts = 0;
                    user.status = 1;
                    res = r.Create(user);
                }
                else
                {
                    Console.WriteLine("Usuario ya existente (nombre de usuario o correo electrónico en uso)");
                }
            }

            return res;
        }

        public Users RetrieveById(int id)
        {
            Users res = null;

            using (var r = RepositoryFactory.CreateRepository())
            {
                res = r.Retrieve<Users>(u => u.id == id);
            }

            return res;
        }

        public bool Update(Users userToUpdate)
        {
            bool res = false;

            using (var r = RepositoryFactory.CreateRepository())
            {
                // Validar que el correo electrónico no esté en uso por otro usuario
                Users temp = r.Retrieve<Users>(u => u.id != userToUpdate.id && u.email == userToUpdate.email);

                if (temp == null)
                {
                    if (!string.IsNullOrEmpty(userToUpdate.password))
                    {
                        userToUpdate.password = BCrypt.Net.BCrypt.HashPassword(userToUpdate.password); // Cifrar el password si es nuevo
                    }
                    res = r.Update(userToUpdate);
                }
                else
                {
                    Console.WriteLine("Correo electrónico ya está en uso por otro usuario");
                }
            }

            return res;
        }

        public bool ChangeStatus(int id)
        {
            bool res = false;

            var user = RetrieveById(id);

            if (user != null)
            {
                if (user.status == 1) // 1 usuario activo
                {
                    user.status = 0;
                } else
                {
                    user.status = 1;
                    user.loginAttempts = 0;
                }
                using (var r = RepositoryFactory.CreateRepository())
                {
                    res = r.Update(user);
                }
            }
            else
            {
                Console.WriteLine("Usuario no encontrado");
            }

            return res;
        }

        public string Login(string username, string password)
        {
            using (var r = RepositoryFactory.CreateRepository())
            {
                var user = r.Retrieve<Users>(u => u.username == username);

                if (user == null || user.status == 0)
                {
                    Console.WriteLine("Usuario no encontrado o inactivo");
                    return null;
                }

                if (!BCrypt.Net.BCrypt.Verify(password, user.password))
                {
                    user.loginAttempts = (user.loginAttempts ?? 0) + 1;
                    r.Update(user);

                    if (user.loginAttempts >= MaxLoginAttempts)
                    {
                        user.status = 0; // Bloquear usuario
                        r.Update(user);
                        Console.WriteLine("Usuario bloqueado por demasiados intentos fallidos");
                    }
                    else
                    {
                        Console.WriteLine("Contraseña incorrecta");
                    }

                    return null;
                }

                // Reiniciar intentos fallidos y generar token
                user.loginAttempts = 0;
                r.Update(user);

                var token = JwtBuilder.Create()
                    .WithAlgorithm(new HMACSHA256Algorithm())
                    .WithSecret(SecretKey)
                    .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds())
                    .AddClaim("username", user.username)
                    .AddClaim("userId", user.id)
                    .Encode();

                return token;
            }
        }

        public bool Logout(string token)
        {
            // En aplicaciones más complejas, los tokens se manejan en una lista negra o sistema de revocación.
            // Aquí, asumimos que el cliente simplemente dejará de usar el token proporcionado.
            Console.WriteLine("Sesión cerrada e invalidada");
            return true;
        }

        public List<Users> Filter(string filterUsername)
        {
            List<Users> res = null;

            using (var repository = RepositoryFactory.CreateRepository())
            {
                if (string.IsNullOrEmpty(filterUsername))
                {
                    res = repository.Filter<Users>(u => true);
                }
                else
                {
                    res = repository.Filter<Users>(u => u.username.Contains(filterUsername));
                }
            }

            return res;
        }
    }
}
