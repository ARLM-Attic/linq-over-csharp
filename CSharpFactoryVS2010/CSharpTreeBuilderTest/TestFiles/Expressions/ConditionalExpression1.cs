public class ConditionalExpression
{
    public void DummyMethod()
    {
        var i1 = true ? 0 : 1;

        // right-associativity test
        var i2 = true ? 2 : false ? 3 : 4;

        var i3 = (5 is int) ? (int)6 : 7;
        var i4 = 5 is int ? 6 : 7;
        var i5 = 5 is int ? (int)6 : 7;
    }
}
