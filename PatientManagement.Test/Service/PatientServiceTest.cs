using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using PatientManagement.Application.Services;
using PatientManagement.Common.Dtos.PatientDtos;
using PatientManagement.Common.Dtos.PatientDtos.Request;
using PatientManagement.Common.Dtos.PatientDtos.Response;
using PatientManagement.Common.Enums;
using PatientManagement.Domain.Models;
using PatientManagement.Infrastructure.Interface;

namespace PatientManagement.Test.Service
{
    public class PatientServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IPatientRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<PatientService>> _mockLogger;
        private readonly PatientService _patientService;

        public PatientServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockRepo = new Mock<IPatientRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<PatientService>>();

            // Mock the repository inside UnitOfWork
            _mockUnitOfWork.Setup(uow => uow.PatientRepository).Returns(_mockRepo.Object);

            _patientService = new PatientService(_mockUnitOfWork.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task CreatePatientAsync_ShouldReturnPatientResponse()
        {
            // Arrange
            var patientDto = new PatientDto { FirstName = "John ", LastName = "Jery ", Age = 30, Status = PatientStatus.Active.ToString() };
            var patient = new Patient { Id = 1, FirstName = "John ", LastName = "Jery ", Age = 30, Status = PatientStatus.Active };
            var patientResponse = new PatientResponse { Id = 1, FirstName = "John ", LastName = "Jery ", Age = 30, Status = PatientStatus.Active.ToString() };

            _mockMapper.Setup(m => m.Map<Patient>(patientDto)).Returns(patient);
            _mockRepo.Setup(repo => repo.AddAsync(It.IsAny<Patient>())).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(uow => uow.CommitAsync()).Returns(Task.CompletedTask);
            _mockMapper.Setup(m => m.Map<PatientResponse>(patient)).Returns(patientResponse);

            // Act
            var result = await _patientService.CreatePatientAsync(patientDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John", result.Result.FirstName);
            Assert.Equal(30, result.Result.Age);
            Assert.Equal(PatientStatus.Active.ToString(), result.Result.Status.ToString());
        }

        [Fact]
        public async Task GetPatientByIdAsync_ShouldReturnPatientResponse_WhenPatientExists()
        {
            // Arrange
            var patient = new Patient { Id = 1, FirstName = "Jane ", LastName = "Jery ", Age = 28, Status = PatientStatus.Active };
            var patientResponse = new PatientResponse { Id = 1, FirstName = "Jane ", LastName = "Jery ", Age = 28, Status = PatientStatus.Active.ToString() };

            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(patient);
            _mockMapper.Setup(m => m.Map<PatientResponse>(patient)).Returns(patientResponse);

            // Act
            var result = await _patientService.GetPatientByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Result.Id);
            Assert.Equal("Jane", result.Result.FirstName);
        }

        [Fact]
        public async Task GetPatientByIdAsync_ShouldReturnNull_WhenPatientDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetByIdAsync(99)).ReturnsAsync((Patient)null);

            // Act
            var result = await _patientService.GetPatientByIdAsync(99);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllPatientsAsync_ShouldReturnListOfPatients()
        {
            // Arrange
            var patients = new List<Patient>
            {
                new Patient { Id = 1, FirstName = "John ", LastName = "Jery ", Age = 30, Status = PatientStatus.Active },
                new Patient { Id = 2, FirstName = "Jane ", LastName = "Jery ", Age = 28, Status = PatientStatus.Inactive }
            };

            var patientResponses = new List<PatientResponse>
            {
                new PatientResponse { Id = 1, FirstName = "John ", LastName = "Jery ", Age = 30, Status = PatientStatus.Active.ToString() },
                new PatientResponse { Id = 2, FirstName = "Jane", LastName = "Jery ", Age = 28, Status = PatientStatus.Inactive.ToString() }
            };

            _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(patients);
            _mockMapper.Setup(m => m.Map<List<PatientResponse>>(patients)).Returns(patientResponses);

            // Act
            var result = await _patientService.GetAllPatientsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Result.Count());
        }

        [Fact]
        public async Task UpdatePatientAsync_ShouldReturnUpdatedPatientResponse()
        {
            // Arrange
            var patientDto = new PatientUpdateDto { FirstName = "John Updated", LastName = "Jery Updated", Age = 32, Status = PatientStatus.Active.ToString() };
            var existingPatient = new Patient { Id = 1, FirstName = "John ", LastName = "Jery ", Age = 30, Status = PatientStatus.Active };
            var updatedPatient = new Patient { Id = 1, FirstName = "John Updated", LastName = "Jery Updated", Age = 32, Status = PatientStatus.Active };
            var patientResponse = new PatientUpdateResponseDto { Id = 1, FirstName = "John Updated", LastName = "Jery Updated", Age = 32, Status = PatientStatus.Active.ToString() };

            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingPatient);
            _mockMapper.Setup(m => m.Map(patientDto, existingPatient)).Returns(updatedPatient);
            _mockUnitOfWork.Setup(uow => uow.CommitAsync()).Returns(Task.CompletedTask);
            _mockMapper.Setup(m => m.Map<PatientUpdateResponseDto>(updatedPatient)).Returns(patientResponse);
            // Act
            var result = await _patientService.UpdatePatientAsync(1, patientDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John Updated", result.Result.FirstName);
            Assert.Equal("Jerry Updated", result.Result.LastName);

            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
        }
    }
}
