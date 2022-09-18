using KawaSklep.Data;
using KawaSklep.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KawaSklep.Services.Inventory
{
    public class InventoryService : IInventoryService
    {
        private readonly CaffeeDbContext _caffeeDbContext;
        private readonly ILogger<InventoryService> _logger;

        public InventoryService(CaffeeDbContext caffeeDbContext, ILogger<InventoryService> logger)
        {
            _caffeeDbContext = caffeeDbContext;
            _logger = logger;
        }

        private void CreateSnapshot()
        {
            var now = DateTime.UtcNow;

            var inventories = _caffeeDbContext
                .ProductInventories.Include(inv => inv.Product)
                .ToList();

            foreach(var inventory in inventories)
            {
                var snapshot = new ProductInventorySnapshot
                {
                    SnapshotTime = now,
                    Product = inventory.Product,
                    QuantityOnHand = inventory.QuantityOnHand,
                };

                _caffeeDbContext.Add(snapshot);
            }           
        }

        public ProductInventory GetByProductId(int productId)
        {
            return _caffeeDbContext.ProductInventories
                .Include(pi => pi.Product)
                .FirstOrDefault(pi => pi.Product.Id == productId);
        }

        public List<ProductInventory> GetCurrentInventory()
        {
            return _caffeeDbContext.ProductInventories
                .Include(pi => pi.Product)
                .Where(pi => !pi.Product.IsArchived)
                .ToList();
        }

        public List<ProductInventorySnapshot> GetSnapshotHistory()
        {
            var earliest = DateTime.UtcNow - TimeSpan.FromHours(6);

            return _caffeeDbContext.ProductInventorySnapshots
                .Include(snap => snap.Product)
                .Where(snap => snap.SnapshotTime > earliest
                        && !snap.Product.IsArchived)
                .ToList();
        }

        public ServiceResponse<ProductInventory> UpdateunitsAvailable(int id, int adjustment)
        {
            try
            {
                var inventory = _caffeeDbContext.ProductInventories
                                .Include(inv => inv.Product)
                                .First(inv => inv.Product.Id == id);

                inventory.QuantityOnHand += adjustment;
                
                try
                {
                    CreateSnapshot();
                }
                catch (Exception e)
                {
                    _logger.LogError("Error creating inventory snapshot");
                    _logger.LogError(e.StackTrace);
                }

                _caffeeDbContext.SaveChanges();

                return new ServiceResponse<ProductInventory>
                {
                    IsSuccess = true,
                    Message = "Inventory adjusted",
                    Time = DateTime.UtcNow,
                    Data = inventory
                };
            }
            catch (Exception e)
            {
                return new ServiceResponse<ProductInventory>
                {
                    IsSuccess = false,
                    Message = e.StackTrace,
                    Time = DateTime.UtcNow,
                    Data = null
                };
            }

        }
    }
}
