using Langua.DataContext.Data;
using Langua.Models;
using Langua.Repositories.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langua.Repositories.Services
{
    public partial class LanguaService
    {
        private readonly LanguaContext _context;
        private readonly NavigationManager navigationManager;
        private readonly Uri baseUri;

        private IRepositoryCrudBase<Session> repositorySession;
        public LanguaService(LanguaContext languacontext, NavigationManager navigation,Uri uri, IRepositoryCrudBase<Session> crudBase)
        {
            _context = languacontext;
            navigationManager = navigation;
            baseUri = uri;
            this.repositorySession = crudBase;
        }


        public LanguaContext Context { get { return _context; } }
        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);

    }
}
