
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

			_ => base.ExecuteCore()
		};
	}

	// protected ConvertibleDateOnly Sum()
	// {
	// 	// return UnwrappedUnnamedArguments.Aggregate(DateOnly.MinValue, (a, c) =>
	// 	// 	a.AddYears(c.Year).AddMonths(c.Month).AddDays(c.Day))
	// 	// 	.AddYears(-DateOnly.MinValue.Year).AddMonths(-DateOnly.MinValue.Month).AddDays(-DateOnly.MinValue.Day);
	// 	throw new NotImplementedException();
	// }

	protected ConvertibleTimeSpan Sub()
	{
		throw new NotImplementedException();
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

	protected override IEnumerable<double> TotalYears()
	{
		return UnwrappedUnnamedArguments.Select(a => a.Year + a.Month / (double)MONTHS_IN_A_YEAR + a.Day / DAYS_IN_A_MONTH);
	}

	protected override IEnumerable<double> TotalMonths()
	{
		return UnwrappedUnnamedArguments.Select(a => a.Year * MONTHS_IN_A_YEAR + a.Month + a.Day / DAYS_IN_A_MONTH);
	}

	protected override IEnumerable<double> TotalDays()
	{
		return UnwrappedUnnamedArguments.Select(a => a.Year * DAYS_IN_A_YEAR + a.Month * DAYS_IN_A_MONTH + a.Day);
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