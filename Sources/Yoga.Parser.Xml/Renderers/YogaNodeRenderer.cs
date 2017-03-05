namespace Yoga.Parser
{
	using Facebook.Yoga;

	public class YogaNodeRenderer : Renderer<YogaNode>
	{
		public YogaNodeRenderer()
		{
			this.RegisterAllTypeProperties();
		}

		protected override bool TryRenderProperty(YogaNode instance, string name, INode node)
		{
			switch (name)
			{
				case nameof(instance.Padding):
					var padding = node.Get<YogaValue[]>(name);
					instance.PaddingLeft = padding[0];
					instance.PaddingTop = padding[1];
					instance.PaddingRight = padding[2];
					instance.PaddingBottom = padding[3];
					return true;

				case nameof(instance.Margin):
					var margin = node.Get<YogaValue[]>(name);
					instance.MarginLeft = margin[0];
					instance.MarginTop = margin[1];
					instance.MarginRight = margin[2];
					instance.MarginBottom = margin[3];
					return true;

				default:
					return base.TryRenderProperty(instance, name, node);
				
			}
		}
	}
}
