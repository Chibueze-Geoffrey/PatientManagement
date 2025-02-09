using System.Collections.Generic;
using System.ComponentModel;

namespace PatientManagement.Common.Dtos.Response
{
   

    
        public class ExecutionResult
        {
            public ExecutionResult()
            {
                Response = ResponseCode.Ok;
            }

            public ResponseCode Response { get; set; }
            public string Message { get; set; }
            public string UserMessage { get; set; }
            public int RetryCount { get; set; }
            public List<string> ValidationMessages { get; set; }
        }

        public class ExecutionResult<T> : ExecutionResult
        {
            public T Result { get; set; }

            public static ExecutionResult<T> Success(T result, string message)
            {
                ExecutionResult<T> response = new ExecutionResult<T> { Response = ResponseCode.Ok, Result = result, Message = message, UserMessage = message };

                return response;
            }

            /// <summary>
            /// Creates a failed result. It takes no result object
            /// </summary>
            /// <param name="errorMessage">The error message returned with the response</param>
            /// <returns>The created response object</returns>
            public static ExecutionResult<T> Failed(string errorMessage, ResponseCode responseCode = ResponseCode.Failed)
            {
                ExecutionResult<T> response = new ExecutionResult<T> { Response = responseCode, Message = errorMessage };

                return response;
            }

            public static ExecutionResult<T> Failed(string errorMessage)
            {
                var response = new ExecutionResult<T> { Response = ResponseCode.ProcessingError, Message = errorMessage };

                return response;
            }
       
        }

        public enum ResponseCode
        {
            [Description("Request was successful")]
            Ok = 0,

            [Description("Invalid Data supplied")]
            ValidationError = 1,

            [Description("No record found")]
            NotFound = 2,

            [Description("Request failed. Please try again")]
            Failed = 3,

            [Description("Authentication failed. Please try again with the right credentials")]
            AuthorizationError = 4,
           
            [Description("Processing Error")]
            ProcessingError,

           
        }

        public class PagedList<T> where T : class
        {
            public T Data { get; set; }
            public int TotalCount { get; set; }

            public int PageSize { get; set; }
            public decimal TotalAmount { get; set; }
            public decimal TotalConvertedAmount { get; set; }
        }

        public class NewPagedList<T> where T : class
        {
            public NewPagedList()
            {
                Data = new List<T>();
            }

            public List<T> Data { get; }

            public int TotalCount { get; set; }

            public int PageSize { get; set; }
        }
    
}
