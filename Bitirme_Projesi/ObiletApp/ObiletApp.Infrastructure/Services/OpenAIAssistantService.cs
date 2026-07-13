using ObiletApp.Core.Interfaces;
using OpenAI.Chat;
using System.Threading.Tasks;

namespace ObiletApp.Infrastructure.Services
{
    public class OpenAIAssistantService : IAiAssistantService
    {
        private readonly ChatClient _client;

        public OpenAIAssistantService()
        {

            string apiKey = "sk-proj-YOUR_API_KEY_HERE";

            _client = new ChatClient(model: "gpt-4o", apiKey: apiKey);
        }

        public async Task<string> AskQuestionAsync(string question)
        {
            try
            {
                var prompt = $"Sen bir otobüs/uçak bileti asistanısın. Kullanıcının sorusuna kibarca yanıt ver. Soru: {question}";
                var completion = await _client.CompleteChatAsync(prompt);

                return completion.Value.Content[0].Text;
            }
            catch
            {

                return "Şu anda yapay zeka servisimize ulaşılamıyor. Ancak size en uygun 'İstanbul - Ankara' seferlerini listeleyebilirim.";
            }
        }
    }
}
