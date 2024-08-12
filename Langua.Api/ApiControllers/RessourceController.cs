using Langua.Account;
using Langua.DataContext.Data;
using Langua.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langua.Api.ApiControllers
{

    [Route("api")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RessourceController : ControllerBase
    {

        private LanguaContext context;
        private SecurityService security;

        public RessourceController(LanguaContext context, SecurityService security)
        {
            this.context = context;
            this.security = security;
        }
        [HttpGet("Contenus")]
        public async Task<ApiResponse<List<ContenuResponse>>> GetRessources(int idgroup)
        {
            try
            {
                var items = context.GroupRessources
               .Where(i => i.GroupId == idgroup)
               .Include(i => i.Ressource)
             /*  .Select(i => i.Ressource)*/.ToList();
                var ItemsToResponse = items.Select(i => new ContenuResponse
                {
                   Id = i.Ressource.Id,
                    Name = i.Ressource.Name,
                    ContentBytes = i.Ressource.ContentBytes,
                    ContentFile = i.Ressource.ContentFile,
                    RessourceType = i.Ressource.RessourceType,
                    Url = i.Ressource.Url,
                    CreatedAt = i.PublishedAt

                }).ToList();
                return await Task.FromResult(new ApiResponse<List<ContenuResponse>>() { Success = true, Data = ItemsToResponse });
            }
            catch(Exception ex)
            {
                return await Task.FromResult(new ApiResponse<List<ContenuResponse>>() { Success = true, Data = null, Message = ex.Message });
            }
        }
        /// <summary>
        /// /////
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("ContenuDetail")]
        public async Task<ApiResponse<ContenuResponse>> GetContenuDetail(int id)
        {
            try
            {
                var i = context.Ressources.Where(r => r.Id == id).FirstOrDefault();
                if (i == null)
                    return await Task.FromResult(new ApiResponse<ContenuResponse>() { Success = true, Data = null!, Message = "Not exist" });


                var ItemsToResponse =  new ContenuResponse
                {
                    Name = i.Name,
                    ContentBytes = i.ContentBytes,
                    ContentFile = i.ContentFile,
                    RessourceType = i.RessourceType,
                    Url = i.Url

                };
                return await Task.FromResult(new ApiResponse<ContenuResponse>() { Success = true, Data = ItemsToResponse });
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ApiResponse<ContenuResponse>() { Success = true, Data = null!, Message = ex.Message });
            }
        }

    }
}
