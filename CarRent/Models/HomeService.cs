using CarRent.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CarRent.Models
{
    public class HomeService
    {
        public async Task<string[]> GetCoordinates(StartPageVM vM)
        {
            var apiString = $"https://maps.googleapis.com/maps/api/geocode/json?address={vM.City}&key=AIzaSyDqQCALQLs6NM9tMpHUWlC2uLNh5Eniz3I";
            string[] coordinates = new string[2];
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(apiString);
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<dynamic>(json);
                coordinates[0] = result.results[0].geometry.location.lat;
                coordinates[1] = result.results[0].geometry.location.lng;
                return coordinates;
            };
        }
    }
}
