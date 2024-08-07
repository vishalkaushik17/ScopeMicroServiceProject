namespace GenericFunction.ResultObject;

public interface ITrace
{
	void AddTrace(string className, string? methodName, bool enable = false, string? message = null, dynamic? dataMessage = null);
	double TimeConsumption();
	string GetTraceLogs(string? message);
	static string LastMethod { get; set; } = string.Empty;
	static string LastClass { get; set; } = string.Empty;
	static string? MainClass { get; set; }
	static string? MainMethod { get; set; }

}