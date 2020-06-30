using Code9.Amazon.WebAPI.Core.IRepository;
using Code9.Amazon.WebAPI.Core.Models;
using Code9.Amazon.WebAPI.Persistance.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code9.Amazon.WebAPI.Persistence
{
    public class ImageRepository : IImageRepository
    {
        private readonly DataContext _context;

        public ImageRepository(DataContext context)
        {
            _context = context;
        }

        public void Add(Image obj)
        {
            _context.Images.Add(obj);
        }

        public void Delete(Image obj)
        {
            _context.Images.Remove(obj);
        }

        public async Task<Image> GetByIdAsync(int id)
        {
            return await _context.Images.FindAsync(id);
        }

        public async Task<Image> GetMainImageForVehicle(int vehicleId)
        {
            return await _context.Images.Where(x => x.VehicleId == vehicleId).FirstOrDefaultAsync(ph => ph.IsMain);
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
