using System;
using System.Configuration;
using System.Collections.Generic;

namespace AfvalwijzerScraper
{
	/// <summary>
	/// Controller
	/// </summary>
	class ScraperController
	{
		/// <summary>
		/// Import trash pickup events
		/// </summary>
		public static void ImportTrashpickupEvent()
		{
			var googleCalendarId = ConfigurationManager.AppSettings["GoogleCalendarId"];
			var postalCode = ConfigurationManager.AppSettings["PostalCode"];
			var housenumber = ConfigurationManager.AppSettings["Housenumber"];
			var suffix = ConfigurationManager.AppSettings["HousenumberSuffix"];
			var scraper = new ScraperService(postalCode, housenumber, suffix);

			var googleClientId = ConfigurationManager.AppSettings["GoogleClientID"];
			var googleClientSecret = ConfigurationManager.AppSettings["GoogleClientSecret"];
			var googleApplicationName = ConfigurationManager.AppSettings["GoogleApplicationName"];
			var service = new GoogleCalendarService(googleClientId, googleClientSecret, googleApplicationName)
			{
				GoogleCalendarTimeZone = ConfigurationManager.AppSettings["TimeZone"]
			};

			var eventDescription = ConfigurationManager.AppSettings["EventDescription"];
			var eventLocation = ConfigurationManager.AppSettings["EventLocation"];
			// TODO: Make trashCalendars dynamic
			var trashCalendars = new SortedList<string, string>
			{
				{ "pmd", "Ophalen papier, blik en drankkartons" },
				{ "restafval", "Ophalen restafval (Grijze container)" },
				{ "papier", "Ophalen oud papier" },
				{ "gft", "Ophalen GFT (Groene container)" }
			};

			// Loop through trashcalendars, get the dates, add them to Google Calendar
			foreach (var calendar in trashCalendars)
			{
				// TODO: Create check if event exists functionality
				List<DateTime> dates = scraper.GetPickupDates(calendar.Key);
				service.AddEvents(googleCalendarId, dates, calendar.Value, eventDescription, eventLocation);
			}
			scraper.Quit();
		}
	}
}
