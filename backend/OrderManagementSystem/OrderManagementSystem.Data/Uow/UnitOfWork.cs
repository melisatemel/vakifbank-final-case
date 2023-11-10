using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Data.Context;
using OrderManagementSystem.Data.Domain;
using OrderManagementSystem.Data.Repository;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Data.Uow;

public class UnitOfWork : IUnitOfWork
{
    private readonly OmsDbContext dbContext;

    public UnitOfWork(OmsDbContext dbContext)
    {
        this.dbContext = dbContext;

        UserRepository = new GenericRepository<User>(dbContext);
        ProductRepository = new GenericRepository<Product>(dbContext);
        ShoppingCartRepository = new GenericRepository<ShoppingCart>(dbContext);
    }

    public void Complete()
    {
        dbContext.SaveChanges();
    }

    public void CompleteTransaction()
    {
        using (var transaction = dbContext.Database.BeginTransaction())
        {
            try
            {
                dbContext.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Log.Error("CompleteTransactionError", ex);
            }
        }
    }

    public IGenericRepository<User> UserRepository { get; private set; }
    public IGenericRepository<Product> ProductRepository { get; private set; }
    public IGenericRepository<ShoppingCart> ShoppingCartRepository { get; private set; }
}
