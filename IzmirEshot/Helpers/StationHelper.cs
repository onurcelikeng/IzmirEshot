namespace IzmirEshot.Helpers
{
    public sealed class StationHelper
    {
        public static string StationChanger(string value)
        {
            if (value == "EVKA - 3") return "EVKA 3";
            else return value;
        }
    }
}