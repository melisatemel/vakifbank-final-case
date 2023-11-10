using OrderManagementSystem.Data.Domain;
using OrderManagementSystem.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Data.Uow;

public interface IUnitOfWork
{
    void Complete();
    void CompleteTransaction();

    IGenericRepository<User> UserRepository { get; }
    IGenericRepository<Product> ProductRepository { get; }
    IGenericRepository<ShoppingCart> ShoppingCartRepository { get; }
}
