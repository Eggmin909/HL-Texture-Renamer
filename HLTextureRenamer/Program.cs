using System;
using System.IO;
using System.Windows.Forms;

namespace HLTextureRenamer {
    static class Program {
        private const string ProgramName = "Half-Life Texture Renamer";
        
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {
            Console.Title = ProgramName;
            
            // Check if Configuration.ini exists
            if (!Configuration.DoesConfigExist()) Configuration.CreateConfigFile();

            // Make sure a folder was dropped into the exe
            if (args.Length == 0 || !Directory.Exists(args[0])) return;
            
            var path = args[0];

            // Check if the folder is empty
            if (!Utils.IsFolderEmpty(path)) {
                MessageBox.Show($"\n{path} is empty.", ProgramName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Check if there are any files with extensions
            if (!Utils.DoesFolderHaveFilesWithExtensions(path)) {
                MessageBox.Show($"\n{path} does not have any files with extensions.", ProgramName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            Configuration.LoadConfigFile();
            
            Console.WriteLine($"Input folder: {path}");
            Console.ResetColor();
            
            var renamedFiles = Utils.RenameFiles(path, Configuration.Configurations);
            
            Console.ResetColor();
            Console.WriteLine("Press any key to exit...");

            if (renamedFiles == 0) {
                MessageBox.Show($"No files were renamed in: \n{path}", ProgramName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else {
                MessageBox.Show($"Successfully renamed {renamedFiles} file(s) in: \n{path}", ProgramName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
            Console.ReadKey();
        }
    }
}