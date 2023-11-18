﻿using Commons.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Generics;
using Repositories.Managers;

namespace Repositories.Implementations.Tasks;

public class TaskRepository : GenericRepository<TaskData>, ITaskRepository
{
    public TaskRepository(UwcDbContext context) : base(context)
    {
    }

    public List<TaskData> GetTasksByDate(DateTime date)
    {
        return Context.TaskDataTable.Where(task => task.CompleteByTimestamp == date.Date).Include(task => task.McpData).ToList();
    }

    public List<TaskData> GetTasksByWorkerId(int workerId)
    {
        return Context.TaskDataTable.Where(task => task.AssigneeId == workerId)
            .Include(task => task.McpData)
            .Include(task => task.AssigneeProfile)
            .Include(task => task.AssignerProfile).ToList();
    }

    public List<TaskData> GetTasksFromTodayOrFuture()
    {
        return Context.TaskDataTable.Where(task => task.CompleteByTimestamp >= DateTime.Now.Date)
            .Include(task => task.McpData)
            .Include(task => task.AssigneeProfile)
            .Include(task => task.AssignerProfile).ToList();
    }
}