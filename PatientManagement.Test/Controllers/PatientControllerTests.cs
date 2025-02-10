using Microsoft.AspNetCore.Mvc;
using Moq;
using PatientManagement.Api.Controllers;
using PatientManagement.Application.Interface;
using PatientManagement.Common.Dtos.PatientDtos;
using PatientManagement.Common.Dtos.PatientDtos.Request;
using PatientManagement.Common.Dtos.PatientDtos.Response;
using PatientManagement.Common.Dtos.Response;
using PatientManagement.Common.Enums;

namespace PatientManagement.Test.Controllers
{
    public class PatientControllerTests
    {
        private readonly Mock<IPatientService> _mockService;
        private readonly Mock<ILogService> _mockLogService;
        private readonly PatientController _controller;

        public PatientControllerTests()
        {
            _mockService = new Mock<IPatientService>();
            _mockLogService = new Mock<ILogService>();
            _controller = new PatientController(_mockService.Object, _mockLogService.Object);
        }

        [Fact]
        public async Task CreatePatient_ShouldReturnCreatedResponse()
        {
            // Arrange
            var patientDto = new PatientDto { FirstName = "John ", LastName = "Jery ", Age = 30, Status = PatientStatus.Active };
            var patientResponse = new ExecutionResult<PatientResponse>
            {
                Response = ResponseCode.Ok,
                Result = new PatientResponse { Id = 1, FirstName = "John ", LastName = "Jery ", Age = 30, Status = PatientStatus.Active.ToString() }
            };

            _mockService.Setup(s => s.CreatePatientAsync(patientDto)).ReturnsAsync(patientResponse);

            // Act
            var result = await _controller.CreatePatient(patientDto);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnedPatient = Assert.IsType<PatientResponse>(actionResult.Value);
            Assert.Equal(1, returnedPatient.Id);
            Assert.Equal("John", returnedPatient.FirstName);
        }

        [Fact]
        public async Task GetPatient_ShouldReturnPatient_WhenFound()
        {
            // Arrange
            var patientResponse = new ExecutionResult<PatientResponse>
            {
                Response = ResponseCode.Ok,
                Result = new PatientResponse { Id = 1, FirstName = "John ", LastName = "Jery ", Age = 30, Status = PatientStatus.Active.ToString() }
            };

            _mockService.Setup(s => s.GetPatientByIdAsync(1)).ReturnsAsync(patientResponse);

            // Act
            var result = await _controller.GetPatient(1);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var returnedPatient = Assert.IsType<PatientResponse>(actionResult.Value);
            Assert.Equal(1, returnedPatient.Id);
        }

        [Fact]
        public async Task GetPatient_ShouldReturnNotFound_WhenNotFound()
        {
            // Arrange
            var patientResponse = new ExecutionResult<PatientResponse>
            {
                Response = ResponseCode.NotFound,
                Result = null
            };

            _mockService.Setup(s => s.GetPatientByIdAsync(1)).ReturnsAsync(patientResponse);

            // Act
            var result = await _controller.GetPatient(1);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetAllPatients_ShouldReturnListOfPatients()
        {
            // Arrange
            var patients = new ExecutionResult<IEnumerable<PatientResponse>>
            {
                Response = ResponseCode.Ok,
                Result = new List<PatientResponse>
                {
                    new PatientResponse { Id = 1, FirstName = "John ", LastName = "Jery ", Age = 30, Status = PatientStatus.Active.ToString() },
                    new PatientResponse { Id = 2, FirstName = "Jane", LastName = "Jery Updated", Age = 25, Status = PatientStatus.Inactive.ToString() }
                }
            };

            _mockService.Setup(s => s.GetAllPatientsAsync()).ReturnsAsync(patients);

            // Act
            var result = await _controller.GetAllPatients();

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var returnedPatients = Assert.IsType<List<PatientResponse>>(actionResult.Value);
            Assert.Equal(2, returnedPatients.Count);
        }

        [Fact]
        public async Task UpdatePatient_ShouldReturnUpdatedPatient_WhenSuccessful()
        {
            // Arrange
            var patientDto = new PatientUpdateDto { FirstName = "John Updated", LastName = "Jery Updated", Age = 31, Status = PatientStatus.Active.ToString() };
            var patientResponse = new ExecutionResult<PatientUpdateResponseDto>
            {
                Response = ResponseCode.Ok,
                Result = new PatientUpdateResponseDto { Id = 1, FirstName = "John ", LastName = "Jery ", Age = 31, Status = PatientStatus.Active.ToString() }
            };

            _mockService.Setup(s => s.UpdatePatientAsync(1, patientDto)).ReturnsAsync(patientResponse);

            // Act
            var result = await _controller.UpdatePatient(1, patientDto);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var returnedPatient = Assert.IsType<PatientResponse>(actionResult.Value);
            Assert.Equal("John Updated", returnedPatient.FirstName);
        }

        [Fact]
        public async Task UpdatePatient_ShouldReturnNotFound_WhenPatientDoesNotExist()
        {
            // Arrange
            var patientDto = new PatientUpdateDto { FirstName = "John Updated",LastName= "Jery Updated", Age = 31, Status = PatientStatus.Active.ToString() };
            var patientResponse = new ExecutionResult<PatientUpdateResponseDto>
            {
                Response = ResponseCode.NotFound,
                Result = null
            };

            _mockService.Setup(s => s.UpdatePatientAsync(1, patientDto)).ReturnsAsync(patientResponse);

            // Act
            var result = await _controller.UpdatePatient(1, patientDto);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task SoftDeletePatient_ShouldReturnNoContent_WhenSuccessful()
        {
            // Arrange
            var patientResponse = new ExecutionResult<bool>
            {
                Response = ResponseCode.Ok,
                Result = true
            };

            _mockService.Setup(s => s.DeletePatientAsync(1)).ReturnsAsync(patientResponse);

            // Act
            var result = await _controller.SoftDeletePatient(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task SoftDeletePatient_ShouldReturnNotFound_WhenPatientDoesNotExist()
        {
            // Arrange
            var patientResponse = new ExecutionResult<bool>
            {
                Response = ResponseCode.NotFound,
                Result = false
            };

            _mockService.Setup(s => s.DeletePatientAsync(1)).ReturnsAsync(patientResponse);

            // Act
            var result = await _controller.SoftDeletePatient(1);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
