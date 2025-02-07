using AutoMapper;
using Moq;
using PatientManagement.Application.Dtos.PatientDtos;
using PatientManagement.Application.Dtos.PatientDtos.Response;
using PatientManagement.Application.Services;
using PatientManagement.Domain.Enums;
using PatientManagement.Domain.Models;
using PatientManagement.Infrastructure.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace PatientManagement.Test.Service
{
    public class PatientServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IPatientRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly PatientService _patientService;

        public PatientServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockRepo = new Mock<IPatientRepository>();
            _mockMapper = new Mock<IMapper>();

            // Mock the repository inside UnitOfWork
            _mockUnitOfWork.Setup(uow => uow.PatientRepository).Returns(_mockRepo.Object);

            _patientService = new PatientService(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task CreatePatientAsync_ShouldReturnPatientResponse()
        {
            // Arrange
            var patientDto = new PatientDto { Name = "John", Age = 30, Status = "Active" };
            var patient = new Patient { Id = 1, Name = "John", Age = 30, Status = PatientStatus.Active };
            var patientResponse = new PatientResponse { Id = 1, Name = "John", Age = 30, Status = "Active" };

            _mockMapper.Setup(m => m.Map<Patient>(patientDto)).Returns(patient);
            _mockRepo.Setup(repo => repo.AddAsync(It.IsAny<Patient>())).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(uow => uow.CommitAsync()).Returns(Task.CompletedTask);
            _mockMapper.Setup(m => m.Map<PatientResponse>(patient)).Returns(patientResponse);

            // Act
            var result = await _patientService.CreatePatientAsync(patientDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John", result.Name);
            Assert.Equal(30, result.Age);
            Assert.Equal("Active", result.Status);
        }

        [Fact]
        public async Task GetPatientByIdAsync_ShouldReturnPatientResponse_WhenPatientExists()
        {
            // Arrange
            var patient = new Patient { Id = 1, Name = "Jane", Age = 28, Status = PatientStatus.Active };
            var patientResponse = new PatientResponse { Id = 1, Name = "Jane", Age = 28, Status = "Active" };

            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(patient);
            _mockMapper.Setup(m => m.Map<PatientResponse>(patient)).Returns(patientResponse);

            // Act
            var result = await _patientService.GetPatientByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Jane", result.Name);
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
                new Patient { Id = 1, Name = "John", Age = 30, Status = PatientStatus.Active },
                new Patient { Id = 2, Name = "Jane", Age = 28, Status = PatientStatus.Inactive }
            };

            var patientResponses = new List<PatientResponse>
            {
                new PatientResponse { Id = 1, Name = "John", Age = 30, Status = "Active" },
                new PatientResponse { Id = 2, Name = "Jane", Age = 28, Status = "Inactive" }
            };

            _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(patients);
            _mockMapper.Setup(m => m.Map<List<PatientResponse>>(patients)).Returns(patientResponses);

            // Act
            var result = await _patientService.GetAllPatientsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
        [Fact]
        public async Task UpdatePatientAsync_ShouldReturnUpdatedPatientResponse()
        {
            // Arrange
            var patientDto = new PatientDto { Name = "John Updated", Age = 32, Status = "Active" };
            var existingPatient = new Patient { Id = 1, Name = "John", Age = 30, Status = PatientStatus.Active };
            var updatedPatient = new Patient { Id = 1, Name = "John Updated", Age = 32, Status = PatientStatus.Active };
            var patientResponse = new PatientResponse { Id = 1, Name = "John Updated", Age = 32, Status = "Active" };

            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingPatient);
            _mockMapper.Setup(m => m.Map(patientDto, existingPatient)).Returns(updatedPatient);
            _mockUnitOfWork.Setup(uow => uow.CommitAsync()).Returns(Task.CompletedTask);
            _mockMapper.Setup(m => m.Map<PatientResponse>(updatedPatient)).Returns(patientResponse);

            // Debugging Assertions
            var patient = await _mockRepo.Object.GetByIdAsync(1);
            Assert.NotNull(patient);  // Ensure existing patient is found

            var mappedPatient = _mockMapper.Object.Map(patientDto, existingPatient);
            Assert.NotNull(mappedPatient);  // Ensure mapping works

            var mappedResponse = _mockMapper.Object.Map<PatientResponse>(updatedPatient);
            Assert.NotNull(mappedResponse);  // Ensure response mapping works

            // Act
            var result = await _patientService.UpdatePatientAsync(1, patientDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John Updated", result.Name);
            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
        }

    }
}
