using System.Collections.Generic;
using System.Threading.Tasks;
using TestTaskRemmeBusinessLayer.Extensions;
using TestTaskRemmeDataLayer.Models;
using TestTaskRemmeDataLayer.ViewModels;

namespace TestTaskRemmeBusinessLayer.Services.DataService
{
    public interface IDataService
    {
        Task<OperationResult<IEnumerable<Todo>>> GetAll(int userId);
        Task<OperationResult<IEnumerable<Todo>>> GetAllDone(int userId);
        Task<OperationResult<Todo>> GetById(int userId, int taskId);
        Task<OperationResult> CreateTask(int userId, CreateTaskModel model);
        Task<OperationResult> UpdateTask(int userId, int taskId, UpdateTaskModel model);
        Task<OperationResult> RemoveTask(int userId, int taskId);
    }
}