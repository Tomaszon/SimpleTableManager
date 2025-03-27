using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace SimpleTableManager.Tests
{
	[MemoryDiagnoser, ExcludeFromCodeCoverage]
	public class Benchmark
	{
		[GlobalSetup]
		public static void SetUp()
		{
			BorderCharacters.FromJson(@"Configs/borderCharacters.json");
		}

		[Benchmark]
		public void StringAlloc1()
		{
			var s = new string(BorderCharacters.Get(BorderType.Up), 10);
		}

		[Benchmark]
		public void SpanAlloc1()
		{
			var s = new ReadOnlySpan<char>([.. Enumerable.Repeat(BorderCharacters.Get(BorderType.Up), 10)]);
		}

		[Benchmark]
		public void StringAlloc2()
		{
			string value = "fraction";

			var s = $"{value.First().ToString().ToUpper()}{new string([.. value.Skip(1)])}";
		}

		[Benchmark]
		public void SpanAlloc2()
		{
			string value = "fraction";

			var s = $"{char.ToUpper(value.First())}{value.AsSpan(1)}";
		}

		[Benchmark]
		public void ArrayAlloc2()
		{
			string value = "fraction";

			var s = $"{char.ToUpper(value.First())}{string.Concat(value.ToArray()[1..])}";
		}

	}

	[ExcludeFromCodeCoverage]
	public class BenchmarkProgram
	{
		public static void Main(string[] args)
		{
			var summary = BenchmarkRunner.Run<Benchmark>();
		}
	}
}