namespace BrewUp.Shared.Entities
{
    public class LastEventPosition : DtoBase
	{
		public ulong CommitPosition { get; set; }
		public ulong PreparePosition { get; set; }
	}
}
