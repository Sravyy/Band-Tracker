using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace BandTracker.Models
{
  public class Venue
  {
    private string _venueName;
    private int _id;

    public Venue(string venueName, int id = 0)
    {
      _venueName = venueName;
      _id = id;
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
        return this.GetId().Equals(newVenue.GetId());
      }
    }

    public override int GetHashCode()
    {
      return this.GetId().GetHashCode();
    }

    public string GetVenueName()
    {
      return _venueName;
    }

    public int GetId()
    {
      return _id;
    }

    public static List<Venue> GetAll()
    {
      List<Venue> allVenues = new List<Venue> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM venues;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int VenueId = rdr.GetInt32(0);
        string VenueName = rdr.GetString(1);
        Venue newVenue = new Venue(VenueName, VenueId);
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
      cmd.CommandText = @"INSERT INTO venues (venue_name) VALUES (@venueName);";

      MySqlParameter venueName = new MySqlParameter();
      venueName.ParameterName = "@venueName";
      venueName.Value = this._venueName;
      cmd.Parameters.Add(venueName);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static Venue Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM venues WHERE id = (@searchId);";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int VenueId = 0;
      string VenueName = "";

      while(rdr.Read())
      {
        VenueId = rdr.GetInt32(0);
        VenueName = rdr.GetString(1);
      }
      Venue newVenue = new Venue(VenueName, VenueId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return newVenue;
    }

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = new MySqlCommand("DELETE FROM venues WHERE id = @VenuesId; DELETE FROM bands_venues WHERE venue_id = @VenuesId;", conn);
      MySqlParameter venueIdParameter = new MySqlParameter();
      venueIdParameter.ParameterName = "@VenuesId";
      venueIdParameter.Value = this.GetId();

      cmd.Parameters.Add(venueIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
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

    public void AddBand(Band newBand)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO bands_venues (venue_id, band_id) VALUES (@VenueId, @BandId);";

      MySqlParameter venue_id = new MySqlParameter();
      venue_id.ParameterName = "@VenueId";
      venue_id.Value = _id;
      cmd.Parameters.Add(venue_id);

      MySqlParameter band_id = new MySqlParameter();
      band_id.ParameterName = "@BandId";
      band_id.Value = newBand.GetId();
      cmd.Parameters.Add(band_id);

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
      cmd.CommandText = @"SELECT band_id FROM bands_venues WHERE venue_id = @venueId;";

      MySqlParameter venueIdParameter = new MySqlParameter();
      venueIdParameter.ParameterName = "@venueId";
      venueIdParameter.Value = _id;
      cmd.Parameters.Add(venueIdParameter);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      List<int> bandIds = new List<int> {};
      while(rdr.Read())
      {
        int bandId = rdr.GetInt32(0);
        bandIds.Add(bandId);
      }
      rdr.Dispose();

      List<Band> bands = new List<Band> {};
      foreach (int bandId in bandIds)
      {
        var bandQuery = conn.CreateCommand() as MySqlCommand;
        bandQuery.CommandText = @"SELECT * FROM bands WHERE id = @BandId;";

        MySqlParameter bandIdParameter = new MySqlParameter();
        bandIdParameter.ParameterName = "@BandId";
        bandIdParameter.Value = bandId;
        bandQuery.Parameters.Add(bandIdParameter);

        var bandQueryRdr = bandQuery.ExecuteReader() as MySqlDataReader;
        while(bandQueryRdr.Read())
        {
          int thisBandId = bandQueryRdr.GetInt32(0);
          string bandName = bandQueryRdr.GetString(1);
          Band foundBand = new Band(bandName, thisBandId);
          bands.Add(foundBand);
        }
        bandQueryRdr.Dispose();
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return bands;
    }

    public List<Band> GetAvailableVenues()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT band_id FROM bands WHERE venue_id = @venueId;";

      MySqlParameter venueIdParameter = new MySqlParameter();
      venueIdParameter.ParameterName = "@venueId";
      venueIdParameter.Value = _id;
      cmd.Parameters.Add(venueIdParameter);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      List<int> bandIds = new List<int> {};
      while(rdr.Read())
      {
        int bandId = rdr.GetInt32(0);
        bandIds.Add(bandId);
      }
      rdr.Dispose();

      List<Band> bands = new List<Band> {};
      foreach (int bandId in bandIds)
      {
        var bandQuery = conn.CreateCommand() as MySqlCommand;
        bandQuery.CommandText = @"SELECT * FROM bands WHERE id = @BandId;";

        MySqlParameter bandIdParameter = new MySqlParameter();
        bandIdParameter.ParameterName = "@BandId";
        bandIdParameter.Value = bandId;
        bandQuery.Parameters.Add(bandIdParameter);

        var bandQueryRdr = bandQuery.ExecuteReader() as MySqlDataReader;
        while(bandQueryRdr.Read())
        {
          int thisBandId = bandQueryRdr.GetInt32(0);
          string bandName = bandQueryRdr.GetString(1);
          Band foundBand = new Band(bandName, thisBandId);
          bands.Add(foundBand);
        }
        bandQueryRdr.Dispose();
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return bands;
    }

    public void UpdateVenueName(string newVenueName)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE venues SET venue_name = @newVenueName WHERE id = @searchId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = _id;
      cmd.Parameters.Add(searchId);

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@newVenueName";
      name.Value = _venueName;
      cmd.Parameters.Add(name);

      _venueName = newVenueName;
      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

  }
}
