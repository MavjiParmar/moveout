using BoxnMove.Business.Services.Account;
using BoxnMove.Database;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxnMove.Business.Services.Order
{
    public class OrderService: IOrderService
    {
        private BoxnMoveDBContext _dbContext;
        private ILogger<OrderService> _logger;
        public OrderService(BoxnMoveDBContext dbContext, ILogger<OrderService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }


    }
}
