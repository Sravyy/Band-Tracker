using Microsoft.VisualStudio.TestTools.UnitTesting;
using BandTracker.Models;
using System.Collections.Generic;
using System;

namespace BandTracker.Tests
{
  [TestClass]
  // public class BandTests : IDisposable
  public class BandTests : IDisposable
  {
    public BandTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=band_tracker_tests;";
    }

    public void Dispose()
    {
      Band.DeleteAll();
      Venue.DeleteAll();
    }

    [TestMethod]
    public void GetAll_DatabaseEmptyAtFirst_0()
    {
      //Arrange, Act
      int result = Band.GetAll().Count;

      //Assert
      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Equals_OverrideTrueIfDescriptionsAreTheSame_Band()
    {
      // Arrange, Act
      Band firstBand = new Band("Pearl Jam");
      Band secondBand = new Band("Pearl Jam");

      // Assert
      Assert.AreEqual(firstBand, secondBand);
    }

    [TestMethod]
    public void Save_SavesToDatabase_BandList()
    {
      //Arrange
      Band testBand = new Band("Pearl Jam");

      //Act
      testBand.Save();
      List<Band> result = Band.GetAll();
      List<Band> testList = new List<Band>{testBand};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void Save_AssignsIdToObject_Id()
    {
      //Arrange
      Band testBand = new Band("Pearl Jam");

      //Act
      testBand.Save();
      Band savedBand = Band.GetAll()[0];

      int result = savedBand.GetId();
      int testId = testBand.GetId();

      //Assert
      Assert.AreEqual(testId, result);
    }

    [TestMethod]
    public void AddVenue_AddsVenueToBand_VenueList()
    {
      //Arrange
      Band testBand = new Band("Pearl Jam");
      testBand.Save();

      Venue testVenue = new Venue("Crowne Plaza");
      testVenue.Save();

      //Act
      testBand.AddVenue(testVenue);

      List<Venue> result = testBand.GetVenues();
      List<Venue> testList = new List<Venue>{testVenue};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void Find_FindsBandInDatabase_Band()
    {
      //Arrange
      Band testBand = new Band("Pearl Jam");
      testBand.Save();

      //Act
      Band foundBand = Band.Find(testBand.GetId());

      //Assert
      Assert.AreEqual(testBand, foundBand);
    }

    [TestMethod]
    public void GetVenues_ReturnsAllBandVenues_VenueList()
    {
      //Arrange
      Band testBand = new Band("Pearl Jam");
      testBand.Save();

      Venue testVenue1 = new Venue("Crowne Plaza");
      testVenue1.Save();

      Venue testVenue2 = new Venue("Crowne Plaza");
      testVenue2.Save();

      //Act
      testBand.AddVenue(testVenue1);
      List<Venue> result = testBand.GetVenues();
      List<Venue> testList = new List<Venue> {testVenue1};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }



  }
}
