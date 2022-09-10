using KawaSklep.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KawaSklep.Services.Customer
{
    public class CustomerService : ICustomerService
    {
        private readonly CaffeeDbContext _caffeeDbContext;

        public CustomerService(CaffeeDbContext caffeeDbContext)
        {
            _caffeeDbContext = caffeeDbContext;
        }

        /// <summary>
        /// Creates a new customer
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ServiceResponse<Data.Models.Customer> CreateCustomer(Data.Models.Customer customer)
        {
            try
            {
                _caffeeDbContext.Add(customer);
                _caffeeDbContext.SaveChanges();
                return new ServiceResponse<Data.Models.Customer>
                {
                    Data = customer,
                    Message = "Customer created",
                    Time = DateTime.UtcNow,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Data.Models.Customer>
                {
                    Data = customer,
                    Message = ex.StackTrace,
                    Time = DateTime.UtcNow,
                    IsSuccess = false
                };
            }
        }

        /// <summary>
        /// Deletes a customer record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ServiceResponse<bool> DeleteCustomer(int id)
        {
            var customer = _caffeeDbContext.Customers.Find(id);

            if (customer == null)
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    Message = "Customer not found",
                    Time = DateTime.UtcNow,
                    IsSuccess = false
                };
            }

            try
            {
                _caffeeDbContext.Customers.Remove(customer);
                _caffeeDbContext.SaveChanges();

                return new ServiceResponse<bool>
                {
                    Data = true,
                    Message = "Customer deleted",
                    Time = DateTime.UtcNow,
                    IsSuccess = true
                };
            }
            catch (Exception e)
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    Message = e.StackTrace,
                    Time = DateTime.UtcNow,
                    IsSuccess = false
                };
            }
        }

        /// <summary>
        /// Get all customers
        /// </summary>
        /// <returns>List of customers</returns>
        public List<Data.Models.Customer> GetAllCustomers()
        {
            return _caffeeDbContext.Customers
                 .Include(customer => customer.PrimaryAddress)
                 .OrderBy(customer => customer.LastName)
                 .ToList();
        }

        /// <summary>
        /// Get customer by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public Data.Models.Customer GetById(int id)
        {
            return _caffeeDbContext.Customers.Find(id);
        }
    }
}