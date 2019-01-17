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
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage;

namespace CarRent.Models
{
    public class CarServices
    {
        CarRentContext context;
        IHostingEnvironment he;
        UserManager<MyIdentityUser> userManager;
        private readonly IConfiguration configuration;


        public CarServices(CarRentContext context, IHostingEnvironment he, UserManager<MyIdentityUser> userManager, IConfiguration config)
        {
            this.context = context;
            this.he = he;
            this.userManager = userManager;
            this.configuration = config;
        }

        public async Task<CarDetailsVM> FindCarByIDAsync(int ID)
        {
            var carr = context.Car
             .Where (c => c.Id == ID)
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
                    Type = d.Type,
                    ImgUrlArr = d.CarImage 
                    .Select(i => i.ImgUrl).ToList()
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

            //carr.car.ImgUrlArr = await GetImageUrl(carr.car.ImgUrlArr);
            carr.car.ImgUrl = carr.car.ImgUrlArr[0];
            return carr;
        }

        private async Task<List<string>> GetImageUrl(List<string> list)
        {
            var imageUrls = new List<string>();

            foreach (var item in list)
            {
                imageUrls.Add(await GetThumbNailUrls(item));
            }
            return imageUrls;
        }

        public CarSearchFilterVM CreateFilterVm(CarSearchVM[] result)
        {
            return new CarSearchFilterVM
            {
                SearchResult = result,
                SearchResultJson = JsonConvert.SerializeObject(result),
                TypeItems = new SelectListItem[]
                {
                       new SelectListItem{Value = "Alla", Text = "Alla"},
                       new SelectListItem{Value = "Sedan", Text = "Sedan"},
                       new SelectListItem{Value = "Kombi", Text = "Kombi"},
                       new SelectListItem{Value = "SUV", Text = "SUV"},
                       new SelectListItem{Value = "Halvkombi", Text = "Halvkombi"},
                       new SelectListItem{Value = "Sportkupé", Text = "Sportkupé"},
                       new SelectListItem{Value = "Cab", Text = "Cab"},
                       new SelectListItem{Value = "Pickup", Text = "Pickup"},
                       new SelectListItem{Value = "Minibuss", Text = "Minibuss"},
                       new SelectListItem{Value = "Husbil", Text = "Husbil"}
                },

                GearItems = new SelectListItem[]
                {
                       new SelectListItem{Value = "Alla", Text = "Alla"},

                    new SelectListItem{Value = "Automat", Text = "Automat"},
                    new SelectListItem{Value = "Manuell", Text = "Manuell"},
                },

                FuelItems = new SelectListItem[]
                {
                       new SelectListItem{Value = "Alla", Text = "Alla"},

                    new SelectListItem{Value = "Bensin", Text = "Bensin"},
                    new SelectListItem{Value = "Diesel", Text = "Diesel"},
                    new SelectListItem{Value = "Etanol", Text = "Etanol"},
                    new SelectListItem{Value = "El", Text = "El"},
                },

                SeatsItem = new SelectListItem[]
                {

                    new SelectListItem{Value = "7", Text = "7+"},
                    new SelectListItem{Value = "6", Text = "6"},
                    new SelectListItem{Value = "5", Text = "5"},
                    new SelectListItem{Value = "4", Text = "4"},
                    new SelectListItem{Value = "3", Text = "3"},
                    new SelectListItem{Value = "2", Text = "2"},
                    new SelectListItem{Value = "1", Text = "1"}
                },

                DoorsItem = new SelectListItem[]
                {
                    new SelectListItem{Value = "6", Text = "6"},
                    new SelectListItem{Value = "5", Text = "5"},
                    new SelectListItem{Value = "4", Text = "4"},
                    new SelectListItem{Value = "3", Text = "3"},
                    new SelectListItem{Value = "2", Text = "2"},
                    new SelectListItem{Value = "1", Text = "1"}
                },
            };
        }

        internal async Task AddRent(CarRentConfirmVM vM, string userId)
        {
            await context.Rent.AddAsync(new Rent { CarId = vM.CarId, Datestart = vM.StartTime, DateEnd = vM.EndTime, CustomerId = userId });

            await context.SaveChangesAsync();
        }

        public async Task AddCarToDatabase(CarRegistrationPostVM vm, string userId)
        {
            var coordinate = await GetCoordinates($"{vm.Street},{vm.City}");
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
                GeoLocation = point,
            };

            context.Car.Add(car);

            if (vm.Image?.Count > 0)
            {
                var result = await UploadFileToStorage(vm, car.Id);
            }
            else
            {
                context.CarImage.Add(new CarImage { ImgUrl = "https://carrentimages.blob.core.windows.net/thumbnails/Logo.png", CarId = car.Id });


            }




            context.SaveChanges();
        }

        //public void UploadImages(CarRegistrationPostVM viewModel)
        //{
        //    foreach (var item in viewModel.Image)
        //    {
        //        var fileName = Path.Combine(he.WebRootPath, Path.GetFileName(item.FileName));
        //        item.CopyTo(new FileStream(fileName, FileMode.Create));
        //    }
        //}

        public async Task<bool> UploadFileToStorage(CarRegistrationPostVM vM, int id)
        {
            StorageCredentials storageCredentials = new StorageCredentials(configuration["AzureAccountName"], configuration["AzureAccountKey"]);
            CloudStorageAccount storageAccount = new CloudStorageAccount(storageCredentials, true);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("thumbnails");

            foreach (var item in vM.Image)
            {
                if (IsImage(item))
                {
                    if(item.Length > 0)
                    {
                        using (Stream stream = item.OpenReadStream())
                        {
                            var fileName = Guid.NewGuid().ToString() + item.FileName;

                            context.CarImage.Add(new CarImage { ImgUrl = "https://carrentimages.blob.core.windows.net/thumbnails/"+fileName, CarId = id });
                            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);
                            await blockBlob.UploadFromStreamAsync(stream);
                        }
                    }
                }
            }

            return await Task.FromResult(true);
        }

        public bool IsImage(IFormFile file)
        {
            if (file.ContentType.Contains("image"))
            {
                return true;
            }

            string[] formats = new string[] { ".jpg", ".png", ".gif", ".jpeg" };

            return formats.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<string> GetThumbNailUrls(string url)
        {
            string imageUrl = "";

            StorageCredentials storageCredentials = new StorageCredentials(configuration["AzureAccountName"], configuration["AzureAccountKey"]);

            CloudStorageAccount storageAccount = new CloudStorageAccount(storageCredentials, true);

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference("thumbnails");

            BlobContinuationToken continuationToken = null;

            BlobResultSegment resultSegment = null;

            //Call ListBlobsSegmentedAsync and enumerate the result segment returned, while the continuation token is non-null.
            //When the continuation token is null, the last page has been returned and execution can exit the loop.
            do
            {
                //This overload allows control of the page size. You can return all remaining results by passing null for the maxResults parameter,
                //or by calling a different overload.
                resultSegment = await container.ListBlobsSegmentedAsync(url, true, BlobListingDetails.All, 10, continuationToken, null, null);

                foreach (var blobItem in resultSegment.Results)
                {
                    imageUrl = blobItem.StorageUri.PrimaryUri.ToString();
                }

                //Get the continuation token.
                continuationToken = resultSegment.ContinuationToken;
            }

            while (continuationToken != null);
            return imageUrl;
            //return await Task.FromResult(imageUrl);
        }


        public string GetContactByID(string ID) 
        {

            string constring = configuration["DefaultConnection"];
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
