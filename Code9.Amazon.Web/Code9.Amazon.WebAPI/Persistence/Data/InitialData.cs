using Code9.Amazon.WebAPI.Core.Models;
using Code9.Amazon.WebAPI.Persistance.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Code9.Amazon.WebAPI.Persistence.Data
{
    public class InitialData
    {
        public static void Initialize(IApplicationBuilder app, UserManager<User> userManager)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<DataContext>();
                string startingPath = Startup.Configuration.GetSection("DefaultImageLocation").Value;

                context.Database.EnsureCreated();

                if (context.Models.Any())
                    return;

                var user = CreateUser();
                userManager.CreateAsync(user, "Password123").Wait();

                var makes = GetMakes().ToArray();
                context.Makes.AddRange(makes);
                context.SaveChanges();

                var models = GetModels(context).ToArray();
                context.Models.AddRange(models);
                context.SaveChanges();

                var features = GetFeatures().ToArray();
                context.Features.AddRange(features);
                context.SaveChanges();

                var vehicles = GetVehicles(context).ToArray();
                context.Vehicles.AddRange(vehicles);
                context.SaveChanges();

                var vehicleFeatures = GetVehicleFeatures(context).ToArray();
                context.VehicleFeatures.AddRange(vehicleFeatures);
                context.SaveChanges();

                var images = GetImages(context, startingPath).ToArray();
                context.Images.AddRange(images);
                context.SaveChanges();
            }


        }

        public static User CreateUser()
        {
            return new User
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "user@example.com",
                NormalizedEmail = "XXXX@EXAMPLE.COM",
                UserName = "JohnDoe",
                NormalizedUserName = "JOHNDOE",
                PhoneNumber = "+111111111111",
            };
        }

        public static List<Image> GetImages(DataContext db, string startingPath)
        {
            List<Image> images = new List<Image>();

            var vehicles = db.Vehicles.ToList();

            foreach (var v in vehicles)
            {
                var fileName = $"{startingPath}{v.Model.Make.Name.ToLower()}-{v.Model.Name.ToLower()}.jpg".Replace(" ", "");

                Image image = new Image
                {
                    VehicleId = v.Id,
                    FileName = fileName,
                    IsMain = true
                };

                images.Add(image);
            }

            var bmwPath1 = $"{startingPath}bmw_x6_inside.png";
            var bmwPath2 = $"{startingPath}bmw_x6_outside.jpg";
            var id = db.Vehicles.Include(m => m.Model).Where(x => x.Model.Name.ToLower() == "X6").FirstOrDefault().Id;

            Image image1 = new Image()
            {
                VehicleId = id,
                FileName = bmwPath1,
                IsMain = false
            };

            images.Add(image1);

            Image image2 = new Image()
            {
                VehicleId = id,
                FileName = bmwPath2,
                IsMain = false
            };

            images.Add(image2);

            return images;
        }

        public static List<Vehicle> GetVehicles(DataContext db)
        {
            Random r = new Random();
            List<Vehicle> vehicles = new List<Vehicle>();
            List<string> fuelTypes = new List<string>()
            {
                "Benzin", "Dizel", "Metan", "Gas", "Električno"
            };

            List<string> cities = new List<string>()
            {
                "Novi Sad", "Beograd", "Kragujevac", "Kraljevo", "Niš", "Sombor"
            };

            for (int i = 1; i < 25; i++)
            {
                Vehicle vehicle = new Vehicle()
                {
                    Images = new List<Image>(),
                    Comments = new List<Comment>(),
                    Features = new List<VehicleFeature>(),
                    Price = r.Next(20, 200) * 100,
                    Mileage = r.Next(10, 300) * 100,
                    ProductionYear = r.Next(2010, 2020),
                    FuelType = fuelTypes.Skip(r.Next(0, 5)).Take(1).FirstOrDefault(),
                    City = cities.Skip(r.Next(0, 6)).Take(1).FirstOrDefault(),
                    LastUpdated = DateTime.Now,
                    Description = "Top stanje, bez ulaganja. Uradjen mali servis pre oko 2000km, sedi i vozi :)",
                    IsRegistered = true,
                    UserId = 1,
                    ModelId = db.Models.Where(x => x.Id == i).FirstOrDefault().Id
                };
                vehicles.Add(vehicle);
            };

            return vehicles;
        }

        public static List<VehicleFeature> GetVehicleFeatures(DataContext db)
        {
            Random r = new Random();

            List<VehicleFeature> vehicleFeatures = new List<VehicleFeature>();

            for (int i = 1; i < 15; i++)
            {
                for (int j = 1; j < 25; j++)
                {
                    var vehicleFeature = new VehicleFeature();
                    vehicleFeature.VehicleId = i;
                    vehicleFeature.FeatureId = j;

                    j += r.Next(0, 3);

                    vehicleFeatures.Add(vehicleFeature);
                }
            }
            return vehicleFeatures;
        }

        public static List<Model> GetModels(DataContext db) => new List<Model>()
        {
            new Model {Name = "Giulia", Make = db.Makes.Where(x => x.Name == "Alfa-Romeo").FirstOrDefault()},
            new Model {Name = "A8", Make = db.Makes.Where(x => x.Name == "Audi").FirstOrDefault()},
            new Model {Name = "Continental GT", Make = db.Makes.Where(x => x.Name == "Bentley").FirstOrDefault()},
            new Model {Name = "X6", Make = db.Makes.Where(x => x.Name == "BMW").FirstOrDefault()},
            new Model {Name = "Chiron", Make = db.Makes.Where(x => x.Name == "Bugatti").FirstOrDefault()},
            new Model {Name = "Cruze", Make = db.Makes.Where(x => x.Name == "Chevrolet").FirstOrDefault()},
            new Model {Name = "C4", Make = db.Makes.Where(x => x.Name == "Citroen").FirstOrDefault()},
            new Model {Name = "488 GTB", Make = db.Makes.Where(x => x.Name == "Ferrari").FirstOrDefault()},
            new Model {Name = "Punto", Make = db.Makes.Where(x => x.Name == "Fiat").FirstOrDefault()},
            new Model {Name = "Focus", Make = db.Makes.Where(x => x.Name == "Ford").FirstOrDefault()},
            new Model {Name = "Accord", Make = db.Makes.Where(x => x.Name == "Honda").FirstOrDefault()},
            new Model {Name = "Accent", Make = db.Makes.Where(x => x.Name == "Hyundai").FirstOrDefault()},
            new Model {Name = "Wrangler", Make = db.Makes.Where(x => x.Name == "Jeep").FirstOrDefault()},
            new Model {Name = "Rio", Make = db.Makes.Where(x => x.Name == "Kia").FirstOrDefault()},
            new Model {Name = "Urus", Make = db.Makes.Where(x => x.Name == "Lamborghini").FirstOrDefault()},
            new Model {Name = "GranTurismo", Make = db.Makes.Where(x => x.Name == "Maserati").FirstOrDefault()},
            new Model {Name = "S600", Make = db.Makes.Where(x => x.Name == "Maybach").FirstOrDefault()},
            new Model {Name = "3", Make = db.Makes.Where(x => x.Name == "Mazda").FirstOrDefault()},
            new Model {Name = "E-class", Make = db.Makes.Where(x => x.Name == "Mercedes").FirstOrDefault()},
            new Model {Name = "Micra", Make = db.Makes.Where(x => x.Name == "Nissan").FirstOrDefault()},
            new Model {Name = "Insignia", Make = db.Makes.Where(x => x.Name == "Opel").FirstOrDefault()},
            new Model {Name = "508", Make = db.Makes.Where(x => x.Name == "Peugeot").FirstOrDefault()},
            new Model {Name = "Macan", Make = db.Makes.Where(x => x.Name == "Porsche").FirstOrDefault()},
            new Model {Name = "SuperB", Make = db.Makes.Where(x => x.Name == "Skoda").FirstOrDefault()},
            new Model {Name = "Model S", Make = db.Makes.Where(x => x.Name == "Tesla").FirstOrDefault()},
            new Model {Name = "Tiguan", Make = db.Makes.Where(x => x.Name == "Volkswagen").FirstOrDefault()}
        };
     
        public static List<Feature> GetFeatures() => new List<Feature>() {
             new Feature {Name = "Metalik boja"},
             new Feature {Name = "Multifunkcionalni volan"},
             new Feature {Name = "Putni računar"},
             new Feature {Name = "Tonirana stakla"},
             new Feature {Name = "Grejači retrovizora"},
             new Feature {Name = "Asistencija praćenja trake"},
             new Feature {Name = "Senzor mrtvog ugla"},
             new Feature {Name = "Ulazak bez ključa"},
             new Feature {Name = "Parking senzori"},
             new Feature {Name = "Grejači vetrobranskog stakla"},
             new Feature {Name = "Elektro podesiva sedišta"},
             new Feature {Name = "Daljinsko zaključavanje"},
             new Feature {Name = "Masažna sedišta"},
             new Feature {Name = "Memorija sedišta"},
             new Feature {Name = "Automatsko parkiranje"},
             new Feature {Name = "LED zadnja svetla"},
             new Feature {Name = "Sedišta podesiva po visini"},
             new Feature {Name = "Tempomat"},
             new Feature {Name = "Vazdušno vešanje"},
             new Feature {Name = "Zavesice na zadnjim prozorima"},
             new Feature {Name = "Upravljanje na sva četiri točka"},
             new Feature {Name = "Elektro zatvaranje prtljažnika"},
             new Feature {Name = "Prednja noćna kamera"},
             new Feature {Name = "Navigacija"},
             new Feature {Name = "Xenon svetla"},
             new Feature {Name = "Branici u boji auta"}
        };

        public static List<Make> GetMakes() => new List<Make>()
        {
             new Make {Name = "Alfa-Romeo"},
             new Make {Name = "Audi"},
             new Make {Name = "Bentley"},
             new Make {Name = "BMW"},
             new Make {Name = "Bugatti"},
             new Make {Name = "Chevrolet"},
             new Make {Name = "Citroen"},
             new Make {Name = "Ferrari"},
             new Make {Name = "Fiat"},
             new Make {Name = "Ford"},
             new Make {Name = "Honda"},
             new Make {Name = "Hyundai"},
             new Make {Name = "Jeep"},
             new Make {Name = "Kia"},
             new Make {Name = "Lamborghini"},
             new Make {Name = "Maserati"},
             new Make {Name = "Maybach"},
             new Make {Name = "Mazda"},
             new Make {Name = "Mercedes"},
             new Make {Name = "Nissan"},
             new Make {Name = "Opel"},
             new Make {Name = "Peugeot"},
             new Make {Name = "Porsche"},
             new Make {Name = "Skoda"},
             new Make {Name = "Tesla"},
             new Make {Name = "Volkswagen"}
        };
     
    }
}
