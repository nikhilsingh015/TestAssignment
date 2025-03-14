namespace Api.Libraries.AddressLibrary.Models
{
    public class Address
    {
        public int? Id { get; set; }
        public required string Name { get; set; }
        public string? Street { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public required string Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
