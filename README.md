# Yoga.Xml

Declare your [Yoga](https://facebook.github.io/yoga/) layouts in XML.

## Install

Available on NuGet

[![NuGet](https://img.shields.io/nuget/v/Yoga.Xml.svg?label=NuGet)](https://www.nuget.org/packages/Yoga.Xml/)

## Quick-start (Xamarin.iOS)

**Sample.xml**

```xml
<View Padding="20" Background="White">
	<View Background="Gray" Height="200"/>
	<View Padding="10" Background="Green" FlexGrow="1" FlexDirection="Row">
		<View Margin="10" Background="Black" Width="20" Height="20" />
		<View Margin="10" Position="Bottom" AlignSelf="FlexEnd" Background="Black" FlexGrow="1" Height="20" />
		<View Margin="10" Background="Black" Width="20" Height="20" />
	</View>
    <View Background="Black" FlexGrow="1"  AlignSelf="Center" Width="100" />
</View>
```

**ViewRenderer.cs**

```csharp
public class ViewRenderer : XmlRenderer<UIView>
{
	public ViewRenderer() : base("View") { }
	
	public override UIView Render(XElement node)
	{
		var view = base.Render(node);
		switch (node.Attribute("Background")?.Value)
		{
			case "Gray":
				view.BackgroundColor = UIColor.FromRGB(246, 247, 249);
				break;

			case "Green":
				view.BackgroundColor = UIColor.FromRGB(151, 220, 207);
				break;

			case "Black":
				view.BackgroundColor = UIColor.FromRGB(48, 56, 70);
				break;

			default:
				view.BackgroundColor = UIColor.FromRGB(255, 255, 255);
				break;
		}
	}
}
```

**ViewController.cs**

```csharp
public partial class ViewController : UIViewController
{
	private YogaNode node;

	protected ViewController(IntPtr handle) : base(handle) { }

   public string LoadFromAssembly(string name)
		{
			var assembly = this.GetType().GetTypeInfo().Assembly;
			using (var stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{name}"))
			using (var reader = new StreamReader(stream))
			{
				return reader.ReadToEnd();
			}
		}

	public override void ViewDidLoad()
	{
		base.ViewDidLoad();
		
		var xml = LoadFromAssembly("Sample.xml");

		var parser = new YogaParser(); 
		parser.Register(new ViewRenderer());
		
		this.node = parser.Parse(xml);
		this.AddSubviews(this.node);
	}
	
	private void AddSubviews(YogaNode node)
	{
		var native = node.Data as UIView;
		foreach (var child in node)
		{
			this.AddSubview(child);
		}
	}
	
	private void ApplyLayouts(nfloat x, nfloat y, YogaNode node)
	{
		var native = node.Data as UIView;
		
		x+= node.LayoutX;
		y+= node.LayoutY;
		
		native.Frame = new CGRect(x,y , node.LayoutWidth, node.LayoutHeight);
		
		foreach (var child in node)
		{
			this.ApplyLayout(x,y,child);
		}
	}

	public override void ViewWillLayoutSubviews()
	{
		this.node.Width = (float)this.View.Frame.Width;
		this.node.Height = (float)this.View.Frame.Height;
		this.node.CalculateLayout();
		this.ApplyLayouts(this.node);

		base.ViewWillLayoutSubviews();
	}
}
```

## Complete sample

For a nicer implementation, with multiple targeted platforms, look at the `Samples` projects in the sources of the repo.

## Roadmap / Ideas

* Create a `C#` code generator sample from layouts

## Contributions

Contributions are welcome! If you find a bug please report it and if you want a feature please report it.

If you want to contribute code please file an issue and create a branch off of the current dev branch and file a pull request.

### License

![MIT © Aloïs](https://img.shields.io/badge/licence-MIT-blue.svg) 

© [Aloïs Deniel](http://aloisdeniel.github.io)
