using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public static class InitData
    {
        public static List<Question> Questions { get; set; } = new List<Question>();
        public static List<Answer> Answers { get; set; } = new List<Answer>();
        public static List<Quiz> Quizzes { get; set; } = new List<Quiz>()
            {
                new Models.Quiz{
                    Id = 1,
                    Description ="Test your knowledge of Blazor",
                    questions = new List<Question>()
                        {
                            new Question{
                                Id = 1,
                                QuizId = 1,
                                Description = "What is the primary language used in Blazor development?",
                                Answers = new List<Answer>()
                                    {
                                        new Answer { Id = 1 , Description ="JavaScript" , QuestionId =1, IsOk =false },
                                        new Answer { Id = 2 , Description ="Java" , QuestionId =1, IsOk =false },
                                        new Answer { Id = 3 , Description ="C Sharp" , QuestionId =1, IsOk =true },
                                        new Answer { Id = 4 , Description ="Python" , QuestionId =1, IsOk =false },
                                    }
                            },
                            new Question{
                                Id = 2,
                                QuizId = 1,
                                Description = "Which hosting models are supported by Blazor?",
                                Answers = new List<Answer>()
                                    {
                                        new Answer { Id = 1 , Description ="Blazor Server and Blazor WebAssembly" , QuestionId =2, IsOk =true },
                                        new Answer { Id = 2 , Description ="Blazor WebForms and Blazor MVC" , QuestionId =2, IsOk =false },
                                        new Answer { Id = 3 , Description ="Blazor Components and Blazor Pages" , QuestionId =2, IsOk =false },
                                        new Answer { Id = 4 , Description ="Blazor Frontend and Blazor Backend" , QuestionId =2, IsOk =false },
                                    }
                            },
                            new Question{
                                Id = 3,
                                QuizId = 1,
                                Description = "What technology allows Blazor WebAssembly applications to run .NET code in the browser?",
                                Answers = new List<Answer>()
                                    {
                                        new Answer { Id = 1 , Description ="JavaScript" , QuestionId =3, IsOk =false },
                                        new Answer { Id = 2 , Description ="TypeScript" , QuestionId =3, IsOk =false },
                                        new Answer { Id = 3 , Description ="C Sharp" , QuestionId =3, IsOk =false },
                                        new Answer { Id = 4 , Description ="WebAssembly" , QuestionId =3, IsOk =true },
                                    }
                            },
                            new Question{
                                Id = 4,
                                QuizId = 1,
                                Description = "Which of the following is true about Blazor Server?",
                                Answers = new List<Answer>()
                                    {
                                        new Answer { Id = 1 , Description ="It executes .NET code directly in the browser." , QuestionId =4, IsOk =false },
                                        new Answer { Id = 2 , Description ="It relies on a real-time connection between the client and the server." , QuestionId =4, IsOk =true },
                                        new Answer { Id = 3 , Description ="It compiles .NET code to WebAssembly." , QuestionId =4, IsOk =false },
                                        new Answer { Id = 4 , Description ="It is suitable for offline scenarios." , QuestionId =4, IsOk =false },
                                    }
                            },
                            new Question{
                                Id = 5,
                                QuizId = 1,
                                Description = "What is the purpose of Blazor components?",
                                Answers = new List<Answer>()
                                    {
                                        new Answer { Id = 1 , Description ="To manage server-side logic" , QuestionId =5, IsOk =false },
                                        new Answer { Id = 2 , Description ="To handle database operations" , QuestionId =5, IsOk =false },
                                        new Answer { Id = 3 , Description ="To encapsulate UI functionality and behavior" , QuestionId =5, IsOk =true },
                                        new Answer { Id = 4 , Description ="To interact with external APIs" , QuestionId =5, IsOk =false },
                                    }
                            }
                        }
                }

            };

    }
}
