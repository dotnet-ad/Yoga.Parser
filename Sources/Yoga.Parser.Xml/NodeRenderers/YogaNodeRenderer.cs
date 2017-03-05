namespace Yoga.Parser
{
	using Facebook.Yoga;

	public class YogaNodeRenderer : NodeRenderer<YogaNode>
	{
		public YogaNodeRenderer(IYogaParser parser) : base(parser)
		{
			this.RegisterAllTypePropertyRenderers();
			this.RegisterPropertyRenderer(new MarginPropertyRenderer(nameof(YogaNode.Margin), UpdateMargin));
			this.RegisterPropertyRenderer(new MarginPropertyRenderer(nameof(YogaNode.Padding), UpdatePadding));
		}

		private void UpdateMargin(YogaNode node, YogaValue left, YogaValue top, YogaValue right, YogaValue bottom)
		{
			node.MarginLeft = left;
			node.MarginTop = top;
			node.MarginRight = right;
			node.MarginBottom = bottom;
		}

		private void UpdatePadding(YogaNode node, YogaValue left, YogaValue top, YogaValue right, YogaValue bottom)
		{
			node.PaddingLeft = left;
			node.PaddingTop = top;
			node.PaddingRight = right;
			node.PaddingBottom = bottom;
		}
	}
}
