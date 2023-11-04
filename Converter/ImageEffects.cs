using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Converter;
using ImageMagick;
public class ImageEffects : FileConverterUtility
{
    //Potential fixes here:
    //A): Generalize/main function for this class to allow 
    public static void Quantize()
    {
        var local = new ImageEffects();
        QuantizeSettings settings;
        int colors;
        local.getLocations();
        Console.WriteLine("How many colors would you like to quantize to?");
        colors = Convert.ToInt32(Console.ReadLine());
        using (MagickImage image = new MagickImage(local._inputFile))
        {
            settings = new QuantizeSettings();
            settings.Colors = colors;
            image.Quantize(new QuantizeSettings
            {
                Colors = colors
            });
            try
            {
                string pngFilePath = Path.Combine(local._outputFolder,
                    Path.GetFileNameWithoutExtension(local._inputFile) + " quantized.png");
                if (File.Exists(pngFilePath))
                {
                    throw new FileLoadException();
                }
                image.Format = MagickFormat.Png;
                image.Write(pngFilePath);
                Console.WriteLine("Filename: " + pngFilePath);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Issue found while writing, likely already exists. Renaming and trying again");
                string time = DateTime.Now.ToShortTimeString();
                Regex rgx = new Regex("[^a-zA-Z0-9 -]");
                time = rgx.Replace(time, "");

                string pngFilePath = Path.Combine(local._outputFolder,
                    Path.GetFileNameWithoutExtension(local._inputFile) + time +".png");
                
                image.Format = MagickFormat.Png;
                image.Write(pngFilePath);
                Console.WriteLine("Filename: " + pngFilePath);
            }
            finally
            {
                Console.WriteLine("Done!");
            }
        }
    }
}