﻿namespace Yoga.Parser
{
	using System;

	public class BaseConverters
	{
		public static IValueConverter<TSource, TDestination> FromCast<TSource,TDestination>() => new RelayValueConverter<TSource, TDestination>((input) =>
		{
			try
			{
				return (true, (TDestination)Convert.ChangeType(input, typeof(TDestination)));
			}
			catch
			{
				return (false, default(TDestination));
			}
		});
	}
}
