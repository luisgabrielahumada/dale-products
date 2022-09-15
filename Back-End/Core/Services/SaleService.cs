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

                sr.Data = new SaleModel
                {
                    CustomerId = resp.CustomerId,
                    IsEnabled = resp.IsEnabled,
                    SaleId = resp.SaleId,
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
                        SaleId = r.SaleId
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
