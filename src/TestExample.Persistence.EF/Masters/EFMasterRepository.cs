using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestExample.Entities.Masters;
using TestExample.Services.Masters.Contracts;

namespace TestExample.Persistence.EF.Masters
{
    public class EFMasterRepository : IMasterRepository
    {
        private readonly DbSet<Master> _masters;

        public EFMasterRepository(EFDataContext context)
        {
            _masters = context.Set<Master>();
        }

        public void Add(Master master)
        {
            _masters.Add(master);
        }
    }
}