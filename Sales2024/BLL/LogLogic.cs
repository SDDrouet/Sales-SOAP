using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Entities;

namespace BLL
{
    public class LogLogic
    {
        public Logs Create(string userModification, string description)
        {
            Logs result = null;
            Logs log = new Logs
            {
                userModification = userModification,
                description = description,
                dateLog = DateTime.Now
            };

            using (var repository = RepositoryFactory.CreateRepository())
            {
                try
                {
                    // Crea el log en la base de datos
                    result = repository.Create(log);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al crear el log: {ex.Message}");
                }
            }

            return result;
        }

        public List<Logs> GetAllLogs()
        {
            List<Logs> logs = new List<Logs>();

            using (var repository = RepositoryFactory.CreateRepository())
            {
                try
                {
                    // Recupera todos los logs sin filtros
                    logs = repository.Filter<Logs>(l => true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al obtener los logs: {ex.Message}");
                }
            }

            return logs;
        }
    }
}
