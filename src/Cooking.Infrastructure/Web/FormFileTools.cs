using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Cooking.Infrastructure.Web
{
    public class FormFileTools
    {
        public async Task<byte[]> ToByteArrayAsync(IFormFile file)
        {
            byte[] fileStream;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                fileStream = memoryStream.ToArray();
                await memoryStream.FlushAsync();
            }

            return fileStream;
        }
    }
}