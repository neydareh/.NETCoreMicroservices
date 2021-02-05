﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecommerce.Api.Orders.Interfaces
{
    public interface IOrdersProvider
    {
        Task<(bool isSuccess, IEnumerable<Models.Order> Orders, string errorMessage)> GetOrdersAsync(int customerId);
        
    }
}
