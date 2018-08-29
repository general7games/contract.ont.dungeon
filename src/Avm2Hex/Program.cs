using System;
using System.IO;
using Neo;
using CommandLine.Options;

namespace Avm2Hex
{
	class Program
	{
		static void Main(string[] args)
		{
			string fileName = null;
			string outputFileName = null;
			var p = new OptionSet()
			{
				{ "i|input=", v => fileName = v },
				{ "o|output=", v => outputFileName = v },
			};
			try
			{
				p.Parse(args);
			}
			catch (Exception e)
			{
				ShowHelp();
				Environment.Exit(1);
			}
			if (fileName == null)
			{
				ShowHelp();
				Environment.Exit(1);
			}
			var avmBytes = File.ReadAllBytes(fileName);
			var hexString = Helper.ToHexString(avmBytes);
			if (outputFileName == null)
			{
				outputFileName = fileName + ".hex";
			}
			File.WriteAllText(outputFileName, hexString);
		}

		static void ShowHelp()
		{
			Console.WriteLine("Avm2Hex --input|-i <inputFileName> [--output|-o <outputFileName>]");
		}
	}
}
