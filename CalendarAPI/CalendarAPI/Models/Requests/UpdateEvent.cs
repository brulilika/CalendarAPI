using System;
namespace CalendarAPI.Models.Requests
{
	public class UpdateEvent
	{
        public int ID { get; set; }
        public string Nome { get; set; }
        public string Description { get; set; }
        public DateTime StartEventDate { get; set; }
        public DateTime EndEventDate { get; set; }
        public EventType EventType { get; set; }
        public EventPriority EventPriority { get; set; }
    }
}

