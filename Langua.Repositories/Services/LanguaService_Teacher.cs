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
            //GrTeachers = GrTeachers.Include(i => i.Group);
            //GrTeachers = GrTeachers.Include(i => i.Teacher);
            if (GrTeachers is null)
                return await Task.FromResult(new Result<IQueryable<GroupTeacher>>(false, null));
            return await Task.FromResult(new Result<IQueryable<GroupTeacher>>(true, GrTeachers));
        }
        public async Task<Result<IQueryable<Groups>>> Groups(List<int> groupIds)
        {
            //var res = Context.Groups.Where(c => groupIds.Contains((int)c.Id)).AsQueryable();
            
            //if (res is null)
            //    return await Task.FromResult(new Result<IQueryable<Candidat>>(false, null));

            return await Task.FromResult(new Result<IQueryable<Groups>>(true, null));
        }



    }
}
