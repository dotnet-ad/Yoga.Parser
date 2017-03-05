namespace Yoga.Parser.Sample
{
	using System.Reflection;
	using Facebook.Yoga;
	using System.Collections.Generic;

	public class LayoutView : IView
	{
		public LayoutView(IYogaParser parser)
		{
			this.parser = parser;
			this.parser.RegisterConverter(ColorConverters.FromString());
		}

		public void Load(string name)
		{
			this.Id = name;
			var assembly = typeof(LayoutView).GetTypeInfo().Assembly;
			using (var stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.Layouts.{name}"))
			{
				this.node = this.parser.Read(stream);
				CalculateLayout();
			}
		}

		#region Fields

		private YogaNode node;

		private List<IView> views;

		private IYogaParser parser;

		private Rectangle frame;

		#endregion

		#region Properties

		public string Id { get; set; }

		public byte[] BackgroundColor { get; set; }

		public Rectangle Frame 
		{
			get => this.frame;
			set
			{
				if (this.frame != value)
				{
					this.frame = value;
					CalculateLayout();
				}
			}
		}

		Color IView.Background { get; set; }

		public IEnumerable<IView> Views => views;

		#endregion

		public void RegisterRenderer<T, TImpl>()
			where T : IView
			where TImpl : T
		{
			this.parser.RegisterNodeRenderer(new ViewRenderer<T, TImpl>(this.parser));
		}

		#region Layout

		private void CalculateLayout()
		{
			this.views = new List<IView>();
			this.GetSubviews(this.node, views);

			this.node.Width = this.Frame.Width;
			this.node.Height = this.Frame.Height;
			this.node.CalculateLayout();
			this.Sublayout(0,0,this.node);
		}

		private void Sublayout(float x, float y, YogaNode n)
		{
			var view = n.Data as IView;

			if(view != null)
			{
				x += n.LayoutX;
				y += n.LayoutY;
				view.Frame = new Rectangle(x, y, n.LayoutWidth, n.LayoutHeight);

				foreach (var item in n)
				{
					Sublayout(x,y,item);
				}
			}
		}

		private void GetSubviews(YogaNode n, List<IView> result)
		{
			var view = n.Data as IView;
			result.Add(view);
			foreach (var item in n)
			{
				GetSubviews(item,result);
			}
		}

		#endregion

	}
}
