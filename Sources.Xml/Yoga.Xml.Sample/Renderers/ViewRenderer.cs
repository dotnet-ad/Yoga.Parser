namespace Yoga.Xml.Sample
{
	using System.Xml.Linq;

	public class ViewRenderer<TView, TImpl> : XmlRenderer<TView, TImpl>
		where TView : IView
		where TImpl : TView
	{
		public override TView Render(XElement node)
		{
			var view = base.Render(node);

			view.Id = node.Attribute("Id")?.Name.LocalName;

			switch (node.Attribute("Background")?.Value)
			{
				case "Gray":
					view.BackgroundColor = new Color(246, 247, 249);
					break;

				case "Green":
					view.BackgroundColor = new Color(151, 220, 207);
					break;

				case "Black":
					view.BackgroundColor = new Color(48, 56, 70);
					break;

				default:
					view.BackgroundColor = new Color(255, 255, 255);
					break;
			}

			return view;
		}
	}
}