using Services.Models;
using Shared.Response;

namespace Services.Interface.Auth
{
    public interface IProduct
    {
        ServiceResponse<ProductModel> Get(int eventId);
        ServiceResponse<ProductsModel> List(Pagination pag);
        ServiceResponse<int> Save(ProductModel request);
        ServiceResponse<int> Delete(int eventId);
    }
}
