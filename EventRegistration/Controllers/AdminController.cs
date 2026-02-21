using EventRegistration.Data;
using EventRegistration.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace EventRegistration.Controllers;

public class AdminController : Controller
{
    public IActionResult Index()
    {
        var isAdmin = HttpContext.Session.GetString("IsAdmin");
        if (isAdmin != "true")
        {
            return RedirectToAction("Login");
        }
        return View(DataStore.Events);
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string password)
    {
        if (password == "qwerty123")
        {
            HttpContext.Session.SetString("IsAdmin", "true");
            return RedirectToAction("Index");
        }
        ViewBag.Error = "Неправильний пароль";
        return View();
    }
    public IActionResult Logout()
    {
        //HttpContext.Session.Remove("IsAdmin");
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
    
    [HttpGet]
    public IActionResult CreateEvent()
    {
        var isAdmin = HttpContext.Session.GetString("IsAdmin");
        if (isAdmin != "true")
        {
            return RedirectToAction("Login");
        }
        return View();
    }

    [HttpPost]
    public IActionResult CreateEvent(Event newEvent)
    {
        var isAdmin = HttpContext.Session.GetString("IsAdmin");
        if (isAdmin != "true")
        {
            return RedirectToAction("Login");
        }
        newEvent.Id = DataStore.GetNextEventId();
        DataStore.Events.Add(newEvent);
        return RedirectToAction("Index");
    }

    public IActionResult ViewRegistrations(int eventId, string email)
    {
        var isAdmin = HttpContext.Session.GetString("IsAdmin");
        if (isAdmin != "true") return RedirectToAction("Login");
        
        var eventItem = DataStore.Events.FirstOrDefault(e => e.Id == eventId);
        if (eventItem == null) return NotFound();
        var registrationsEmail = DataStore.Registrations.Where(r => r.EventId == eventId);

        if (!string.IsNullOrWhiteSpace(email))
        {
            registrationsEmail = registrationsEmail
                .Where(r => r.Email.Contains(email, StringComparison.OrdinalIgnoreCase));
        }

        var registrations = registrationsEmail.ToList();
        
        
        ViewBag.EventTitle = eventItem.Title;
        ViewBag.EventId = eventId;

        
        return View(registrations);
    }

    public IActionResult DeleteEvent(int eventId)
    {
        var isAdmin = HttpContext.Session.GetString("IsAdmin");
        if (isAdmin != "true")
        {
            return RedirectToAction("Login");
        }
        var evetItem = DataStore.Events.FirstOrDefault(e => e.Id == eventId);
        if (evetItem == null) {return NotFound();}
        DataStore.Events.Remove(evetItem);
        
        var registerRemuve = DataStore.Registrations.Where(r => r.EventId == eventId).ToList();
        foreach (var r in registerRemuve)
        {
            DataStore.Registrations.Remove(r);
        }
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult EditEvent(int eventId)
    {
        var isAdmin = HttpContext.Session.GetString("IsAdmin");
        if (isAdmin != "true") return RedirectToAction("Login");
        
        var evetItem = DataStore.Events.FirstOrDefault(e => e.Id == eventId);
        if (evetItem == null) {return NotFound();}
        
        return View(evetItem);
    }

    [HttpPost]
    public IActionResult EditEvent(Event newEvent)
    {
        var isAdmin = HttpContext.Session.GetString("IsAdmin");
        if (isAdmin != "true") return RedirectToAction("Login");
        
        var evItem = DataStore.Events.FirstOrDefault(e => e.Id == newEvent.Id);
        if (evItem == null) return NotFound();
        
        evItem.Title = newEvent.Title;
        evItem.Description = newEvent.Description;
        evItem.EventDate = newEvent.EventDate;
        evItem.Location = newEvent.Location;
        
        return RedirectToAction("Index");
    }

    
    
}