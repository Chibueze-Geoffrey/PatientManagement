using Microsoft.AspNetCore.Mvc;
using Moq;
using PatientManagement.Api.Controllers;
using PatientManagement.Application.Dtos.PatientDtos;
using PatientManagement.Application.Dtos.PatientDtos.Response;
using PatientManagement.Application.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;


namespace PatientManagement.Test.Controllers
{
    public class PatientControllerTests
    {
        private readonly Mock<IPatientService> _mockService;
        private readonly PatientController _controller;

        public PatientControllerTests()
        {
            _mockService = new Mock<IPatientService>();
            _controller = new PatientController(_mockService.Object);
        }

        [Fact]
        public async Task CreatePatient_ShouldReturnCreatedResponse()
        {
            // Arrange
            var patientDto = new PatientDto { Name = "John", Age = 30, Status = "Active" };
            var patientResponse = new PatientResponse { Id = 1, Name = "John", Age = 30, Status = "Active" };

            _mockService.Setup(s => s.CreatePatientAsync(patientDto)).ReturnsAsync(patientResponse);

            // Act
            var result = await _controller.CreatePatient(patientDto);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedPatient = Assert.IsType<PatientResponse>(actionResult.Value);
            Assert.Equal(1, returnedPatient.Id);
            Assert.Equal("John", returnedPatient.Name);
        }

        [Fact]
        public async Task GetPatient_ShouldReturnPatient_WhenFound()
        {
            // Arrange
            var patientResponse = new PatientResponse { Id = 1, Name = "John", Age = 30, Status = "Active" };

            _mockService.Setup(s => s.GetPatientByIdAsync(1)).ReturnsAsync(patientResponse);

            // Act
            var result = await _controller.GetPatient(1);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedPatient = Assert.IsType<PatientResponse>(actionResult.Value);
            Assert.Equal(1, returnedPatient.Id);
        }

        [Fact]
        public async Task GetPatient_ShouldReturnNotFound_WhenNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.GetPatientByIdAsync(1)).ReturnsAsync((PatientResponse)null);

            // Act
            var result = await _controller.GetPatient(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetAllPatients_ShouldReturnListOfPatients()
        {
            // Arrange
            var patients = new List<PatientResponse>
            {
                new PatientResponse { Id = 1, Name = "John", Age = 30, Status = "Active" },
                new PatientResponse { Id = 2, Name = "Jane", Age = 25, Status = "Inactive" }
            };

            _mockService.Setup(s => s.GetAllPatientsAsync()).ReturnsAsync(patients);

            // Act
            var result = await _controller.GetAllPatients();

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedPatients = Assert.IsType<List<PatientResponse>>(actionResult.Value);
            Assert.Equal(2, returnedPatients.Count);
        }

        [Fact]
        public async Task UpdatePatient_ShouldReturnUpdatedPatient_WhenSuccessful()
        {
            // Arrange
            var patientDto = new PatientDto { Name = "John Updated", Age = 31, Status = "Active" };
            var patientResponse = new PatientResponse { Id = 1, Name = "John Updated", Age = 31, Status = "Active" };

            _mockService.Setup(s => s.UpdatePatientAsync(1, patientDto)).ReturnsAsync(patientResponse);

            // Act
            var result = await _controller.UpdatePatient(1, patientDto);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var returnedPatient = Assert.IsType<PatientResponse>(actionResult.Value);
            Assert.Equal("John Updated", returnedPatient.Name);
        }

        [Fact]
        public async Task UpdatePatient_ShouldReturnNotFound_WhenPatientDoesNotExist()
        {
            // Arrange
            var patientDto = new PatientDto { Name = "John Updated", Age = 31, Status = "Active" };

            _mockService.Setup(s => s.UpdatePatientAsync(1, patientDto)).ReturnsAsync((PatientResponse)null);

            // Act
            var result = await _controller.UpdatePatient(1, patientDto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task SoftDeletePatient_ShouldReturnNoContent_WhenSuccessful()
        {
            // Arrange
            _mockService.Setup(s => s.DeletePatientAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.SoftDeletePatient(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task SoftDeletePatient_ShouldReturnNotFound_WhenPatientDoesNotExist()
        {
            // Arrange
            _mockService.Setup(s => s.DeletePatientAsync(1)).ReturnsAsync(false);

            // Act
            var result = await _controller.SoftDeletePatient(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
