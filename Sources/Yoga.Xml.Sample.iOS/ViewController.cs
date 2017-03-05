namespace Yoga.Parser.Sample.iOS
{
	using System;
	using UIKit;
	using CoreGraphics;

	public partial class ViewController : UIViewController
	{
		#region Native view

		public class YogaView : UIView, IView
		{
			public string Id { get; set; }

			Color IView.BackgroundColor 
			{ 
				get
				{
					var r = (byte)(this.BackgroundColor.CGColor.Components[0] * 255f);
					var g = (byte)(this.BackgroundColor.CGColor.Components[1] * 255f);
					var b = (byte)(this.BackgroundColor.CGColor.Components[2] * 255f);
					return new Color(r, g, b);
				}
				set => this.BackgroundColor = UIColor.FromRGB(value.R, value.G, value.B);
			}

			Rectangle IView.Frame 
			{ 
				get => new Rectangle((float)this.Frame.X, (float)this.Frame.Y, (float)this.Frame.Width, (float)this.Frame.Height);
				set => this.Frame = new CGRect(value.X, value.Y, value.Width, value.Height);
			}
		}

		public IView Create(string name) => new YogaView();

		#endregion

		private LayoutView layout;

		protected ViewController(IntPtr handle) : base(handle) { }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			this.layout = new LayoutView();
			this.layout.RegisterRenderer<IView, YogaView>();
			this.layout.Load("Sample.xml");

 			foreach (var view in this.layout.Views)
			{
				var native = view as YogaView;
				this.View.AddSubview(native);
			}
		}

		private void ApplyLayout() => this.layout.Frame = new Rectangle(0,0, (float)this.View.Frame.Width, (float)this.View.Frame.Height);

		public override void ViewWillLayoutSubviews()
		{
			ApplyLayout();
			base.ViewWillLayoutSubviews();
		}
	}
}
