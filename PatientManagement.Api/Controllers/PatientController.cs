using Microsoft.AspNetCore.Mvc;
using PatientManagement.Application.Dtos.PatientDtos;
using PatientManagement.Application.Dtos.PatientDtos.Response;
using PatientManagement.Application.Interface;
using PatientManagement.Application.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PatientManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        // Create Patient - Returns Response with ID
        [HttpPost]
        public async Task<ActionResult<PatientResponse>> CreatePatient([FromBody] PatientDto patientDto)
        {
            var patient = await _patientService.CreatePatientAsync(patientDto);
            return CreatedAtAction(nameof(GetPatient), new { id = patient.Id }, patient);
        }

        // Get Patient by ID - Returns Response with ID
        [HttpGet("{id}")]
        public async Task<ActionResult<PatientResponse>> GetPatient(int id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);
            if (patient == null) return NotFound();
            return Ok(patient);
        }

        // Get All Patients - Returns List of Patients with IDs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientResponse>>> GetAllPatients()
        {
            var patients = await _patientService.GetAllPatientsAsync();
            return Ok(patients);
        }

        // Update Patient - Accepts Request DTO, Returns Response DTO
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(int id, [FromBody] PatientDto patientDto)
        {
            var patient = await _patientService.UpdatePatientAsync(id, patientDto);
            if (patient == null) return NotFound();
            return Ok(patient);
        }

        // Soft Delete Patient
        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDeletePatient(int id)
        {
            var success = await _patientService.DeletePatientAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
