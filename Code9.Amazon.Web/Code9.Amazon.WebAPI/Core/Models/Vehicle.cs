using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Code9.Amazon.WebAPI.Core.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public int ModelId { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public int Mileage { get; set; }
        public int ProductionYear { get; set; }
        public string FuelType { get; set; }
        public Model Model { get; set; }
        public bool IsRegistered { get; set;}
        public int UserId { get; set; }
        public DateTime LastUpdated { get; set; }
        public ICollection<VehicleFeature> Features { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Image> Images { get; set; }

        public Vehicle()
        {
            Features = new Collection<VehicleFeature>();
            Images = new Collection<Image>();
        }

    }
}
