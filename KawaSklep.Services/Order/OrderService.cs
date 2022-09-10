using KawaSklep.Data;
using KawaSklep.Data.Models;
using KawaSklep.Services.Inventory;
using KawaSklep.Services.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KawaSklep.Services.Order
{
    public class OrderService : IOrderService
    {
        private readonly CaffeeDbContext _caffeeDbContext;
        private readonly ILogger<OrderService> _logger;
        private readonly IInventoryService _inventoryService;
        private readonly IProductService _productService;

        public OrderService(CaffeeDbContext caffeeDbContext, ILogger<OrderService> logger, IInventoryService inventoryService, IProductService productService)
        {
            _caffeeDbContext = caffeeDbContext;
            _logger = logger;
            _inventoryService = inventoryService;
            _productService = productService;
        }

        /// <summary>
        /// Creates a new order and saves it to the database
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public ServiceResponse<bool> GenerateOpenOrder(SalesOrder order)
        {
            _logger.LogInformation("Generating new order");

            foreach (var item in order.SalesOrderItems)
            {
                item.Product = _productService.GetProductByID(item.Product.Id);

                var inventoryId = _inventoryService.GetByProductId(item.Product.Id).Id;

                _inventoryService.UpdateunitsAvailable(inventoryId, -item.Quantity);
            }

            try
            {
                _caffeeDbContext.SalesOrders.Add(order);
                _caffeeDbContext.SaveChanges();

                return new ServiceResponse<bool>
                {
                    IsSuccess = true,
                    Message = "Order created.",
                    Time = DateTime.UtcNow,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool>
                {
                    IsSuccess = false,
                    Message = ex.StackTrace,
                    Time = DateTime.UtcNow,
                    Data = false
                };
            }
        }

        /// <summary>
        /// Get orders list
        /// </summary>
        /// <returns>List of orders</returns>
        public List<SalesOrder> GetOrders()
        {
            return _caffeeDbContext.SalesOrders
                .Include(so => so.Customer)
                    .ThenInclude(customer => customer.PrimaryAddress)
                .Include(so => so.SalesOrderItems)
                    .ThenInclude(item => item.Product)
                .ToList();
        }

        public ServiceResponse<bool> MarkFulfilled(int id)
        {
            var now = DateTime.UtcNow;
            var order = _caffeeDbContext.SalesOrders.Find(id);
            order.UpdatedOn = now;
            order.IsPaid = true;
            try
            {
                _caffeeDbContext.SalesOrders.Update(order);
                _caffeeDbContext.SaveChanges();

                return new ServiceResponse<bool>
                {
                    IsSuccess = true,
                    Data = true,
                    Message = $"Order {order.Id} closed: Invoice paid in full.",
                    Time = now,
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool>
                {
                    IsSuccess = false,
                    Data = false,
                    Message = ex.StackTrace,
                    Time = now,
                };
            }
        }
    }
}
