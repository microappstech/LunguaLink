
using System;
using System.Text;
using Langua.Models;
using Langua.WebUI.Pages.Components;
//using static Langua.WebUI.Pages.Components.LanguaGrid<dynamic>;


namespace Langua.WebUI.Pages.Candidates
{

    public partial class Candidates //: BasePage
    {
        public List<GridColumn<Candidat>> columns { get; set; }


        public List<Candidat> people { get; set; }



        protected override async Task OnInitializedAsync()
        {
            people = new List<Candidat>
            {
                new Candidat { FullName ="Hamza mouddakir", Email="HamzaMoudddakur@gmail.com", Phone ="063751556656", UserId="1546", CreatedAt = DateTime.Now },
                new Candidat { FullName ="Anas Omari", Email="AnasaOmari@gmail.com", Phone ="000000000", UserId="1546", CreatedAt = DateTime.Now },
                new Candidat { FullName ="Maher Hilali", Email="maherhl@gmail.com", Phone ="123456", UserId="5555", CreatedAt = DateTime.Now }
            };
        }
        public async Task Delete(Candidat candidat)
        {

        }
        public async Task Edit(Candidat candidat)
        {

        }

    }




}