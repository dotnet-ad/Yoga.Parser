namespace Yoga.Parser.Sample
{
	public class ViewRenderer<TView, TImpl> : XmlRenderer<TView, TImpl>
		where TView : IView
		where TImpl : TView
	{
		public override TView Render(INode node)
		{
			var view = base.Render(node);
			view.Id = node.Get<string>("Id");
			view.BackgroundColor = node.Get<Color>("Background");
			return view;
		}
	}
}