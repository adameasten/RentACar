using CarRent.Models.ViewModels;
using GeoAPI.Geometries;
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
        public async Task<Coordinate> GetCoordinates(StartPageVM vM)
        {
            var apiString = $"https://maps.googleapis.com/maps/api/geocode/json?address={vM.City}&key=AIzaSyDqQCALQLs6NM9tMpHUWlC2uLNh5Eniz3I";
            var Coordinates = new Coordinate();
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(apiString);
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<dynamic>(json);
                Coordinates.X = (double)result.results[0].geometry.location.lat;
                Coordinates.Y = (double)result.results[0].geometry.location.lng;


                return Coordinates;
            };
        }
    }
}
