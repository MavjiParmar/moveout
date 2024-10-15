using BoxnMove.Business.Services.Account;
using BoxnMove.Business.Services.Order;
using BoxnMove.Business.Services.Shared;
using BoxnMove.Models.Models;
using BoxnMove.Shared.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoxnMoveAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : BaseController
    {
        private IOrderService _orderService;
        private SMSService _sMSService;
        private ISharedService _sharedService;
        private IConfiguration _configuration;
        private ILogger<ProductController> _logger;
        public OrderController(IOrderService orderService, IConfiguration configuration, ILogger<ProductController> logger, ISharedService sharedService, SMSService sMSService)
        {
            this._orderService = orderService;
            this._configuration = configuration;
            this._logger = logger;
            this._sharedService = sharedService;
            this._sMSService = sMSService;
        }

        [HttpPost("CreateOrder")]
        public IActionResult CreateOrder([FromBody] OrderModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                //var response = _orderService.CreateOrder(model);
                //calculate price
                return Success(30658);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during CreateOrder.");
                return Error("Internal Server Error!.", 500);
            }
        }
    }
}
