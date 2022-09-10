using KawaSklep.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KawaSklep.Data
{
    public class CaffeeDbContext : IdentityDbContext
    {
        public CaffeeDbContext()
        {
            
        }

        public CaffeeDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public virtual DbSet<Customer> Customers { get; set; }

        public virtual DbSet<CustomerAddress> CustomerAddresses { get; set; }
    }
}
