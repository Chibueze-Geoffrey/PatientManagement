using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientManagement.Infrastructure.Interface
{
    public interface IUnitOfWork
    {
        IPatientRepository PatientRepository { get; }
        Task CommitAsync();
    }
}
