
using System;
using Langua.Models;
using Langua.WebUI.Pages.Components;
//using static Langua.WebUI.Pages.Components.LanguaGrid<dynamic>;


namespace Langua.WebUI.Pages.Candidates
{

    public partial class Candidates //: BasePage
    {
        public List<GridColumn<Candidat>> columns { get; set; }


        public List<Candidat> people = new List<Candidat>
        {
            new Candidat { FullName ="Hamza mouddakir", Email="HamzaMoudddakur@gmail.com", Phone ="063751556656", UserId="1546", CreatedAt = DateTime.Now },
            new Candidat { FullName ="Anas Omari", Email="AnasaOmari@gmail.com", Phone ="000000000", UserId="1546", CreatedAt = DateTime.Now },
            new Candidat { FullName ="Maher Hilali", Email="maherhl@gmail.com", Phone ="123456", UserId="5555", CreatedAt = DateTime.Now }
        };



        protected override async Task OnInitializedAsync()
        {
            columns = new List<GridColumn<Candidat>> {
                new GridColumn<Candidat> { Header = "Full Name" , Template = people => people.FullName, Property="FullName" },
                new GridColumn<Candidat> { Header = "Email" , Template = people=>people.Email , Property="Email"},
                new GridColumn<Candidat> { Header = "Phone" , Template = people => people.Phone , Property="Phone" },
                new GridColumn<Candidat> { Header = "Creation Date" , Template = people=>people.CreatedAt , Property = "CreatedAt"}
                };
        }
    }




}