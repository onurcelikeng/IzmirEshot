namespace IzmirEshot.Models
{
    public sealed class StationModel : BaseModel
    {
        public string Bus { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double Distance { get; set; }
    }
}