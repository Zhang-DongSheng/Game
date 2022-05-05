namespace Game
{
    public delegate void Function();

    public delegate void FunctionByInt(int value);

    public delegate void FunctionByInts(params int[] values);

    public delegate void FunctionBySingle(float value);

    public delegate void FunctionBySingles(params float[] values);

    public delegate void FunctionByDouble(double value);

    public delegate void FunctionByDoubles(params double[] values);

    public delegate void FunctionByString(string value);

    public delegate void FunctionByStrings(params string[] values);

    public delegate void FunctionByObject(object value);

    public delegate void FunctionByObjects(params object[] values);
}