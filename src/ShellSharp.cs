class ShellSharp
{
    HashSet<string> builtinCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "exit", "echo", "type" };

    public void Start()
    {
        while (true)
        {
            Console.Write("$ ");

            var input = Console.ReadLine();

            if (input != null)
            {
                string[] inputArgs = input.Split(" ");

                string command = inputArgs[0];
                inputArgs = inputArgs[1..];

                if (String.Equals(command, "exit", StringComparison.OrdinalIgnoreCase))
                {
                    int exitValue = 0;
                    if (inputArgs.Length != 0 && int.TryParse(inputArgs[0], out int parsedValue))
                    {
                        exitValue = parsedValue;
                    }
                    Environment.Exit(exitValue);
                }
                else if (String.Equals(command, "echo", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"{String.Join(" ", inputArgs)}");
                }
                else if (String.Equals(command, "type", StringComparison.OrdinalIgnoreCase))
                {
                    if (inputArgs.Length == 0 || String.IsNullOrEmpty(inputArgs[0]))
                    {
                        Console.WriteLine("type: missing operand");
                    }
                    else
                    {
                        string commandName = inputArgs[0].Trim();
                        bool isFound = false;

                        if (builtinCommands.Contains(commandName))
                        {
                            Console.WriteLine($"{commandName} is a shell builtin");
                            isFound = true;
                        }
                        else
                        {
                            string? pathEnv = Environment.GetEnvironmentVariable("PATH");

                            if (!String.IsNullOrEmpty(pathEnv))
                            {
                                foreach (string directory in pathEnv.Split(Path.PathSeparator))
                                {
                                    if (File.Exists(Path.Combine(directory, commandName)))
                                    {
                                        Console.WriteLine($"{commandName} is {Path.Combine(directory, commandName)}");
                                        isFound = true;
                                        break;
                                    }
                                }
                            }
                        }

                        if (!isFound)
                        {
                            Console.WriteLine($"{commandName}: not found");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"{command}: command not found");
                }
            }
        }
    }
}