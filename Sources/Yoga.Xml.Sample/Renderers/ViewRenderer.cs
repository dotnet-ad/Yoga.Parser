namespace Yoga.Parser.Sample
{
	public class ViewRenderer<TView, TImpl> : NodeRenderer<TView, TImpl>
		where TView : IView
		where TImpl : TView
	{
		public ViewRenderer(IYogaParser parser) : base(parser)
		{
			this.RegisterAllTypePropertyRenderers(nameof(IView.Frame));
		}
	}
}