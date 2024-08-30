using AutoLot.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace AutoLot.Mvc.TagHelpers.Car
{
    public class CustomerDetailsTagHelper : ItemDetailsTagHelper
    {
        public CustomerDetailsTagHelper(IActionContextAccessor contextAccessor, IUrlHelperFactory urlHelperFactory)
            : base(contextAccessor, urlHelperFactory)
        {
            ControllerName = nameof(CustomersController); // Set the controller name
        }
    }
}
