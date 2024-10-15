using BoxnMove.Business.Services.Interface;
using BoxnMove.Database;
using BoxnMove.Database.DbModels;
using BoxnMove.Models.Models;
using BoxnMove.Models.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxnMove.Business.Services.Account
{
    public class ProductService : IProductService
    {
        private BoxnMoveDBContext _dbContext;
        private ILogger<ProductService> _logger;
        public ProductService(BoxnMoveDBContext dbContext, ILogger<ProductService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<ServiceResponse<ProductItemsModel>> GetProductItems()
        {
            try
            {
                _logger.LogDebug($"Begin: GetProductItems");
                var productItemsModel = new ProductItemsModel
                {
                    Categories = await _dbContext.Categories
    .AsNoTracking()
    .Select(c => new CategoryModel
    {
        CategoryId = c.CategoryId,
        CategoryName = c.Name,
        Products = c.Products.Select(p => new ProductModel
        {
            ProductId = p.ProductId,
            ProductName = p.Name,
            ProductTypes = p.ProductTypes.Select(pt => new ProductTypeModel
            {
                ProductTypeId = pt.ProductTypeId,
                ProductTypeName = pt.Name
            }).ToList()
        }).ToList()
    }).ToListAsync()
                };

                return new ServiceResponse<ProductItemsModel>
                {
                    Data = productItemsModel,
                    Status = 1
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while GetProductItems");
                return new ServiceResponse<ProductItemsModel>
                {
                    Status = 0,
                    Message = "An error occurred during GetProductItems."
                };
            }
        }

        public async Task<ServiceResponse<List<(int ProductTypeId, int CFTValue)>>> GetCFTValues(List<ProductTypeRequestModel> productTypes)
        {
            try
            {
                _logger.LogDebug($"Begin: GetCFTValues");

                var productTypeIds = productTypes?.Select(x => x.ProductTypeId)?.ToList();

                var cftValues = await _dbContext.ProductTypes
                    .Where(pt => productTypeIds.Contains(pt.ProductTypeId))
                    .Select(pt => new { pt.ProductTypeId, pt.CFT })
                    .ToListAsync();

                var result = cftValues.Select(x => (x.ProductTypeId, x.CFT))
                                      .ToList();

                return new ServiceResponse<List<(int ProductTypeId, int CFTValue)>>
                {
                    Data = result,
                    Status = 1
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while GetCFTValues");
                return new ServiceResponse<List<(int ProductTypeId, int CFTValue)>>
                {
                    Status = 0,
                    Message = "An error occurred during GetCFTValues."
                };
            }
        }
    }
}
