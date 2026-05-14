using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.Win32;

namespace RepairTrackerSystem.Core
{
    public static class PythonAnalyticsService
    {
        private static readonly string PythonScriptPath = GetNormalizedPythonScriptPath();

        // ═══════════════════════════════════════════════════════════════════
        // GET PYTHON EXECUTABLE PATH
        // ═══════════════════════════════════════════════════════════════════
        private static string GetNormalizedPythonScriptPath()
        {
            // BaseDirectory: bin\Debug\net472\
            // Solution root is 3 levels up
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string solutionRoot = Path.GetFullPath(Path.Combine(baseDirectory, "..", "..", ".."));
            string scriptPath = Path.Combine(solutionRoot, "RepairTrackerSystem", "analytics", "main.py");

            // Return the normalized absolute path
            return Path.GetFullPath(scriptPath);
        }

        private static string GetPythonExecutable()
        {
            // Try 1: Python in PATH (most common)
            if (CanExecutePython("python"))
                return "python";

            if (CanExecutePython("python3"))
                return "python3";

            // Try 2: Check registry for Python installations (Windows)
            string registryPath = @"Software\Python\PythonCore";
            using (var key = Registry.CurrentUser.OpenSubKey(registryPath))
            {
                if (key != null)
                {
                    foreach (string subKeyName in key.GetSubKeyNames())
                    {
                        using (var subKey = key.OpenSubKey(subKeyName + @"\InstallPath"))
                        {
                            if (subKey != null)
                            {
                                string pythonPath = Path.Combine(
                                    subKey.GetValue("") as string ?? "",
                                    "python.exe");
                                if (File.Exists(pythonPath))
                                    return pythonPath;
                            }
                        }
                    }
                }
            }

            // Try 3: Common installation paths
            string[] commonPaths = new[]
            {
                @"C:\Users\" + Environment.UserName + @"\AppData\Local\Programs\Python\Python310\python.exe",
                @"C:\Users\" + Environment.UserName + @"\AppData\Local\Programs\Python\Python311\python.exe",
                @"C:\Users\" + Environment.UserName + @"\AppData\Local\Programs\Python\Python39\python.exe",
                @"C:\Program Files\Python310\python.exe",
                @"C:\Program Files\Python311\python.exe",
            };

            foreach (string path in commonPaths)
            {
                if (File.Exists(path))
                    return path;
            }

            return null;
        }

        private static bool CanExecutePython(string pythonCmd)
        {
            try
            {
                var processInfo = new ProcessStartInfo
                {
                    FileName = pythonCmd,
                    Arguments = "--version",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(processInfo))
                {
                    process?.WaitForExit();
                    return process?.ExitCode == 0;
                }
            }
            catch
            {
                return false;
            }
        }

        // ═══════════════════════════════════════════════════════════════════
        // CHECK IF PYTHON IS INSTALLED
        // ═══════════════════════════════════════════════════════════════════
        public static bool IsPythonInstalled()
        {
            return GetPythonExecutable() != null;
        }

        // ═══════════════════════════════════════════════════════════════════
        // GENERATE ALL REPORTS
        // ═══════════════════════════════════════════════════════════════════
        public static bool GenerateAllReports(out string output, out string error)
        {
            return RunPythonScript("--all", out output, out error);
        }

        // ═══════════════════════════════════════════════════════════════════
        // GENERATE REPAIRS PER DAY
        // ═══════════════════════════════════════════════════════════════════
        public static bool GenerateRepairsPerDay(int days, out string output, out string error)
        {
            return RunPythonScript($"--repairs-per-day {days}", out output, out error);
        }

        // ═══════════════════════════════════════════════════════════════════
        // GENERATE ISSUE FREQUENCY
        // ═══════════════════════════════════════════════════════════════════
        public static bool GenerateIssueFrequency(int topN, out string output, out string error)
        {
            return RunPythonScript($"--issue-frequency {topN}", out output, out error);
        }

        // ═══════════════════════════════════════════════════════════════════
        // GENERATE TECHNICIAN PERFORMANCE
        // ═══════════════════════════════════════════════════════════════════
        public static bool GenerateTechnicianPerformance(out string output, out string error)
        {
            return RunPythonScript("--technician-perf", out output, out error);
        }

        // ═══════════════════════════════════════════════════════════════════
        // GENERATE COST ANALYSIS
        // ═══════════════════════════════════════════════════════════════════
        public static bool GenerateCostAnalysis(out string output, out string error)
        {
            return RunPythonScript("--cost-analysis", out output, out error);
        }

        // ═══════════════════════════════════════════════════════════════════
        // GENERATE TREND ANALYSIS
        // ═══════════════════════════════════════════════════════════════════
        public static bool GenerateTrendAnalysis(out string output, out string error)
        {
            return RunPythonScript("--trends", out output, out error);
        }

        // ═══════════════════════════════════════════════════════════════════
        // GET OUTPUT FOLDER PATH
        // ═══════════════════════════════════════════════════════════════════
        public static string GetOutputFolderPath()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string solutionRoot = Path.GetFullPath(Path.Combine(baseDirectory, "..\\..\\.."));

            return Path.Combine(solutionRoot, "output", "graphs");
        }

        // ═══════════════════════════════════════════════════════════════════
        // GET DATABASE PATH (absolute path in bin\Debug)
        // ═══════════════════════════════════════════════════════════════════
        private static string GetDatabasePath()
        {
            // AppDomain.CurrentDomain.BaseDirectory = bin\Debug\net472\ or bin\Debug\
            string dbPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, 
                "repairtracker.db");
            
            return Path.GetFullPath(dbPath);
        }

        // ═══════════════════════════════════════════════════════════════════
        // INTERNAL: RUN PYTHON SCRIPT
        // ═══════════════════════════════════════════════════════════════════
        private static bool RunPythonScript(string arguments, out string output, out string error)
        {
            output = "";
            error = "";

            try
            {
                string pythonExe = GetPythonExecutable();
                if (string.IsNullOrEmpty(pythonExe))
                {
                    error = "Python executable not found. Please install Python and ensure it's in your PATH.";
                    return false;
                }

                if (!File.Exists(PythonScriptPath))
                {
                    error = $"Python script not found: {PythonScriptPath}";
                    return false;
                }

                // Get the database path and verify it exists
                string dbPath = GetDatabasePath();
                if (!File.Exists(dbPath))
                {
                    error = $"Database file not found at: {dbPath}\n\nExpected location: {dbPath}";
                    return false;
                }

                var analyticsDir = Path.GetDirectoryName(PythonScriptPath);

                // Ensure output directory exists
                string outputFolder = GetOutputFolderPath();
                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }

                // Pass both database path AND output directory to Python script
                string fullArguments = $"\"{PythonScriptPath}\" {arguments} --db \"{dbPath}\" --output \"{outputFolder}\"";

                var processInfo = new ProcessStartInfo
                {
                    FileName = pythonExe,
                    Arguments = fullArguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WorkingDirectory = analyticsDir,
                    StandardOutputEncoding = Encoding.UTF8,
                    StandardErrorEncoding = Encoding.UTF8
                };

                using (var process = Process.Start(processInfo))
                {
                    output = process?.StandardOutput.ReadToEnd() ?? "";
                    error = process?.StandardError.ReadToEnd() ?? "";

                    process?.WaitForExit();

                    return process?.ExitCode == 0;
                }
            }
            catch (Exception ex)
            {
                error = $"Exception: {ex.Message}";
                return false;
            }
        }
    }
}