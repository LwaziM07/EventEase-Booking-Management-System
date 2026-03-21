## Event Ease | Booking Made Easy
### Overview
This program, made in Visual Studio 202 was designed to help users save data persistently within the website they accessed. The system would work by having specialists working in the events and secretary industry set bookings for events in different loactions, The logic:
 * The specialist adds a venue available for booking
 * The Specialist lists an event independent of the venue, just in case a venue is to be made available in advance for said event to be reserved for the venue.
 * The specialist creates the booking based on the venue information and event information added.
 * Once added, table summaries of all events, venues, and bookings are displayed to the specialist
 * Should the user need to edit or delete any entries in the system, they should be able to.
### Features
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

