namespace Yoga.Parser
{
	using Facebook.Yoga;
	using System;

	public class MarginPropertyRenderer : IPropertyRenderer
	{
		public MarginPropertyRenderer(string name, Action<YogaNode, YogaValue, YogaValue, YogaValue, YogaValue> render)
		{
			this.Name = name;
			this.render = render;
		}

		private Action<YogaNode, YogaValue, YogaValue, YogaValue, YogaValue> render;

		public string Name { get; }

		public Type Type => typeof(YogaValue[]);

		public virtual void Render(object parent, object value)
		{
			var output = (YogaValue[])value;

			if(output != null)
			{
				switch (output.Length)
				{
					case 1:
						output = new[] { output[0], output[0], output[0], output[0] };
						break;
					case 2:
					case 3:
						output = new[] { output[0], output[1], output[0], output[1] };
						break;
					default:
						output = new[] { output[0], output[1], output[2], output[3] };
						break;
				}


				this.render((YogaNode)parent, output[0], output[1], output[2], output[3]);
			}
		}
	}
}
