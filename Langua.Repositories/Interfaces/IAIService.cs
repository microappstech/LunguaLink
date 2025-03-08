using Langua.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langua.Repositories.Interfaces
{
    public interface IAIService
    {
        public Task<GeminiApiResponse> AskGemini(GeminiRequest geminiRequest);

    }
}
