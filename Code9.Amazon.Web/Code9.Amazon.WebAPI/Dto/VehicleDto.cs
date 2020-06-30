using Code9.Amazon.WebAPI.Core.Models;
using System;
using System.Collections.Generic;

namespace Code9.Amazon.WebAPI.Dto
{
    public class VehicleDto
    {
        public int Id { get; set; }
        public KeyValuePairDto Model { get; set; }
        public KeyValuePairDto Make { get; set; }
        public string ModelName { get; set; }
        public bool IsRegistered { get; set; }
        public int Price { get; set; }
        public string City { get; set; }
        public int Mileage { get; set; }
        public int ProductionYear { get; set; }
        public string FuelType { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public DateTime LastUpdated { get; set; }
        public ICollection<KeyValuePairDto> Features { get; set; }
        public ICollection<CommentDto> Comments { get; set; }
        public ICollection<ImageDto> Images { get; set; }
    }
}
