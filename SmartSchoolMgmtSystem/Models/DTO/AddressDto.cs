namespace SmartSchool.Models.DTO
{
    public class AddressDto
    {
        public int AddressId { get; set; }

        public int? UserId { get; set; }

        public string AddressLine { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string PinCode { get; set; }

    }
}
