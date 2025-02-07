using PatientManagement.Application.Dtos.PatientDtos;
using PatientManagement.Application.Dtos.PatientDtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientManagement.Application.Interface
{
    public interface IPatientService
    {

        Task<PatientResponse> CreatePatientAsync(PatientDto patientDto);
        Task<PatientResponse> GetPatientByIdAsync(int id);
        Task<IEnumerable<PatientResponse>> GetAllPatientsAsync();
        Task<PatientResponse> UpdatePatientAsync(int id, PatientDto patientDto);
        Task<bool> DeletePatientAsync(int id);
    }
}
