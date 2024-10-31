using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Patient_Api_Test.Contracts;
using Patient_Api_Test.Helpers;
using PatientLibrary.Models;
using PatientLibrary.Persistence;

namespace Patient_Api_Test.Controllers
{

    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PatientController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all Patients with filter by birthDate
        /// </summary>
        /// <param name="searchTemplateByBirthDate">Template for searching {eq|ne|gt|lt|ge|le|ap}{dateBirthDay} in format yyyy-MM-dd</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllPatientsAsync(
            [FromQuery] string? searchTemplateByBirthDate,
            CancellationToken cancellationToken)
        {

            var dateValue = FiltrationHelpers.ValidateSearchTemplateByBirthDate(searchTemplateByBirthDate);

            if (string.IsNullOrEmpty(dateValue))
            {

                var patientsWithoutFilter = await _context.Patients.Include(p => p.Name).Select(item =>
                    new PatientDataResponse(item.Name, item.Gender.ToString(), item.BirthDate, item.Active)).ToListAsync(cancellationToken);

                HttpContext.Response.Headers.Append("X-Total-Count", patientsWithoutFilter.Count().ToString());

                return Ok(patientsWithoutFilter);
            }

            var parsedDate = FiltrationHelpers.ParseBirthDate(dateValue);

            if (parsedDate == null)
            {
                return BadRequest("Invalid date format");
            }

            var prefixFilter = searchTemplateByBirthDate?.Substring(0, 2);

            var filteredPatients = _context.Patients.Include(p => p.Name).AsQueryable();

            filteredPatients = FiltrationHelpers.FiltrationByPrefixAndDate(prefixFilter, parsedDate, filteredPatients);

            var result = await filteredPatients.Select(item =>
                new PatientDataResponse(item.Name, item.Gender.ToString(), item.BirthDate, item.Active)).ToListAsync(cancellationToken);

            HttpContext.Response.Headers.Append("X-Total-Count", filteredPatients.Count().ToString());

            return Ok(result);
        }

        /// <summary>
        /// Get patient by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<PatientDataResponse>> GetPatient(Guid id, CancellationToken cancellationToken)
        {
            var patient = await _context.Patients.Include(p => p.Name).FirstOrDefaultAsync(p => p.Name.Id == id, cancellationToken);

            if (patient == null)
            {
                return NotFound($"Patient with id {id} not found");
            }

            var response = new PatientDataResponse(patient.Name, patient.Gender.ToString(), patient.BirthDate, patient.Active);
            return response;
        }

        /// <summary>
        /// Create new patient
        /// </summary>
        /// <param name="patientRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Patient>> CreatePatient([FromBody] CreatePatientRequest patientRequest, CancellationToken cancellationToken)
        {
            if (patientRequest.BirthDate == null)
            {
                return BadRequest("BirthDate field is required");
            }

            if (string.IsNullOrEmpty(patientRequest.Name.Family))
            {
                return BadRequest("Name.Family field is required");
            }

            var patient = new Patient
            {
                Name = new Name(),
                PatientId = Guid.NewGuid()
            };


            patient.Name.Family = patientRequest.Name.Family;
            patient.Name.Given = patientRequest.Name.Given;
            patient.Name.Use = patientRequest.Name.Use;
            patient.Name.Id = Guid.NewGuid();
            patient.Gender = patientRequest.Gender;
            patient.BirthDate = patientRequest.BirthDate;
            patient.Active = patientRequest.Active;


            await _context.Patients.AddAsync(patient, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return CreatedAtAction(nameof(GetPatient), new { id = patient.PatientId }, patient);
        }

        /// <summary>
        /// Update existing patient
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patientUpdateRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Patient>> UpdatePatient(
            [FromRoute] Guid id,
            [FromBody] PatientUpdateRequest patientUpdateRequest,
            CancellationToken cancellationToken
            )
        {
            if (patientUpdateRequest.BirthDate == null)
            {
                return BadRequest("BirthDate field is required");
            }

            if (string.IsNullOrEmpty(patientUpdateRequest.Family))
            {
                return BadRequest("Family field is required");
            }


            var patientForUpdate = await _context.Patients.Include(p => p.Name).FirstOrDefaultAsync(p => p.PatientId == id, cancellationToken);

            if (patientForUpdate == null)
            {
                return NotFound($"Cannot find patient with Id {id}");
            }

            patientForUpdate.Gender = patientUpdateRequest.Gender;
            patientForUpdate.Name.Family = patientUpdateRequest.Family;
            patientForUpdate.Name.Given = patientUpdateRequest.Given;
            patientForUpdate.Name.Use = patientUpdateRequest.Use;
            patientForUpdate.BirthDate = patientUpdateRequest.BirthDate;
            patientForUpdate.Active = patientUpdateRequest.Active;

            _context.Patients.Update(patientForUpdate);

            await _context.SaveChangesAsync(cancellationToken);

            return Ok($"Patient with ID {id} was Updated");
        }

        /// <summary>
        /// Remove patient
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult<Patient>> DeletePatient([FromBody] Guid id, CancellationToken cancellationToken)
        {

            var patientForDelete = await _context.Patients.FirstOrDefaultAsync(p => p.Name.Id == id, cancellationToken);

            if (patientForDelete == null)
            {
                return NotFound($"Patient with ID {id} not found");
            }

            _context.Patients.Remove(patientForDelete);

            await _context.SaveChangesAsync(cancellationToken);

            return Ok($"Patient with ID {id} was deleted");
        }
    }
}