using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BandTracker.Models;
using System;

namespace BandTracker.Controllers
{
    public class BandsController : Controller
    {
      [HttpGet("/bands")]
      public ActionResult Index()
      {
          List<Band> allBands = Band.GetAllBands();
          return View(allBands);
      }
      [HttpGet("/bands/new")]
      public ActionResult CreateForm()
      {
        return View("BandForm");
      }
      [HttpPost("/bandForm")]
      public ActionResult Create()
      {
        Band newBand = new Band(Request.Form["bandName"], Request.Form["bandGenre"]);
        newBand.Save();
        return RedirectToAction("Success", "Home");
      }
      [HttpGet("/bands/{id}")]
      public ActionResult BandDetails(int id)
      {
        Dictionary<string, object> foundBandDetails = new Dictionary<string, object> ();
        Band selectedBand = Band.Find(id);
        List<Venue> bandsVenues = selectedBand.GetVenues();
        List<Venue> allVenues = Venue.GetAllVenues();
        foundBandDetails.Add("selectedBand", selectedBand);
        foundBandDetails.Add("bandsVenues", bandsVenues);
        foundBandDetails.Add("allVenues", allVenues);
        return View(foundBandDetails);
      }
      [HttpPost("/bands/{bandId}/venues/new")]
      public ActionResult AddAVenue(int bandId)
      {
        Band band = Band.Find(bandId);
        Venue venue = Venue.Find(Int32.Parse(Request.Form["venue-id"]));
        band.AddVenue(venue);
        return RedirectToAction("BandDetails", new { id = bandId });
      }

    }
}
