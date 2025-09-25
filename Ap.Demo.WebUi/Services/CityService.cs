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

        public async Task<List<CityDto>> GetAllCities(string sortOrder = "asc")
        {
            return await httpClient.GetFromJsonAsync<List<CityDto>>($"/api/v1/City") ?? new List<CityDto>();
        }

        public async Task<CityDto> GetCityById(int id)
        {
            return await httpClient.GetFromJsonAsync<CityDto>($"api/v1/City/{id}") ?? new CityDto();
        }

        public async Task<UpdateCityDto> UpdateCity(int id, UpdateCityDto updateDto)
        {
            var response = await httpClient.PutAsJsonAsync($"/api/v1/City/{id}", updateDto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<UpdateCityDto>() ?? new UpdateCityDto();
        }
    }
}
