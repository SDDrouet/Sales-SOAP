using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using JWT.Algorithms;
using JWT.Builder;
using System.Net.Http;

namespace Security
{
    public static class Auth
    {
        private static readonly string SecretKey = "DALJb15cjk_Wq2@sMOZ15"; // Reemplazar con una clave segura

        public static string GenerateJwtToken(Users user)
        {
            return JwtBuilder.Create()
                                .WithAlgorithm(new HMACSHA256Algorithm())
                                .WithSecret(SecretKey)
                                .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds())
                                .AddClaim("username", user.username)
                                .AddClaim("userId", user.id)
                                .AddClaim("rol", user.rol)
                                .Encode();
        }

        public static string GetTokenFromRequest(HttpRequestMessage request)
        {
            if (request.Headers.Authorization != null && request.Headers.Authorization.Scheme == "Bearer")
            {
                return request.Headers.Authorization.Parameter;
            }
            return null;
        }

        private static IDictionary<string, object> DecodeToken(string token)
        {
            try
            {
                return JwtBuilder.Create()
                                .WithAlgorithm(new HMACSHA256Algorithm())
                                .WithSecret(SecretKey)
                                .MustVerifySignature()
                                .Decode<IDictionary<string, object>>(token);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al decodificar el token: {ex.Message}");
                return null;
            }
        }

        public static string GetUsernameFromToken(string token)
        {
            try
            {
                // Decodificar el token usando la clave secreta
                IDictionary<string, object> payload = DecodeToken(token);

                // Verificar que el payload contiene el nombre de usuario
                if (payload.ContainsKey("username"))
                {
                    return payload["username"].ToString();
                }
                else
                {
                    Console.WriteLine("El token no contiene el nombre de usuario.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al decodificar el token: {ex.Message}");
                return null;
            }
        }

        public static string GetRolFromToken(string token)
        {
            // Decodificar el token usando la clave secreta
            IDictionary<string, object> payload = DecodeToken(token);

            if (payload != null)
            {
                // Verificar que el payload contiene el nombre de usuario
                if (payload.ContainsKey("rol"))
                {
                    return payload["rol"].ToString();
                }
                else
                {
                    Console.WriteLine("El token no contiene rol de usuario.");
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public static int GetUserIdFromToken(string token)
        {
            // Decodificar el token usando la clave secreta
            IDictionary<string, object> payload = DecodeToken(token);

            if (payload != null)
            {
                // Verificar que el payload contiene el nombre de usuario
                if (payload.ContainsKey("userId"))
                {
                    return int.Parse(payload["userId"].ToString());
                }
                else
                {
                    Console.WriteLine("El token no contiene UserId de usuario.");
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        }

        public static bool IsTokenValid(string token)
        {
            IDictionary<string, object> payload = DecodeToken(token);
            return payload != null;
        }
    }
}
