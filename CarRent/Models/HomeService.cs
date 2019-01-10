﻿using CarRent.Models.Entities;
using CarRent.Models.ViewModels;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
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
        CarRentContext context;

        public HomeService(CarRentContext context)
        {
            this.context = context;
        }

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

        public CarSearchVM[] CompareCoords(Coordinate coordinate)
        {
            var point = new Point(coordinate);
            point.SRID = 4326;

            var cars = context.Car.OrderBy(o => o.GeoLocation.Distance(point)).Select
             (c => new CarSearchVM
              {
                  Id = c.Id,
                  Model = c.Model,
                  ImgUrl = c.ImgUrl,
                  Price = c.Price,
                  YearModel = c.YearModel,
                  Rating = c.Rent.SelectMany(r => r.Review).Count() > 0 ? c.Rent.SelectMany(r => r.Review).Average(s => s.Rating) : 0
              }).ToArray();

            return cars;
        }

        public bool CheckAvailability(int id, DateTime start, DateTime end)
        {
            var rents = context.Rent.Where(r => r.CarId == id);

            foreach (var item in rents)
            {
                if (Math.Max(start.Ticks, item.Datestart.Ticks) < Math.Min(end.Ticks, item.DateEnd.Ticks))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
