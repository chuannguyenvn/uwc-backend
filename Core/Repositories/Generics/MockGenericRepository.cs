using Commons.Extensions;
using Repositories.Managers;
using Commons.Models;

namespace Repositories.Generics;

public class MockGenericRepository<T> : IGenericRepository<T> where T : IndexedEntity
{
    private int _nextId = 1;
    protected readonly MockUwcDbContext Context;

    public MockGenericRepository(MockUwcDbContext mockUwcDbContext)
    {
        Context = mockUwcDbContext;
    }

    public virtual void Add(T entity)
    {
        entity.Id = _nextId++;
        Context.Set<T>().Add(entity);
    }

    public void AddRange(IEnumerable<T> entities)
    {
        Context.Set<T>().AddRange(entities);
    }

    public void Update(T entity)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<T> Find(Func<T, bool> condition)
    {
        return Context.Set<T>().Where(condition);
    }

    public T GetUnique(Func<T, bool> condition)
    {
        var possibleEntities = Context.Set<T>().Where(condition);
        if (!possibleEntities.Any()) return null!;
        if (possibleEntities.Count() > 1) return null!;

        return possibleEntities.First();
    }

    public IEnumerable<T> GetAll()
    {
        return Context.Set<T>().ToList();
    }

    public IEnumerable<T> GetRandom(int count = 1)
    {
        var entities = Context.Set<T>().ToList();
        if (count > entities.Count) return entities;
        return entities.GetRandom(count);
    }

    public IEnumerable<T> GetRandomWithCondition(Func<T, bool> condition, int count = 1)
    {
        var entities = Context.Set<T>().Where(condition).ToList();
        if (count > entities.Count) return entities;
        return entities.GetRandom(count);
    }

    public T GetById(int id)
    {
        return Context.Set<T>().Find(t => t.Id == id);
    }

    public void Remove(T entity)
    {
        Context.Set<T>().Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        Context.Set<T>(Context.Set<T>().Where(t => !entities.Contains(t)));
    }

    public bool DoesIdExist(int id)
    {
        return Context.Set<T>().Any(entity => entity.Id == id);
    }

    public bool HasAny()
    {
        return Context.Set<T>().Any();
    }

    public bool HasAny(Func<T, bool> predicate)
    {
        return Count(predicate) > 0;
    }

    public void RemoveById(int id)
    {
        Context.Set<T>().Remove(Context.Set<T>().Single(x => x.Id == id));
    }

    public int Count(Func<T, bool> predicate)
    {
        var count = 0;
        foreach (var item in Context.Set<T>())
            if (predicate(item))
                count++;
        return count;
    }
}