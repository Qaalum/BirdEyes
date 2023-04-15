namespace BirdEyes.Shared //WHOLE THINK IS PROBABLY UNNECESSARY, KEEPING JUST IN CASE 
{
	public class Application //Add in-app purchases var 
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Shop { get; set; }
		public double Price { get; set; }
		public bool OnSale { get; set; }
		public double? Version { get; set; }
		public int? Rating { get; set; }
		public string PubName { get; set; }
		public string DevName { get; set; }
		public int Downloads { get; set; }


		public Application(string title, string shop, double price)
		{
			Title = title;
			Shop = shop;
			Price = price;
		}

		public Application(int id, string title, string shop, double price, bool onSale, double? version, int? rating, string pubName, string devName, int downloads)
		{
			Id = id;
			Title = title;
			Shop = shop;
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
