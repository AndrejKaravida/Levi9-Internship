using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Code9.Amazon.WebAPI.Core.IService
{
    public interface IImageWriter
    {
        Task<string> UploadImage(IFormFile file);
    }
}
