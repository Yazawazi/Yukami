using System.Text;
using CommandLine;
using Yukami.Lib;

namespace Yukami;

public class Program
{
    private class Options
    {
        [Option('i', "input", Required = true, HelpText = "Input file to be processed.")]
        public string Input { get; set; } = null!;

        [Option('o', "output", Required = true, HelpText = "Output folder to be written.")]
        public string Output { get; set; } = null!;
    }

    private static void Main(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args)
            .WithParsed(opts =>
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                if (!Directory.Exists(opts.Output))
                {
                    Directory.CreateDirectory(opts.Output);
                }
                
                var inputFile = File.OpenRead(opts.Input);
                
                var splitFolder = Path.Combine(opts.Output, "01_SPLIT");
                Directory.CreateDirectory(splitFolder);
                
                var dumpFolder = Path.Combine(opts.Output, "02_DUMP");
                Directory.CreateDirectory(dumpFolder);

                var splitter = new SCRSplitter(inputFile);

                foreach (var content in splitter.Contents)
                {
                    Console.WriteLine($"Writing {content.Name}");
                    var splitFilePath = Path.Combine(splitFolder, content.Name);
                    File.WriteAllBytes(splitFilePath, content.Data);

                    var dumpFilePath = Path.Combine(dumpFolder, content.Name + ".txt");
                    
                    var memoryReader = new MemoryStream(content.Data);
                    try
                    {
                        var dumper = new SCRDumper(memoryReader);
                        File.WriteAllLines(dumpFilePath, dumper.Texts);
                    } catch (Exception e)
                    {
                        Console.WriteLine($"Error while dumping {content.Name}: {e.Message}");
                        throw;
                    }
                }
            });
    }
}