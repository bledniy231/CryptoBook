using Newtonsoft.Json.Linq;

namespace WebAPITutorial.DollarCurrency
{
    public class DollarCurrencyJSON : IDollarCurrency
    {
        public async Task<decimal> GetDollarCurrencyAsync()
        {
			HttpClient client = new HttpClient();
			HttpResponseMessage response = await client.GetAsync("https://www.cbr-xml-daily.ru/daily_json.js");
			string? content = await response.Content.ReadAsStringAsync();
			JToken? dollar = new JValue(0);
			dollar = JObject.Parse(content)?["Valute"]?["USD"]?["Value"] ?? 0;
			return dollar.Value<decimal>();
		}
    }
}
