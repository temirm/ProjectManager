﻿using ProjectManager.Application.Interfaces;
using ProjectManager.Domain.Entities;
using UnitTests.Extensions;

namespace UnitTests.FakeRepositories;

public class FakeProjectRepository : IProjectRepository
{
    readonly DataDictionary _data;

    public FakeProjectRepository(DataDictionary data)
    {
        _data = data;
    }

    public async Task DeleteAsync(Project project)
    {       
        _data.Projects.Remove(project);
    }

    public async Task<Project?> GetByIdAsync(Guid projectId)
    {
        return _data.Projects.FirstOrDefault(p => p.Id == projectId);        
    }

    public async Task<Project?> GetByIdWithTasksAndCommentsAsync(Guid projectId)
    {
        var project = _data.Projects.FirstOrDefault(p => p.Id == projectId);

        if (project is null)
        {
            return project;
        }

        var tasks = _data.Tasks.Where(t => t.ProjectId == projectId).ToList();

        for (int i = 0; i < tasks.Count; i++)
        {
            var comments = _data.TaskComments.Where(c => c.TaskId == tasks[i].Id).ToList();
            tasks[i] = tasks[i].IncludeComments(comments);
        }

        return project.IncludeTasks(tasks);
    }

    public async Task<IEnumerable<Project>> ListForUserAsync(Guid userId)
    {
        return _data.Projects.Where(p => p.IsPublic == true || p.OwnerId == userId || p.Collaborators.Any(c => c.Id == userId));
    }

    public async Task<IEnumerable<Project>> ListPublicAsync()
    {
        return _data.Projects.Where(p => p.IsPublic);
    }

    public async Task<Project> SaveAsync(Project project)
    {
        var existingProject = _data.Projects.FirstOrDefault(p => p.Id == project.Id);

        if (existingProject is not null)
        {
            _data.Projects.Remove(existingProject);
        }

        _data.Projects.Add(project.WithId(Guid.NewGuid()));
        
        return project;
    }

    public async Task<Project?> GetByIdWithTasksAsync(Guid projectId)
    {
        var project = _data.Projects.FirstOrDefault(p => p.Id == projectId);

        if (project is null)
        {
            return project;
        }

        return project.IncludeTasks(_data.Tasks.Where(t => t.ProjectId == projectId).ToList());
    }
}
