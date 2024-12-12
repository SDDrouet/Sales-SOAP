using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BLL;
using Entities;
using Security;
using SLC;

namespace Service.Controllers
{
    public class LogController : ApiController, ILogService
    {
        [AuthorizeRoles("ADMIN")]
        public List<Logs> GetAllLogs()
        {
            var logLogic = new LogLogic();
            var logs = logLogic.GetAllLogs();
            return logs;
        }
    }
}
