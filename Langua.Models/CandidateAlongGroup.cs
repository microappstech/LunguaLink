using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Langua.Models
{
    public class CandidateAlongGroup
    {
        public string? GroupName { get; set; }
        public int NbCans { get; set; }
    }
    public class SessionByGroup
    {
        public string? GroupName { get; set; }
        public int NbSessions { get;  set; }
    }
    public class TotalDureeSessions
    {
        public string? GroupName { get; set; }
        public float TotalInMenute { get; set; }
    }
}
