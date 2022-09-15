using Services.Models;
using System.Collections.Generic;

namespace Services.Interface.Auth
{
    public interface ICustomerRepository
    {
        Customer Get(int id);
        List<Customer> List(Pagination pag);
        int Save(Customer request);
        int Delete(int eventId);
        int TotalRow(Pagination pag);
    }
}
