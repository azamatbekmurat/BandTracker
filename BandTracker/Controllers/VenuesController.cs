using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BandTracker.Models;
using System;

namespace BandTracker.Controllers
{
    public class VenuesController : Controller
    {
      [HttpGet("/venues")]
      public ActionResult Index()
      {
          List<Venue> allVenues = Venue.GetAllVenues();
          return View(allVenues);
      }
      [HttpGet("/venues/new")]
      public ActionResult CreateForm()
      {
        return View("VenueForm");
      }
      [HttpPost("/venueForm")]
      public ActionResult Create()
      {
        Venue newVenue = new Venue(Request.Form["venueName"], Request.Form["venueDescription"], Request.Form["venueLocation"], Int32.Parse(Request.Form["venueCapacity"]));
        newVenue.Save();
        return RedirectToAction("Success", "Home");
      }
      [HttpGet("/venues/{id}")]
      public ActionResult VenueDetails(int id)
      {
        Dictionary<string, object> foundVenueDetails = new Dictionary<string, object> ();
        Venue selectedVenue = Venue.Find(id);
        List<Band> venueBands = selectedVenue.GetBands();
        List<Band> allBands = Band.GetAllBands();
        foundVenueDetails.Add("selectedVenue", selectedVenue);
        foundVenueDetails.Add("venueBands", venueBands);
        foundVenueDetails.Add("allBands", allBands);
        return View(foundVenueDetails);
      }
      [HttpPost("/venues/{venueId}/bands/new")]
      public ActionResult AddABand(int venueId)
      {
        Venue venue = Venue.Find(venueId);
        Band band = Band.Find(Int32.Parse(Request.Form["band-id"]));
        venue.AddBand(band);
        return RedirectToAction("VenueDetails", new { id = venueId });
      }
      [HttpGet("/venues/{id}/edit")]
      public ActionResult VenueEditForm(int id)
      {
        Venue thisVenue = Venue.Find(id);
        return View("VenueEditForm", thisVenue);
      }
      [HttpPost("/venues/edit/{venueId}/change")]
      public ActionResult EditVenueInfo(int venueId)
      {
        Venue selectedVenue = Venue.Find(venueId);
        selectedVenue.UpdateVenueDetails(Request.Form["editName"], Request.Form["editDescription"], Request.Form["editLocation"], Int32.Parse(Request.Form["editCapacity"]));
        return RedirectToAction("Update", "Home");
      }
      [HttpPost("/venues/{venueId}/delete")]
      public ActionResult DeleteVenue(int venueId)
      {
        Venue selectedVenue = Venue.Find(venueId);
        selectedVenue.Delete();
        return RedirectToAction("Delete", "Home");
      }

    }
}
