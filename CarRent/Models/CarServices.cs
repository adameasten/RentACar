using CarRent.Models.Entities;
using CarRent.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CarRent.Models
{
    public class CarServices
    {
        CarRentContext context;
        public CarServices(CarRentContext context)
        {
            this.context = context;
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
                    .Select(o => new ReviewCarDetailsVM() {
                        DateCreated = o.DateCreated,
                        Rating = o.Rating,
                        Review = o.Review1,
                        UserName = GetContactByID(o.Rent.CustomerId)

                    }).ToList(),
                form = new CarRentFormVM()
                {
                    Price = d.Price
                }
            }).FirstOrDefault();

            return carr;
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
    }
}
