## Event Ease | Booking Made Easy
### Overview
This program, made in Visual Studio 2026 was designed to help users save data persistently within the website they accessed. The system would work by having specialists working in the events and secretary industry set bookings for events in different loactions, The logic:
 * The specialist adds a venue available for booking
 * The Specialist lists an event independent of the venue, just in case a venue is to be made available in advance for said event to be reserved for the venue.
 * The specialist creates the booking based on the venue information and event information added.
 * Once added, table summaries of all events, venues, and bookings are displayed to the specialist
 * Should the user need to edit or delete any entries in the system, they should be able to.
### Features
### Interface
* ## Index pages:
<img width="1356" height="721" alt="Screenshot 2026-04-24 125904" src="https://github.com/user-attachments/assets/29d89619-e24a-4a4a-a886-3e3386730aa2" />

<img width="1365" height="711" alt="Screenshot 2026-04-24 125923" src="https://github.com/user-attachments/assets/cac4f3d2-104d-4faa-b632-eb73459e060d" />

<img width="1365" height="719" alt="Screenshot 2026-04-24 125941" src="https://github.com/user-attachments/assets/893a1429-b81d-4f3b-90dc-28074b4679c9" />

<img width="1365" height="717" alt="Screenshot 2026-04-24 130002" src="https://github.com/user-attachments/assets/0fa5aeeb-91e0-429a-ba76-a62f9c2af3f5" />

* ## CRUD Pages (Logic is consistent with all tables)
<img width="1365" height="767" alt="Screenshot 2026-04-24 130028" src="https://github.com/user-attachments/assets/fe921041-973a-41be-8a29-d1d266b49e91" />

<img width="1365" height="767" alt="Screenshot 2026-04-24 130042" src="https://github.com/user-attachments/assets/61d11a28-4cc6-4fdc-8fa3-db6dd20d8f42" />

<img width="1364" height="766" alt="Screenshot 2026-04-24 130103" src="https://github.com/user-attachments/assets/9a5a2fb4-5373-42fd-abe3-30b082c929a2" />

#### Data persistence
To make sure that data remained persistent after the specialist would finishe their session, a database was created to store all of the values of of each entity (Bookings, Venues and Events).

Each entity has its own attributes.

Venues:
  * Venue ID, Venue Name, Venue Location, Capacity, Image for Venue.
 
Events:
  * Event ID, Event Name, Event description, Start Date, End Date.
  
Bookings:
  * Booking ID, Specialist Name, Venue Name, Event Name, Date of Booking,
  
These were created translated into tables from the Sql Server Management System, to be used in Visual Studio.

#### Controllers and CRUD operations
Controllers, View and Models were added to the visual studio project after reverse engineering the file and assimilating it with the contents of the database that was created. Using the extension EF Core Power tools, The process was complete.

Making it so that each entity would have its own CRUD properties, where its contents can be modified or deleted , and ultimately have those changes reflect across the entire website.

#### Search and filter
To keep track of the bookings made, the ability to search for who made what booking was necessary. This problem was solved through the use of LINQ. Where the data was queried searches would take place as the algorithym would select entries made based on the specialist's name and return all entries associated with that search. 

#### File uploads
Modified the image submission function to add files from local storage instead of using URL links, which makes it easier to rely on.

The specialist would be able to interact with this feature through the bookings web page, crafted from html with the search option spaced right across the propmt to add a new booking.

### Styling (CSS and Bootstrap)
The colors, composition, and layout of the site were modified through CSS in the layout.cshtml.css file (in the shared folder) and the site.css file (in the wwwroot folder). Aside from CSS, Bootstrap was also used to help with intuitive and attractive design templates that were integrated into the classes of all the elements used within each of the webpages.

# 🚀 .NET Web App Setup Guide

This guide will help you set up and run the project locally using Visual Studio.

---

## 📦 Prerequisites

Make sure you have the following installed:

- Visual Studio (with ASP.NET and web development workload)
- SQL Server (or SQL Server Express)
- SQL Server Management Studio (SSMS)
- EF Core Power Tools (Visual Studio extension)

---

## 📥 1. Clone the Repository

``` bash
git clone [[INSERT REPO URL](https://github.com/LwaziM07/EventEase-Booking-Management-System.git)]
cd [PROJECT FOLDER NAME]. 
```
## 🗄️ 2. Set Up the Database
1. Locate the SQL file in the project:
[EventEaseBooking.sql]

2. Open SQL Server Management Studio (SSMS)
3. Connect to your local SQL Server instance
4. Open the SQL file and execute it to:
  * Create the database
  * Create tables
  * Seed initial data (if included)
    
## Open the Project in Visual Studio
1. Open Visual Studio
2. Click Open a project or solution
3. Select the .sln file from the cloned repository

## 3. Configure the Connection String
1. Open:
   ```
   appsettings.json

2. Verify that the connection string is set up verbatim to this snippet:
   ```
    {
    "Logging": {
      "LogLevel": {
        "Default": "Information",
        "Microsoft.AspNetCore": "Warning"
      }
    },
    "AllowedHosts": "*",
    "ConnectionStrings": {
      "Conn": "Server=localhost; Database =EventBaseDB;Trusted_connection=true;TrustServerCertificate=true",
      "storage": "UseDevelopmentStorage=True;"
     }
    }

## 4. Reverse Engineer the Database (EF Core)
1. Right-click the project in Solution Explorer
2. Select EF Core Power Tools → Reverse Engineer
3. Choose your database connection
4. Select the tables you want
5. Complete the wizard

This will generate:

* Entity models
* DbContext class
## ▶️ 6. Build and Run the Application

* Build the project.
  ```
  Crtl + shift +B
* Run the project.
  ```
  F5
## 🛠️ Troubleshooting

### App not running?

* Check your connection string
* Ensure SQL Server is running
* Rebuild the solution

### EF Core Power Tools not showing?

* Install it via Visual Studio Extensions

### Port conflicts?

* Update the port in:
```
 Properties/launchSettings.json
```
📌 Notes
* Ensure your database is created before running the app
* Always rebuild after making major changes
* If something breaks, clean + rebuild usually helps
