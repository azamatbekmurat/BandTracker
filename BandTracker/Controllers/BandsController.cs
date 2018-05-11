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

    }
}
