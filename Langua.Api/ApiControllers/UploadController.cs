using Langua.DataContext.Data;
using Langua.Models;
using Langua.Shared.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langua.ApiControllers.ApiControllers
{
    [Route("api/Upload")]
    public class UploadController : ControllerBase
    {
        public static byte[] bytes;
        private LanguaContext _context;
        public UploadController(LanguaContext languaContext)
        {
            _context = languaContext;
        }

        [HttpPost]
        public async Task<Result<byte[]>> Upload(IFormFile file)
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
                return await Task.FromResult(new Result<byte[]>(true, ResultBytes));
    }
            catch (Exception ex)
            {
                return await Task.FromResult(new Result<byte[]>(false, null));
            }
        }
        [HttpGet]
        public async Task<Result<byte[]>> GetBytesFile()
        {
            byte[] fileBytes = bytes; //
            return await Task.FromResult(new Result<byte[]>(true, fileBytes));
        }
    }
}
