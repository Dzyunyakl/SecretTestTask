using System.Net;
namespace TestTaskRemmeBusinessLayer.Extensions
{
    public class OperationResult
    {
        public OperationResult()
        {
        }

        public OperationResult(HttpStatusCode httpStatusCode, string message)
        {
            this.HttpStatusCode = httpStatusCode;
            this.Message = message;
        }

        public bool BadRequestResult => this.HttpStatusCode == HttpStatusCode.BadRequest;

        public bool ForbiddenResult => this.HttpStatusCode == HttpStatusCode.Forbidden;

        public HttpStatusCode HttpStatusCode { get; set; }

        public string Message { get; set; }

        public bool Successful => this.HttpStatusCode == HttpStatusCode.OK;

        public static OperationResult BadRequest(string message = null)
        {
            return new OperationResult(HttpStatusCode.BadRequest, message);
        }

        public static OperationResult Conflict(string message = "Access denied!")
        {
            return new OperationResult(HttpStatusCode.Conflict, message);
        }

        public static OperationResult Forbidden(string message = "Access denied!")
        {
            return new OperationResult(HttpStatusCode.Forbidden, message);
        }

        public static OperationResult Ok(string message = null)
        {
            return new OperationResult(HttpStatusCode.OK, message);
        }

        public static OperationResult NotFound(string message = null)
        {
            return new OperationResult(HttpStatusCode.NotFound, message);
        }

        public static OperationResult Unauthorized(string message = null)
        {
            return new OperationResult(HttpStatusCode.Unauthorized, message);
        }

        public override string ToString() => this.HttpStatusCode.ToString();
    }

    public class OperationResult<TEntity> : OperationResult
    {
        public OperationResult()
        {
        }

        public OperationResult(HttpStatusCode httpStatusCode, TEntity entity, string message)
            : base(httpStatusCode, message)
        {
            this.Entity = entity;
        }

        public TEntity Entity { get; set; }

        public static OperationResult<TEntity> Conflict(string message = null, TEntity entity = default(TEntity))
        {
            return new OperationResult<TEntity>(HttpStatusCode.Conflict, entity, message);
        }

        public static OperationResult<TEntity> Error(TEntity entity = default(TEntity), string message = null)
        {
            return new OperationResult<TEntity>(HttpStatusCode.BadRequest, entity, message);
        }

        public static OperationResult<TEntity> Unauthorized(TEntity entity = default(TEntity), string message = null)
        {
            return new OperationResult<TEntity>(HttpStatusCode.Unauthorized, entity, message);
        }

        public static OperationResult<TEntity> FromStatus(
            HttpStatusCode statusCode,
            string message = null,
            TEntity entity = default(TEntity))
        {
            return new OperationResult<TEntity>(statusCode, entity, message);
        }

        public static OperationResult<TEntity> NotFound(string message = null, TEntity entity = default(TEntity))
        {
            return new OperationResult<TEntity>(HttpStatusCode.NotFound, entity, message);
        }

        public static OperationResult<TEntity> NoPremissions()
        {
            return FromStatus(HttpStatusCode.Forbidden);
        }

        public static OperationResult<TEntity> Ok(TEntity entity = default(TEntity))
        {
            return new OperationResult<TEntity>(HttpStatusCode.OK, entity, null);
        }
    }
}