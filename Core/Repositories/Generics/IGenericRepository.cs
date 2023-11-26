using Commons.Models;

namespace Repositories.Generics;

public interface IGenericRepository<T> where T : IndexedEntity
{
    void Add(T entity);
    void AddRange(IEnumerable<T> entities);
    void Update(T entity);
    IEnumerable<T> Find(Func<T, bool> condition);
    T GetUnique(Func<T, bool> condition);
    IEnumerable<T> GetAll();
    IEnumerable<T> GetRandom(int count = 1);
    IEnumerable<T> GetRandomWithCondition(Func<T, bool> condition, int count = 1);
    T GetById(int id);
    void Remove(T entity);
    void RemoveAll();
    void RemoveRange(IEnumerable<T> entities);
    bool DoesIdExist(int id);
    bool HasAny();
    bool HasAny(Func<T, bool> predicate);
    void RemoveById(int id);
    int Count(Func<T, bool> predicate);
}