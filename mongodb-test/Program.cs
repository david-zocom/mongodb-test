using System;
using MongoDB.Driver.Core;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Linq;

namespace mongodb_test
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
			MongoClient client = new MongoClient("mongodb://localhost:27017");
			var db = client.GetDatabase("animaldb");
			var collection = db.GetCollection<BsonDocument>("animaldb");

			/* Add one document
			var one = new BsonDocument
			{
				{"name", "MongoDB" },
				{"type", "Database" },
				{"count",1 },
				{"info", new BsonDocument
					{
						{"x",203 },
						{"y",102 }
					}}
			};
			collection.InsertOne(one);*/
			Console.WriteLine("Looking for Mongoose...\n");
			const string mongoose = "Mongoose";
			var another = new BsonDocument
			{
				{"name", mongoose },
				{"value", 10}
			};
			var filter = Builders<BsonDocument>.Filter.Eq("name", mongoose);
			var found = collection.Find(filter);//new BsonDocument { { "name", mongoose } });
			if (found.Count() < 1)
			{
				Console.WriteLine("Not found");
				collection.InsertOne(another);
			}
			else
			{
				Console.WriteLine("Found " + found.Count() + ": " + found.First().ToString());
				int value = found.First()["value"].AsInt32;
				var update = Builders<BsonDocument>.Update.Set("value", value + 1);
				collection.UpdateOne(filter, update);
			}
			Console.WriteLine("");
			/* Insert many
			var many = Enumerable.Range(1, 100).Select(i => new BsonDocument("counter", i));
			collection.InsertMany(many);
			*/

			var count = collection.Count(new BsonDocument());
			Console.WriteLine($"Db has {count} documents.");

			var first = collection.Find(new BsonDocument()).FirstOrDefault();
			Console.WriteLine("First: " + first.ToString());

			var all = collection.Find(new BsonDocument()).ToEnumerable();
			foreach(var x in all)
				Console.WriteLine("-> " + x.ToString());

			Console.WriteLine("press any key to exit");
			Console.ReadKey();
		}
	}
}
