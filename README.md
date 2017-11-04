# _Band Tracker_

#### 11-3-2017

#### By _**Sravanthi Velaga**_

## Description

_A BandTracker website that allows the user to  add the Venues and Bands, for each Venue ,user can update number of bands, for each band, he will be able to update different available venues. Each venue can be edited or deleted if needed._

## Specs

* _Creating a Homepage named BandTracker where an individual can add a Venue, view all Venues and all the bands for each venue._
* _Allow the user to add a Venue using a form and once added, redirect him to the home page that displays the list of Venues and there bands._
* _View all Venues , View all Bands has the list of Venues and Bands respectively, that can be updated and deleted._

## Setup/Installation Requirements

* _easy to clone_
* _<a href="https://github.com/Sravyy/Band-Tracker.git" target="_blank">Click here</a> for the github link_

## Known Bugs

_No known bugs_

## Technologies Used

* _HTML_
* _CSS_
* _C#_
* _.Net Framework_
* _MVC_

## Creating Database
* _CREATE TABLE venues (id serial PRIMARY KEY, venue_name VARCHAR(255))_
* _CREATE TABLE bands (id serial PRIMARY KEY, band_name VARCHAR(255))_
* _CREATE TABLE bands_venues (id serial PRIMARY KEY, venue_id INT, band_id INT)_
* _Then change the collation to utf8_general_ci_

### License

Copyright (c) 2017 **_Sravanthi Velaga_**
