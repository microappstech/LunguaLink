using Data.Models;
using Data;

namespace Quiz.Pages
{
    public partial class Quizes
    {
        // [Parameter] public dynamic Id {get;set;}
        public bool dataReady = false;
        public List<Data.Models.Quiz> Quizzes { get; set; }
        public Data.Models.Quiz Quize { get; set; }
        public List<Question> questions { get; set; }
        public int pos { get; set; }
        public Question currentQuestion { get; set; }
        public int result { get; set; } = 0;
        public bool answered { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {

            Quize = InitData.Quizzes.FirstOrDefault();
            questions = new List<Question>();
            //questions = InitData.Quizzes.questions.ToList();
            // dataReady = true;
             questions = (List<Question>)Quize.questions;
            currentQuestion = questions[pos];
            dataReady =true;
        }

        public void check()
        {
            answered = true;
        }
        public void Next()
        {
            pos++;
            currentQuestion = new Question() { };
            currentQuestion = questions[pos];
        }
        public void Checked(int questId, bool answerId)
        {
            if (answerId)
            {
                result++;
            }
            else
            {

            }
        }

    }
}