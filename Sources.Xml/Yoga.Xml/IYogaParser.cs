namespace Yoga.Xml
{
	using System.IO;
	using Facebook.Yoga;

	public interface IYogaParser
	{
		YogaNode Parse(Stream stream);
	}
}
