using System.Text.Json.Serialization;

namespace BirdEyes.Shared //WHOLE THINK IS PROBABLY UNNECESSARY, KEEPING JUST IN CASE 
{
	public class Application //Add in-app purchases var 
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public double Price { get; set; }
		public bool OnSale { get; set; }
		public double? Version { get; set; }
		public int? Rating { get; set; }
		public string PubName { get; set; }
		public string DevName { get; set; }
		public int Downloads { get; set; }


		[JsonConstructor]
		public Application(string title, double price)
		{
			Title = title;
			Price = price;
		}

		public Application(int id, string title, double price, bool onSale, double? version, int? rating, string pubName, string devName, int downloads)
		{
			Id = id;
			Title = title;
			Price = price;
			OnSale = onSale;
			Version = version;
			Rating = rating;
			PubName = pubName;
			DevName = devName;
			Downloads = downloads;
		}
	}
}
