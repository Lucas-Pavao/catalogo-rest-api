using System.Linq.Expressions;

namespace APICatalogo.Interfaces;

public interface IRepository<T>
{
    //cuidado para não violar o principio ISP
    IEnumerable<T> GetAll();
    T? Get(Expression<Func<T, bool>> predicate);
    T Create(T entity);
    T Update(T entity);
    T Delete(T entity);
}
