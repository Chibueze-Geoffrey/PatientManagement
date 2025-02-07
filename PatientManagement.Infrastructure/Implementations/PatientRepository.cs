using Microsoft.EntityFrameworkCore;
using PatientManagement.Domain.Enums;
using PatientManagement.Domain.Models;
using PatientManagement.Infrastructure.Data;
using PatientManagement.Infrastructure.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientManagement.Infrastructure.Implementations
{
    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
        private readonly ApplicationDbContext _context;

        public PatientRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        // Get all active patients (excluding "Deleted" ones)
        public async Task<IEnumerable<Patient>> GetAllActivePatientsAsync()
        {
            return await _context.Patients
                .Where(p => p.Status != PatientStatus.Deleted)
                .ToListAsync();
        }

        // Get a patient by ID (only if not deleted)
        public async Task<Patient> GetByIdAsync(int id)
        {
            return await _context.Patients
                .Where(p => p.Id == id && p.Status != PatientStatus.Deleted)
                .FirstOrDefaultAsync();
        }

        // Soft delete a patient (by changing status)
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

        // Restore a soft-deleted patient (if needed)
        public async Task RestorePatientAsync(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient != null && patient.Status == PatientStatus.Deleted)
            {
                patient.Status = PatientStatus.Active;
                _context.Patients.Update(patient);
                await _context.SaveChangesAsync();
            }
        }
    }
}
