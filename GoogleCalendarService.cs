using System;
using System.Configuration;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;

namespace AfvalwijzerScraper
{
	/// <summary>
	/// Google Calender Service to connect with Google Calendar API
	/// </summary>
	class GoogleCalendarService
	{
		/// <summary>
		/// Google Calendar API Service
		/// </summary>
		protected CalendarService Service;

		/// <summary>
		/// Timezone used when adding events in Google Calendar
		/// </summary>
		public string GoogleCalendarTimeZone { get; set; } = "Europe/Amsterdam";

		/// <summary>
		/// Constructor
		/// </summary>
		public GoogleCalendarService(string googleClientId, string googleClientSecret, string googleApplicationName)
		{
			UserCredential credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
				new ClientSecrets
				{
					ClientId = googleClientId,
					ClientSecret = googleClientSecret
				},
				new[] { CalendarService.Scope.Calendar },
				"user",
				CancellationToken.None).Result;

			Service = new CalendarService(new BaseClientService.Initializer()
			{
				HttpClientInitializer = credential,
				ApplicationName = googleApplicationName
			});
		}

		/// <summary>
		/// Adds multiple dates to calendar.
		/// </summary>
		/// <param name="calendarName">Name of calendar in Google Calendar</param>
		/// <param name="dates">List with dates</param>
		/// <param name="summary">Event name</param>
		/// <param name="description">Longer description of event</param>
		/// <param name="location">Location of event</param>
		public void AddEvents(string calendarName, List<DateTime> dates, string summary, string description = "", string location = "")
		{
			var list = Service.CalendarList.List().Execute();
			var calendar = list.Items.SingleOrDefault(c => c.Summary == calendarName);

			foreach (DateTime date in dates)
			{
				Service.Events.Insert(new Event()
				{
					Summary = summary,
					Location = location,
					Description = description,
					Start = new EventDateTime()
					{
						DateTime = date.AddHours(7),
						TimeZone = GoogleCalendarTimeZone
					},
					End = new EventDateTime()
					{
						DateTime = date.AddHours(7.5),
						TimeZone = GoogleCalendarTimeZone
					},
					Recurrence = new List<string>(),
					Reminders = new Event.RemindersData()
					{
						UseDefault = false,
						Overrides = new EventReminder[]
						{
							new EventReminder() { Method = "popup", Minutes=24*60 }
						}
					}
				}, calendar.Id).Execute();
			}
		}
	}
}
