using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HLTextureRenamer {
    public static class Utils {
        /// <summary>
        /// Checks if a folder is empty.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsFolderEmpty(string path) {
            var files = Directory.EnumerateFiles(path, "*", SearchOption.TopDirectoryOnly).ToList();

            return files.Count != 0;
        }

        /// <summary>
        /// Checks if the folder has any files with extensions.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool DoesFolderHaveFilesWithExtensions(string path) {
            var files = Directory.EnumerateFiles(path, "*", SearchOption.TopDirectoryOnly);

            return files.Any() && files.All(file => !string.IsNullOrWhiteSpace(Path.GetExtension(file)));
        }

        private const string LogFileName = "Output.log";
        
        private static readonly string OutputLogFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LogFileName);

        /// <summary>
        /// Replaces all specified keywords in a string
        /// </summary>
        /// <param name="text"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string ReplaceIgnoreCase(string text, string key, string value) {
            var index = text.IndexOf(key, StringComparison.OrdinalIgnoreCase);

            while (index >= 0) {
                text = text.Remove(index, key.Length).Insert(index, value);
                index = text.IndexOf(key, index + value.Length, StringComparison.OrdinalIgnoreCase);
            }
            
            return text;
        }
        
        /// <summary>
        /// Rename files using a configuration to replace key words such as "brick" being renamed to "brk".
        /// </summary>
        /// <param name="path"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static int RenameFiles(string path, Dictionary<string, string> configuration) {
            var renamedCount = 0;
            
            using var sw = new StreamWriter(OutputLogFilePath);

            foreach (var file in Directory.EnumerateFiles(path, "*", SearchOption.TopDirectoryOnly)) {
                var fileName = Path.GetFileNameWithoutExtension(file);
                var extension = Path.GetExtension(file);
                var newName = fileName;

                foreach (var (key, value) in configuration.Where(pair => fileName.Contains(pair.Key, StringComparison.OrdinalIgnoreCase))) {
                    newName = fileName.Replace(key, value, StringComparison.OrdinalIgnoreCase);
                    break;
                }

                foreach (var (key, value) in configuration) {
                    if (newName.Contains(key, StringComparison.OrdinalIgnoreCase)) {
                        newName = ReplaceIgnoreCase(newName, key, value);
                    }
                }

                if (newName == fileName) continue;
                var newPath = Path.Combine(path, newName + extension);
                try {
                    File.Move(file, newPath);
                    Console.WriteLine($"{fileName + extension} -> {Path.GetFileName(newPath)}");
                    sw.WriteLine($"{Path.GetFileName(file)} -> {Path.GetFileName(newPath)}");
                    renamedCount++;
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                    throw;
                }
            }
            
            return renamedCount;
        }
    }
}