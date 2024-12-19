namespace ConceptZeeWebAPI.Models
{
    public class CZeeContact
    {
        public int Id { get; set; }
        public required string ContactName { get; set; }
        public required string ContactEmail { get; set; }
        public DateTime ContactDOB { get; set; }
        public required string ContactRemarks { get; set; }
    }
}
