using System;
using System.Collections.Generic;
using System.IO;

namespace HLTextureRenamer {
    public static class Configuration {
        private const string ConfigurationFileName = "Configuration.ini";

        private static string ConfigurationFilePath = Path.Combine(Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName), ConfigurationFileName);

        public static readonly Dictionary<string, string> Configurations =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        private static readonly Dictionary<string, string> DefaultConfigurations =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) {
                {"stone", "stn"},
                {"wood", "wd"},
                {"floor", "flr"},
                {"metal", "mtl"},
                {"glass", "gl"},
                {"brick", "brk"},
                {"window", "wnd"},
                {"building", "bld"},
                {"plaster", "plst"},
                {"citadel", "cit"},
                {"measurement", "msr"},
                {"wall", "wl"},
                {"generic", "gen"},
                {"cardboard", "cardb"},
                {"sign_", "s_"},
            };
        
        /// <summary>
        /// Checks if Configuration.ini exists.
        /// </summary>
        /// <param name="path"></param>
        public static bool DoesConfigExist() {
            return File.Exists(ConfigurationFilePath);
        }

        /// <summary>
        /// Create Configuration.ini
        /// </summary>
        public static void CreateConfigFile() {
            using var sw = new StreamWriter(ConfigurationFilePath);
            foreach (var (key, value) in DefaultConfigurations) {
                sw.WriteLine($"{key}={value}");
            }
            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{ConfigurationFileName} could not be found.");
            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Created {ConfigurationFileName}");
        }

        /// <summary>
        /// Loads Configuration.ini
        /// </summary>
        public static void LoadConfigFile() {
            Configurations.Clear();

            foreach (var line in File.ReadAllLines(ConfigurationFilePath)) {
                if (string.IsNullOrWhiteSpace(line) || !line.Contains("=")) continue;
                
                var parts = line.Split(new[] {'='}, 2, StringSplitOptions.RemoveEmptyEntries);
                
                if (parts.Length != 2) continue;
                
                var key = parts[0].Trim();
                var value = parts[1].Trim();

                if (!Configurations.ContainsKey(key)) {
                    Configurations[key] = value;
                }
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Loaded configuration file from: {ConfigurationFilePath}");
            Console.ResetColor();
        }
    }
}