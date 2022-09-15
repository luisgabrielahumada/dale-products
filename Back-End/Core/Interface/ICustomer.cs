using Services.Models;
using Shared.Response;

namespace Services.Interface.Auth
{
    public interface ICustomer
    {
        ServiceResponse<CustomerModel> Get(int eventId);
        ServiceResponse<CustomersModel> List(Pagination pag);
        ServiceResponse<int> Save(CustomerModel request);
        ServiceResponse<int> Delete(int eventId);
    }
}
