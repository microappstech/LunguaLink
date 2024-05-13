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
        }

    }
}
