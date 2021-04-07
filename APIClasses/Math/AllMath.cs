using System.Collections.Generic;

namespace APIClasses.Math
{
    //TODO - move all of these out into separate classes
    public class MathResult
    {
        public int Result;

        public MathResult(int result)
        {
            Result = result;
        }
    }

    public class BooleanResult
    {
        public bool Result;

        public BooleanResult(bool result)
        {
            Result = result;
        }
    }

    public class OneValueInput
    {
        public int Value;
    }

    public class TwoValueInput
    {
        public int Value1;
        public int Value2;
    }
    
    public class ThreeValueInput
    {
        public int Value1;
        public int Value2;
        public int Value3;
    }
}
