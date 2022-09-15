using Services.Models;
using System.Collections.Generic;

namespace Services.Interface.Auth
{
    public interface ISaleRepository
    {
        Sale Get(int id);
        List<Sale> List(Pagination pag);
        int Save(Sale request);
        int Delete(int eventId);
        int TotalRow(Pagination pag);
    }
}
