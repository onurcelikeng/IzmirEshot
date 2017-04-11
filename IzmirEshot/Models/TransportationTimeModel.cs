namespace IzmirEshot.Models
{
    public sealed class TransportationTimeModel
    {
        public string Gidis { get; set; }

        public string Donus { get; set; }

        public string BgColor { get; set; }

        public string GidisWheelChair
        {
            get
            {
                if (!string.IsNullOrEmpty(Gidis))
                {
                    return "ms-appx:///Assets/Icons/wheelchair.png";
                }

                else
                {
                    return null;
                }
            }

            set { }
        }

        public string DonusWheelChair
        {
            get
            {
                if (!string.IsNullOrEmpty(Donus))
                {
                    return "ms-appx:///Assets/Icons/wheelchair.png";
                }

                else
                {
                    return null;
                }
            }

            set { }
        }
    }
}