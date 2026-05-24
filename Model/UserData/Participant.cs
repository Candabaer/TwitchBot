using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot.Model.UserData
{
	public class Participant
	{
		public User User;
		public int Betting;
		public bool Prediction;

		public Participant(User user, int betting, bool predication)
		{
			this.User = user;
			Betting = betting;
			Prediction = predication;
		}

		public void Resolve(bool result)
		{
			if (result == Prediction)
				User.ChannelPoints += Betting;
			else
				User.ChannelPoints -= Betting;
		}
	}
}
