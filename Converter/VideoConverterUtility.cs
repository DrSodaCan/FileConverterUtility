using FFMpegCore.Enums;

namespace Converter;
using FFMpegCore;

public class VideoConverterUtility : FileConverterUtility
{
    
    public VideoConverterUtility()
    { }
    
    public static void CreateVideo()
    {
        var utility = new VideoConverterUtility();
        var codec = VideoCodec.LibX264;
        Dictionary<string, Action> conversions = new Dictionary<string, Action>()
        {
            {"mp4", () => utility.ConvertToMp4(utility._inputFile, utility._outputFolder)},
            {"mp4 codec", () => utility.ConvertToMp4Codec(utility._inputFile, utility._outputFolder, codec)},
            {"mp4 resolution", (() => utility.ConvertToMp4Resolution(utility._inputFile, utility._outputFolder))},
            {"mov", () => utility.ConvertToMov(utility._inputFile, utility._outputFolder)},
            
        };

        utility.getLocations();
        Console.WriteLine("File locations set!");
        utility.setType(conversions);
    }
    
    private void ConvertToMp4(string inputFilePath, string outputFolderPath)
    {
        EnsureOutputDirectoryExists(outputFolderPath);
        string mp4FilePath = Path.Combine(outputFolderPath, Path.GetFileNameWithoutExtension(inputFilePath) + ".mp4");
        FFMpegArguments.FromFileInput(inputFilePath).OutputToFile(mp4FilePath).ProcessSynchronously();
    }
    
    private void ConvertToMp4Codec(string inputFilePath, string outputFolderPath, Codec codec)
    {
        EnsureOutputDirectoryExists(outputFolderPath);
        string mp4FilePath = Path.Combine(outputFolderPath, Path.GetFileNameWithoutExtension(inputFilePath) + ".mp4");
        FFMpegArguments.FromFileInput(inputFilePath).OutputToFile(mp4FilePath, true, options => options.WithVideoCodec(codec)).ProcessSynchronously();
    }

    private void ConvertToMp4Resolution(string inputFilePath, string outputFolderPath)
    {
        int[] resolution = ResolutionStringToArr(GetResolution());
        string mp4FilePath = Path.Combine(outputFolderPath,
            Path.GetFileNameWithoutExtension(inputFilePath) + "_converted.mp4");
        FFMpegArguments.FromFileInput(inputFilePath).OutputToFile(mp4FilePath, true, options => options.Resize(resolution[0], resolution[1]));
    }

    private string? GetResolution()
    {
        string? input;
        Dictionary<string, string?> resolutionToHW = new Dictionary<string, string?>()
        {
            { "480p", "854x480" },
            { "720p ", "1280x720" },
            {"1080p", "1920x1080"},
            {"1440p", "2560x1440"},
            {"2160p", "3840x2160"},
            {"4k", "3840x2160"}
        };
        Console.WriteLine("Enter a resolution. Allowed formats: \"xxxp\" or \"widthxheight\"");
        input = Console.ReadLine().ToLower();
        //Exit case
        if (input == "" || input == "exit")
        {
            return null;
        }
        if (resolutionToHW.ContainsKey(input))
        {
            return resolutionToHW[input];
        }else if (resolutionToHW.ContainsValue(input))
            return input;

        Console.WriteLine("Resolution not found!");
            return GetResolution();
    }

    private int[] ResolutionStringToArr(string input)
    {
        int[] output = {0, 0};
        string[] tempStrings = input.Split(" ");
        output[0] = Int32.Parse(tempStrings[0]);
        output[1] = Int32.Parse(tempStrings[1]);
        return output;
    }
    

    private void ConvertToMov(string inputFilePath, string outputFolderPath)
    {
        EnsureOutputDirectoryExists(outputFolderPath);
        string movFilePath = Path.Combine(outputFolderPath, Path.GetFileNameWithoutExtension(inputFilePath) + ".mov");
        FFMpegArguments.FromFileInput(inputFilePath).OutputToFile(movFilePath).ProcessSynchronously();
    }
    
}