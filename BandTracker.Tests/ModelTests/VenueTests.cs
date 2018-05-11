using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using BandTracker.Models;
using BandTracker;
using MySql.Data.MySqlClient;

namespace BandTracker.Tests
{

  [TestClass]
  public class VenueTests : IDisposable
  {
    public VenueTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=band_tracker_test;";
    }
    public void Dispose()
    {
      Venue.DeleteAll();
      Band.DeleteAll();
    }

    [TestMethod]
    public void Equals_OverrideTrueForSameDescription_Venue()
    {
      //Arrange, Act
      Venue firstVenue = new Venue("Madison Square Garden", "Indoor Arena", "New-York", 10000);
      Venue secondVenue = new Venue("Madison Square Garden", "Indoor Arena", "New-York", 10000);

      //Assert
      Assert.AreEqual(firstVenue, secondVenue);
    }
    [TestMethod]
    public void Save_SavesVenueToDatabase_Venue()
    {
      Venue testVenue = new Venue("Madison Square Garden", "Indoor Arena", "New-York", 10000);
      testVenue.Save();

      List<Venue> result = Venue.GetAllVenues();
      List<Venue> testList = new List<Venue>{testVenue};

      CollectionAssert.AreEqual(result, testList);

    }
    [TestMethod]
    public void Save_DatabaseAssignsIdToObject_Id()
    {
      //Arrange
      Venue testVenue = new Venue("Mandalay Bay", "Hotel", "Las-Vegas", 5000);
      testVenue.Save();

      //Act
      Venue savedVenue = Venue.GetAllVenues()[0];

      int result = savedVenue.GetId();
      int testId = testVenue.GetId();

      //Assert
      Assert.AreEqual(result, testId);
    }
    [TestMethod]
    public void Find_FindsVenueInDatabaseById_Venue()
    {
      //Arrange
      Venue testVenue = new Venue("Mandalay Bay", "Hotel", "Las-Vegas", 5000);
      testVenue.Save();

      //Act
      Venue foundVenue = Venue.Find(testVenue.GetId());

      //Assert
      Assert.AreEqual(testVenue, foundVenue);
    }
    [TestMethod]
    public void Find_FindsVenueInDatabaseByName_Venue()
    {
      //Arrange
      Venue testVenue1 = new Venue("Mandalay Bay", "Hotel", "Las-Vegas", 5000);
      testVenue1.Save();

      Venue testVenue2 = new Venue("Mandalay Arena", "Stadium", "York", 4000);
      testVenue2.Save();

      //Act
      List<Venue> result  = Venue.FindVenueByName("Mandalay");
      List<Venue> testList = new List<Venue>{testVenue1, testVenue2};

      // Console.WriteLine to see what number of elements in the List
      Console.WriteLine(result.Count);
      Console.WriteLine(testList.Count);

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }
    [TestMethod]
    public void GetBands_ReturnsAllVenueBands_BandsList()
    {
      //Arrange
      Venue testVenue = new Venue("Madison Square Garden", "Indoor Arena", "New-York", 10000);
      testVenue.Save();

      Band testBand1 = new Band("Linkin Park", "rock");
      testBand1.Save();

      Band testBand2 = new Band("Metallica", "heavy metall");
      testBand2.Save();

      //Act
      testVenue.AddBand(testBand1);
      testVenue.AddBand(testBand2);
      List<Band> result = testVenue.GetBands();
      List<Band> testList = new List<Band> {testBand1, testBand2};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }
    [TestMethod]
    public void Delete_DeletesVenueAssociationsFromDatabase_VenueList()
    {
      //Arrange
      Band testBand = new Band("Linkin Park", "rock");
      testBand.Save();

      string testVenueName = "MSG";
      string testVenueDescription = "Indoor Arena";
      string testVenueLocation = "New-York";
      int testVenueCapacity = 10000;
      Venue testVenue = new Venue(testVenueName, testVenueDescription, testVenueLocation, testVenueCapacity);
      testVenue.Save();

      //Act
      testVenue.AddBand(testBand);
      testVenue.Delete();

      List<Venue> resultBandVenues = testBand.GetVenues();
      List<Venue> testBandVenues = new List<Venue> {};

      //Assert
      CollectionAssert.AreEqual(testBandVenues, resultBandVenues);
    }
    [TestMethod]
    public void Update_UpdatesVenueAssociationsInDatabase_Venue()
    {
      //Arrange
      Venue testVenue = new Venue("mmmmmm", "Arena", "York", 2000);
      testVenue.Save();

      //Act
      string newName = "MSG";
      string newDescription = "Indoor Arena";
      string newLocation = "New-York";
      int newCapacity = 10000;
      testVenue.UpdateVenueDetails(newName, newDescription, newLocation, newCapacity);
      Venue result = Venue.Find(testVenue.GetId());

      //Assert
      Assert.AreEqual("MSG", result.GetName());
      Assert.AreEqual("Indoor Arena", result.GetDescription());
      Assert.AreEqual("New-York", result.GetLocation());
      Assert.AreEqual(10000, result.GetCapacity());
    }

  }
}
