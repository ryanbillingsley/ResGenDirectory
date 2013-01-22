using System;
using System.IO;
using System.Text;

namespace ResGenDirectory
{
    class Program
    {
        static void Main(string[] args)
        {
            var directory = args[0];
            if (!Directory.Exists(directory))
            {
                throw new DirectoryNotFoundException("Can't find source directory");
            }


            var destinationDirectory = "";

            if (!String.IsNullOrEmpty(args[1]))
            {
                if (Directory.Exists(args[1]))
                {
                    destinationDirectory = args[1];
                }
                else
                {
                    throw new DirectoryNotFoundException("Can't find destination directory.");
                }
            }

            var files = Directory.GetFiles(directory, "*.resx", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                try
                {
                    var command = new StringBuilder();
                    command.Append("\"C:\\Program Files (x86)\\Microsoft SDKs\\Windows\\v8.0A\\bin\\NETFX 4.0 Tools\\resgen.exe\" ");
                    command.Append(Path.GetFullPath(file));
                    command.Append(" ");

                    if (!String.IsNullOrEmpty(destinationDirectory))
                    {
                        var subDirectories = Path.GetFullPath(file).Replace(directory, "");

                        var combinded = destinationDirectory + subDirectories;

                        var combinedDirectory = Path.GetDirectoryName(combinded);

                        if (!Directory.Exists(combinedDirectory))
                        {
                            Console.WriteLine("Creating Directory: {0}", combinedDirectory);
                            Directory.CreateDirectory(combinedDirectory);
                        }

                        command.AppendFormat("{0}\\{1}{2}", Path.GetDirectoryName(combinded),
                                             Path.GetFileNameWithoutExtension(combinded), ".txt");
                    }
                    else
                    {
                        command.Append(Path.GetFileNameWithoutExtension(file));
                        command.Append(".txt");
                    }

                    var procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command)
                         {
                             RedirectStandardOutput = true,
                             UseShellExecute = false,
                             CreateNoWindow = true
                         };

                    var proc = new System.Diagnostics.Process { StartInfo = procStartInfo };
                    proc.Start();

                    var result = proc.StandardOutput.ReadToEnd();

                    Console.WriteLine(result);
                }
                catch (Exception objException)
                {
                    // Log the exception
                }

            }
        }
    }
}
