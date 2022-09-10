using KawaSklep.Data;
using KawaSklep.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KawaSklep.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly CaffeeDbContext _caffeeDbContext;

        public ProductService(CaffeeDbContext caffeeDbContext)
        {
            _caffeeDbContext = caffeeDbContext;
        }

        /// <summary>
        /// Archives a Product by setting boolean IsArchived to true
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ServiceResponse<Data.Models.Product> ArchiveProduct(int id)
        {
            try
            {
                var product = _caffeeDbContext.Products.Find(id);
                product.IsArchived = true;
                _caffeeDbContext.SaveChanges();

                return new ServiceResponse<Data.Models.Product>
                {
                    Data = product,
                    Message = "Product archived",
                    Time = DateTime.UtcNow,
                    IsSuccess = true
                };
            }
            catch (Exception e)
            {
                return new ServiceResponse<Data.Models.Product>
                {
                    Data = null,
                    Message = e.StackTrace,
                    Time = DateTime.UtcNow,
                    IsSuccess = false
                };
            }
        }

        /// <summary>
        /// Adds new product to database
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public ServiceResponse<Data.Models.Product> CreateProduct(Data.Models.Product product)
        {
            try
            {
                _caffeeDbContext.Products.Add(product);

                var newInventory = new ProductInventory
                {
                    Product = product,
                    QuantityOnHand = 0,
                    IdealQuantity = 10,
                };

                _caffeeDbContext.ProductInventories.Add(newInventory);

                _caffeeDbContext.SaveChanges();

                return new ServiceResponse<Data.Models.Product>
                {
                    Data = product,
                    IsSuccess = true,
                    Message = "Product created",
                    Time = DateTime.UtcNow
                };
            }
            catch (Exception e)
            {
                return new ServiceResponse<Data.Models.Product>
                {
                    IsSuccess = false,
                    Message = e.StackTrace,
                    Time = DateTime.UtcNow,
                    Data = product,
                };
            }
        }

        /// <summary>
        /// Gets all products from databse
        /// </summary>
        /// <returns></returns>
        public List<Data.Models.Product> GetAllProducts()
        {
            return _caffeeDbContext.Products.ToList();
        }

        /// <summary>
        /// Retrieves a Product by primary key
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Data.Models.Product GetProductByID(int id)
        {
            return _caffeeDbContext.Products.Find(id);
        }
    }
}
