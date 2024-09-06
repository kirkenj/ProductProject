namespace Application.Models.User
{
    public class ProductFilter
    {
        public IEnumerable<Guid>? Ids { get; set; }
        public string? NamePart { get; set; }
        public string? DescriptionPart { get; set; }
        public decimal? PriceStart { get; set; }
        public decimal? PriceEnd { get; set; }
        public bool? IsAvailable { get; set; }
        public IEnumerable<Guid>? ProducerIds { get; set; }
        public DateTime? CreationDateStart { get; set; }
        public DateTime? CreationDateEnd { get; set; }
    }
}
