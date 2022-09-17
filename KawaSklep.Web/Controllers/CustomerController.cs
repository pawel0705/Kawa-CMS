using KawaSklep.Services.Customer;
using KawaSklep.Web.Serialization;
using KawaSklep.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace KawaSklep.Web.Controllers
{
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerService _customerService;

        public CustomerController(ILogger<CustomerController> logger,
            ICustomerService customerService)
        {
            _logger = logger;
            _customerService = customerService;
        }

        [HttpPost("/api/customer")]
        public ActionResult CreateCustomer([FromBody] CustomerModel customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation("CreateCustomer() called");
            customer.CreatedOn = DateTime.UtcNow;
            customer.CreatedOn = DateTime.UtcNow;
            var customerData = CustomerMapper.SerializeCustomer(customer);
            var newCustomer = _customerService.CreateCustomer(customerData);

            return Ok(newCustomer);
        }

        [HttpGet("/api/customer")]
        public ActionResult GetCustomers()
        {
            _logger.LogInformation("GetCustomers() called");
            var customers = _customerService.GetAllCustomers();

            var customerModels = customers
                .Select(customer => new CustomerModel
                {
                    Id = customer.Id,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    PrimaryAddress = CustomerMapper
                    .MapCustomerAddress(customer.PrimaryAddress),
                    CreatedOn = customer.CreatedOn,
                    UpdatedOn = customer.UpdatedOn,
                })
                .OrderByDescending(customer => customer.CreatedOn)
            .ToList();

            return Ok(customerModels);
        }

        [HttpDelete("/api/customer/{id}")]
        public ActionResult DeleteCustomer(int id)
        {
            _logger.LogInformation("Deleting a customer");
            var response = _customerService.DeleteCustomer(id);
            return Ok(response);
        }
    }
}
