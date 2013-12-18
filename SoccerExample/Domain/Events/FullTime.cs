using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Infrastructure;

namespace Domain.Events
{
    public class FullTime : Event
    {
        public Guid GameId { get; private set; }
        public DateTime DateTime { get; private set; }

        public FullTime(Guid gameId, DateTime dateTime)
        {
            GameId = gameId;
            DateTime = dateTime;
        }
    }
}
