namespace Yoga.Parser.Sample
{
	public class ViewRenderer<TView, TImpl> : XmlRenderer<TView, TImpl>
		where TView : IView
		where TImpl : TView
	{
		public ViewRenderer()
		{
			this.RegisterAllTypeProperties(nameof(IView.Frame));
		}
	}
}