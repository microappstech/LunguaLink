using Langua.DataContext.Data;
using Langua.Models;
using Langua.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Radzen;
using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;


namespace Langua.Repositories.Services
{
    public partial class LanguaService
    {
        #region oldservice
        private IRepositoryCrudBase<Session> repositorySession;
        public LanguaService(IRepositoryCrudBase<Session> crudBase)
        {
            this.repositorySession = crudBase;
        }
        public async Task<IQueryable<Session>> GetAll()
        {
            var items = Context.Sessions.AsQueryable();
            return await Task.FromResult(items);
        }
        public async Task<Session> GetSessionId(int Id)
        {
            var item = Context.Sessions.AsQueryable().Where(i => i.Id == Id).FirstOrDefault();
            return await Task.FromResult(item);
        }
        public async Task<IQueryable<Session>> GetSessionByTeacher(int teacherId)
        {
            var items = Context.Sessions.AsQueryable().Where(i => i.TeacherId == teacherId);
            return await Task.FromResult(items);
        }
        public async Task<IQueryable<Session>> GetSessionByGroup(int GroupId)
        {
            var items = Context.Sessions.AsQueryable().Where(i => i.GroupId == GroupId);
            return await Task.FromResult(items);
        }
        #endregion


        

        public void ApplyQuery<T>(ref IQueryable<T> items, Query query = null)
        {
            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }
        }

        public async Task ExportSessionsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/languadb/sessions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/languadb/sessions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportSessionsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/languadb/sessions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/languadb/sessions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        

        public async Task<IQueryable<Models.Session>> GetSessions(Query query = null)
        {
            var items = Context.Sessions.AsQueryable();

            items = items.Include(i => i.Group);
            items = items.Include(i => i.Teacher);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach (var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }
            return await Task.FromResult(items);
        }

        partial void OnSessionGet(Models.Session item);
        partial void OnGetSessionById(ref IQueryable<Models.Session> items);


        public async Task<Models.Session> GetSessionById(int id)
        {
            var items = Context.Sessions
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            items = items.Include(i => i.Group);
            items = items.Include(i => i.Teacher);

            OnGetSessionById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnSessionGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnSessionCreated(Models.Session item);
        partial void OnAfterSessionCreated(Models.Session item);

        public async Task<Models.Session> CreateSession(Models.Session session)
        {
            OnSessionCreated(session);

            var existingItem = Context.Sessions
                              .Where(i => i.Id == session.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
                throw new Exception("Item already available");
            }

            try
            {
                Context.Sessions.Add(session);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(session).State = EntityState.Detached;
                throw;
            }

            OnAfterSessionCreated(session);

            return session;
        }

        public async Task<Models.Session> CancelSessionChanges(Models.Session item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
                entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
                entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnSessionUpdated(Models.Session item);
        partial void OnAfterSessionUpdated(Models.Session item);

        public async Task<Models.Session> UpdateSession(int id, Models.Session session)
        {
            OnSessionUpdated(session);

            var itemToUpdate = Context.Sessions
                              .Where(i => i.Id == session.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
                throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(session);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterSessionUpdated(session);

            return session;
        }

        partial void OnSessionDeleted(Models.Session item);
        partial void OnAfterSessionDeleted(Models.Session item);

        public async Task<Models.Session> DeleteSession(int id)
        {
            var itemToDelete = Context.Sessions
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
                throw new Exception("Item no longer available");
            }

            OnSessionDeleted(itemToDelete);


            Context.Sessions.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterSessionDeleted(itemToDelete);

            return itemToDelete;
        }

    }
}
