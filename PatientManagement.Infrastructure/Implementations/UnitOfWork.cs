using PatientManagement.Infrastructure.Data;
using PatientManagement.Infrastructure.Interface;
using System.Threading.Tasks;

namespace PatientManagement.Infrastructure.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IPatientRepository PatientRepository { get; }

        public UnitOfWork(ApplicationDbContext context, IPatientRepository patientRepository)
        {
            _context = context;
            PatientRepository = patientRepository;
        }

        public async Task CommitAsync() => await _context.SaveChangesAsync();
    }
}
