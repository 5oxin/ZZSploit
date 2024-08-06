using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using System.Diagnostics;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Threading.Tasks;

namespace ZZSploit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            using (Stream stream = File.OpenRead("./Syntax/Lua.xshd"))
            {
                using (XmlReader reader = XmlReader.Create(stream))
                {
                    // Load the syntax highlighting definition from the XML file
                    IHighlightingDefinition definition = HighlightingLoader.Load(reader, HighlightingManager.Instance);
                    LuaEditor.SyntaxHighlighting = definition;
                }
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string directoryPath = Directory.GetCurrentDirectory(); // Gets the directory of the currently running application
            string pattern = @"^0VEGJqTAY6SKOOkb.*\.exe$"; // Pattern to match the executable name

            try
            {
                // Search for the file that matches the pattern
                var files = Directory.GetFiles(directoryPath, "*.exe");
                foreach (var file in files)
                {
                    if (Regex.IsMatch(System.IO.Path.GetFileName(file), pattern))
                    {
                        // File found, start it
                        Process.Start(file);
                        return; // Exit after running the first matching file
                    }
                }

                // Optionally handle the case where no file was found
                MessageBox.Show("No executable matching the pattern was found.");
            }
            catch (Exception ex)
            {
                // Handle any errors that may have occurred
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void btnTopmost_Click(object sender, RoutedEventArgs e)
        {
            this.Topmost = !this.Topmost;
        }

        private void btnFixRBX_Click(object sender, RoutedEventArgs e)
        {
            // Define the name of the Roblox process
            string robloxProcessName = "RobloxPlayerBeta"; // This is the typical name for the Roblox process

            try
            {
                // Find all processes with that name
                var robloxProcesses = Process.GetProcessesByName(robloxProcessName);

                if (robloxProcesses.Length == 0)
                {
                    MessageBox.Show("No Roblox processes found.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                foreach (var process in robloxProcesses)
                {
                    try
                    {
                        // Close the process
                        process.Kill();
                        process.WaitForExit(); // Optional: Wait for the process to exit
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error closing Roblox process: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                MessageBox.Show("Roblox processes closed successfully.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private async void Execute_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Retrieve the Lua script content from LuaEditor
                string luaScriptContent = LuaEditor.Text;

                // Call ExecuteScript to handle the scheduling
                await ExecuteScript(luaScriptContent);
            }
            catch (Exception ex)
            {
                // Handle any errors that may have occurred
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task ExecuteScript(string script, string pid = null)
        {
            string uniqueId = Guid.NewGuid().ToString(); // Generate a unique identifier

            await Task.Delay(1);

            try
            {
                string schedulerPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "scheduler");
                if (!Directory.Exists(schedulerPath))
                {
                    Directory.CreateDirectory(schedulerPath);
                }

                // Name the file according to the pid if provided
                string fileName = pid != null ? $"PID{pid}_{uniqueId}.lua" : $"{uniqueId}.lua";
                string filePath = System.IO.Path.Combine(schedulerPath, fileName);
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    await writer.WriteLineAsync(script + "@@FileFullyWritten@@");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error writing file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click_2(object sender, EventArgs e)
        {
            string url = "https://discord.gg/8BsU2PekSs";
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("ZZSploit just ratted you!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}