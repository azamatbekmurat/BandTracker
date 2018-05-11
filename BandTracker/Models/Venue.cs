using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace BandTracker.Models
{
  public class Venue
  {
    private string _name;
    private string _description;
    private string _location;
    private int _capacity;
    private int _id;

    public Venue(string venueName, string venueDescription, string locationName, int capacity, int id = 0)
    {
      _name = venueName;
      _description = venueDescription;
      _location = locationName;
      _capacity = capacity;
      _id = id;
    }
    public string GetName()
    {
      return _name;
    }
    public void SetName(string newName)
    {
      _name = newName;
    }
    public string GetDescription()
    {
      return _description;
    }
    public void SetDescription(string newDescription)
    {
      _description = newDescription;
    }
    public string GetLocation()
    {
      return _location;
    }
    public void SetLocation(string newLocation)
    {
      _location = newLocation;
    }
    public int GetCapacity()
    {
      return _capacity;
    }
    public void GetCapacity(int newCapacity)
    {
      _capacity = newCapacity;
    }
    public int GetId()
    {
      return _id;
    }
    public override bool Equals(System.Object otherVenue)
    {
      if (!(otherVenue is Venue))
      {
        return false;
      }
      else
      {
        Venue newVenue = (Venue) otherVenue;
        bool idEquality = this.GetId() == newVenue.GetId();
        bool nameEquality = this.GetName() == newVenue.GetName();
        bool descriptionEquality = this.GetDescription() == newVenue.GetDescription();
        bool locationEquality = this.GetLocation() == newVenue.GetLocation();
        bool capacityEquality = this.GetCapacity() == newVenue.GetCapacity();
        return (idEquality && nameEquality && descriptionEquality && locationEquality && capacityEquality);
      }
    }
    public override int GetHashCode()
    {
      return this.GetName().GetHashCode();
    }
    public static List<Venue> GetAllVenues()
    {
      List<Venue> allVenues = new List<Venue> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM venues;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int venueId = rdr.GetInt32(0);
        string venueName = rdr.GetString(1);
        string venueDescription = rdr.GetString(2);
        string venueLocation = rdr.GetString(3);
        int venueCapacity = rdr.GetInt32(4);
        Venue newVenue = new Venue(venueName, venueDescription, venueLocation, venueCapacity, venueId);
        allVenues.Add(newVenue);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allVenues;
    }
    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO venues (name, description, location, capacity) VALUES (@thisVenueName, @thisVenueDescription, @thisVenueLocation, @thisVenueCapacity);";

      cmd.Parameters.Add(new MySqlParameter("@thisVenueName", _name));
      cmd.Parameters.Add(new MySqlParameter("@thisVenueDescription", _description));
      cmd.Parameters.Add(new MySqlParameter("@thisVenueLocation", _location));
      cmd.Parameters.Add(new MySqlParameter("@thisVenueCapacity", _capacity));

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    public static Venue Find (int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM venues WHERE id = (@searchId);";

      cmd.Parameters.Add(new MySqlParameter("@searchId", id));

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int venueId = 0;
      string venueName = "";
      string venueDescription = "";
      string venueLocation = "";
      int venueCapacity = 0;

      while(rdr.Read())
      {
        venueId = rdr.GetInt32(0);
        venueName = rdr.GetString(1);
        venueDescription = rdr.GetString(2);
        venueLocation = rdr.GetString(3);
        venueCapacity = rdr.GetInt32(4);
      }
      Venue newVenue = new Venue(venueName, venueDescription, venueLocation, venueCapacity, venueId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return newVenue;
    }
    public static List<Venue> FindVenueByName(string searchValue)
    {
      List<Venue> allFoundVenues = new List<Venue> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM venues WHERE name LIKE @partOfVenueName;";

      cmd.Parameters.Add(new MySqlParameter("@partOfVenueName", "%"+searchValue+"%"));

      // Console.WriteLine(cmd.CommandText);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      while(rdr.Read())
      {
        int venueId = rdr.GetInt32(0);
        string venueName = rdr.GetString(1);
        string venueDescription = rdr.GetString(2);
        string venueLocation = rdr.GetString(3);
        int venueCapacity = rdr.GetInt32(4);
        Venue newVenue = new Venue(venueName, venueDescription, venueLocation, venueCapacity, venueId);
        allFoundVenues.Add(newVenue);
      }

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allFoundVenues;
    }
    public void AddBand(Band newBand)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO venues_bands (venue_id, band_id) VALUES (@venueId, @bandId);";

      cmd.Parameters.Add(new MySqlParameter("@venueId", _id));
      cmd.Parameters.Add(new MySqlParameter("@bandId", newBand.GetId()));

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public List<Band> GetBands()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT bands.* FROM venues
        JOIN venues_bands ON (venues.id=venues_bands.venue_id)
        JOIN bands ON (venues_bands.band_id=bands.id)
      WHERE venues.id=@VenueId;";

      cmd.Parameters.Add(new MySqlParameter("@VenueId", _id));

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Band> bands = new List<Band> {};
      while(rdr.Read())
      {
        int bandId = rdr.GetInt32(0);
        string bandName = rdr.GetString(1);
        string bandGenre = rdr.GetString(2);
        Band newBand = new Band(bandName, bandGenre, bandId);
        bands.Add(newBand);
      }

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return bands;
    }
    public void UpdateVenueDetails(string newName, string newDescription, string newLocation, int newCapacity)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE venues SET name = @newName, description = @newDescription, location = @newLocation, capacity = @newCapacity WHERE id = @searchId;";

      cmd.Parameters.Add(new MySqlParameter("@searchId", _id));
      cmd.Parameters.Add(new MySqlParameter("@newName", newName));
      cmd.Parameters.Add(new MySqlParameter("@newDescription", newDescription));
      cmd.Parameters.Add(new MySqlParameter("@newLocation", newLocation));
      cmd.Parameters.Add(new MySqlParameter("@newCapacity", newCapacity));

      cmd.ExecuteNonQuery();
      _name = newName;
      _description = newDescription;
      _location = newLocation;
      _capacity = newCapacity;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM venues;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM venues WHERE id = @venueId; DELETE FROM venues_bands WHERE venue_id = @venueId;";

      cmd.Parameters.Add(new MySqlParameter("@venueId", this.GetId()));

      cmd.ExecuteNonQuery();
      if (conn != null)
      {
        conn.Close();
      }
    }

  }
}
