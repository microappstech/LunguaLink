using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langua.DAL.Sp
{
    public static class SqlProcedure
    {
        public static Hashtable sp = new Hashtable();
        static SqlProcedure()
        {
            sp["LoadCandidateGroup"] = """
                SELECT 
                gr.Id AS Id,
                gr.Name AS Name,
                gr.Description AS Description
                FROM Groups gr INNER JOIN  GroupCandidates gc ON gr.Id = gc.GroupId
                INNER JOIN Candidates c ON c.Id = gc.CandidatId WHERE c.UserId = @UserId
                """;
            sp["CandidateAlongDepartment"] = """
                select Dep.Name as DaprtementName , count(Can.Id) as NbCans  from Candidates Can inner join Departments Dep 
                	On Dep.Id = Can.DepartementId
                group by  dep.Name
                """;
            sp["CandidateAlongGroup"] = """    
                select Gr.Name as GroupName, Count(Can.Id) NbCans from Groups Gr Left Outer join Candidates Can ON Gr.Id = Can.GroupId group by Gr.Name;
                """;
            sp["CandidateAlongGroupByDep"] = """    
                SELECT Gr.Name AS GroupName, COUNT(Can.Id) AS NbCans
                FROM   Groups AS Gr LEFT OUTER JOIN
                             Candidates AS Can ON Gr.Id = Can.GroupId
                Where Gr.DepartmentId = @Depart
                GROUP BY Gr.Name 
                """;
            sp["NbrSessionsForGroup"] = """
                SELECT Gr.Name AS GroupName, COUNT(S.Id) AS NbSessions
                    FROM   Groups AS Gr LEFT OUTER JOIN
                                    Sessions AS S ON Gr.Id = S.GroupId
                    group by Gr.Name;
                """;
            sp["NbrSessionsForGroupByDep"] = """
                SELECT Gr.Name AS GroupName, COUNT(S.Id) AS NbSessions FROM   Groups AS Gr LEFT OUTER JOIN
                    Sessions AS S ON Gr.Id = S.GroupId 
                where Gr.DepartmentId = @Depart group by Gr.Name
                """;
            sp["TotalSesOfGroupInMenute"] = """
                                
                SELECT Sum(DATEDIFF(Minute ,[Start],[End])) as TotalInMenute, Gr.Name
                FROM   Groups AS Gr LEFT OUTER JOIN
                             Sessions AS S ON Gr.Id = S.GroupId 

                group by Gr.Name
                """;
            sp["TotalSesOfGroupInMenuteByDep"] = """                                
                SELECT Sum(DATEDIFF(Minute ,[Start],[End])) as TotalInMenute, Gr.Name
                FROM   Groups AS Gr LEFT OUTER JOIN
                             Sessions AS S ON Gr.Id = S.GroupId 
                             where Gr.DepartmentId=@Depart
                group by Gr.Name
                """;
        }

    }
}
