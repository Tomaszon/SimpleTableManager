using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SimpleTableManager.Models
{
	public class FunctionParameter<T>
	{
		public T? ConstValue { get; set; }

		public Position? Reference { get; set; }

		public bool? HorizontallyLocked { get; set; }

		public bool? VerticallyLocked { get; set; }

		public static explicit operator T?(FunctionParameter<T> parameter)
		{
			return parameter.ConstValue;
		}

		public static explicit operator FunctionParameter<T>(T value)
		{
			return new FunctionParameter<T>() { ConstValue = value };
		}
	}
}