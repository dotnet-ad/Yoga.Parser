namespace Yoga.Parser.Sample
{
	public interface IView
	{
		string Id { get; set; }

		Rectangle Frame { get; set; }

		Color Background { get; set; }
	}
}
