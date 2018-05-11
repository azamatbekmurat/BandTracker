# _Band Tracker project with C#_

#### _C# application for Band Tracker using database, 05/11/2018_

#### By _**Azamat Bekmuratov**_

## Description
This web application is built with C# Asp Net Core MVC framework as well as MySQL. This program is developed to track bands and the venues where they've played their concerts.

## Setup/Installation Requirements

* Clone this repository to your desktop.
* Navigate to the cloned directory in a terminal capable of running dotnet commands.
* Run the command >dotnet add package Microsoft.AspNetCore.StaticFiles -v 1.1.3
* Run the command >dotnet restore
* Run the command >dotnet build
* Run the command >dotnet run
* Open http://localhost:5000 link in your browser.
* Open MySql and run following commands:
  * CREATE DATABASE band_tracker;
  * USE band_tracker;
  * CREATE TABLE venues (id serial PRIMARY KEY, name VARCHAR(255), description VARCHAR(255), location VARCHAR(255), capacity INT);
  * CREATE TABLE bands (id serial PRIMARY KEY, name VARCHAR(255), genre VARCHAR(255));
  * CREATE TABLE venues_bands (id serial PRIMARY KEY, venue_id INT, band_id INT);

## Known Bugs

_No known bugs at this time_

## Support and contact details

Please feel free to contact at azaege@gmail.com with any suggestions or feedback.

## Technologies Used
* C# .Net Core MVC
* Razor
* HTML
* MAMP
* MySQL

### License

*This software is licensed under the MIT license.*

Copyright (c) 2018 **Azamat Bekmuratov**
