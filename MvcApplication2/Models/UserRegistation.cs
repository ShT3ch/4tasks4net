using System;
using System.Collections.Generic;

namespace MvcApplication2.Models
{
    public class UserRegistation
    {
        public List<Tuple<DateTime, string>> Visits = new List<Tuple<DateTime, string>>();
        public DateTime LastPostTime;
        public DateTime LastVoteTime;
    }
}