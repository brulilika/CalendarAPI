using System;
namespace CalendarAPI.Models
{
	public class Event
	{
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public EventType EventType { get; set; }
        public EventPriority EventPriority { get; set; }

        //FK with User
        public Guid UserId { get; set; }
    }
}

