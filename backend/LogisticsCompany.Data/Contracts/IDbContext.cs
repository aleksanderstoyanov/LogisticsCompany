using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsCompany.Data.Contracts
{
    public interface IDbContext
    {
        public string GetConnectionString();
    }
}
