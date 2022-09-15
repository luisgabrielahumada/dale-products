using Common.Infrastructure;
using Core.Component.Library.SQL;
using Services.Database;
using Services.Interface.Auth;
using Services.Models;
using System.Collections.Generic;
using System.Linq;

namespace Services.Repository.Auth
{
    public class ProductRepository : IProductRepository
    {
        readonly AppDatabase db;
        Settings _settings;
        public ProductRepository(Settings settings)
        {
            _settings = settings;
            db = new AppDatabase(settings.ConnectionStrings.ConnectionString);
        }

        public int Delete(int id)
        {
            return new Execute(db, "DB_Product_Delete",
                              new
                              {
                                  productId = id,
                                  isActive=false,
                              }).Procedure<int>()
                                .FirstOrDefault();
        }

        public Product Get(int id)
        {
            return new Execute(db, "DB_Product_Get",
                              new
                              {
                                  productId = id,
                              }).Procedure<Product>()
                                .FirstOrDefault();
        }

        public List<Product> List(Pagination pag)
        {
            return new Execute(db, "DB_Product_List",
                             new
                             {
                                 pageIndex = pag.PageIndex,
                                 pageSize = pag.PageSize,
                             }).Procedure<Product>()
                               .ToList();
        }

        public int TotalRow(Pagination pag)
        {
            return new Execute(db, "DB_Product_List",
                             new
                             {
                                 pageIndex = pag.PageIndex,
                                 pageSize = pag.PageSize,
                                 totalRow = 1,
                             }).Procedure<int>()
                               .FirstOrDefault();
        }

        public int Save(Product request)
        {
            return new Execute(db, "DB_Product_Save",
                            new
                            {
                              name= request.Name,
                              unitPrice=request.UnitPrice,
                              inventory=request.Inventory,
                              isActive=request.IsActive
                            }).Procedure<int>()
                              .FirstOrDefault();
        }
    }
}