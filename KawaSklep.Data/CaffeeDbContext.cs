﻿using KawaSklep.Data.Models;
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
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductInventory> ProductInventories { get; set; }
        public virtual DbSet<ProductInventorySnapshot> ProductInventorySnapshots { get; set; }
        public virtual DbSet<SalesOrder> SalesOrders { get; set; }
        public virtual DbSet<SalesOrderItem> SalesOrderItems { get; set; }
        
    }
}
