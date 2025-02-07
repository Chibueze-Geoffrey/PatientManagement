using AutoMapper;
using PatientManagement.Application.Dtos.PatientDtos;
using PatientManagement.Application.Dtos.PatientDtos.Response;
using PatientManagement.Application.Interface;
using PatientManagement.Domain.Models;
using PatientManagement.Infrastructure.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PatientManagement.Application.Services
{
    public class PatientService : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PatientService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Create Patient - Accepts PatientDto (Request), Returns PatientResponse (with ID)
        public async Task<PatientResponse> CreatePatientAsync(PatientDto patientDto)
        {
            var patient = _mapper.Map<Patient>(patientDto);
            await _unitOfWork.PatientRepository.AddAsync(patient);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<PatientResponse>(patient); // Return response with ID
        }

        // Get Patient by ID - Returns PatientResponse
        public async Task<PatientResponse> GetPatientByIdAsync(int id)
        {
            var patient = await _unitOfWork.PatientRepository.GetByIdAsync(id);
            return patient == null ? null : _mapper.Map<PatientResponse>(patient);
        }

        // Get All Active Patients - Returns List of PatientResponse
        public async Task<IEnumerable<PatientResponse>> GetAllPatientsAsync()
        {
            var patients = await _unitOfWork.PatientRepository.GetAllActivePatientsAsync();
            return _mapper.Map<IEnumerable<PatientResponse>>(patients);
        }

        // Update Patient - Accepts PatientDto, Returns PatientResponse
        public async Task<PatientResponse> UpdatePatientAsync(int id, PatientDto patientDto)
        {
            var patient = await _unitOfWork.PatientRepository.GetByIdAsync(id);
            if (patient == null) return null;

            _mapper.Map(patientDto, patient);
            _unitOfWork.PatientRepository.UpdateAsync(patient);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<PatientResponse>(patient);
        }

        // Soft Delete Patient - Returns true/false
        public async Task<bool> DeletePatientAsync(int id)
        {
            var patient = await _unitOfWork.PatientRepository.GetByIdAsync(id);
            if (patient == null) return false;

            await _unitOfWork.PatientRepository.SoftDeleteAsync(id);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
