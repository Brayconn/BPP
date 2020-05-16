using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CommandLine;

namespace BPP
{
    static class Program
    {
        class Options
        {
            public const char HackFolderShortName = 'h';
            [Option(HackFolderShortName, Default = null, Required = false, HelpText = "What hack folder to use. (this does not overwrite the current setting)")]
            public string HackFolder { get; set; }

            public const char ExecutableShortName = 'e';

            [Option(ExecutableShortName, Default = null, Required = false, HelpText = "What exe to hack")]
            public string Executable { get; set; }

            [Option('b', Default = BrayconnsPatchingFramework.PatchApplier.DefaultBaseAddress, Required = false, HelpText = "What the base address of the executable is")]
            public ulong BaseAddress { get; set; }

            [Option('l', Default = null, Required = false, HelpText = "What patch history file to load")]
            public string LoadFile { get; set; }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                if (File.Exists(args[0]))
                    args = new string[] { "-" + Options.ExecutableShortName, args[0] };
                else if (Directory.Exists(args[0]))
                    args = new string[] { "-" + Options.HackFolderShortName, args[0] };
            }            
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(ParseSucceed)
                .WithNotParsed(ParseError);
        }

        private static void ParseError(IEnumerable<Error> errors)
        {
            MessageBox.Show(string.Join("\n", errors.Select(x => x.Tag.ToString())),"Error");
        }

        private static void ParseSucceed(Options options)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain(options.HackFolder, options.Executable, options.BaseAddress, options.LoadFile));
        }
    }
}
