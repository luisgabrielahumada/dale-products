using Services.Models;
using System.Collections.Generic;

namespace Services.Interface.Auth
{
    public interface IProductRepository
    {
        Product Get(int id);
        List<Product> List(Pagination pag);
        int Save(Product request);
        int Delete(int eventId);
        int TotalRow(Pagination pag);
    }
}
