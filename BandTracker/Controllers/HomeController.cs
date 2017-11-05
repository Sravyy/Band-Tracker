using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using BandTracker.Models;

namespace BandTracker.Controllers
{
  public class HomeController : Controller
  {
    //Home and routes
    [HttpGet("/")]
    public ActionResult Index()
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      List<Band> allBands = Band.GetAll();
      List<Venue> allVenues = Venue.GetAll();

      model.Add("band",allBands);
      model.Add("venue",allVenues);

      return View(model);
    }

    [HttpGet("/band/add")]
    public ActionResult AddBand()
    {
      return View();
    }

    [HttpPost("/band/view-all")]
    public ActionResult AddBandViewBand()
    {
      Band newBand = new Band(Request.Form["band-name"]);
      newBand.Save();

      List<Band> allBands = Band.GetAll();

      return View("BandList", allBands);

    }

    [HttpGet("/band/view-all")]
    public ActionResult ViewBand()
    {
      List<Band> allBands = Band.GetAll();

      return View("BandList", allBands);
    }

    [HttpPost("/band/{bandId}/venue/update")]
    public ActionResult ViewBand(int bandId)
    {
      Band thisBand = Band.Find(bandId);
      Venue thisVenue = Venue.Find(Int32.Parse(Request.Form["select-venues"]));
      // var myBoxes = Request.Form["select-venues"];
      // Console.WriteLine(myBoxes);
      thisBand.AddVenue(thisVenue);
      List<Band> allBands = Band.GetAll();

      return View("BandList", allBands);
    }

    [HttpGet("/venue/add")]
    public ActionResult AddVenue()
    {
      return View();
    }

    [HttpPost("/venue/view-all")]
    public ActionResult AddVenueViewVenue()
    {
      Venue newVenue = new Venue(Request.Form["venue-name"]);
      newVenue.Save();

      List<Venue> allVenues = Venue.GetAll();

      return View("VenueList", allVenues);
    }

    [HttpGet("/venue/view-all")]
    public ActionResult ViewVenue()
    {
      List<Venue> allVenues = Venue.GetAll();

      return View("VenueList", allVenues);
    }

    [HttpGet("/band/{bandId}/venues/add")]
    public ActionResult AddVenuesToBand(int bandId)
    {
      Band thisBand = Band.Find(bandId);
      List<Venue> allVenues = Venue.GetAll();
      Dictionary<string, object> model = new Dictionary<string, object>();

      model.Add("band", thisBand);
      model.Add("venues", allVenues);

      return View("BandProfile", model);
    }

    [HttpGet("/venue/{id}/edit")]
    public ActionResult VenueEdit(int id)
    {
      Venue thisVenue = Venue.Find(id);
      return View(thisVenue);
    }

    [HttpPost("/venue/{id}/edit")]
    public ActionResult VenueEditConfirm(int id)
    {
      Venue thisVenue = Venue.Find(id);
      thisVenue.UpdateVenueName(Request.Form["venue-name"]);
      return RedirectToAction("Index");
    }

    [HttpGet("/venue/{id}/delete")]
     public ActionResult VenueDeleteConfirm(int id)
     {
       Venue thisVenue = Venue.Find(id);
       thisVenue.Delete();
       Venue selectedVenue = Venue.Find(id); //Venue is selected as an object
      //  List<Client> stylistClients = selectedVenue.GetClients(); //Clients are displayed in a list
      //  Client thisClient = Client.Find(id);
      //  thisClient.DeleteClient();
       return RedirectToAction("Index");
     }

     [HttpPost("/venue/{venueId}/venue/update")]
     public ActionResult ViewVenue(int venueId)
     {
       Venue thisVenue = Venue.Find(venueId);
       Band thisBand = Band.Find(Int32.Parse(Request.Form["select-bands"]));
       // var myBoxes = Request.Form["select-venues"];
       // Console.WriteLine(myBoxes);
       thisVenue.AddBand(thisBand);
       List<Venue> allVenues = Venue.GetAll();

       return View("VenueList", allVenues);
     }

     [HttpGet("/venue/{venueId}/bands/add")]
     public ActionResult AddBandsToVenue(int venueId)
     {
       Venue thisVenue = Venue.Find(venueId);
       List<Band> allBands = Band.GetAll();
       Dictionary<string, object> model = new Dictionary<string, object>();

       model.Add("venue", thisVenue);
       model.Add("bands", allBands);

       return View("VenueProfile", model);
     }

  }
}
