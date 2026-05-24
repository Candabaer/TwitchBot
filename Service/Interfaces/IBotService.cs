namespace TwitchBot.Service.Interfaces
{
	public interface IBotService
	{
		Task Run(CancellationToken cancellationToken);
	}
}