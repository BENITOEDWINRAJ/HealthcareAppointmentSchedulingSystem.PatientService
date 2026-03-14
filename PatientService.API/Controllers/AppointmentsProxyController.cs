using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PatientService.Application.Common.Interfaces;

namespace PatientService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsProxyController : ControllerBase
    {
        private readonly IAppointmentServiceClient _appointmentClient;

        public AppointmentsProxyController(IAppointmentServiceClient appointmentClient)
        {
            _appointmentClient = appointmentClient;
        }

        [Authorize(Roles = "Doctor,Patient")]
        [HttpGet]
        public async Task<IActionResult> GetAppointments()
        {
            var result = await _appointmentClient.GetAppointments();

            return Ok(new
            {
                Message = "Appointments retrieved from AppointmentService",
                Data = result
            });
        }
    }
}
