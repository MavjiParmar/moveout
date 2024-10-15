using BoxnMove.Models.Models;
using BoxnMove.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxnMove.Business.Services.Account
{
    public interface IProductService
    {
        Task<ServiceResponse<ProductItemsModel>> GetProductItems();
        Task<ServiceResponse<List<(int ProductTypeId, int CFTValue)>>> GetCFTValues(List<ProductTypeRequestModel> productTypes);
    }
}
