using System.Collections.Generic;
using FG12.Models;

namespace FG12.ViewModels
{
    public class MatchListViewModel
    {
        public Mode Mode { get; set; }
        public IEnumerable<IMatch> ComingMatches { get; set; }
        public IEnumerable<IMatch> PlayedMatches { get; set; }
        public IEnumerable<IMatch> AllMatches { get; set; }
    }

    public enum Mode
    {
        Group,
        MiddleStage,
        Knockout
    }
}