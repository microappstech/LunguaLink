using Langua.DataContext.Data;
using Langua.Models;
using Langua.Shared.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text; 
using System.Threading.Tasks;

namespace Langua.ApiControllers.ApiControllers
{
    [Route("api/Upload")]
    public class UploadController : ControllerBase
    {
        public static byte[] bytes;
        public static string Base64;
        private LanguaContext _context;
        public UploadController(LanguaContext languaContext)
        {
            _context = languaContext;
        }

        [HttpPost]
        public async Task<byte[]> Upload(IFormFile file)
        {
            byte[] ResultBytes;
            try
            {
                using (var ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    ResultBytes = ms.ToArray();
                    bytes = ResultBytes;
                }
                Base64 = Convert.ToBase64String(ResultBytes,0,ResultBytes.Length);
                return await Task.FromResult(ResultBytes);
    }
            catch (Exception ex)
            {
                throw ;
            }
        }
        [HttpGet]
        public async Task<byte[]> GetBytesFile()
        {
            byte[] fileBytes = bytes; //
            return await Task.FromResult(fileBytes);
        }

        [HttpGet("Base64")]
        public async Task<string> GetBase64File()
        {
            byte[] fileBytes = bytes; //
            return await Task.FromResult(Base64);
        }
    }
}
