using Microsoft.EntityFrameworkCore;
using PatientManagement.Common.Enums;
using PatientManagement.Domain.Models;
using PatientManagement.Infrastructure.Data;
using PatientManagement.Infrastructure.Interface;

namespace PatientManagement.Infrastructure.Implementations
{
    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
        private readonly ApplicationDbContext _context;

        public PatientRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Patient>> GetAllActivePatientsAsync()
        {
            return await _context.Patients
                .Where(p => p.Status != PatientStatus.Deleted)
                .ToListAsync();
        }

        public async Task<Patient> GetByIdAsync(int id)
        {
            return await _context.Patients
                .Where(p => p.Id == id && p.Status != PatientStatus.Deleted)
                .FirstOrDefaultAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient != null)
            {
                patient.Status = PatientStatus.Deleted;
                _context.Patients.Update(patient);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<Patient> GetPatientIncludingDeletedAsync(int id)
        {
            return await _context.Patients.FindAsync(id);
        }

        public async Task RestorePatientAsync(int id)
        {
            var patient = await GetPatientIncludingDeletedAsync(id);
            if (patient != null && patient.Status == PatientStatus.Deleted)
            {
                patient.Status = PatientStatus.Active;
                _context.Patients.Update(patient);
                await _context.SaveChangesAsync();
            }
        }

    }
}
