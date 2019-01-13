using CarRent.Models.Entities;
using CarRent.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using GeoAPI.Geometries;
using System.Net.Http;
using Newtonsoft.Json;
using NetTopologySuite.Geometries;

namespace CarRent.Models
{
    public class CarServices
    {
        CarRentContext context;
        IHostingEnvironment he;
        UserManager<MyIdentityUser> userManager;

        public CarServices(CarRentContext context, IHostingEnvironment he, UserManager<MyIdentityUser> userManager)
        {
            this.context = context;
            this.he = he;
            this.userManager = userManager;
        }

        public CarDetailsVM FindCarByID(int ID)
        {
            var carr = context.Car
             .Where(c => c.Id == ID)
            .Select(d => new CarDetailsVM()
            {
                car = new CarVM()
                {
                    Ac = d.Ac,
                    ChildSeat = d.ChildSeat,
                    Description = d.Description,
                    Doors = d.Doors,
                    Fuel = d.Fuel,
                    Gear = d.Gear,
                    ImgUrl = d.ImgUrl,
                    Km = d.Km,
                    Model = d.Model,
                    Pets = d.Pets,
                    Price = d.Price,
                    RoofRack = d.RoofRack,
                    Seats = d.Seats,
                    TowBar = d.TowBar,
                    Type = d.Type
                },
                reviews = context.Rent
                    .Where(o => o.Car.Id == ID)
                    .SelectMany(o => o.Review.Select(r => r))
                    .Select(o => new ReviewCarDetailsVM()
                    {
                        DateCreated = o.DateCreated,
                        Rating = o.Rating,
                        Review = o.Review1,
                        UserName = GetContactByID(o.Rent.CustomerId)

                    }).ToList(),
                form = new CarRentFormVM()
                {
                    CarId = ID,
                    Price = d.Price
                }
            }).FirstOrDefault();

            return carr;
        }

        internal async Task AddRent(CarRentFormVM vM, string userId)
        {
            await context.Rent.AddAsync(new Rent { CarId = vM.CarId, Datestart = vM.StartTime, DateEnd = vM.EndTime, CustomerId = userId});

            await context.SaveChangesAsync();
        }

        public async Task AddCarToDatabase(CarRegistrationPostVM vm, string userId)
        {
            if (vm.Image.Count > 0)
                UploadImages(vm);

            var coordinate = await GetCoordinates(vm.City);
            var point = new Point(coordinate);
            point.SRID = 4326;


            var car = new Car
            {
                OwnerId = userId,
                Model = vm.Model,
                Km = vm.Km,
                Ac = vm.Ac,
                ChildSeat = vm.ChildSeat,
                Gear = vm.Gear,
                Doors = vm.Doors,
                Description = vm.Description,
                Fuel = vm.Fuel,
                Pets = vm.Pets,
                Price = vm.Price,
                RoofRack = vm.RoofRack,
                YearModel = vm.YearModel,
                Seats = vm.Seats,
                TowBar = vm.TowBar,
                Type = vm.Type,
                ImgUrl = vm.Image[0].FileName,
                GeoLocation = point
            };

            context.Car.Add(car);

            context.SaveChanges();

            foreach (var item in vm.Image)
            {
                context.CarImage.Add(new CarImage
                {
                    CarId = car.Id,
                    ImgUrl = item.FileName,
                    
                });
            }

            context.SaveChanges();
        }

        public void UploadImages(CarRegistrationPostVM viewModel)
        {
            foreach (var item in viewModel.Image)
            {
                var fileName = Path.Combine(he.WebRootPath, Path.GetFileName(item.FileName));
                item.CopyTo(new FileStream(fileName, FileMode.Create));
            }

        }

        public string GetContactByID(string ID)
        {
            string constring = "Data Source=carrentacademy.database.windows.net;Initial Catalog=CarRentDb;User ID=adameasten;Password=Pennskrin1;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            string queryString =
            "SELECT * from dbo.aspnetusers "
                + "WHERE ID = @ID ";
            try
            {
                using (SqlConnection con = new SqlConnection(constring))
                {

                    using (SqlCommand command = new SqlCommand(queryString, con))
                    {
                        command.Parameters.AddWithValue("@ID", ID);
                        con.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        reader.Read();

                        var name = reader.GetString(1);

                        reader.Close();

                        return name;
                    }
                }
            }
            catch
            {
                throw new Exception();
            }

        }

        public async Task<Coordinate> GetCoordinates(string city)
        {
            var apiString = $"https://maps.googleapis.com/maps/api/geocode/json?address={city}&key=AIzaSyDqQCALQLs6NM9tMpHUWlC2uLNh5Eniz3I";
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
