using System.Linq.Expressions;
using Commons.Models;

namespace Repositories;

public class GenericRepository<T> where T : IndexedEntity
{
    protected readonly UwcDbContext _context;

    public GenericRepository(UwcDbContext context)
    {
        _context = context;
    }
    
    public void Add(T entity)
    {
        _context.Set<T>().Add(entity);
    }
    
    public void AddRange(IEnumerable<T> entities)
    {
        _context.Set<T>().AddRange(entities);
    }

    public IEnumerable<T> Find(Expression<Func<T, bool>> condition)
    {
        return _context.Set<T>().Where(condition);
    }
    
    public T GetUnique(Expression<Func<T, bool>> condition)
    {
        var possibleEntities = _context.Set<T>().Where(condition);
        if (!possibleEntities.Any()) return null!;
        if (possibleEntities.Count() > 1) return null!;

        return possibleEntities.First();
    }

    public IEnumerable<T> GetAll()
    {
        return _context.Set<T>().ToList();
    }
    
    public T GetById(int id)
    {
        return _context.Set<T>().Find(id);
    }
    
    public void Remove(T entity)
    {
        _context.Set<T>().Remove(entity);
    }
    
    public void RemoveRange(IEnumerable<T> entities)
    {
        _context.Set<T>().RemoveRange(entities);
    }

    public bool DoesIdExist(int id)
    {
        return _context.Set<T>().Any(entity => entity.Id == id);
    }

    public bool HasAny()
    {
        return _context.Set<T>().Any();
    }

    public bool HasAny(Func<T, bool> predicate)
    {
        return Count(predicate) > 0;
    }

    public void RemoveById(int id)
    {
        _context.Set<T>().Remove(_context.Set<T>().Single(x => x.Id == id));
    }

    public int Count(Func<T, bool> predicate)
    {
        var count = 0;
        foreach (var item in _context.Set<T>())
            if (predicate(item))
                count++;
        return count;
    }
}