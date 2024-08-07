namespace GenericFunction.ResultObject;

public class ExceptionOnMethod
{
    public string? MethodName { get; set; } = string.Empty;
    public string? ClassName { get; set; } = string.Empty;
    public int AtLineNo { get; set; } = 0;

    public override string ToString()
    {
        return $"<b>Class : </b> {ClassName} <br> <b>Method : </b>{MethodName} <br> <b>Line No :</b> {AtLineNo}";
    }
}