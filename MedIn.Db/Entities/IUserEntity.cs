namespace MedIn.Db.Entities
{
	public interface IUserEntity : IEntity
	{
		string Username { get; set; }
		string Email { get; set; }
	}
}