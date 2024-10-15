using BoxnMove.Business.Services.Account;
using BoxnMove.Business.Services.Interface;
using BoxnMove.Business.Services.Shared;
using BoxnMove.Models.Models;
using BoxnMove.Shared.Utilities;
using BoxnMoveAPI.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoxnMoveAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseController
    {
        private IProductService _productService;
        private SMSService _sMSService;
        private ISharedService _sharedService;
        private IConfiguration _configuration;
        private ILogger<ProductController> _logger;
        public ProductController(IProductService productService, IConfiguration configuration, ILogger<ProductController> logger, ISharedService sharedService, SMSService sMSService)
        {
            this._productService = productService;
            this._configuration = configuration;
            this._logger = logger;
            this._sharedService = sharedService;
            this._sMSService = sMSService;
        }


        [HttpGet("ProductItems")]
        public async Task<IActionResult> ProductItems()
        {
            try
            {
                return Success(await _productService.GetProductItems());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ProductItems.");
                return Error("Internal Server Error in ProductItems!.", 500);
            }
        }


        [HttpPost("OrderSummary")]
        public IActionResult OrderSummary([FromBody] ProductItemsRequestModel productNodel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = _productService.GetCFTValues(productNodel.ProductTypes);
                //calculate price
                return Success(30658);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during PricingCalculation.");
                return Error("Internal Server Error!.", 500);
            }
        }

        [HttpGet("ValidateCoupon")]
        public async Task<IActionResult> ValidateCoupon(string coupon)
        {
            try
            {
                if (string.IsNullOrEmpty(coupon))
                {
                    return BadRequest();
                }

                var response = await _sharedService.ValidateCoupon(coupon);
                return Success(response.Data, response.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during PricingCalculation.");
                return Error("Internal Server Error!.", 500);
            }
        }
    }
}
