namespace FG12.Models
{
    public interface IMatch
    {
        int Id { get; }
        int? HomeTeamGoals { get; set; }
        int? AwayTeamGoals { get; set; }
        string HomeTeamName { get; }
        string AwayTeamName { get; }
    }
}