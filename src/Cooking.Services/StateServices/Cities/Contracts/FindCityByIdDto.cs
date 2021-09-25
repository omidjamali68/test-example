namespace Cooking.Services.StateServices.Cities.Contracts
{
    public class FindCityByIdDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ProvinceId { get; set; }
    }
}