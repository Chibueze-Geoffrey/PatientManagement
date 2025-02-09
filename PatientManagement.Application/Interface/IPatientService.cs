using PatientManagement.Common.Dtos.PatientDtos;
using PatientManagement.Common.Dtos.PatientDtos.Request;
using PatientManagement.Common.Dtos.PatientDtos.Response;
using PatientManagement.Common.Dtos.Response;

namespace PatientManagement.Application.Interface
{
    public interface IPatientService
    {

        Task<ExecutionResult<PatientResponse>> CreatePatientAsync(PatientDto patientDto);
        Task<ExecutionResult<PatientResponse>> GetPatientByIdAsync(int id);
        Task<ExecutionResult<IEnumerable<PatientResponse>>> GetAllPatientsAsync();
        Task <ExecutionResult<PatientUpdateResponseDto>> UpdatePatientAsync(int id, PatientUpdateDto patientDto);
        Task<ExecutionResult<bool>> DeletePatientAsync(int id);
    }
}
