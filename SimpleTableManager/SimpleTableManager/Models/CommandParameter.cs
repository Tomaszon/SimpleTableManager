﻿using System;

namespace SimpleTableManager.Models
{
	public class CommandParameter
	{
		public Type Type { get; set; }

		public string Name { get; set; }

		public bool Optional { get; set; }

		public override string ToString()
		{
			return $"{{{Name}:{Type.Name}{(Optional ? ":optional" : "")}}}";
		}
	}
}
