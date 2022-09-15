using Services.Models;
using Shared.Response;

namespace Services.Interface.Auth
{
    public interface ISale
    {
        ServiceResponse<SaleModel> Get(int eventId);
        ServiceResponse<SalesModel> List(Pagination pag);
        ServiceResponse<int> Save(SaleModel request);
        ServiceResponse<int> Delete(int eventId);
    }
}
