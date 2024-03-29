﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Langua.DataContext.Data;

namespace Langua.Repositories.Services
{
    public class BaseService
    {
        private readonly LanguaContext _context;
        public BaseService(LanguaContext context)
        {
            _context = context;
        }
        public IQueryable Apply<T>(IQueryable<T> items, IQueryCollection query = null) where T : class
        {
            if (query is not null)
            {
                if (query.ContainsKey("include"))
                {
                    var includes = query["include"].ToString().Split(",");
                    foreach (var include in includes)
                    {
                        items = items.Include(include);
                    }
                }
                var filter = query.ContainsKey("where") ? query["where"].ToString() : null;
                if (!string.IsNullOrWhiteSpace(filter))
                {
                    items = items.Where(filter);
                }

            }
            return items;
        }

        public async Task<int> NBItems<T>() where T : class
        {
            var count = await _context.Set<T>().CountAsync();
            return count;
        }
    }
}
