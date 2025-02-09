using Newtonsoft.Json;
using PatientManagement.Application.Interface;
using PatientManagement.Common.Dtos.Response;
using PatientManagement.Common.Enums;

namespace PatientManagement.Application.Services
{
    public class LogService : ILogService
    {
        public double ReturnTimeSpent(DateTime StartTime)
        {
            var timespent = (DateTime.Now - StartTime).TotalMilliseconds;
            return timespent;
        }

        public void LogMethodCall(string MethodName, object request, object response, double TimeSpent = 0)
        {
            try
            {
                Serilog.Log.Information("Request from {} Method {Request:} with {Response: token}. {TimeSpent:}", MethodName, JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(response), TimeSpent);
            }
            catch (Exception ex)
            {
                Serilog.Log.Information($"Exception" + "{@Log}", ex.Message);
            }
        }

        public void InsertEvent(LogModel model)
        {
            try
            {
                LogModel entity = new LogModel
                {
                    Action = model.Action,
                    CreatedOn = DateTime.Now,
                    Message = model.Message,
                    Request = model.Request,
                    RequestTime = model.RequestTime,
                    Response = model.Response,
                    ResponseTime = model.ResponseTime,
                    LogMode = model.LogMode,
                    TimeSpent = model.TimeSpent
                };
                LogEnumMode CurrentLogEnum = (LogEnumMode)Enum.Parse(typeof(LogEnumMode), model.LogMode);
                switch (CurrentLogEnum)
                {
                    case LogEnumMode.Error:
                        Serilog.Log.Error($"{model.Action}" + "{@Log}", entity);
                        break;

                    case LogEnumMode.Warning:
                        Serilog.Log.Warning($"{model.Action}" + "{@Log}", entity);
                        break;

                    default:
                        Serilog.Log.Information($"{model.Action}" + "{@Log}", entity);
                        break;
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Information($"Exception" + "{@Log}", ex.Message);
            }
        }

        public void LogTypeResponse<T, U>(T req, U response, string action, double TimeSpent = 0)
        {
            try
            {
                LogModel logresp = new LogModel
                {
                    CreatedOn = DateTime.Now,
                    Action = $"{action} - ",
                    Request = JsonConvert.SerializeObject(req),
                    Response = response != null ? JsonConvert.SerializeObject(response) : "",
                    ResponseTime = DateTime.Now,
                    LogMode = "Response",
                    TimeSpent = TimeSpent
                };
                InsertEvent(logresp);
            }
            catch (Exception ex)
            {
                Serilog.Log.Information($"Exception" + "{@Log}", ex.Message);
            }
        }

        public void LogTypeRequest<T>(T req, string action, double TimeSpent = 0)
        {
            try
            {
                LogModel logreq = new LogModel
                {
                    CreatedOn = DateTime.Now,
                    Action = $"{action} - ",
                    Request = JsonConvert.SerializeObject(req),
                    Response = "",
                    RequestTime = DateTime.Now,
                    LogMode = "Request",
                    TimeSpent = TimeSpent
                };
                InsertEvent(logreq);
            }
            catch (Exception ex)
            {
                Serilog.Log.Information($"Exception" + "{@Log}", ex.Message);
            }
        }

        public void LogTypeWarning<T>(T req, string action, double TimeSpent = 0)
        {
            try
            {
                LogModel logreq = new LogModel
                {
                    CreatedOn = DateTime.Now,
                    Action = $"{action} - ",
                    Request = JsonConvert.SerializeObject(req),
                    Response = "",
                    RequestTime = DateTime.Now,
                    LogMode = "Warning",
                    TimeSpent = TimeSpent
                };
                InsertEvent(logreq);
            }
            catch (Exception ex)
            {
                Serilog.Log.Information($"Exception" + "{@Log}", ex.Message);
            }
        }

        public void LogTypeError<T>(T req, string action, double TimeSpent = 0)
        {
            try
            {
                LogModel logreq = new LogModel
                {
                    CreatedOn = DateTime.Now,
                    Action = $"{action} - ",
                    Request = JsonConvert.SerializeObject(req),
                    Response = "",
                    RequestTime = DateTime.Now,
                    LogMode = "Error",
                    TimeSpent = TimeSpent
                };
                InsertEvent(logreq);
            }
            catch (Exception ex)
            {
                Serilog.Log.Information($"Exception" + "{@Log}", ex.Message);
            }
        }
    }

}
