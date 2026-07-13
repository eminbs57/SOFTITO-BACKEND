using System.Threading.Tasks;

namespace ObiletApp.Core.Interfaces
{
    public interface IAiAssistantService
    {
        Task<string> AskQuestionAsync(string question);
    }
}
