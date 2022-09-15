using Services.Models;
using System;
using System.Collections.Generic;

namespace Services.Interface.Auth
{
    public interface ISaleRepository
    {
        Tuple<Sale, List<SaleProducts>> Get(int id);
        List<Sale> List(Pagination pag);
        int Save(Sale request);
        int Delete(int eventId);
        int TotalRow(Pagination pag);
    }
}
