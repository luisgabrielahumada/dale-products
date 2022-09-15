using Common.Infrastructure;
using Services.Interface.Auth;
using Services.Models;
using Shared.Response;
using System;
using System.Linq;

namespace Services.Auth
{
    public class SaleService : ISale
    {
        public ISaleRepository _db;
        public readonly Settings _settings;
        public SaleService(ISaleRepository db)
        {
            _db = db;
        }
        public ServiceResponse<SaleModel> Get(int id)
        {
            var sr = new ServiceResponse<SaleModel>();
            try
            {
                var resp = _db.Get(id);
                if (resp == null)
                    return sr;

                var sale = resp.Item1;
                sr.Data = new SaleModel
                {
                    CustomerId = sale.CustomerId,
                    IsEnabled = sale.IsEnabled,
                    SaleId = sale.SaleId,
                    Customer = new CustomerModel
                    {
                        CustomerId = sale.CustomerId,
                        Identification = sale.Identification,
                        FirstName = sale.FirstName,
                        LastName = sale.LastName,
                        Phone = sale.Phone,
                        Email = sale.Email
                    },
                    SaleProducts = resp.Item2.Select(m => new SaleProductsModel
                    {
                        ProductId = m.ProductId,
                        Quantity = m.Quantity,
                        SaleProductsId = m.SaleProductsId,
                        UnitPrice = m.UnitPrice,
                        IsEnabled = m.IsEnabled,
                        SaleId = m.SaleId,
                        Product = new ProductModel
                        {
                            Name = m.Name,
                            Inventory = m.Inventory,
                        }

                    }).ToList()
                };
            }
            catch (Exception e)
            {
                sr.AddError(e);
            }

            return sr;
        }
        public ServiceResponse<SalesModel> List(Pagination pag)
        {
            var sr = new ServiceResponse<SalesModel>();
            try
            {

                var resp = _db.List(pag);
                sr.Data = new SalesModel
                {
                    TotalRow = _db.TotalRow(pag),
                    Sales = resp.Select(r => new SaleModel
                    {
                        CustomerId = r.CustomerId,
                        IsEnabled = r.IsEnabled,
                        SaleId = r.SaleId,
                        Customer = new CustomerModel
                        {
                            CustomerId = r.CustomerId,
                            Identification = r.Identification,
                            FirstName = r.FirstName,
                            LastName = r.LastName,
                            Phone = r.Phone,
                            Email = r.Email
                        }
                    }).ToList()
                };
            }
            catch (Exception e)
            {
                sr.AddError(e);
            }

            return sr;
        }
        public ServiceResponse<int> Save(SaleModel request)
        {
            var sr = new ServiceResponse<int>();
            try
            {
                var model = new Sale
                {
                    CustomerId = request.CustomerId,
                    SaleId = request.SaleId,
                    IsEnabled = request.IsEnabled,
                    SaleProducts = request.SaleProducts.Select(m => new SaleProducts
                    {
                        Quantity = m.Quantity,
                        ProductId = m.ProductId,
                        SaleProductsId = m.SaleProductsId,
                        UnitPrice = m.UnitPrice,
                        IsEnabled = m.IsEnabled
                    }).ToList()

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
