using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TestTaskRemmeBusinessLayer.Services.DataService;
using TestTaskRemmeDataLayer.ViewModels;

namespace TestTaskRemme.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly IDataService _dataService;
        public TasksController(IDataService dataService)
        {
            _dataService = dataService;
        }
        // GET api/tasks/all
        [HttpGet("all")]
        public ActionResult<IEnumerable<GetAllModel>> GetAll()
        {
            var userId = Request.Headers["id"];
            if (userId.ToString() == "")
                return Unauthorized();
            var result = _dataService.GetAll(int.Parse(userId));

            if (result.HttpStatusCode == HttpStatusCode.Unauthorized)
                return Unauthorized();

            if (result.Successful)
                
                return Ok(result.Entity.Select(i => 
                    new GetAllModel
                    {
                        Id = i.Id,
                        Info = i.Info,
                        IsDone = i.IsDone,
                        UserId = i.User.Id, 
                        UserName = i.User.Name
                    }));
            return BadRequest();
        }
        
        // GET api/tasks/done
        [HttpGet("done")]
        public ActionResult<IEnumerable<GetAllModel>> GetAllDone()
        {
            var userId = Request.Headers["id"];
            if (userId.ToString() == "")
                return Unauthorized();
            var result = _dataService.GetAllDone(int.Parse(userId));

            if (result.HttpStatusCode == HttpStatusCode.Unauthorized)
                return Unauthorized();

            if (result.Successful)
                
                return Ok(result.Entity.Select(i => 
                    new GetAllModel
                    {
                        Id = i.Id,
                        Info = i.Info,
                        IsDone = i.IsDone,
                        UserId = i.User.Id, 
                        UserName = i.User.Name
                    }));
            return BadRequest();
        }

        // GET api/tasks/get/5
        [HttpGet("get/{taskId}")]
        public ActionResult<Task> GetById(int taskId)
        {
            var userId = Request.Headers["id"];
            if (userId.ToString() == "")
                return Unauthorized();
            var result = _dataService.GetById(int.Parse(userId), taskId);
            
            if (result.HttpStatusCode == HttpStatusCode.Unauthorized)
                return Unauthorized();
            
            if (result.Successful)
                return Ok(new
                {
                    result.Entity.Id,
                    result.Entity.Info,
                    result.Entity.IsDone,
                    UserId = result.Entity.User.Id,
                    UserName = result.Entity.User.Name
                });
            return BadRequest();
        }

        // PUT api/tasks
        [HttpPut]
        public ActionResult<string> CreateTask([FromBody]CreateTaskModel model)
        {
            var userId = Request.Headers["id"];
            if (userId.ToString() == "")
                return Unauthorized();
            var result = _dataService.CreateTask(int.Parse(userId), model);
            
            if (result.HttpStatusCode == HttpStatusCode.Unauthorized)
                return Unauthorized();
            
            if (result.Successful)
                return Ok(result.Message);
            return BadRequest();
        }

        // POST api/tasks/5
        [HttpPost("{taskId}")]
        public ActionResult<string> Update(int taskId, [FromBody]UpdateTaskModel model)
        {
            var userId = Request.Headers["id"];
            if (userId.ToString() == "")
                return Unauthorized();
            
            var result = _dataService.UpdateTask(int.Parse(userId), taskId, model);
            
            if (result.HttpStatusCode == HttpStatusCode.Unauthorized)
                return Unauthorized();
            
            if (result.Successful)
                return Ok(result.Message);
            return BadRequest();
        }

        // DELETE api/tasks/5
        [HttpDelete("{taskId}")]
        public ActionResult<string> Delete(int taskId)
        {
            var userId = Request.Headers["id"];
            if (userId.ToString() == "")
                return Unauthorized();
            var result = _dataService.RemoveTask(int.Parse(userId), taskId);
            
            if (result.HttpStatusCode == HttpStatusCode.Unauthorized)
                return Unauthorized();
            
            if (result.Successful)
                return Ok(result.Message);
            return BadRequest();
        }
    }
}