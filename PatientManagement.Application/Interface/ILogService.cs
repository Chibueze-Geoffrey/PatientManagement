using PatientManagement.Common.Dtos.Response;

namespace PatientManagement.Application.Interface
{
    public interface ILogService
    {
        double ReturnTimeSpent(DateTime StartTime);

        public void LogMethodCall(string MethodName, object request, object response, double TimeSpent = 0);

        void InsertEvent(LogModel model);

        void LogTypeResponse<T, U>(T req, U response, string action, double TimeSpent = 0);

        void LogTypeRequest<T>(T req, string action, double TimeSpent = 0);

        void LogTypeError<T>(T req, string action, double TimeSpent = 0);

        void LogTypeWarning<T>(T req, string action, double TimeSpent = 0);
    }

}
