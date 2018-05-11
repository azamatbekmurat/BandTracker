using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace BandTracker.Models
{
  public class Band
  {
    private string _name;
    private string _genre;
    private int _id;

    public Band(string bandName, string bandGenre, int id = 0)
    {
      _name = bandName;
      _genre = bandGenre;
      _id = id;
    }
    public string GetBandName()
    {
      return _name;
    }
    public void SetBandName(string newBandName)
    {
      _name = newBandName;
    }
    public string GetGenre()
    {
      return _genre;
    }
    public void SetGenre(string newGenre)
    {
      _genre = newGenre;
    }
    public int GetId()
    {
      return _id;
    }
    public override bool Equals(System.Object otherBand)
    {
      if (!(otherBand is Band))
      {
        return false;
      }
      else
      {
        Band newBand = (Band) otherBand;
        bool idEquality = this.GetId() == newBand.GetId();
        bool nameEquality = this.GetBandName() == newBand.GetBandName();
        bool genreEquality = this.GetGenre() == newBand.GetGenre();
        return (idEquality && nameEquality && genreEquality);
      }
    }
    public override int GetHashCode()
    {
      return this.GetBandName().GetHashCode();
    }
    public static List<Band> GetAllBands()
    {
      List<Band> allBands = new List<Band> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM bands;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int bandId = rdr.GetInt32(0);
        string bandName = rdr.GetString(1);
        string bandGenre = rdr.GetString(2);
        Band newBand = new Band(bandName, bandGenre, bandId);
        allBands.Add(newBand);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allBands;
    }
    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO bands (name, genre) VALUES (@bandName, @bandGenre);";

      cmd.Parameters.Add(new MySqlParameter("@bandName", _name));
      cmd.Parameters.Add(new MySqlParameter("@bandGenre", _genre));

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    public static Band Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM bands WHERE id = (@searchId);";

      cmd.Parameters.Add(new MySqlParameter("@searchId", id));

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int bandId = 0;
      string bandName = "";
      string bandGenre = "";

      while(rdr.Read())
      {
        bandId = rdr.GetInt32(0);
        bandName = rdr.GetString(1);
        bandGenre = rdr.GetString(2);
      }
      Band newBand = new Band(bandName, bandGenre, bandId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return newBand;
    }
    public List<Venue> GetVenues()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT venues.* FROM bands
      JOIN venues_bands ON (bands.id=venues_bands.band_id)
      JOIN venues ON (venues_bands.venue_id=venues.id)
      WHERE bands.id=@BandId;";

      cmd.Parameters.Add(new MySqlParameter("@BandId", _id));

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Venue> venues = new List<Venue> {};
      while(rdr.Read())
      {
        int venueId = rdr.GetInt32(0);
        string venueName = rdr.GetString(1);
        string venueDescription = rdr.GetString(2);
        string venueLocation = rdr.GetString(3);
        int venueCapacity = rdr.GetInt32(4);
        Venue newVenue = new Venue(venueName, venueDescription, venueLocation, venueCapacity, venueId);
        venues.Add(newVenue);
      }

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return venues;
    }
    public void AddVenue(Venue newVenue)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO venues_bands (venue_id, band_id) VALUES (@venueId, @bandId);";

      cmd.Parameters.Add(new MySqlParameter("@bandId", _id));
      cmd.Parameters.Add(new MySqlParameter("@venueId", newVenue.GetId()));

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
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
      cmd.CommandText = @"DELETE FROM bands;";
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
      cmd.CommandText = @"DELETE FROM bands WHERE id = @bandId; DELETE FROM venues_bands WHERE band_id = @bandId;";

      cmd.Parameters.Add(new MySqlParameter("@bandId", this.GetId()));

      cmd.ExecuteNonQuery();
      if (conn != null)
      {
        conn.Close();
      }
    }

  }
}
