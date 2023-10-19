using Azure;
using Azure.AI.TextAnalytics;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace AzLangExample.CLI
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			var extractor = new EntityExtractor();

			if (args.Length == 0)
			{
				DisplayUsage();
				return;
			}

			var inputType = args[0].ToLower();

			if (inputType == "-h" || inputType == "--help")
			{
				DisplayHelp();
				return;
			}

			if (args.Length > 2)
			{
				DisplayUsage();
				return;
			}

			if (inputType == "-i")
			{
				var entities = await extractor.GetEntities(args[1]);
				DisplayEntities(entities);
			}

			else if (inputType == "-f")
			{
				var filePath = args[1];
				if (!File.Exists(filePath))
				{
					Console.WriteLine("Not a valid file path.");
					return;
				}

				var content = File.ReadAllText(filePath);

				var entities = await extractor.GetEntities(content);

				DisplayEntities(entities);
			}

			else
			{
				DisplayUsage();
				return;
			}
		}

		private static void DisplayEntities(List<Entity> entities)
		{
            Console.WriteLine("{0,-30} {1,-40} {2,-5}", "Type", "Text", "Score");
            foreach (var e in entities)
            {
                Console.WriteLine("{0,-30} {1,-40} {2,-5:N2}", e.Type, e.Text, e.Score);
            }
        }

		private static void DisplayUsage()
		{
			Console.WriteLine("Usage:");
			Console.WriteLine("\t AzLangExample.CLI.exe -f | -i <input_text|file_path>");
			Console.WriteLine("\t AzLangExample.CLI.exe -h | --help");
		}

		private static void DisplayHelp()
		{
			DisplayUsage();
			Console.WriteLine();

			Console.WriteLine("Available Commands:");
			Console.WriteLine("\t-i - Inline. Text to exract entities from is inline as a command line argument");
			Console.WriteLine("\t-f - File. Text to exract entities from is in the text file path as a command line argument");
		}
	}
}