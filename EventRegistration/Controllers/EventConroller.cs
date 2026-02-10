using EventRegistration.Data;
using EventRegistration.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventRegistration.Controllers;

public class EventController : Controller
{
    // GET
    public IActionResult Register(int id)
    {
        var eventItem = DataStore.Events.FirstOrDefault(e => e.Id == id);
        if(eventItem == null) {return NotFound("Подію не найдено");}
        
        ViewBag.EventId = eventItem.Id;
        ViewBag.EventTitle = eventItem.Title;
        ViewBag.EventDescription = eventItem.Description;
        ViewBag.EventDate = eventItem.EventDate.ToString("dd.MM.yyyy HH:mm");
        ViewBag.EventLocation = eventItem.Location;
        
        return View();
    }

    [HttpPost]
    public IActionResult Register(Registration registration)
    {
        registration.Id = DataStore.GetNextRegistrationId();
        registration.RegisteredAt = DateTime.Now;
        DataStore.Registrations.Add(registration);
        return View("Success");
    }
}