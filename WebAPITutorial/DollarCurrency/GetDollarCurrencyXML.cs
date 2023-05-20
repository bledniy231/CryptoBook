using System.Xml.Linq;

namespace WebAPITutorial.DollarCurrency
{
	public class GetDollarCurrencyXML : IDollarCurrency
	{
		public async Task<decimal> GetDollarCurrencyAsync()
		{
			HttpClient client = new HttpClient();
			HttpResponseMessage response = await client.GetAsync("https://www.cbr-xml-daily.ru/daily.xml");
			string? content = await response.Content.ReadAsStringAsync();
			XDocument xdoc = XDocument.Parse(content);
			string? dollar = xdoc?.Element("ValCurs")?.
				Elements("Valute").
				Where(x => x?.Attribute("ID")?.Value == "R01235").
				Select(x => x?.Element("Value")?.Value).FirstOrDefault();
			if (Decimal.TryParse(dollar, out decimal result))
				return result;
			else return Decimal.Zero;
		}
	}
}
