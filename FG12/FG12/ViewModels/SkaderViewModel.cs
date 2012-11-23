namespace FG12.ViewModels
{
    public class SkaderViewModel
    {
        public string Hometeam { get; set; }
        public string Awayteam { get; set; }

        public SkaderViewModel(string hometeam, string awayteam)
        {
            Hometeam = hometeam;
            Awayteam = awayteam;
        }
    }
}