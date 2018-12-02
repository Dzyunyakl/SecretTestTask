using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public async Task<OperationResult<IEnumerable<Todo>>> GetAll(int userId)
        {
            var authResult = await _authService.Authorize(userId);

            if (authResult.HttpStatusCode == HttpStatusCode.Unauthorized)
                return OperationResult<IEnumerable<Todo>>.Unauthorized();

            var user = authResult.Entity;
            switch (user.Role)
            {
                case 0:
                    return await GetAllForUser(userId);
                case 1:
                    return await GetAllForAdmin();
            }

            return OperationResult<IEnumerable<Todo>>.NotFound($"Role {user.Role} isn't provided.");
        }

        public async Task<OperationResult<IEnumerable<Todo>>> GetAllDone(int userId)
        {
            var authResult = await _authService.Authorize(userId);

            if (authResult.HttpStatusCode == HttpStatusCode.Unauthorized)
                return OperationResult<IEnumerable<Todo>>.Unauthorized();

            var user = authResult.Entity;
            switch (user.Role)
            {
                case 0:
                    return await GetAllDoneForUser(userId);
                case 1:
                    return await GetAllDoneForAdmin();
            }

            return OperationResult<IEnumerable<Todo>>.NotFound($"Role {user.Role} isn't provided.");
        }

        public async Task<OperationResult<Todo>> GetById(int userId, int taskId)
        {
            var authResult = await _authService.Authorize(userId);

            if (authResult.HttpStatusCode == HttpStatusCode.Unauthorized)
                return OperationResult<Todo>.Unauthorized();

            var user = authResult.Entity;
            switch (user.Role)
            {
                case 0:
                    return await GetByIdForUser(userId, taskId);
                case 1:
                    return await GetByIdForAdmin(taskId);
            }

            return OperationResult<Todo>.NotFound($"Role {user.Role} isn't provided.");
        }

        public async Task<OperationResult> CreateTask(int userId, CreateTaskModel model)
        {
            var authResult = await _authService.Authorize(userId);

            if (authResult.HttpStatusCode == HttpStatusCode.Unauthorized)
                return OperationResult.Unauthorized();

            var user = authResult.Entity;

            _db.Todos.Add(new Todo
            {
                Info = model.Info,
                IsDone = model.IsDone,
                User = user
            });
            await _db.SaveChangesAsync();
            return OperationResult.Ok("successfully added.");
        }

        public async Task<OperationResult> UpdateTask(int userId, int taskId, UpdateTaskModel model)
        {
            var authResult = await _authService.Authorize(userId);

            if (authResult.HttpStatusCode == HttpStatusCode.Unauthorized)
                return OperationResult.Unauthorized();

            var user = authResult.Entity;

            var todo = await _db.Todos.FirstOrDefaultAsync(t => t.Id == taskId);
            if (todo == null)
                return OperationResult.NotFound("Any task with such an id has been found.");
            switch (user.Role)
            {
                case 0:
                    if (todo.User == user)
                    {
                        if(todo.IsDone != model.IsDone)
                            todo.IsDone = model.IsDone;
                        if(todo.Info != model.Info && model.Info != null)
                            todo.Info = model.Info;
                        _db.Todos.Update(todo);
                        await _db.SaveChangesAsync();
                        return OperationResult.Ok("successfully updated.");
                    }
                    else
                        return OperationResult.BadRequest("Permission denied.");
                case 1:
                    if(todo.IsDone != model.IsDone)
                        todo.IsDone = model.IsDone;
                    if(todo.Info != model.Info)
                        todo.Info = model.Info;
                    _db.Todos.Update(todo);
                    await _db.SaveChangesAsync();
                    return OperationResult.Ok("successfully updated.");
            }

            return OperationResult.NotFound($"Role {user.Role} isn't provided.");
        }

        public async Task<OperationResult> RemoveTask(int userId, int taskId)
        {
            var authResult = await _authService.Authorize(userId);

            if (authResult.HttpStatusCode == HttpStatusCode.Unauthorized)
                return OperationResult.Unauthorized();

            var user = authResult.Entity;


            var todo = await _db.Todos.FirstOrDefaultAsync(t => t.Id == taskId);


            if (todo == null)
                return OperationResult.NotFound();

            switch (user.Role)
            {
                case 0:
                    if (todo.User == user)
                    {
                        _db.Todos.Remove(todo);
                        await _db.SaveChangesAsync();
                        return OperationResult.Ok("successfully deleted");
                    }
                    else
                        return OperationResult.BadRequest("Permission denied.");
                case 1:
                    _db.Todos.Remove(todo);
                    await _db.SaveChangesAsync();
                    return OperationResult.Ok("successfully deleted");
            }

            return OperationResult.NotFound($"Role {user.Role} isn't provided.");
        }

        private async Task<OperationResult<IEnumerable<Todo>>> GetAllForAdmin() =>
            OperationResult<IEnumerable<Todo>>.Ok(await _db.Todos.ToListAsync());
        
        private async Task<OperationResult<IEnumerable<Todo>>> GetAllForUser(int userId) =>
            OperationResult<IEnumerable<Todo>>.Ok(await _db.Todos.Where(t => t.User.Id == userId).ToListAsync());

        private async Task<OperationResult<IEnumerable<Todo>>> GetAllDoneForAdmin() =>
            OperationResult<IEnumerable<Todo>>.Ok(await _db.Todos.Where(t => t.IsDone).ToListAsync());

        private async Task<OperationResult<Todo>> GetByIdForAdmin(int taskId)
        {
            var todo = await _db.Todos.FirstOrDefaultAsync(t => t.Id == taskId);
            if (todo == null)
                return OperationResult<Todo>.NotFound();
            return OperationResult<Todo>.Ok(todo);
        }

        

        private async Task<OperationResult<IEnumerable<Todo>>> GetAllDoneForUser(int userId) =>
            OperationResult<IEnumerable<Todo>>.Ok(await _db.Todos.Where(t => t.User.Id == userId && t.IsDone).ToListAsync());

        private async Task<OperationResult<Todo>> GetByIdForUser(int userId, int taskId)
        {
            var todo = await _db.Todos.FirstOrDefaultAsync(t => t.Id == taskId && t.User.Id == userId);
            if (todo == null)
                return OperationResult<Todo>.NotFound();
            return OperationResult<Todo>.Ok(todo);
        }
    }
}