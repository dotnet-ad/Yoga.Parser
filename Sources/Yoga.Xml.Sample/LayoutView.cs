namespace Yoga.Parser.Sample
{
	using System.IO;
	using System.Reflection;
	using Facebook.Yoga;
	using System.Collections.Generic;

	public class LayoutView : IView
	{
		public LayoutView( float density = 1.0f)
		{
			this.parser = new YogaParser();
			this.parser.RegisterValueParser(new YogaValueParser(density));
			this.parser.RegisterValueParser(new ColorParser());
		}

		public void Load(string name)
		{
			this.Id = name;
			var assembly = typeof(LayoutView).GetTypeInfo().Assembly;
			using (var stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.Layouts.{name}"))
			using (var reader = new StreamReader(stream))
			{
				var xml = reader.ReadToEnd();
				this.node = this.parser.Parse(xml);
				CalculateLayout();
			}
		}

		#region Fields

		private YogaNode node;

		private List<IView> views;

		private YogaParser parser;

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
			this.parser.Register(new ViewRenderer<T, TImpl>());
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
