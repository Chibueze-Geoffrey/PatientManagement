using Microsoft.AspNetCore.Mvc;
using PatientManagement.Application.Interface;
using PatientManagement.Common.Dtos.PatientDtos;
using PatientManagement.Common.Dtos.PatientDtos.Request;
using PatientManagement.Common.Dtos.PatientDtos.Response;
using PatientManagement.Common.Dtos.Response;

namespace PatientManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ProducesResponseType(typeof(ExecutionResult<>), 400)]

    public class PatientController : BaseController
    {
        private readonly IPatientService _patientService;
        private readonly ILogService _LogService;

        public PatientController(IPatientService patientService, ILogService LogService)
        {
            _patientService = patientService;
            _LogService = LogService;
        }

        [ProducesResponseType(typeof(ExecutionResult<PatientResponse>), 200)]
        [HttpPost]
        public async Task<IActionResult> CreatePatient([FromBody] PatientDto patientDto)
        {
            string MethodName = "CreatePatient";
            var startTime = DateTime.Now;
            IActionResult result = CustomResponse(await _patientService.CreatePatientAsync(patientDto).ConfigureAwait(false));
            _LogService.LogTypeResponse(patientDto, result, MethodName, _LogService.ReturnTimeSpent(startTime));
            return result;
        }

        [ProducesResponseType(typeof(ExecutionResult<PatientResponse>), 200)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatient(int id)
        {
            string MethodName = "GetPatient";
            var startTime = DateTime.Now;
            IActionResult result = CustomResponse(await _patientService.GetPatientByIdAsync(id).ConfigureAwait(false));
            _LogService.LogTypeResponse(id, result, MethodName, _LogService.ReturnTimeSpent(startTime));
            return result;
        }

        [ProducesResponseType(typeof(ExecutionResult<IEnumerable<PatientResponse>>), 200)]
        [HttpGet]
        public async Task<IActionResult> GetAllPatients()
        {
            string MethodName = "GetAllPatients";
            var startTime = DateTime.Now;
            IActionResult result = CustomResponse(await _patientService.GetAllPatientsAsync().ConfigureAwait(false));
            _LogService.LogTypeResponse("", result, MethodName, _LogService.ReturnTimeSpent(startTime));
            return result; 
        }

        [ProducesResponseType(typeof(ExecutionResult<PatientResponse>), 200)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(int id, [FromBody] PatientUpdateDto patientDto)
        {
            string MethodName = "UpdatePatient";
            var startTime = DateTime.Now;
            IActionResult result = CustomResponse(await _patientService.UpdatePatientAsync(id, patientDto).ConfigureAwait(false));
            _LogService.LogTypeResponse(new { id, patientDto }, result, MethodName, _LogService.ReturnTimeSpent(startTime));
            return result;
        }

        [ProducesResponseType(typeof(ExecutionResult<bool>), 200)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDeletePatient(int id)
        {
            string MethodName = "SoftDeletePatient";
            var startTime = DateTime.Now;
            IActionResult result = CustomResponse(await _patientService.DeletePatientAsync(id).ConfigureAwait(false));
            _LogService.LogTypeResponse(id, result, MethodName, _LogService.ReturnTimeSpent(startTime));
            return result;
        }
    }
}
