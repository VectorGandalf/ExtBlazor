namespace ExtBlazor.Events
{
    public class EventRoute
    {
        public bool All { get; set; }
        public IEnumerable<string> Groups { get; set; } = [];
        public IEnumerable<string> Users { get; set; } = [];
        public IEnumerable<string> Clients { get; set; } = [];
    }
}
