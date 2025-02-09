using PatientManagement.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PatientManagement.Infrastructure.Interface
{
    public interface IPatientRepository : IRepository<Patient>
    {
        Task<IEnumerable<Patient>> GetAllActivePatientsAsync();
        Task SoftDeleteAsync(int id);
        Task<Patient> GetPatientIncludingDeletedAsync(int id);
        Task RestorePatientAsync(int id);
    }
}
