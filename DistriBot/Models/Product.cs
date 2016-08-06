using System;
namespace DistriBot
{
	public class Product
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public double UnitPrice { get; set; }

		public Product(int id, string name, double unitPrice)
		{
			this.Id = id;
			this.Name = name;
			this.UnitPrice = unitPrice;
		}
	}
}
