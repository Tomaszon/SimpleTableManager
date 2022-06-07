using System;
using System.Runtime.CompilerServices;

namespace SimpleTableManager.Models
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public class CommandReferenceAttribute : Attribute
	{
		public string MethodInformation { get; set; }

		public string MethodReference { get; set; }

		public CommandReferenceAttribute(string methodInformation = null, [CallerMemberName] string reference = null)
		{
			MethodInformation = methodInformation;
			MethodReference = reference;
		}
	}
}
