using System;
using CalendarAPI.Authentication.Model;
using CalendarAPI.Database;
using CalendarAPI.Models;
using CalendarAPI.Models.Requests;

namespace CalendarAPI.Repositories
{
	public class EventRepository
	{
        public async Task<List<Event>> GetEventsByUser(int Id)
        {
            try
            {
                using (var ctx = new CalendarDBContext())
                {
                    IEnumerable<Event> userEvents = ctx.Events.Where(w=> w.UserId== Id);

                    return userEvents.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        

        public async Task<Event> GetEventsById(int eventId)
        {
            try
            {
                using (var ctx = new CalendarDBContext())
                {
                    Event existEvent = ctx.Events.Where(w => w.ID == eventId)?.FirstOrDefault();
                    
                    return existEvent;
                   
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<Event>> OrganizeDateEvents(List<Event> eventos)
        {
            try
            {
                List<Event> responseList = new List<Event>();
                for (int i = 1; i < eventos.Count(); i++)
                {
                    if (eventos[i].EventType == eventos[i - 1].EventType && eventos[i].EndEventDate.Day == eventos[i - 1].EndEventDate.Day)
                    {
                        DateTime auxEventTimeEnd = eventos[i + 1].EndEventDate;
                        DateTime auxEventTimeStart = eventos[i + 1].StartEventDate;

                        eventos[i + 1].EndEventDate = eventos[i].EndEventDate;
                        eventos[i + 1].StartEventDate = eventos[i].StartEventDate;
                        eventos[i].EndEventDate = auxEventTimeEnd;
                        eventos[i].StartEventDate = auxEventTimeStart;
                    }
                }

                return eventos;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<Event>> OrganizeEvents(DateTime date,int userId)
        {
            try
            {
                using (var ctx = new CalendarDBContext())
                {
                    List<Event> eventos = ctx.Events.Where(w => w.UserId == userId && w.EndEventDate.Date == date.Date)?.OrderBy(ob => ob.EndEventDate).ThenBy(tb => tb.EventType).ToList();
                    Boolean unordered = true;
                    while (unordered)
                    {
                        eventos = (await OrganizeDateEvents(eventos)).OrderBy(ob=>ob.EndEventDate).ToList();
                        unordered = false;
                        for (int i = 1; i < eventos.Count(); i++)
                        {
                            if (eventos[i].EventType == eventos[i - 1].EventType)
                                unordered = true;
                        }
                    }

                    return eventos;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Event?> CreateEvent(CreateEvent createEvent, int userId)
        {
            try
            {
                using (var ctx = new CalendarDBContext())
                {
                    Event newEvent = new Event
                    {
                        Nome = createEvent.Nome,
                        Description = createEvent.Description,
                        UserId = userId,
                        EventPriority = createEvent.EventPriority,
                        EventType = createEvent.EventType,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        StartEventDate = createEvent.StartEventDate,
                        EndEventDate = createEvent.EndEventDate
                    };
                    ctx.Events.Add(newEvent);
                    ctx.SaveChanges();
                    return ctx.Events.OrderBy(ob=>ob.ID).LastOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Event?> UpdateEvent(UpdateEvent updateEvent)
        {
            try
            {
                using (var ctx = new CalendarDBContext())
                {
                    Event existEvent = ctx.Events.Where(w => w.ID == updateEvent.ID)?.FirstOrDefault();
                    if (existEvent != null)
                    {
                        existEvent.Nome = updateEvent.Nome;
                        existEvent.Description = updateEvent.Description;
                        existEvent.StartEventDate = updateEvent.StartEventDate;
                        existEvent.EndEventDate = updateEvent.EndEventDate;
                        existEvent.UpdatedAt = DateTime.Now;
                        existEvent.EventType = updateEvent.EventType;
                        existEvent.EventPriority = updateEvent.EventPriority;

                        ctx.Events.Update(existEvent);
                        ctx.SaveChanges();
                        return ctx.Events.Where(w => w.ID == updateEvent.ID)?.FirstOrDefault();
                    }
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> DeleteEvent(int id)
        {
            try
            {
                using (var ctx = new CalendarDBContext())
                {
                    Event existEvent = ctx.Events.Where(w => w.ID == id)?.FirstOrDefault();
                    if (existEvent != null)
                    {
                        ctx.Events.Remove(existEvent);
                        ctx.SaveChanges();
                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}

