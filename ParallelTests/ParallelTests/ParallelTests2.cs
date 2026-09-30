namespace ParallelTests;

public class ParallelTests2
{
    [Fact]
    public void Test1() => Thread.Sleep(3000);

    [Fact]
    public void Test2() => Thread.Sleep(3000); 

    [Theory]
    [InlineData(0), InlineData(1), InlineData(2)]
    public void Test3(int _) => Thread.Sleep(3000);           
}
