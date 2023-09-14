using System.Linq.Expressions;
using Commons.Models;

namespace Repositories;

public interface IGenericRepository<T> where T : IndexedEntity
{
    void Add(T entity);
    void AddRange(IEnumerable<T> entities);
    IEnumerable<T> Find(Func<T, bool> condition);
    T GetUnique(Func<T, bool> condition);
    IEnumerable<T> GetAll();
    T GetById(int id);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
    bool DoesIdExist(int id);
    bool HasAny();
    bool HasAny(Func<T, bool> predicate);
    void RemoveById(int id);
    int Count(Func<T, bool> predicate);
}