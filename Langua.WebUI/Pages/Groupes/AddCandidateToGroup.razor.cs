using Langua.Models;

namespace Langua.WebUI.Pages.Groupes
{
    public partial class AddCandidateToGroupComponent:BasePage
    {
        public Groups groupe { get; set; }
        public IEnumerable<Candidat> Candidates { get; set; }

    }
}