using OrderManagementSystem.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Data.Repository;

public interface IGenericRepository<TEntity> where TEntity : BaseModel
{
    TEntity GetById(int id, params string[] includes);
    List<TEntity> GetAll(params string[] includes);
    void Delete(int id);
    void Delete(TEntity entity);
    void Remove(int id);
    void Remove(TEntity entity);
    void Update(TEntity entity);
    void Insert(TEntity entity);
    void InsertRange(List<TEntity> entities);
    IQueryable<TEntity> GetAsQueryable(params string[] includes);
    IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> expression, params string[] includes);
}
