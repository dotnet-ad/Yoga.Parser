using System.Reflection;

namespace Yoga.Parser
{
	public static class ArrayConverters
	{
		public static IValueConverter<TItem, TItem[]> FromSingleItem<TItem>() => new RelayValueConverter<TItem, TItem[]>((input) =>
		 {
#pragma warning disable RECS0017 // Possible compare of value type with 'null'
			 if (!typeof(TItem).GetTypeInfo().IsValueType && input == null)
#pragma warning restore RECS0017 // Possible compare of value type with 'null'
				 return (true, new TItem[0]);
			 return (true, new[] { input });
		 });

		public static IValueConverter<string, string[]> Split(char separator = ',') => new RelayValueConverter<string, string[]>((input) =>
		{
			return (true, input.Split(separator));
		});
	}
}
