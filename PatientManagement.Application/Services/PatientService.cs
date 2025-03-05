using AutoMapper;
using Microsoft.Extensions.Logging;
using PatientManagement.Application.Interface;
using PatientManagement.Common.Dtos.PatientDtos;
using PatientManagement.Common.Dtos.PatientDtos.Request;
using PatientManagement.Common.Dtos.PatientDtos.Response;
using PatientManagement.Common.Dtos.Response;
using PatientManagement.Common.Enums;
using PatientManagement.Domain.Models;
using PatientManagement.Infrastructure.Interface;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace PatientManagement.Application.Services
{
    public class PatientService : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<PatientService> _logger;

        public PatientService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PatientService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        // Create Patient - 
        public async Task<ExecutionResult<PatientResponse>> CreatePatientAsync(PatientDto patientDto)
        {
            try
            {
                var patient = _mapper.Map<Patient>(patientDto);
                await _unitOfWork.PatientRepository.AddAsync(patient);
                await _unitOfWork.CommitAsync();

                _logger.LogInformation($"Patient created successfully with ID: {patient.Id}");
                return ExecutionResult<PatientResponse>.Success(_mapper.Map<PatientResponse>(patient), "Patient created successfully.");
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning($"Validation error: {ex.Message}");
                return ExecutionResult<PatientResponse>.Failed(ex.Message, ResponseCode.ValidationError);
            }       
            catch (Exception ex)
            {
                _logger.LogError($"Error creating patient: {ex.Message}");
                return ExecutionResult<PatientResponse>.Failed("An error occurred while creating the patient.");
            }
        }

        // Get Patient by ID - 
        public async Task<ExecutionResult<PatientResponse>> GetPatientByIdAsync(int id)
        {
            try
            {
                var patient = await _unitOfWork.PatientRepository.GetByIdAsync(id);
                if (patient == null)
                {
                    _logger.LogWarning($"Patient with ID {id} not found.");
                    return ExecutionResult<PatientResponse>.Failed($"Patient with ID {id} not found.", ResponseCode.NotFound);
                }
                _logger.LogInformation($"Patient with ID {id} retrieved successfully.");
                return ExecutionResult<PatientResponse>.Success(_mapper.Map<PatientResponse>(patient), "Patient retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving patient with ID {id}: {ex.Message}");
                return ExecutionResult<PatientResponse>.Failed("An error occurred while retrieving the patient.");
            }
        }

        // Get All Active Patients - 
        public async Task<ExecutionResult<IEnumerable<PatientResponse>>> GetAllPatientsAsync()
        {
            try
            {
                var patients = await _unitOfWork.PatientRepository.GetAllActivePatientsAsync();
                _logger.LogInformation($"Retrieved {patients.Count()} patients.");
                return ExecutionResult<IEnumerable<PatientResponse>>.Success(_mapper.Map<IEnumerable<PatientResponse>>(patients), " patients retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving all patients: {ex.Message}");
                return ExecutionResult<IEnumerable<PatientResponse>>.Failed("An error occurred while retrieving all  patients.");
            }
        }

        // Update Patient
        public async Task<ExecutionResult<PatientUpdateResponseDto>> UpdatePatientAsync(int id, PatientUpdateDto patientDto)
        {
            try
            {
                var patient = await _unitOfWork.PatientRepository.GetByIdAsync(id);
                if (patient == null)
                {
                    _logger.LogWarning($"Patient with ID {id} not found.");
                    return ExecutionResult<PatientUpdateResponseDto>.Failed($"Patient with ID {id} not found.", ResponseCode.NotFound);
                }

                _mapper.Map(patientDto, patient);
                await _unitOfWork.PatientRepository.UpdateAsync(patient);
                await _unitOfWork.CommitAsync();

                _logger.LogInformation($"Patient with ID {id} updated successfully.");
                return ExecutionResult<PatientUpdateResponseDto>.Success(_mapper.Map<PatientUpdateResponseDto>(patient), "Patient updated successfully.");
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning($"Validation error: {ex.Message}");
                return ExecutionResult<PatientUpdateResponseDto>.Failed(ex.Message, ResponseCode.ValidationError);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating patient with ID {id}: {ex.Message}");
                return ExecutionResult<PatientUpdateResponseDto>.Failed("An error occurred while updating the patient.");
            }
        }

        // Soft Delete Patient 
        public async Task<ExecutionResult<bool>> DeletePatientAsync(int id)
        {
            try
            {
                var patient = await _unitOfWork.PatientRepository.GetByIdAsync(id);
                if (patient == null)
                {
                    _logger.LogWarning($"Patient with ID {id} not found.");
                    return ExecutionResult<bool>.Failed($"Patient with ID {id} not found.", ResponseCode.NotFound);
                }

                await _unitOfWork.PatientRepository.SoftDeleteAsync(id);
                await _unitOfWork.CommitAsync();

                _logger.LogInformation($"Patient with ID {id} soft deleted successfully.");
                return ExecutionResult<bool>.Success(true, "Patient soft deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error soft deleting patient with ID {id}: {ex.Message}");
                return ExecutionResult<bool>.Failed("An error occurred while soft deleting the patient.");
            }
        }

        // Restore
        public async Task<ExecutionResult<bool>> RestorePatientAsync(int id)
        {
            try
            {
                var patient = await _unitOfWork.PatientRepository.GetPatientIncludingDeletedAsync(id);
                if (patient == null || patient.Status != PatientStatus.Deleted)
                {
                    _logger.LogWarning($"Patient with ID {id} not found or not deleted.");
                    return ExecutionResult<bool>.Failed($"Patient with ID {id} not found or not deleted.", ResponseCode.NotFound);
                }

                patient.Status = PatientStatus.Active; // Update status
                await _unitOfWork.PatientRepository.UpdateAsync(patient);
                await _unitOfWork.CommitAsync();

                _logger.LogInformation($"Patient with ID {id} restored successfully.");
                return ExecutionResult<bool>.Success(true, "Patient restored successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error restoring patient with ID {id}: {ex.Message}");
                return ExecutionResult<bool>.Failed("An error occurred while restoring the patient.");
            }
        }
        // Permanent Delete Patient
        public async Task<ExecutionResult<bool>> PermanentlyDeletePatientAsync(int id)
        {
            try
            {
                var patient = await _unitOfWork.PatientRepository.GetPatientIncludingDeletedAsync(id);
                if (patient == null)
                {
                    _logger.LogWarning($"Patient with ID {id} not found.");
                    return ExecutionResult<bool>.Failed($"Patient with ID {id} not found.", ResponseCode.NotFound);
                }

                await _unitOfWork.PatientRepository.DeleteAsync(patient.Id);
                await _unitOfWork.CommitAsync();

                _logger.LogInformation($"Patient with ID {id} permanently deleted.");
                return ExecutionResult<bool>.Success(true, "Patient permanently deleted.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error permanently deleting patient with ID {id}: {ex.Message}");
                return ExecutionResult<bool>.Failed("An error occurred while permanently deleting the patient.");
            }
        }


    }
}
