﻿using Commons.Extensions;
using Repositories.Managers;
using Commons.Models;

namespace Repositories.Generics;

public abstract class GenericRepository<T> : IGenericRepository<T> where T : IndexedEntity
{
    protected readonly UwcDbContext Context;

    public GenericRepository(UwcDbContext context)
    {
        Context = context;
    }

    public void Add(T entity)
    {
        Context.Set<T>().Add(entity);
    }

    public void AddRange(IEnumerable<T> entities)
    {
        Context.Set<T>().AddRange(entities);
    }

    public void Update(T entity)
    {
        Context.Set<T>().Update(entity);
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
        return Context.Set<T>().Find(id);
    }

    public void Remove(T entity)
    {
        Context.Set<T>().Remove(entity);
    }

    public void RemoveAll()
    {
        Context.Set<T>().RemoveRange(Context.Set<T>());
    }

    public void RemoveWhere(Func<T, bool> predicate)
    {
        Context.Set<T>().RemoveRange(Context.Set<T>().Where(predicate));
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        Context.Set<T>().RemoveRange(entities);
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