using Common.Infrastructure;
using Services.Interface.Auth;
using Services.Models;
using Shared.Response;
using System;
using System.Linq;

namespace Services.Auth
{
    public class ProductService : IProduct
    {
        public IProductRepository _db;
        public readonly Settings _settings;
        public ProductService(IProductRepository db)
        {
            _db = db;
        }
        public ServiceResponse<ProductModel> Get(int id)
        {
            var sr = new ServiceResponse<ProductModel>();
            try
            {
                var resp = _db.Get(id);
                if (resp == null)
                    return sr;

                sr.Data = new ProductModel
                {
                    ProductId = resp.ProductId,
                    Inventory = resp.Inventory,
                    IsActive = resp.IsActive,
                    Name = resp.Name,
                    UnitPrice = resp.UnitPrice,
                };
            }
            catch (Exception e)
            {
                sr.AddError(e);
            }

            return sr;
        }
        public ServiceResponse<ProductsModel> List(Pagination pag)
        {
            var sr = new ServiceResponse<ProductsModel>();
            try
            {

                var resp = _db.List(pag);
                sr.Data = new ProductsModel
                {
                    TotalRow = _db.TotalRow(pag),
                    Products = resp.Select(r => new ProductModel
                    {
                        ProductId = r.ProductId,
                        Inventory = r.Inventory,
                        IsActive = r.IsActive,
                        Name = r.Name,
                        UnitPrice = r.UnitPrice,
                    }).ToList()
                };
            }
            catch (Exception e)
            {
                sr.AddError(e);
            }

            return sr;
        }
        public ServiceResponse<int> Save(ProductModel request)
        {
            var sr = new ServiceResponse<int>();
            try
            {
                var model = new Product
                {
                    ProductId = request.ProductId,
                    Inventory = request.Inventory,
                    IsActive = request.IsActive,
                    Name = request.Name,
                    UnitPrice = request.UnitPrice,
                };
                sr.Data = _db.Save(model);
            }
            catch (Exception e)
            {
                sr.AddError(e);
            }

            return sr;
        }
        public ServiceResponse<int> Delete(int id)
        {
            var sr = new ServiceResponse<int>();
            try
            {
                sr.Data = _db.Delete(id);
            }
            catch (Exception e)
            {
                sr.AddError(e);
            }

            return sr;
        }
    }
}
