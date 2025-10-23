namespace AMCode.Common.UnitTests.Extensions.Mocks
{
    /// <summary>
    /// Just an empty interface.
    /// </summary>
    public interface IEmptyInterface { }

    /// <summary>
    /// Has a single <see cref="string"/> <c>Value<c/> property.
    /// </summary>
    public interface ISingleValueProperty : IEmptyInterface
    {
        string Value { get; set; }
    }

    /// <summary>
    /// Has a single <c>Value</c> property.
    /// </summary>
    public class SingleValueProperty : ISingleValueProperty
    {
        public string Value { get; set; }
    }

    /// <summary>
    /// Has a two <see cref="string"/> <c>Value<c/> properties.
    /// </summary>
    public interface IDoubleValueProperty
    {
        string Value { get; set; }
        string Value2 { get; set; }
    }

    /// <summary>
    /// Has two <c>Value</c> properties.
    /// </summary>
    public class DoubleValueProperty : IDoubleValueProperty
    {
        public string Value { get; set; }
        public string Value2 { get; set; }
    }

    public interface ICloneObjectTestMain
    {
        int IntValue { get; set; }
        string StringValue { get; set; }
        int Subtract();
        string ToString();
        CloneObjectTestAddition AdditionObject { get; set; }
    }

    public class CloneObjectTestMain : ICloneObjectTestMain
    {
        public int SubtractValue1;
        public int SubtractValue2;

        public int IntValue { get; set; }
        public string StringValue { get; set; }

        public int Subtract() => new SubtractionClass(SubtractValue1, SubtractValue2).Subtract();

        public override string ToString() => $"{IntValue} {StringValue}";

        public CloneObjectTestAddition AdditionObject { get; set; }

        class SubtractionClass
        {
            private readonly int value1;
            private readonly int value2;

            public SubtractionClass(int value1, int value2)
            {
                this.value1 = value1;
                this.value2 = value2;
            }

            public int Subtract() => value1 - value2;
        }
    }

    public class CloneObjectTestAddition
    {
        public int Value1 { get; set; }
        public int Value2 { get; set; }

        public int AddValues() => Value1 + Value2;
    }
}