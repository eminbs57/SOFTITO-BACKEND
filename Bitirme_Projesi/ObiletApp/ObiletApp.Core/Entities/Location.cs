using ObiletApp.Core.Common;

namespace ObiletApp.Core.Entities
{
    public class Location : BaseEntity
    {
        public string? CityName { get; set; }
        public string? TerminalName { get; set; }
    }
}