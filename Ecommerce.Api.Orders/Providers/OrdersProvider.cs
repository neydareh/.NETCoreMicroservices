﻿using AutoMapper;
using Ecommerce.Api.Orders.Db;
using Ecommerce.Api.Orders.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Api.Orders.Providers
{
    public class OrdersProvider : IOrdersProvider
    {
        private readonly OrdersDbContext dbContext;
        private readonly ILogger<OrdersProvider> logger;
        private readonly IMapper mapper;

        public OrdersProvider(OrdersDbContext dbContext, ILogger<OrdersProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }
        private void SeedData()
        {
            if (!dbContext.Orders.Any())
            {
                dbContext.Orders.Add(new Db.Order()
                {
                    Id = 1,
                    CustomerId = 1,
                    OrderDate = DateTime.Now,
                    Items = new List<Db.OrderItem>()
                    {
                        new Db.OrderItem() { OrderId = 1, ProductId = 1, Quantity = 10, UnitPrice = 10 },
                        new Db.OrderItem() { OrderId = 1, ProductId = 2, Quantity = 10, UnitPrice = 10 },
                        new Db.OrderItem() { OrderId = 1, ProductId = 3, Quantity = 10, UnitPrice = 10 },
                        new Db.OrderItem() { OrderId = 2, ProductId = 2, Quantity = 10, UnitPrice = 10 },
                        new Db.OrderItem() { OrderId = 3, ProductId = 3, Quantity = 1, UnitPrice = 100 }
                    },
                    Total = 100
                });
                dbContext.Orders.Add(new Db.Order()
                {
                    Id = 2,
                    CustomerId = 1,
                    OrderDate = DateTime.Now.AddDays(-1),
                    Items = new List<Db.OrderItem>()
                    {
                        new Db.OrderItem() { OrderId = 1, ProductId = 1, Quantity = 10, UnitPrice = 10 },
                        new Db.OrderItem() { OrderId = 1, ProductId = 2, Quantity = 10, UnitPrice = 10 },
                        new Db.OrderItem() { OrderId = 1, ProductId = 3, Quantity = 10, UnitPrice = 10 },
                        new Db.OrderItem() { OrderId = 2, ProductId = 2, Quantity = 10, UnitPrice = 10 },
                        new Db.OrderItem() { OrderId = 3, ProductId = 3, Quantity = 1, UnitPrice = 100 }
                    },
                    Total = 100
                });
                dbContext.Orders.Add(new Db.Order()
                {
                    Id = 3,
                    CustomerId = 2,
                    OrderDate = DateTime.Now,
                    Items = new List<Db.OrderItem>()
                    {
                        new Db.OrderItem() { OrderId = 1, ProductId = 1, Quantity = 10, UnitPrice = 10 },
                        new Db.OrderItem() { OrderId = 2, ProductId = 2, Quantity = 10, UnitPrice = 10 },
                        new Db.OrderItem() { OrderId = 3, ProductId = 3, Quantity = 1, UnitPrice = 100 }
                    },
                    Total = 100
                });
                dbContext.SaveChanges();
            }
        }

        public async Task<(bool isSuccess, IEnumerable<Models.Order> Orders, string errorMessage)> GetOrdersAsync(int customerId)
        {
            try
            {
                logger?.LogInformation($"Querying orders table");
                var orders = await dbContext.Orders
                    .Where(o => o.CustomerId == customerId)
                    .Include(oi => oi.Items)
                    .ToListAsync();

                if (orders != null && orders.Any())
                {
                    logger?.LogInformation($"{orders.Count} order(s) found");
                    var result = mapper.Map<IEnumerable<Db.Order>, IEnumerable<Models.Order>>(orders);
                    return (true, result, null);
                }
                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }


    }
}
