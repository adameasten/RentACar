﻿using CarRent.Models.Entities;
using CarRent.Models.ViewModels;
using GeoAPI.Geometries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        CarServices carservices;
        IConfiguration config;

        public HomeService(CarRentContext context, CarServices carservices, IConfiguration config)
        {
            this.context = context;
            this.carservices = carservices;
            this.config = config;
        }

        public async Task<Coordinate> GetCoordinates(string city)
        {
            var key = config["GetLocationKey"];
            var apiString = $"https://maps.googleapis.com/maps/api/geocode/json?address={city}&key={key}";
            var Coordinates = new Coordinate();
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(apiString);
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<dynamic>(json);
                if (result.status != "ZERO_RESULTS")
                {
                    Coordinates.X = (double)result.results[0].geometry.location.lat;
                    Coordinates.Y = (double)result.results[0].geometry.location.lng;
                }
                else
                {
                    Coordinates.X = 59.32932349999999;
                    Coordinates.Y = 18.0685808;
                }
                return Coordinates;
            };
        }

        internal StartPageVM AddTimeToDates(StartPageVM vM)
        {
            vM.StartDate = vM.StartDate.Add(DateTime.Parse(vM.StartingHour).TimeOfDay);
            vM.EndDate = vM.EndDate.Add(DateTime.Parse(vM.EndingHour).TimeOfDay);
            return vM;
        }

        public CarSearchVM[] CompareCoords(Coordinate coordinate, StartPageVM vM)
        {
            var point = new Point(coordinate);
            point.SRID = 4326;

            return context.Car.Include(x => x.Rent).Where(a => CheckAvailability(a.Rent.ToArray(), vM.StartDate, vM.EndDate))
            .OrderBy(o => o.GeoLocation.Distance(point))
            .Select(c => new CarSearchVM
            {
                Id = c.Id,
                Model = c.Model,
                Distance = c.GeoLocation.Distance(point) / 1600,
                ImgUrl = c.CarImage.Select(d => d.ImgUrl).FirstOrDefault(),
                Price = c.Price,
                YearModel = c.YearModel,
                Rating = c.Rent.SelectMany(r => r.Review).Count() > 0 ? c.Rent.SelectMany(r => r.Review).Average(s => s.Rating) : 0,
                Ac = c.Ac,
                ChildSeat = c.ChildSeat,
                Doors = c.Doors,
                Fuel = c.Fuel,
                Gear = c.Gear,
                Pets = c.Pets,
                RoofRack = c.RoofRack,
                Seats = c.Seats,
                TowBar = c.TowBar,
                Type = c.Type
            }).Where(x => x.Distance < 80).ToArray();
        }

        internal CarRentConfirmVM MakeConfirmation(CarRentFormVM vM)
        {
            return new CarRentConfirmVM
            {
                CarId = vM.CarId,
                CarName = context.Car.SingleOrDefault(c => c.Id == vM.CarId).Model,
                StartTime = vM.StartTime,
                EndTime = vM.EndTime,
                Price = vM.Price
            };
        }

        internal bool CarIsAvailable(CarRentConfirmVM vM)
        {
            return CheckAvailability(context.Car.Include(x => x.Rent)
                .SingleOrDefault(c => c.Id == vM.CarId)
                .Rent.ToArray(), vM.StartTime, vM.EndTime);
        }

        internal CarReceiptVM MakeRecipt(CarRentFormVM vM)
        {
            return new CarReceiptVM
            {
                CarName = context.Car.SingleOrDefault(c => c.Id == vM.CarId).Model,
                StartDate = vM.StartTime,
                EndDate = vM.EndTime,
                Price = context.Car.SingleOrDefault(c => c.Id == vM.CarId).Price,
                Total = ((vM.EndTime - vM.StartTime).Ticks / TimeSpan.TicksPerMinute) * (vM.Price / (60 * 24)),
            };
        }

        public bool CheckAvailability(Rent[] rents, DateTime start, DateTime end)
        {

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
