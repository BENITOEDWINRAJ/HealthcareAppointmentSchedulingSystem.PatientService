using Microsoft.AspNetCore.Http;
using PatientService.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace PatientService.Infrastructure.Services
{
    public class AppointmentServiceClient: IAppointmentServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AppointmentServiceClient(HttpClient httpClient,
                                        IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<object> GetAppointments()
        {
            // Forward JWT token from incoming request
            var token = _httpContextAccessor.HttpContext?
                .Request.Headers["Authorization"]
                .ToString();

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    AuthenticationHeaderValue.Parse(token);
            }

            var response = await _httpClient.GetAsync("api/appointments");

            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadFromJsonAsync<object>();

            return data;
        }
    }
}
