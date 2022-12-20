namespace Domain.Dtos
{
    public class LocationDto
    {
        public LocationDto() { }
        public LocationDto(string cityName, string createdBy, List<string> townships)
        {
            CityName = cityName;
            CreatedBy = createdBy;
            Townships = townships;
        }

        public string CityName { get;set;}
        public string CreatedBy { get;set;} 
        public List<string> Townships { get; set;}
    }
}
