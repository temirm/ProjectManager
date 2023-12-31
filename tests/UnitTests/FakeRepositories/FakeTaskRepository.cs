﻿using ProjectManager.Application.Interfaces;
using ProjectManager.Domain.Entities;

namespace UnitTests.FakeRepositories;

public class FakeTaskRepository : ITaskRepository
{
    readonly DataDictionary _data;

    public FakeTaskRepository(DataDictionary data)
    {
        _data = data;
    }

    public async Task DeleteAsync(ProjectTask task)
    {
        _data.Tasks.Remove(task);
    }

    public async Task<ProjectTask> SaveAsync(ProjectTask task)
    {
        var existingTask = _data.Tasks.FirstOrDefault(p => p.Id == task.Id);

        if (existingTask is not null)
        {
            _data.Tasks.Remove(existingTask);
        }

        _data.Tasks.Add(task);

        return task;
    }
}
