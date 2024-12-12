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
using Security;

namespace BLL
{
    public class UserLogic
    {
        private const int MaxLoginAttempts = 3;
        private readonly string SecretKey = "DALJb15cjk_Wq2@sMOZ15"; // Reemplazar con una clave segura
        private LogLogic logLogic = new LogLogic();

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
                    user.status = 0;
                    res = r.Create(user);
                }
                else
                {
                    Console.WriteLine("Usuario ya existente (nombre de usuario o correo electrónico en uso)");
                }
            }

            if (res != null)
            {
                logLogic.Create("Public", "Se creo el nuevo usuario: " + res.username + ", con id: " + res.id);
                // Generar enlace de activación
                string confirmationLink = "http://localhost:5123/api/User/ActivateAccount?token=" + Auth.GenerateJwtToken(user);

                // Enviar correo de confirmación
                EmailLogic.SendConfirmationEmail(user.email, confirmationLink);
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
                // Obtener el usuario actual desde la base de datos
                Users currentUser = r.Retrieve<Users>(u => u.id == userToUpdate.id);

                if (currentUser == null)
                {
                    Console.WriteLine("Usuario no encontrado");
                    return res;
                }

                // Validar que el correo electrónico no esté en uso por otro usuario
                Users temp = r.Retrieve<Users>(u => u.id != userToUpdate.id && u.email == userToUpdate.email);

                if (temp == null)
                {
                    // Actualizar solo los campos no nulos o no vacíos
                    if (!string.IsNullOrEmpty(userToUpdate.username))
                    {
                        currentUser.username = userToUpdate.username;
                    }

                    if (!string.IsNullOrEmpty(userToUpdate.password))
                    {
                        currentUser.password = BCrypt.Net.BCrypt.HashPassword(userToUpdate.password); // Cifrar el password
                    }

                    if (!string.IsNullOrEmpty(userToUpdate.email))
                    {
                        currentUser.email = userToUpdate.email;
                    }

                    if (!string.IsNullOrEmpty(userToUpdate.rol))
                    {
                        currentUser.rol = userToUpdate.rol;
                    }

                    if (userToUpdate.status.HasValue)
                    {
                        currentUser.status = userToUpdate.status;
                    }

                    if (userToUpdate.loginAttempts.HasValue)
                    {
                        currentUser.loginAttempts = userToUpdate.loginAttempts;
                    }

                    // Guardar los cambios
                    res = r.Update(currentUser);
                }
                else
                {
                    Console.WriteLine("Correo electrónico ya está en uso por otro usuario");
                }
            }

            if (res)
            {
                logLogic.Create(SessionContext.Username, "Actualizo el usuario con id: " + userToUpdate.id);
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
                }
                else
                {
                    user.status = 1;
                    user.loginAttempts = 0;
                }
                using (var r = RepositoryFactory.CreateRepository())
                {
                    res = r.Update(user);
                }

                if (res)
                {
                    logLogic.Create(
                        SessionContext.Username,
                        (user.status == 1 ? "Activo" : "Bloqueo") + " el usuario con id: " + user.id
                        );
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
                    logLogic.Create("Public", "Inicio de Sesión fallido, usuario: " + user.username + ", con id: " + user.id + ", intento: " + user.loginAttempts);

                    if (user.loginAttempts >= MaxLoginAttempts)
                    {
                        user.status = 0; // Bloquear usuario
                        r.Update(user);
                        notifyUserBlocked(user);
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
                string token = Auth.GenerateJwtToken(user);
                logLogic.Create("Public", "Inicio de Sesión exitoso, usuario: " + user.username + ", con id: " + user.id);

                return token;
            }
        }

        private void notifyUserBlocked(Users user)
        {
            Console.WriteLine("Usuario bloqueado por demasiados intentos fallidos");
            logLogic.Create("Public", "Bloqueo por intentos fallidos, usuario: " + user.username + ", con id: " + user.id);

            // Generar enlace de activación
            string reactivationLink = "http://localhost:5123/api/User/ActivateAccount?token=" + Auth.GenerateJwtToken(user);

            // Enviar correo de confirmación
            EmailLogic.SendCriticalLogEmail(user.email, user.username, reactivationLink);
        }

        public bool Logout(string token)
        {
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

        public bool ActivateAccountEmail(string token)
        {
            int userId = Auth.GetUserIdFromToken(token);
            string username = Auth.GetUsernameFromToken(token);

            bool res = false;

            var user = RetrieveById(userId);

            if (user != null)
            {
                user.status = 1;
                user.loginAttempts = 0;
                
                using (var r = RepositoryFactory.CreateRepository())
                {
                    res = r.Update(user);
                }

                if (res)
                {
                    logLogic.Create(username, "Activo via email el usuario con id: " + user.id);
                }
            }
            else
            {
                Console.WriteLine("Usuario no encontrado");
            }

            return res;
        }
    }
}
