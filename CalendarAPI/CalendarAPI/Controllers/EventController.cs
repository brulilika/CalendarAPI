using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CalendarAPI.Authentication.Models;
using CalendarAPI.Authentication.Repositories;
using CalendarAPI.Models;
using CalendarAPI.Models.Requests;
using CalendarAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace CalendarAPI.Controllers
{
    [Authorize]
    [ApiController]
    public class EventController : Controller
    {
        [HttpGet("event")]
        public async Task<ActionResult<IEnumerable<Event>>> GetByUser([FromServices] EventRepository eventRepository)
        {
            var accessToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ","");
            var handler = new JwtSecurityTokenHandler();
            var t = handler.ReadJwtToken(accessToken).Payload;

            var userEvents = await eventRepository.GetEventsByUser(Convert.ToUInt16(t.GetValueOrDefault("userId")));

            return Ok(userEvents); 
        }

        [HttpGet("event/{id}")]
        public async Task<ActionResult<IEnumerable<Event>>> GetById([FromServices] EventRepository eventRepository, int id)
        {
            var accessToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var t = handler.ReadJwtToken(accessToken).Payload;

            var userEvents = await eventRepository.GetEventsById(id);

            return Ok(userEvents);
        }

        [HttpPost("event")]
        public async Task<ActionResult<Event>> CreateEvent([FromServices] EventRepository eventRepository, [FromBody] CreateEvent createEvent)
        {
            var accessToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var t = handler.ReadJwtToken(accessToken).Payload;

            var createRepo = await eventRepository.CreateEvent(createEvent, Convert.ToUInt16(t.GetValueOrDefault("userId")));
            
            return Ok(createRepo);
        }

        [HttpPut("event")]
        public async Task<ActionResult<Event>> UpdateEvent([FromServices] EventRepository eventRepository, [FromBody] UpdateEvent updateEvent)
        {
            var accessToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var t = handler.ReadJwtToken(accessToken).Payload;

            var createRepo = await eventRepository.UpdateEvent(updateEvent);

            return Ok(createRepo);
        }

        [HttpDelete("event/{id}")]
        public async Task<ActionResult<bool>> DeleteEvent([FromServices] EventRepository eventRepository, int id)
        {
            var accessToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var t = handler.ReadJwtToken(accessToken).Payload;

            var createRepo = await eventRepository.DeleteEvent(id);

            return Ok(createRepo);
        }
    }
}

