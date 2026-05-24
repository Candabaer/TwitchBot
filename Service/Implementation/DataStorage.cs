using System.Text.Json;
using TwitchBot.Model;

namespace TwitchBot.Service.Implementation
{
	public class DataStorage
	{
		public void Create(User user)
		{
			var json = JsonSerializer.Serialize(user, new JsonSerializerOptions
			{
				WriteIndented = true
			});

			File.WriteAllText("TwitchBotData.json", json);
		}
		public void Update()
		{
			throw new NotImplementedException();
		}
		public void Delete()
		{
			throw new NotImplementedException();
		}
		public List<User> GetUsers()
		{
			if (!File.Exists("TwitchBotData.json"))
				return new List<User>();

			var json = File.ReadAllText("TwitchBotData.json");
			var result = JsonSerializer.Deserialize<List<User>>(json);
			return result;
		}
		public void Get(int Id)
		{
			throw new NotImplementedException();
		}
	}
}
