using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SimpleTableManager.Services
{
	public class ClearPropertyContractResolver : DefaultContractResolver
	{
		protected override JsonArrayContract CreateArrayContract(Type objectType)
		{
			var contract = base.CreateArrayContract(objectType);

			contract.OnDeserializingCallbacks.Add((o, c) =>
			{
				if (o is IList array && array is not null)
				{
					array.Clear();
				}
			});

			return contract;
		}

		protected override JsonDictionaryContract CreateDictionaryContract(Type objectType)
		{
			var contract = base.CreateDictionaryContract(objectType);

			contract.OnDeserializingCallbacks.Add((o, c) =>
			{
				if (o is IDictionary dictionary && dictionary is not null)
				{
					dictionary.Clear();
				}
			});

			return contract;
		}
	}
}