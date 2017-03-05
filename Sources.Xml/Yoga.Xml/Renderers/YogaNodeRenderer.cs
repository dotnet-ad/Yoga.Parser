namespace Yoga.Xml
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using Facebook.Yoga;

	public class YogaNodeRenderer : Renderer<YogaNode>
	{
		#region Property caching

		private static Dictionary<string, PropertyInfo> nodePropertySetters = typeof(YogaNode).GetRuntimeProperties()
																					 .ToDictionary(x => x.Name, x => x);

		#endregion

		public override YogaNode Render(INode node)
		{
			var result = base.Render(node);

			foreach (var p in node.Properties)
			{
				switch (p)
				{
					case nameof(result.Padding):
						var padding = node.Get<YogaValue[]>(p);
						result.PaddingLeft = padding[0];
						result.PaddingTop = padding[1];
						result.PaddingRight = padding[2];
						result.PaddingBottom = padding[3];
						break;
					case nameof(result.Margin):
						var margin = node.Get<YogaValue[]>(p);
						result.MarginLeft = margin[0];
						result.MarginTop = margin[1];
						result.MarginRight = margin[2];
						result.MarginBottom = margin[3];
						break;
					default:
						PropertyInfo property;
						if(nodePropertySetters.TryGetValue(p, out property))
						{
							var v = node.Get(p, property.PropertyType);
							property.SetValue(result, v);
						}
						break;
				}
			}

			return result;
		}
	}
}
