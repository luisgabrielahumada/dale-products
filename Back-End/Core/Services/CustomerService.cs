using Common.Infrastructure;
using Services.Interface.Auth;
using Services.Models;
using Shared;
using Shared.Extensions;
using Shared.Response;
using System;
using System.Linq;

namespace Services.Auth
{
    public class CustomerService : ICustomer
    {
        public ICustomerRepository _db;
        public readonly Settings _settings;
        public CustomerService(ICustomerRepository db)
        {
            _db = db;
        }
        public ServiceResponse<CustomerModel> Get(int id)
        {
            var sr = new ServiceResponse<CustomerModel>();
            try
            {
                var resp = _db.Get(id);
                if (resp == null)
                    return sr;

                sr.Data = new CustomerModel
                {
                    FirstName = resp.FirstName,
                    LastName = resp.LastName,
                    Email = resp.Email,
                    CustomerId = resp.CustomerId,
                    IsActive = resp.IsActive,
                    Phone = resp.Phone,
                    Identification = resp.Identification,
                };
            }
            catch (Exception e)
            {
                sr.AddError(e);
            }

            return sr;
        }
        public ServiceResponse<CustomersModel> List(Pagination pag)
        {
            var sr = new ServiceResponse<CustomersModel>();
            try
            {

                var resp = _db.List(pag);
                sr.Data = new CustomersModel
                {
                    TotalRow = _db.TotalRow(pag),
                    Customers = resp.Select(r => new CustomerModel
                    {
                        FirstName = r.FirstName,
                        LastName = r.LastName,
                        Email = r.Email,
                        CustomerId = r.CustomerId,
                        IsActive = r.IsActive,
                        Phone = r.Phone,
                        Identification = r.Identification,
                    }).ToList()
                };
            }
            catch (Exception e)
            {
                sr.AddError(e);
            }

            return sr;
        }
        public ServiceResponse<int> Save(CustomerModel request)
        {
            var sr = new ServiceResponse<int>();
            try
            {
                var model = new Customer
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    CustomerId = request.CustomerId,
                    IsActive = request.IsActive,
                    Phone = request.Phone,
                    Identification = request.Identification,
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
