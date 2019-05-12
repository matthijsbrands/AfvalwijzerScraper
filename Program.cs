using System;
using System.Collections.Generic;

namespace AfvalwijzerScraper
{
	/// <summary>
	/// Main function is called on startup.
	/// </summary>
	class Program
	{
		/// <summary>
		/// Function that is called on startup of app.
		/// </summary>
		/// <param name="args">Arguments</param>
		static void Main(string[] args)
		{
			ScraperController.ImportTrashpickupEvent();
		}
	}
}
