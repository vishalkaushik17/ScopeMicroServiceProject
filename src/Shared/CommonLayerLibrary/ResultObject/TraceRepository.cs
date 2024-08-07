using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Text;
using static GenericFunction.CommonMessages;
namespace GenericFunction.ResultObject;

public class TraceRepository : ITrace
{
	public string? UserId { get; set; }
	public IHttpContextAccessor _httpContextAccessor { get; set; }
	public string? SessionId { get; set; }
	public string? ScopeId { get; set; }
	public string ClientMachineName { get; set; }
	public string? ClientMachineIp { get; set; }

	public static List<TraceEntity>? TraceLog;
	public static Dictionary<string, List<string>>? ActionLogs { get; set; }
	private long _Startwatch;
	private long _Endwatch;

	private Clock _clock;

	public static string? LastClass { get; set; }
	public static string? LastMethod { get; set; }
	public static string? MainClass { get; set; }
	public static string? MainMethod { get; set; }

	public static int Nos { get; set; }


	public TraceRepository(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor;
		try
		{
			_clock = new Clock
			{
				StartDateTime = DateTime.Now.ToString("T")
			};

			TraceLog = new List<TraceEntity>();
			ActionLogs = new Dictionary<string, List<string>>();  //new List<string>();
			Nos = 0;
			UserId = httpContextAccessor?.HttpContext?.User?.FindFirst("Id")?.Value;
			ClientMachineIp = httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.MapToIPv4().ToString();
			ClientMachineName = System.Environment.MachineName + " - Ip :" + httpContextAccessor?.HttpContext?.Request?.Host.Value;
		}
		catch (Exception ex)
		{
			ex.SendExceptionMailAsync().GetAwaiter().GetResult();
			throw;
		}
	}
	public void AddTrace(string className, string? methodName, bool enable = false, string? message = null, dynamic? dataMessage = null)
	{
		if (!enable)
			return;

		SessionId = _httpContextAccessor?.HttpContext?.GetHeader("TokenSessionId");
		ScopeId = _httpContextAccessor?.HttpContext?.GetHeader("TokenScopeId");
		if (string.IsNullOrWhiteSpace(SessionId) || string.IsNullOrWhiteSpace(ScopeId))
			return;


		UserId = _httpContextAccessor?.HttpContext?.GetHeader("UserId");


		if (string.IsNullOrEmpty(message)) message = "N/A";

		Nos++;

		if (!ActionLogs.ContainsKey(ScopeId ?? "Default"))
		{
			ActionLogs.Add(ScopeId ?? "Default", new List<string> { "" });
		}
		var list = ActionLogs[ScopeId ?? "Default"];

		if (message == OperationStart.MarkProcess() ||
			message == OperationStart
			)
		{
			_clock = new Clock
			{
				StartDateTime = DateTime.Now.ToString("T"),
				StartTimeSpan = Stopwatch.GetTimestamp(),
			};
			if (_Startwatch == 0)
			{
				_Startwatch = Stopwatch.GetTimestamp();
			}
			TraceLog?.Add(new TraceEntity()
			{
				ApiAction = $"{className}-{methodName}",
				Timer = _clock,
				Stopwatch = System.Diagnostics.Stopwatch.StartNew(),
				UserId = UserId ?? "Default",
				ClientMachine = ClientMachineName,
				ClientIp = ClientMachineIp,
				ScopeId = ScopeId,
				SessionId = SessionId
			});
			//ActionLogs.Add("<strong>Start On: " + clock.StartDateTime + "</strong>");
			if (!list.Any(m => m.Contains(" Start!", StringComparison.OrdinalIgnoreCase) || m.Contains(" Client Ip!", StringComparison.OrdinalIgnoreCase)))
			{
				list.Add("<span class ='logNo' style='color:black;'> " + Nos
				+ "</span> &nbsp :: &nbsp" + DateTime.Now.ToString("h: mm:ss tt")
				+ " : " + $"{className}-{methodName}" + " : " + " - Client Ip : " + ClientMachineIp + " : Hosted Server : " + ClientMachineName + " - <span style='color:blue;'> ScopeId :</span> " + ScopeId + " - <span style='color:darkgreen;'> User Id :</span> " + UserId);
			}

			Nos++;
			MainMethod ??= methodName;
			MainClass ??= className;
		}

		if (message == OperationEnd.MarkProcess() ||
			message == OperationEnd ||
			message == OperationFailed.MarkProcess() ||
			message == OperationFailed ||
			message == OperationSuccessful ||
			message == OperationSuccessful.MarkProcess() ||
			message == InvalidCredentials ||
			message == InvalidCredentials.MarkProcess()
			)
		{
			_clock = new Clock
			{
				EndDateTime = DateTime.Now.ToString("T")
			};
			//_watch = System.Diagnostics.Stopwatch.StartNew();
			var tl = TraceLog?.FirstOrDefault(x => x.ScopeId == ScopeId && x.ApiAction == $"{className}-{methodName}" && x.Timer?.EndDateTime == null);
			if (tl?.Timer != null)
			{
				tl.Timer.EndDateTime = DateTime.Now.ToString("T");
				tl.Stopwatch?.Stop();

				if (tl.Stopwatch != null) tl.Timer.EndTimeSpan = Stopwatch.GetTimestamp();
				if (!ActionLogs.ContainsKey(ScopeId ?? "Default"))
				{
					ActionLogs.Add(ScopeId ?? "Default", new List<string> { });
				}
				//var list = ActionLogs[_sessionId];
				//ActionLogs = new List<string> { "<strong>End On: " + tl.Timer.EndDateTime + "</strong>" };
				//ActionLogs.Add("<span>Time taken " + tl.Timer.TimeSpan + "</span>");
				var timeSpan = Stopwatch.GetElapsedTime(tl.Timer.StartTimeSpan, tl.Timer.EndTimeSpan).TotalSeconds;
				if (timeSpan > 3)
				{
					list?.Add($"<span class ='logNo' style='color:red;'>   {Nos}"
											+ $"</span> &nbsp :: &nbsp {DateTime.Now.ToString("h: mm:ss tt")}"
											+ $": {className} : {message} Process completed in : {timeSpan.ToString("##000.000")}  seconds! {dataMessage}  </span>");
				}
				else
				{
					list?.Add($"<span class ='logNo' style='color:black;'>  {Nos}"
											+ $"</span> &nbsp :: &nbsp {DateTime.Now.ToString("h: mm:ss tt")}"
											+ $": {className} : {message} Process completed in : {timeSpan.ToString("##000.000")}  seconds! {dataMessage}  </span>");
				}

				//ActionLogs?.Add("<span class ='logNo' style='color:red;'> " + Nos);
				TraceLog?.Remove(tl);
			}

			return;
		}

		list?.Add($"<span class ='logNo' style='color:black;'>  {Nos}"
				+ $"</span> &nbsp :: &nbsp {DateTime.Now.ToString("h: mm:ss tt")}"
				+ $": {className} :{message}  :{dataMessage?.ToString()}");
	}


	public double TimeConsumption()
	{
		return 0;
	}

	public string GetTraceLogs(string? message = "")
	{


		_clock.EndDateTime = DateTime.Now.ToString("T");
		_Endwatch = Stopwatch.GetTimestamp();
		_clock.TotalTimeSpan = Stopwatch.GetElapsedTime(_Startwatch, _Endwatch).TotalSeconds;

		if (ActionLogs != null && !ActionLogs.ContainsKey(ScopeId ?? "Default"))
		{
			return string.Empty;
		}
		var list = ActionLogs?[ScopeId ?? "Default"];

		var stimeSpan = _clock.TotalTimeSpan;
		if (stimeSpan > 3)
		{
			list?.Add("<span style='background-color:cornsilk; color:crimson;font-weight: bold;'> Execution of "
							+ MainClass + ">>" + MainMethod + " has taken : "
							+ (_clock.TotalTimeSpan).ToString("##000.000") + " seconds!</span>");
		}
		else
		{
			list?.Add("<span style='background-color:cornsilk; color:darkgreen;font-weight: bold;'> Execution of "
							+ MainClass + ">>" + MainMethod + " has taken : "
							+ (_clock.TotalTimeSpan).ToString("##000.000") + " seconds!</span>");

		}
		//ActionLogs.Add("<strong>End On: " + DateTime.Now.ToString("T") + "</strong>");
		var actionLog = ActionLogs?[ScopeId ?? "Default"];

		StringBuilder sbHtmlLogs = new StringBuilder();
		sbHtmlLogs.Append("<hr>");
		sbHtmlLogs.Append("<ul>");
		sbHtmlLogs.Append("<li style='border-top:2px bold purple;color:darkgreen;background-color:aliceblue;'> Log Trace Start" + " </li>");
		if (actionLog != null)
			foreach (var item in actionLog)
			{

				sbHtmlLogs.Append("<li>" + item + " </li>");
			}
		if (!string.IsNullOrEmpty(message))
		{
			sbHtmlLogs.Append("<li style='background-color:yellow;color:black;'>" + message + " </li>");
		}

		sbHtmlLogs.Append("<li style='order-bottom:2px bold purple;color:darkgreen;background-color:aliceblue;'> Log Trace End " + " </li>");
		sbHtmlLogs.Append("</ul>");
		sbHtmlLogs.Append("<hr>");
		ActionLogs?.Remove(ScopeId ?? "Default");
		return sbHtmlLogs.ToString();
	}


}