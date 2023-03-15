using System.Text.Json.Serialization;

namespace BirdEyes.Shared
{
	public class ITADGameModel
	{
		public string Title { get; set; }
		public double Price { get; set; }
		public string Store { get; set; }
		public bool OnSale { get; set; }

		[JsonConstructor]
		public ITADGameModel(string title, double price, string store)
		{
			Title=title;
			Price=price;
			Store=store;
		}
		public ITADGameModel(string title, double price)
		{
			Title = title;
			Price=price;
		}
	}
}
