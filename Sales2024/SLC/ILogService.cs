using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace SLC
{
    public interface ILogService
    {
        List<Logs> GetAllLogs();
    }
}
