using SimpleTableManager.Services.Functions;

namespace SimpleTableManager.Tests.FunctionTests;

[ExcludeFromCodeCoverage]
public class FunctionTests : TestBase
{
    [Test]
    public void ExceptionTest()
    {
        var fn = CreateFunction<IntegerType>(NumericFunctionOperator.Const, 1);

        CheckResults([fn.GetInType(), fn.GetOutType()], [typeof(long), typeof(long)]);

        CheckResult(fn.GetError(), "None");

        fn.SetError("ASD");

        CheckResult(fn.GetError(), "ASD");

        var ex = ((FunctionBase<NumericFunctionOperator, FractionType, FractionType>)fn).GetInvalidOperatorException();

        CheckResult(ex.GetType(), typeof(InvalidOperationException));
    }
}