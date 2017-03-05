namespace Yoga.Xml.Sample
{
	public interface IView
	{
		string Id { get; set; }

		Rectangle Frame { get; set; }

		Color BackgroundColor { get; set; }
	}
}
