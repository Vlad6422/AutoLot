﻿using AutoLot.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AutoLot.Mvc.TagHelpers.Car
{
    public class CarDetailsTagHelper : ItemDetailsTagHelper
    {
        public CarDetailsTagHelper(IActionContextAccessor contextAccessor, IUrlHelperFactory urlHelperFactory)
            : base(contextAccessor, urlHelperFactory)
        {
            ControllerName = nameof(CarsController); // Set the controller name
        }
    }
}
