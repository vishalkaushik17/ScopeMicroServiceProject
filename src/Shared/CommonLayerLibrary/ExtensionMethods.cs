using GenericFunction.DefaultSettings;
using GenericFunction.Enums;
using GenericFunction.ServiceObjects.EmailService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
namespace GenericFunction;
/// <summary>
/// Public generic extension methods class
/// </summary>
public static class ExtensionMethods
{

    // private static ApplicationSettings _applicationSettings;
    // static ExtensionMethods()
    // {
    //     _applicationSettings = SettingsConfigHelper.GetOptions<ApplicationSettings>();
    // }


    public static IEnumerable<SelectListItem> GetEnumSelectList<T>(string defaultText = "Select") where T : Enum
    {
        var items = Enum.GetValues(typeof(T)).Cast<T>().Select(e => new SelectListItem
        {
            Value = Convert.ToInt32(e).ToString(),
            Text = GetEnumDisplayName(e)
        }).OrderBy(o => o.Text)
       .ToList();
        items.Insert(0, new SelectListItem() { Value = "-1", Text = defaultText });

        return items;
    }

    /// <summary>
    /// Get Display name of the enum
    /// </summary>
    /// <param name="enumValue">Enum object</param>
    /// <typeparam name="T">Enum type</typeparam>
    /// <returns>string</returns>
    private static string? GetEnumDisplayName<T>(T enumValue) where T : Enum
    {
        var displayAttribute = enumValue.GetType()
                                        .GetMember(enumValue.ToString())
                                        .First()
                                        .GetCustomAttributes(false)
                                        .OfType<DisplayAttribute>()
                                        .FirstOrDefault();

        return displayAttribute != null ? displayAttribute.Name : enumValue.ToString();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="file"></param>
    /// <param name="height"></param>
    /// <param name="width"></param>
    /// <returns></returns>
    public static byte[]? ChangeImageSize(this IFormFile? file, int height = 0, int width = 0)
    {
        if (file == null || file.Length == 0)
            return default;

        using var image = SixLabors.ImageSharp.Image.Load(file.OpenReadStream());
        //image.Mutate(x => x
        //     .Resize(new ResizeOptions
        //     {
        //         Mode = ResizeMode.Min,
        //         Size = new Size(height == 0 ? 120 : height, width == 0 ? 100 : width)
        //     }));
        var resizedImage = image.Clone(ctx => ctx.Resize(new ResizeOptions
        {
            Mode = ResizeMode.Min,
            Size = new Size(height == 0 ? 120 : height, width == 0 ? 100 : width)
        }));
        var pngEncoder = new SixLabors.ImageSharp.Formats.Png.PngEncoder() { ColorType = SixLabors.ImageSharp.Formats.Png.PngColorType.Rgb };

        using var stream = new MemoryStream();
        resizedImage.Save(stream, pngEncoder);
        stream.Seek(0, SeekOrigin.Begin);
        // Save or return the resized image
        // For simplicity, returning the image as a byte array
        return ConvertImageToByteArray(stream);
    }
    /// <summary>
    /// Convert Image to ByteArray
    /// </summary>
    /// <param name="imageStream"></param>
    /// <returns></returns>
    private static byte[]? ConvertImageToByteArray(Stream imageStream)
    {
        using var image = System.Drawing.Image.FromStream(imageStream);
        using var memoryStream = new MemoryStream();
        image.Save(memoryStream, ImageFormat.Jpeg); // or other format like ImageFormat.Png
        return memoryStream.ToArray();
    }
    /// <summary>
    /// get list of item as comma separated string
    /// </summary>
    /// <param name="list">IList collection</param>
    /// <returns>return string with comma separated</returns>
    public static string ToString(this IList<string> list)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var item in list)
        {
            sb.Append($"{item}, ");
        }
        return sb.ToString().Substring(0, sb.ToString().Length - 2);
    }

    /// <summary>
    /// Read key life in double from appsettings.json.
    /// </summary>
    /// <param name="dblKey">ModuleName as key</param>
    /// <returns>return DateTimeOffset</returns>
    public static DateTimeOffset GetKeyLifeForCacheStorage(this double dblKey)
    {
        if (dblKey == 0)
            dblKey = 100;
        return DateTimeOffset.Now.AddMinutes(dblKey);
    }

    /// <summary>
    /// Generic method for converting string value to enum, 
    /// </summary>
    /// <typeparam name="T">Enum</typeparam>
    /// <param name="value">string value</param>
    /// <returns>Enum</returns>
    public static T? ToEnum<T>(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return default(T);
        }
        return (T)Enum.Parse(typeof(T), value, true);
    }

    /// <summary>
    /// Get name of the class
    /// </summary>
    /// <param name="o">class object</param>
    /// <returns>returns name of the class</returns>
    public static string NameOfClass(this object o)
    {
        return o.GetType().Name;
    }


    /// <summary>
    /// Get current method name
    /// </summary>
    /// <returns>return name of the method</returns>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string? GetCurrentMethodName()
    {
        var st = new StackTrace(true);
        var sf = st.GetFrame(1);
        string? name = sf?.GetMethod()?.Name;

        if (name != null && name.Equals("MoveNext"))
        {
            // We're inside an async method
            name = sf?.GetMethod()?.ReflectedType?.Name
                     .Split(new char[] { '<', '>' }, StringSplitOptions.RemoveEmptyEntries)[0];
        }

        return name;
    }

    /// <summary>
    /// Build version number of the file.
    /// </summary>
    /// <param name="filename">file name with full path.</param>
    /// <returns>return version number as string.</returns>
    public static string CalculateMd5(this string filename)
    {
        string file = filename.Replace(@"\\", @"\");
        using var md5 = MD5.Create();
        using var stream = File.OpenRead(file);
        var hash = md5.ComputeHash(stream);
        return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
    }

    /// <summary>
    /// Converting object to JSON
    /// </summary>
    /// <param name="obj">object</param>
    /// <returns>returns json string.</returns>
    public static string ToJson(this object obj)
    {
        return JsonConvert.SerializeObject(obj, Formatting.Indented);
    }

    /// <summary>
    /// Convert JSON string to object
    /// </summary>
    /// <typeparam name="T">T type object</typeparam>
    /// <param name="str">JSON string</param>
    /// <returns>return T type object</returns>
    public static T? FromJsonToObject<T>(this string str)
    {
        if (string.IsNullOrWhiteSpace(str))
            return default;
        return JsonConvert.DeserializeObject<T>(str);
    }
    public static string GetCurrentLineNo(this string message,
    [CallerLineNumber] int lineNumber = 0,
    [CallerMemberName] string caller = null!)
    {
        return (message + " at line <span 'style= color:purple;'><b> " + lineNumber + "</b></span> (" + caller + ")");
    }
    //public static string GetCallerName([CallerMemberName] string caller = null)
    //{
    //	return caller;
    //}
    //public static string ComputeSha256Hash(this string rawData, string salt = "")
    //{
    //	// Create a SHA256
    //	using (SHA256 sha256Hash = SHA256.Create())
    //	{
    //		byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(rawData + salt);
    //		byte[] hashBytes = sha256Hash.ComputeHash(textBytes);

    //		// ComputeHash - returns byte array
    //		//            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData+salt));
    //		string hash = BitConverter
    //		   .ToString(hashBytes)
    //		   .Replace("-", String.Empty);
    //		// Convert byte array to a string
    //		StringBuilder builder = new StringBuilder();
    //		for (int i = 0; i < hashBytes.Length; i++)
    //		{
    //			builder.Append(hashBytes[i].ToString("x2"));
    //		}
    //		return builder.ToString();
    //	}
    //}

    /// <summary>
    /// Get header name from current context.
    /// </summary>
    /// <param name="context">HttpContext</param>
    /// <param name="key">key to read from header</param>
    /// <returns>return string.</returns>
    public static string GetHeader(this HttpContext? context, string key)
    {
        if (context != null && context.Request.Headers.ContainsKey(key))
        {
            return context.Request.Headers[key]!;

        }
        return string.Empty;
    }

    /// <summary>
    /// Set string as a key value pair in context header.
    /// </summary>
    /// <param name="context">HttpContext object</param>
    /// <param name="key">dictionary key as string.</param>
    /// <param name="value">value as string to store in header.</param>
    public static void SetHeader(this HttpContext context, string key, string value)
    {
        if (context.Request.Headers.ContainsKey(key))
        {
            context?.Request?.Headers?.Remove(key);

        }
        else
        {
            context?.Request?.Headers?.Add(key, value);    
        }
        
    }
    //public static T? GetContextItem<T>(this HttpContext context, string key)
    //{
    //    var value = context?.Items[key];
    //    if (value == null)
    //    {
    //        return default(T);
    //    }
    //    return (T)value;
    //}
    //public static bool SetContextItem<T>(this HttpContext context, string key, object value, bool forceToRemove = false)
    //{
    //    if (context.GetContextItem<T>(key) == null)
    //    {
    //        context.Items.Add(key, value);
    //        return true;
    //    }
    //    else
    //    {
    //        if (forceToRemove)
    //        {
    //            context.Items.Remove(key);
    //            context.Items.Add(key, value);
    //            return true;
    //        }
    //    }
    //    return false;
    //}
    //public static string HashString(this string text, string salt = "")
    //{
    //	if (String.IsNullOrEmpty(text))
    //	{
    //		return String.Empty;
    //	}

    //	// Uses SHA256 to create the hash
    //	using (var sha = new SHA256Managed())
    //	{
    //		// Convert the string to a byte array first, to be processed
    //		byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(text + salt);
    //		byte[] hashBytes = sha.ComputeHash(textBytes);

    //		// Convert back to a string, removing the '-' that BitConverter adds
    //		string hash = BitConverter
    //			.ToString(hashBytes)
    //			.Replace("-", String.Empty);

    //		return hash;
    //	}
    //}
    public static async Task SendExceptionMailAsync(this Exception e)
    {
        var sendEmail = AppSettingsConfigurationManager.AppSetting.GetValue<bool>("ApplicationSettings:SendCacheEmail");
        if (!sendEmail) { return; }
        MailRequest mailRequest = new()
        {
            Subject = e.GetType().ToString(),
            EmailType = EmailNotificationType.ALERT,
            ToEmail = new[] { SettingsConfigHelper.AppSetting("ApplicationSettings", "SendExceptionEmailTo:Email") },
            CCEmail = Array.Empty<string>(),
            Body = $"<strong style='color:red;'>{"Exception thrown by Class/Method :" + new StackTrace(e)}</strong> </br> <hr></br>{e.Message} </br> {e.InnerException}"
        };

        await ScopeMailService.SendEmailAsync(mailRequest);
    }
    public static async Task SendCacheFailedEmail(this string message)
    {
        var sendEmail = AppSettingsConfigurationManager.AppSetting.GetValue<bool>("ApplicationSettings:SendCacheEmail");
        if (!sendEmail) { return; }
        MailRequest mailRequest = new()
        {
            Subject = message,
            EmailType = EmailNotificationType.ALERT,
            ToEmail = new[] { SettingsConfigHelper.AppSetting("ApplicationSettings", "SendExceptionEmailTo:Email") },
            CCEmail = Array.Empty<string>(),
            Body = message,
        };

        await ScopeMailService.SendEmailAsync(mailRequest);
    }
    private static readonly Random _rand = new Random();

    const string ReservedCharacters = "!*'();:@&=+$,/?%#[]";

    public static string UrlEncode(string value)
    {
        if (String.IsNullOrEmpty(value))
            return String.Empty;

        var sb = new StringBuilder();

        foreach (char @char in value)
        {
            if (ReservedCharacters.IndexOf(@char) == -1)
                sb.Append(@char);
            else
                sb.AppendFormat("%{0:X2}", (int)@char);
        }
        return sb.ToString();
    }
    public static Dictionary<string, string?> GetFieldValues(object obj)
    {
        return obj.GetType()
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.FieldType == typeof(string))
            .ToDictionary(f => f.Name,
                f => (string?)f.GetValue(null));
    }
    public static bool VerifyHashedPassword(string? hashedPassword, string password)
    {
        byte[] buffer4;
        if (hashedPassword == null)
        {
            return false;
        }
        if (password == null)
        {
            throw new ArgumentNullException("password");
        }
        byte[] src = Convert.FromBase64String(hashedPassword);
        if ((src.Length != 0x31) || (src[0] != 0))
        {
            return false;
        }
        byte[] dst = new byte[0x10];
        Buffer.BlockCopy(src, 1, dst, 0, 0x10);
        byte[] buffer3 = new byte[0x20];
        Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
        using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
        {
            buffer4 = bytes.GetBytes(0x20);
        }
        return ByteArraysEqual(buffer3, buffer4);
    }
    public static bool ByteArraysEqual(byte[] b1, byte[] b2)
    {
        if (b1 == b2) return true;
        if (b1 == null || b2 == null) return false;
        if (b1.Length != b2.Length) return false;
        for (int i = 0; i < b1.Length; i++)
        {
            if (b1[i] != b2[i]) return false;
        }
        return true;
    }
    public static string HashPassword(string password)
    {
        byte[] salt;
        byte[] buffer2;
        if (password == null)
        {
            throw new ArgumentNullException("password");
        }
        using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
        {
            salt = bytes.Salt;
            buffer2 = bytes.GetBytes(0x20);
        }
        byte[] dst = new byte[0x31];
        Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
        Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
        return Convert.ToBase64String(dst);
    }
    public static string GeneratePassword(int length = 24)
    {
        const string lower = "abcdefghijklmnopqrstuvwxyz";
        const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string number = "1234567890";
        const string special = "!@#$%^&*_-=+";

        // Get cryptographically random sequence of bytes
        var bytes = new byte[length];
        new RNGCryptoServiceProvider().GetBytes(bytes);

        // Build up a string using random bytes and character classes
        var res = new StringBuilder();
        foreach (byte b in bytes)
        {
            // Randomly select a character class for each byte
            switch (_rand.Next(4))
            {
                // In each case use mod to project byte b to the correct range
                case 0:
                    res.Append(lower[b % lower.Count()]);
                    break;
                case 1:
                    res.Append(upper[b % upper.Count()]);
                    break;
                case 2:
                    res.Append(number[b % number.Count()]);
                    break;
                case 3:
                    res.Append(special[b % special.Count()]);
                    break;
            }
        }
        return res.ToString();
    }
    /// <summary>
    /// Verify Url 
    /// </summary>
    /// <param name="source">source url</param>
    /// <returns>return true if url is value.</returns>
    public static bool CheckUrlValid(this string source)
    {
        return Uri.TryCreate(source, UriKind.Absolute, out var uriResult) && uriResult.Scheme == Uri.UriSchemeHttp;
    }

    /// <summary>
    /// To add special CSS style on the string.
    /// </summary>
    /// <param name="sourceString">string on which this CSS get applied.</param>
    /// <param name="className">name of the class</param>
    /// <returns></returns>
    public static string MarkProcess(this string? sourceString, string className = "logClassMarkProcess")
    {
        return $"<span class ='{className}'>{sourceString}</span>";

    }
    public static string MarkInformation(this string? sourceString, string className = "logClassInformation")
    {
        return $"<span class ='{className}'>{sourceString}</span>";
    }
    //public static string ToCamelCase(this string text)
    //{
    //    return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text);
    //}

    /// <summary>
    /// Convert string to Camel Case 
    /// </summary>
    /// <param name="text">Input type string</param>
    /// <returns>String</returns>
    public static string ToCamelCase(this string text)
    {

        char[] a = text.ToLower().ToCharArray();

        for (int i = 0; i < a.Count(); i++)
        {
            a[i] = i == 0 || a[i - 1] == ' ' ? char.ToUpper(a[i]) : a[i];

        }

        return new string(a).RemoveSpaces();
    }

    /// <summary>
    /// Remove extra spaces
    /// </summary>
    /// <param name="text">string</param>
    /// <returns>string</returns>
    public static string RemoveSpaces(this string text)
    {

        string sentence = text;
        RegexOptions options = RegexOptions.None;
        Regex regex = new Regex("[ ]{2,}", options);
        sentence = regex.Replace(sentence, " ");
        return sentence.Trim();
    }

    /// <summary>
    /// Remove symbol character
    /// </summary>
    /// <param name="text">string</param>
    /// <returns>string</returns>
    public static string RemoveSymbols(this string text)
    {

        text = Regex.Replace(text, "[@,-\\.\";'\\\\]", string.Empty);

        return text.RemoveSpaces();
    }


    /// <summary>
    /// GenerateRandom Alpha Numeric string
    /// </summary>
    /// <param name="str"></param>
    /// <param name="length"></param>
    /// <param name="existingStrings"></param>
    /// <param name="isLowerCharRequired"></param>
    /// <param name="isUpperCharRequired"></param>
    /// <param name="isNumericRequired"></param>
    /// <returns></returns>
    public static string GenerateRandomString(this string str, int length, List<string>? existingStrings,
        bool isLowerCharRequired = true, bool isUpperCharRequired = true, bool isNumericRequired = true)

    {
        StringBuilder stringToBuild = new StringBuilder();

        //if string format is not specified
        if (!isLowerCharRequired && !isUpperCharRequired && !isNumericRequired)
        {
            return stringToBuild.ToString();
        }

        // Add lower string
        if (isLowerCharRequired)
        {
            stringToBuild.Append("abcdefghijklmnopqrstuvwxyz");
        }

        // Add upper string
        if (isUpperCharRequired)
        {
            stringToBuild.Append("ABCDEFGHIJKLMNOPQRSTUVWXY");
        }

        // Add number as a string
        if (isNumericRequired)
        {
            stringToBuild.Append("1234567890");
        }

        string strToBuild = stringToBuild.ToString();

        StringBuilder res = new StringBuilder();
        Random rnd = new Random();
        while (0 < length--)
        {
            res.Append(strToBuild[rnd.Next(strToBuild.Length)]);
        }

        string newString = res.ToString();

        if (existingStrings == null) return res.ToString();
        var result = existingStrings.Contains(newString);
        if (result)
        {
            GenerateRandomString(str, length, existingStrings);
        }

        return res.ToString();
    }
    
}
