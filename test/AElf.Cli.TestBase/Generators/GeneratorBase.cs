namespace AElf.Cli.TestBase.Generators;

public abstract class GeneratorBase<T>
{
    protected internal T TestObjectInternal { get; set; }

    public T TestObject
    {
        get
        {
            BeforeReturn();
            return TestObjectInternal;
        }

        protected set => TestObjectInternal = value;
    }

    public T[] TestObjectNTimes(int n)
    {
        T testObject = TestObject;
        return Enumerable.Repeat(testObject, n).ToArray();
    }

    protected virtual void BeforeReturn()
    {
    }
}