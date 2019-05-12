using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AfvalwijzerScraper
{
	/// <summary>
	/// Scraperservice using selenium and Chrome webdriver
	/// </summary>
	class ScraperService
	{
		/// <summary>
		/// WebDriver object
		/// </summary>
		public IWebDriver Driver;

		/// <summary>
		/// Constructor function
		/// </summary>
		/// <param name="postalcode">String containing postalcode</param>
		/// <param name="housenumber">String containing housenumber</param>
		/// <param name="housenumbersuffix">String containing housenumbersuffix</param>
		public ScraperService(string postalcode, string housenumber, string housenumbersuffix)
		{
			string urlFormat = "https://www.mijnafvalwijzer.nl/nl/{0}/{1}/";
			if(!String.IsNullOrEmpty(housenumbersuffix))
			{
				urlFormat += "{2}/";
			}

			Driver = new ChromeDriver(@"C:\Projects\HadeejerScraper\bin\Debug\netcoreapp2.1");
			Driver.Url = String.Format(urlFormat, postalcode.Replace(" ", "").Trim(), housenumber.Trim(), housenumbersuffix.Trim());
		}

		/// <summary>
		/// Gets dates of trash pickup off of afvalwijzer.
		/// </summary>
		/// <param name="classname">CSS classnames of trashtype</param>
		/// <returns></returns>
		public List<DateTime> GetPickupDates(string classname)
		{
			string text;
			DateTime dt;
			var elements = Driver.FindElements(By.CssSelector(".ophaaldagen ." + classname));
			List<DateTime> dates = new List<DateTime>();
			foreach (IWebElement el in elements)
			{
				text = el.GetAttribute("innerHTML").Trim().Split("<br>")[0].Trim() + " " + DateTime.Now.Year;
				dt = DateTime.ParseExact(text, "dddd d MMMM yyyy", null);
				if (dt <= DateTime.Today)
				{
					continue;
				}

				dates.Add(dt);
			}
			return dates;
		}

		/// <summary>
		/// Calls the quit function of the webdriver
		/// </summary>
		public void Quit()
		{
			Driver.Quit();
		}
	}
}
