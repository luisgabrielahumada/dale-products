using Common.Infrastructure;
using Core.Component.Library.SQL;
using Services.Database;
using Services.Interface.Auth;
using Services.Models;
using System.Collections.Generic;
using System.Linq;

namespace Services.Repository.Auth
{
    public class CustomerRepository : ICustomerRepository
    {
        readonly AppDatabase db;
        Settings _settings;
        public CustomerRepository(Settings settings)
        {
            _settings = settings;
            db = new AppDatabase(settings.ConnectionStrings.ConnectionString);
        }

        public int Delete(int id)
        {
            return new Execute(db, "DB_Customer_Delete",
                              new
                              {
                                  customerId = id,
                                  isActive = false
                              }).Procedure<int>()
                                .FirstOrDefault();
        }

        public Customer Get(int id)
        {
            return new Execute(db, "DB_Customer_Get",
                              new
                              {
                                  customerId = id,
                              }).Procedure<Customer>()
                                .FirstOrDefault();
        }

        public List<Customer> List(Pagination pag)
        {
            return new Execute(db, "DB_Customer_List",
                             new
                             {
                                 pageIndex = pag.PageIndex,
                                 pageSize = pag.PageSize,
                             }).Procedure<Customer>()
                               .ToList();
        }

        public int TotalRow(Pagination pag)
        {
            return new Execute(db, "DB_Customer_List",
                             new
                             {
                                 pageIndex = pag.PageIndex,
                                 pageSize = pag.PageSize,
                                 totalRow = 1,
                             }).Procedure<int>()
                               .FirstOrDefault();
        }

        public int Save(Customer request)
        {
            return new Execute(db, "DB_Customer_Save",
                            new
                            {
                                 customerId=request.CustomerId,
                                  isActive=request.IsActive,
                                  firstName=request.FirstName,
                                  lastName=request.LastName,
                                  email=request.Email,
                                  phone=request.Phone,  
                            }).Procedure<int>()
                              .FirstOrDefault();
        }
    }
}