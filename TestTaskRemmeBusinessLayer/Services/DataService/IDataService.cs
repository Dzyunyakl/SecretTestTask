using System.Collections.Generic;
using TestTaskRemmeBusinessLayer.Extensions;
using TestTaskRemmeDataLayer.Models;
using TestTaskRemmeDataLayer.ViewModels;

namespace TestTaskRemmeBusinessLayer.Services.DataService
{
    public interface IDataService
    {
        OperationResult<IEnumerable<Task>> GetAll(int userId);
        OperationResult<IEnumerable<Task>> GetAllDone(int userId);
        OperationResult<Task> GetById(int userId, int taskId);
        OperationResult CreateTask(int userId, CreateTaskModel model);
        OperationResult UpdateTask(int userId, int taskId, UpdateTaskModel model);
        OperationResult RemoveTask(int userId, int taskId);
    }
}