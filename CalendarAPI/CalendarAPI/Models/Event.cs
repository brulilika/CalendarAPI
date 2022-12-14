using System;
namespace CalendarAPI.Models
{
	public class Event
	{
        public int ID { get; set; }
        public string Nome { get; set; }
        public string Description { get; set; }
        public DateTime StartEventDate { get; set; }
        public DateTime EndEventDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public EventType EventType { get; set; }
        public EventPriority EventPriority { get; set; }

        //FK with User
        public int UserId { get; set; }
    }
}