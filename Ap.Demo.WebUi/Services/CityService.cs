using Ap.Demo.Application.CQRS.City;
using Ap.Demo.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace Ap.Demo.WebUi.Services
{
    public class CityService
    {
        private readonly HttpClient httpClient;

        public CityService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<CityDTO>> GetAllCities(string sortOrder = "asc")
        {
            return await httpClient.GetFromJsonAsync<List<CityDTO>>($"/api/v1/cities") ?? new List<CityDTO>();
        }

        public async Task<CityDTO> GetCityById(int id)
        {
            return await httpClient.GetFromJsonAsync<CityDTO>($"api/v1/cities/{id}") ?? new CityDTO();
        }

        public async Task<UpdateCityDTO> UpdateCity(int id, UpdateCityDTO updateDto)
        {
            var response = await httpClient.PutAsJsonAsync($"/api/v1/cities/{id}", updateDto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<UpdateCityDTO>() ?? new UpdateCityDTO();
        }
    }
}