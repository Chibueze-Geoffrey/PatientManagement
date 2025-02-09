using PatientManagement.Infrastructure.Data;
using PatientManagement.Infrastructure.Interface;
using System;
using System.Threading.Tasks;

namespace PatientManagement.Infrastructure.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IPatientRepository PatientRepository { get; }

        public UnitOfWork(ApplicationDbContext context, IPatientRepository patientRepository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            PatientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
        }

        public async Task CommitAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }
       
    }
}


