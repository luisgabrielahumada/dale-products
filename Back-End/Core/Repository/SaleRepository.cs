using Common.Infrastructure;
using Core.Component.Library.SQL;
using Services.Database;
using Services.Interface.Auth;
using Services.Models;
using System.Collections.Generic;
using System.Linq;

namespace Services.Repository.Auth
{
    public class SaleRepository : ISaleRepository
    {
        readonly AppDatabase db;
        Settings _settings;
        public SaleRepository(Settings settings)
        {
            _settings = settings;
            db = new AppDatabase(settings.ConnectionStrings.ConnectionString);
        }

        public int Delete(int id)
        {
            return new Execute(db, "DB_Sale_Delete",
                              new
                              {
                                  saleId = id,
                                  isEnalbed = false
                              }).Procedure<int>()
                                .FirstOrDefault();
        }

        public Sale Get(int id)
        {
            return new Execute(db, "DB_Sale_Get",
                              new
                              {
                                  saleId = id,
                              }).Procedure<Sale>()
                                .FirstOrDefault();
        }


        public List<Sale> List(Pagination pag)
        {
            return new Execute(db, "DB_Sale_List",
                             new
                             {
                                 pageIndex = pag.PageIndex,
                                 pageSize = pag.PageSize,
                             }).Procedure<Sale>()
                               .ToList();
        }

        public int TotalRow(Pagination pag)
        {
            return new Execute(db, "DB_Sale_List",
                             new
                             {
                                 pageIndex = pag.PageIndex,
                                 pageSize = pag.PageSize,
                                 totalRow = 1,
                             }).Procedure<int>()
                               .FirstOrDefault();
        }

        public int Save(Sale request)
        {
            return new Execute(db, "DB_Sale_Save",
                            new
                            {
                                saleId= request.SaleId,
                                customerId=request.CustomerId,
                                isEnabled=request.IsEnabled,
                            }).Procedure<int>()
                              .FirstOrDefault();
        }
    }
}