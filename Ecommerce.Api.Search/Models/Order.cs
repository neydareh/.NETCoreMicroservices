﻿using System;
using System.Collections.Generic;

namespace Ecommerce.Api.Search.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Total { get; set; }
        public List<Models.OrderItem> Items { get; set; }
    }
}
