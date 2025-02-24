
namespace SimpleTableManager.Services.Functions;

[NamedArgument<string>(ArgumentName.Offset, "0")]
[FunctionMappingType(typeof(ConvertibleDateOnly))]
public class DateFunction : DateTimeFunctionBase<ConvertibleDateOnly, IConvertible>
{
	public override IEnumerable<IConvertible> ExecuteCore()
	{
		return Operator switch
		{
			DateTimeFunctionOperator.Const or
			DateTimeFunctionOperator.Now => UnwrappedUnnamedArguments,
			DateTimeFunctionOperator.Offset => Offset(),
			DateTimeFunctionOperator.Sub => Sub().Wrap(),
			DateTimeFunctionOperator.TotalDays => throw GetInvalidOperatorException(),

			_ => base.ExecuteCore()
		};
	}

	protected ConvertibleTimeSpan Sub()
	{
		return UnwrappedUnnamedArguments.Skip(1).Aggregate(UnwrappedUnnamedArguments.First().ToTimeSpan(), (a, c) => a.Subtract(c.ToTimeSpan()));
	}

	protected IEnumerable<ConvertibleDateOnly> Offset()
	{
		var offset = TimeSpan.Parse(GetNamedArgument<string>(ArgumentName.Offset));

		return UnwrappedUnnamedArguments.Select(a => a.Add(offset));
	}

	protected override IEnumerable<int> Years()
	{
		return UnwrappedUnnamedArguments.Select(a => a.Year);
	}

	protected override IEnumerable<int> Months()
	{
		return UnwrappedUnnamedArguments.Select(a => a.Month);
	}

	protected override IEnumerable<int> Days()
	{
		return UnwrappedUnnamedArguments.Select(a => a.Day);
	}

	public override Type GetOutType()
	{
		return Operator switch
		{
			DateTimeFunctionOperator.Const or
			DateTimeFunctionOperator.Now => typeof(ConvertibleDateOnly),

			_ => base.GetOutType()
		};
	}
}