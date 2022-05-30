using System;
using Newtonsoft.Json;

namespace SimpleTableManager.Models
{
	public class ConsoleColorSet
	{
		public ConsoleColor Foreground { get; set; }

		public ConsoleColor Background { get; set; }

		[JsonConstructor]
		public ConsoleColorSet(ConsoleColor foreground, ConsoleColor background)
		{
			Foreground = foreground;
			Background = background;
		}

		public ConsoleColorSet(ConsoleColorSet colorSet)
		{
			Foreground = colorSet.Foreground;
			Background = colorSet.Background;
		}

		public static implicit operator ConsoleColorSet((ConsoleColor foreground, ConsoleColor background) record)
		{
			return new ConsoleColorSet(record.foreground, record.background);
		}

		public override bool Equals(object obj)
		{
			if (obj is ConsoleColorSet another && another is not null)
			{
				return Foreground == another.Foreground && Background == another.Background;
			}

			return false;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
