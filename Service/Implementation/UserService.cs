using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchBot.Model;
using TwitchBot.Model.WebSocket;

namespace TwitchBot.Service.Implementation
{
	public class UserService
	{
		private readonly DataStorage _dataStorage;
		private List<User> UserList;
		public UserService(DataStorage dataStorage)
		{
			_dataStorage = dataStorage;
			UserList = _dataStorage.GetUsers();
		}

		public User CreateUser(ReceiverEvent message)
		{
			if (UserList.Any(u => u.user_id == message.chatter_user_id))
				return null;

			var result = new User()
			{
				ChannelPoints = 500,
				user_id = message.chatter_user_id,
				Name = message.chatter_user_name,
			};

			_dataStorage.Create(result);

			UserList.Add(result);

			return result;
		}

	}
}
