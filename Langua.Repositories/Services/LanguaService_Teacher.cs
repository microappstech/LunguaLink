using Langua.Account;
using Langua.Models;
using Langua.Shared.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langua.Repositories.Services
{
    public partial class LanguaService
    {
        public async Task<Result<Teacher>> GetTeacherById(string userId)
        {
            Teacher teacher = Context.Teachers.Where(i => i.UserId == userId).FirstOrDefault();
            if(teacher is not null)
                return new Result<Teacher>(true, teacher, "");
            return await Task.FromResult(new Result<Teacher>(false, null, "No Teacher Exist"));
        }
        public async Task<Result<IQueryable<GroupTeacher>>> GetGroupByTeacher(int teacherid)
        {
            var GrTeachers = Context.GroupTeachers.Where(i => i.TeacherId == teacherid).AsQueryable().Include(i => i.Teacher).Include(i => i.Group).AsNoTracking();
            GrTeachers = GrTeachers.Include(i => i.Group).ThenInclude(i=>i.Candidats);
            //GrTeachers = GrTeachers.Include(i => i.Teacher);
            if (GrTeachers is null)
                return await Task.FromResult(new Result<IQueryable<GroupTeacher>>(false, null));
            return await Task.FromResult(new Result<IQueryable<GroupTeacher>>(true, GrTeachers));
        }
        public async Task<Result<IQueryable<GroupTeacher>>> GetGroupsForTeacher(int TeacherId)
        {
            var res = Context.GroupTeachers.Where(i => i.TeacherId == TeacherId)
                .Include(i=>i.Group)
                .AsQueryable();
            
            if (res is null)
                return await Task.FromResult(new Result<IQueryable<GroupTeacher>>(false, null));

            return await Task.FromResult(new Result<IQueryable<GroupTeacher>>(true, res));
        }

        public async Task<Result<IQueryable<Teacher>>> GetTeachers(string includes = "")
        {
            var items = Context.Teachers.AsQueryable();
            if (!string.IsNullOrEmpty(includes))
                foreach (var inc in includes.Split(","))
                {
                    items = items.Include(inc);
                }
            if (!items.Any())
                return await Task.FromResult(new Result<IQueryable<Teacher>>(false, null));
            if (SecurityService.IsAdmin)
                return await Task.FromResult(new Result<IQueryable<Teacher>>(true, items));
            var manager = Context.Managers.Where(i => i.UserId == security.User.Id).FirstOrDefault();
            if (manager == null)
                return await Task.FromResult(new Result<IQueryable<Teacher>>(false, null));
            items = items.Where(i=>i.DepartementId == manager.DepartmentId);
            return await Task
                .FromResult(new Result<IQueryable<Teacher>>(true, items));
        }

        public async Task<Result<IQueryable<GroupTeacher>>> GetGroupTeachers(string includes = "")
        {
            try
            {
                var items = Context.GroupTeachers.AsQueryable();
                if (!string.IsNullOrEmpty(includes))
                    foreach (var inc in includes.Split(","))
                    {
                        items = items.Include(inc);
                    }
                if (SecurityService.IsAdmin)
                    return await Task.FromResult(new Result<IQueryable<GroupTeacher>>(true, items));
                var manager = Context
                    .Managers
                    .Where(i=>i.UserId == security.User.Id).FirstOrDefault();
                if (manager == null)
                    return new Result<IQueryable<GroupTeacher>>(false, null);
                var teacherIds = Context.Teachers.Where(t => t.DepartementId == manager.DepartmentId)?.Select(i => i.Id)?.ToList();
                var groupIds = Context.Groups.Where(i => i.DepartmentId == manager.DepartmentId)?.Select(i => i.Id)?.ToList();
                items = items.Where(i=> teacherIds.Contains(i.TeacherId)==true || groupIds.Contains(i.GroupId)).AsQueryable();
                return await Task.FromResult(new Result<IQueryable<GroupTeacher>>(true, items));

            }
            catch(Exception ex)
            {
                return new Result<IQueryable<GroupTeacher>>(false, null, $"Ex : {ex.Message}, InnerEx : {ex.InnerException.Message}, Stack : {ex.StackTrace}");
            }
        }


    }
}
