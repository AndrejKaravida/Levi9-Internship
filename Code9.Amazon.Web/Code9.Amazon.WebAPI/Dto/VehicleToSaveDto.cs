using Code9.Amazon.WebAPI.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Code9.Amazon.WebAPI.Dto
{
    public class VehicleToSaveDto
    {
        public int ModelId { get; set; }
        public bool IsRegistered { get; set; }
        public int Price { get; set; }
        public string City { get; set; }
        public int Mileage { get; set; }
        public int ProductionYear { get; set; }
        public int UserId { get; set; }
        public string FuelType { get; set; }
        public string Description { get; set; }
        public ICollection<int> Features { get; set; }

        public VehicleToSaveDto()
        {
            Features = new Collection<int>();
        }
    }
}
