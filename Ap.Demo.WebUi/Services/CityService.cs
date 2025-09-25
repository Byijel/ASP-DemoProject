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
        private object json;

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

        public async Task<CityDto> UpdateCity(int id, CityDto city)
        {
/*            var json = JsonSerializer.Serialize(city);
            Console.WriteLine($"Sending PUT request with JSON: {json}");
            Console.WriteLine($"Updating City ID: {id}, DTO ID: {city.Id}");*/
            var response = await httpClient.PutAsJsonAsync($"/api/v1/City/{id}", city);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CityDto>() ?? city;
        }
    }
}
