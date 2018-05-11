using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BandTracker.Models;
using System;

namespace BandTracker.Controllers
{
    public class HomeController : Controller
    {
      [HttpGet("/")]
      public ActionResult Index()
      {
          return View();
      }
      [HttpGet("/success")]
      public ActionResult Success()
      {
          return View();
      }
      [HttpGet("/venues/search")]
      public ActionResult SearchResult()
      {
        string searchString = Request.Query["venueName"];
        List<Venue> allFoundVenuesByName = Venue.FindVenueByName(searchString);

        return View(allFoundVenuesByName);
      }
      [HttpGet("/update")]
      public ActionResult Update()
      {
          return View();
      }
      [HttpGet("/delete")]
      public ActionResult Delete()
      {
          return View();
      }

    }
}
