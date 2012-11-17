using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FG12.Models
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