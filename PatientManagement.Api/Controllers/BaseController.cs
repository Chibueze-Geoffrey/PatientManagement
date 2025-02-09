using Microsoft.AspNetCore.Mvc;
using PatientManagement.Common.Dtos.Response;

namespace PatientManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
     
        public readonly string FailureMessage = "Request failed, please try again";

        protected IActionResult CustomResponse<T>(ExecutionResult<T> result)
        {
            try
            {
                if (result == null)
                {
                    result = new ExecutionResult<T>();
                    result.Message = string.IsNullOrEmpty(result.Message) ? FailureMessage : result.Message;
                    result.UserMessage = string.IsNullOrEmpty(result.UserMessage) ? FailureMessage : result.UserMessage;
                    return BadRequest(result);
                }
                switch (result.Response)
                {
                        
                    case ResponseCode.ValidationError:
                        {
                            result.Message = GenerateMessage(ResponseCode.ValidationError, result.Message);
                            result.UserMessage = GenerateMessage(ResponseCode.ValidationError, result.UserMessage);
                            return BadRequest(result);
                        }
          
                    case ResponseCode.AuthorizationError:
                        {
                            result.Message = GenerateMessage(ResponseCode.AuthorizationError, result.Message);
                            return Unauthorized(result);
                        }

                    case ResponseCode.NotFound:
                        {
                            result.Message = GenerateMessage(ResponseCode.NotFound, result.Message);
                            return BadRequest(result);
                        }
                    case ResponseCode.Ok:
                        {
                            result.Message = GenerateMessage(ResponseCode.Ok, result.Message);
                            result.UserMessage = GenerateMessage(ResponseCode.Ok, result.UserMessage);
                            return Ok(result);
                        }
            case ResponseCode.Failed:
                    default:
                        {
                            result.UserMessage = GenerateMessage(result.UserMessage, FailureMessage);
                            result.Message = GenerateMessage(result.Message, FailureMessage);
                            return BadRequest(result);
                        }
                }
            }
            catch (Exception)
            {
                result.UserMessage = GenerateMessage(result.UserMessage, FailureMessage);
                result.Message = GenerateMessage(result.Message, FailureMessage);
                return BadRequest(result);
            }
        }

      

        private string GenerateMessage(ResponseCode Response, string FieldMessage)
        {
            return string.IsNullOrEmpty(FieldMessage) ? Response.ToString() : FieldMessage;
        }



        private string GenerateMessage(string FieldMessage, string Response)
        {
            return string.IsNullOrEmpty(FieldMessage) ? Response : FieldMessage;
        }
    }
}
