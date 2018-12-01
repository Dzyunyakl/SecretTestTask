using System.Collections.Generic;
using System.Linq;
using System.Net;
using TestTaskRemmeBusinessLayer.Extensions;
using TestTaskRemmeBusinessLayer.Services.AuthService;
using TestTaskRemmeDataLayer.Database;
using TestTaskRemmeDataLayer.Models;
using TestTaskRemmeDataLayer.ViewModels;

namespace TestTaskRemmeBusinessLayer.Services.DataService
{
    public class DataService : IDataService
    {
        private readonly AppContext _db;
        private readonly IAuthService _authService;

        public DataService(AppContext db, IAuthService authService)
        {
            _db = db;
            _authService = authService;
        }

        public OperationResult<IEnumerable<Task>> GetAll(int userId)
        {
            var authResult = _authService.Authorize(userId);

            if (authResult.HttpStatusCode == HttpStatusCode.Unauthorized)
                return OperationResult<IEnumerable<Task>>.Unauthorized();

            var user = authResult.Entity;
            switch (user.Role)
            {
                case 0:
                    return GetAllForUser(userId);
                case 1:
                    return GetAllForAdmin();
            }

            return OperationResult<IEnumerable<Task>>.NotFound($"Role {user.Role} isn't provided.");
        }

        public OperationResult<IEnumerable<Task>> GetAllDone(int userId)
        {
            var authResult = _authService.Authorize(userId);

            if (authResult.HttpStatusCode == HttpStatusCode.Unauthorized)
                return OperationResult<IEnumerable<Task>>.Unauthorized();

            var user = authResult.Entity;
            switch (user.Role)
            {
                case 0:
                    return GetAllDoneForUser(userId);
                case 1:
                    return GetAllDoneForAdmin();
            }

            return OperationResult<IEnumerable<Task>>.NotFound($"Role {user.Role} isn't provided.");
        }

        public OperationResult<Task> GetById(int userId, int taskId)
        {
            var authResult = _authService.Authorize(userId);

            if (authResult.HttpStatusCode == HttpStatusCode.Unauthorized)
                return OperationResult<Task>.Unauthorized();

            var user = authResult.Entity;
            switch (user.Role)
            {
                case 0:
                    return GetByIdForUser(userId, taskId);
                case 1:
                    return GetByIdForAdmin(taskId);
            }

            return OperationResult<Task>.NotFound($"Role {user.Role} isn't provided.");
        }

        public OperationResult CreateTask(int userId, CreateTaskModel model)
        {
            var authResult = _authService.Authorize(userId);

            if (authResult.HttpStatusCode == HttpStatusCode.Unauthorized)
                return OperationResult.Unauthorized();

            var user = authResult.Entity;

            _db.Tasks.Add(new Task
            {
                Info = model.Info,
                IsDone = model.IsDone,
                User = user
            });
            _db.SaveChanges();
            return OperationResult.Ok("successfully added.");
        }

        public OperationResult UpdateTask(int userId, int taskId, UpdateTaskModel model)
        {
            var authResult = _authService.Authorize(userId);

            if (authResult.HttpStatusCode == HttpStatusCode.Unauthorized)
                return OperationResult.Unauthorized();

            var user = authResult.Entity;

            var task = _db.Tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                return OperationResult.NotFound("Any task with such an id has been found.");
            switch (user.Role)
            {
                case 0:
                    if (task.User == user)
                    {
                        if(task.IsDone != model.IsDone)
                            task.IsDone = model.IsDone;
                        if(task.Info != model.Info && model.Info != null)
                            task.Info = model.Info;
                        _db.Tasks.Update(task);
                        _db.SaveChanges();
                        return OperationResult.Ok("successfully updated.");
                    }
                    else
                        return OperationResult.BadRequest("Permission denied.");
                case 1:
                    if(task.IsDone != model.IsDone)
                        task.IsDone = model.IsDone;
                    if(task.Info != model.Info)
                        task.Info = model.Info;
                    _db.Tasks.Update(task);
                    _db.SaveChanges();
                    return OperationResult.Ok("successfully updated.");
            }

            return OperationResult.NotFound($"Role {user.Role} isn't provided.");
        }

        public OperationResult RemoveTask(int userId, int taskId)
        {
            var authResult = _authService.Authorize(userId);

            if (authResult.HttpStatusCode == HttpStatusCode.Unauthorized)
                return OperationResult.Unauthorized();

            var user = authResult.Entity;


            var task = _db.Tasks.FirstOrDefault(t => t.Id == taskId);


            if (task == null)
                return OperationResult.NotFound();

            switch (user.Role)
            {
                case 0:
                    if (task.User == user)
                    {
                        _db.Tasks.Remove(task);
                        _db.SaveChanges();
                        return OperationResult.Ok("successfully deleted");
                    }
                    else
                        return OperationResult.BadRequest("Permission denied.");
                case 1:
                    _db.Tasks.Remove(task);
                    _db.SaveChanges();
                    return OperationResult.Ok("successfully deleted");
            }

            return OperationResult.NotFound($"Role {user.Role} isn't provided.");
        }

        private OperationResult<IEnumerable<Task>> GetAllForAdmin() =>
            OperationResult<IEnumerable<Task>>.Ok(_db.Tasks.ToList());
        
        private OperationResult<IEnumerable<Task>> GetAllForUser(int userId) =>
            OperationResult<IEnumerable<Task>>.Ok(_db.Tasks.Where(t => t.User.Id == userId).ToList());

        private OperationResult<IEnumerable<Task>> GetAllDoneForAdmin() =>
            OperationResult<IEnumerable<Task>>.Ok(_db.Tasks.Where(t => t.IsDone).ToList());

        private OperationResult<Task> GetByIdForAdmin(int taskId)
        {
            var task = _db.Tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                return OperationResult<Task>.NotFound();
            return OperationResult<Task>.Ok(task);
        }

        

        private OperationResult<IEnumerable<Task>> GetAllDoneForUser(int userId) =>
            OperationResult<IEnumerable<Task>>.Ok(_db.Tasks.Where(t => t.User.Id == userId && t.IsDone).ToList());

        private OperationResult<Task> GetByIdForUser(int userId, int taskId)
        {
            var task = _db.Tasks.FirstOrDefault(t => t.Id == taskId && t.User.Id == userId);
            if (task == null)
                return OperationResult<Task>.NotFound();
            return OperationResult<Task>.Ok(task);
        }
    }
}