using System.Diagnostics;

namespace Converter;

class Program
{
    static void Main(string[] args)
    {
        Dictionary<string, Action> conversions = new Dictionary<string, Action>()
        {
            {"image", () => ImageConverterUtility.CreateImage()},
            {"image effect", () => ImageEffects.Quantize()},
            {"video", () => VideoConverterUtility.CreateVideo()},
        };
        
        Console.WriteLine("What type of file do you want to convert?");
        foreach (var conversion in conversions)
        {
            Console.WriteLine(conversion.Key);
        }
        
        conversions[GetConversion(conversions)].Invoke();
        
        
    }

    private static string GetConversion(Dictionary <string, Action> conversions)
    {

        
        string? conversionType = Console.ReadLine()?.ToLower().Trim();
        if(conversionType != null && !conversions.ContainsKey(conversionType))
        {
            Console.WriteLine("Invalid conversion type.");
            return GetConversion(conversions);
        }
        
        Debug.Assert(conversionType != null, nameof(conversionType) + " != null");
        return conversionType;
    }
}