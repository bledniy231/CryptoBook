namespace WebAPITutorial.DollarCurrency
{
	public interface IDollarCurrency
	{
		Task<decimal> GetDollarCurrencyAsync();
	}
}
