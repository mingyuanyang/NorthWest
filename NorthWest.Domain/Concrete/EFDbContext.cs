using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NorthWest.Domain.Entities;
using System.Data.Entity;

namespace NorthWest.Domain.Concrete
{
    public class EFDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
    }
}
