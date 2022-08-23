using System.Text;

public class JPA_task2
{
    public enum ArgsTypeEnum
    {
        HelpRequest,
        CommandLineArgs,
        None
    }
    
    public enum OutputTypeEnum
    {
        Error,
        Help,
        Information
    }

    private class Defender
    {
        private string[] _args;
        private bool _errorFlag;
        private string _errorText;
        private readonly ArgsTypeEnum _argsType;

        public string[] Args
        {
            set => _args = value;
        }
        public bool ErrorFlag => _errorFlag;
        public string ErrorText => _errorText;
        public ArgsTypeEnum ArgsType => _argsType;

        public Defender(string[] Args)
        {
            _errorFlag = false;
            _errorText = string.Empty;
            _args = Args;
            _argsType = ArgsTypeEnum.None;
            switch (_args.Length)
            {
                case 2:
                    if (_args[1].ToLower() is "/help" or "--help" or "/?")  // Help request
                    {
                        _argsType = ArgsTypeEnum.HelpRequest;
                    }
                    break;

                case 6:
                    _argsType = ArgsTypeEnum.CommandLineArgs;
                    break;

                default: 
                    break;
            }
        }

        public void CheckArgs()
        {
            switch (_args.Length)
            {
                case 1:  // No command line args
                    break;
                case 2:  // Probably help request

                    if (_args[1].ToLower() is "/help" or "--help" or "/?")  // Help request
                    {
                        break;
                    }

                    _errorFlag = true;
                    _errorText += "  Options error: Undefined command\n";
                    _errorText += "  Use /help for more information.";
 
                    break;

                case 6:  // Check command line args before initialization

                    int tmp;
                    foreach (var _arg in _args.Skip(1))
                    {
                        try
                        {
                            tmp = Convert.ToInt32(_arg);
                        }
                        catch (OverflowException)
                        {
                            _errorFlag = true;
                            _errorText += "  " + _arg + " is outside the range of the Int32 type.";
                            break;
                        }
                        catch (FormatException)
                        {
                            _errorFlag = true;
                            _errorText += "  \"" + _arg + "\" can't recognized as a number.";
                            break;
                        }
                    }
                    break;

                default:  // Parameters count error

                    _errorFlag = true;
                    _errorText += "  Options error: Incorrect parameter count";
                    break;
            }
        }

        public void CheckConstrains()
        {
            if (_errorFlag
                || _argsType != ArgsTypeEnum.CommandLineArgs)
                return;

            Dictionary<string, int> keyValuePairs = new()
            {
                { "n", Convert.ToInt32(_args[1]) },
                { "a", Convert.ToInt32(_args[2]) },
                { "x", Convert.ToInt32(_args[3]) },
                { "b", Convert.ToInt32(_args[4]) },
                { "y", Convert.ToInt32(_args[5]) }
            };

            if (keyValuePairs.Values.Skip(1).Distinct().Count() != keyValuePairs.Count - 1)
            {
                _errorFlag = true;
                _errorText += "  Options error: Unique argument issue\n";
                _errorText += "  \"a\", \"x\", \"b\" and \"y\" must be unique";
                return;
            }

            if (keyValuePairs["n"] < 4)
            {
                _errorFlag = true;
                _errorText += "  Options error: Argument \"n\" issue\n";
                _errorText += "  Too small value. (3 < n < 101)";
                return;
            }

            if (keyValuePairs["n"] > 100)
            {
                _errorFlag = true;
                _errorText += "  Options error: Argument \"n\" issue\n";
                _errorText += "  Too high value. (3 < n < 101)";
                return;
            }

            foreach (KeyValuePair<string, int> pair in keyValuePairs.Skip(1))
            {
                if (pair.Value < 1)
                {
                    _errorFlag = true;
                    _errorText += "  Options error: Argument \"" + pair.Key + "\" issue\n";
                    _errorText += "  Too small value. (0 < " + pair.Key + " < n + 1)";
                    return;
                }
                if (pair.Value > keyValuePairs["n"])
                {
                    _errorFlag = true;
                    _errorText += "  Options error: Argument \"" + pair.Key + "\" issue\n";
                    _errorText += "  Too high value. (0 < " + pair.Key + " < n + 1)";
                    return;
                }
            }
        }
    }
    
    private class TickProcessor
    {
        private readonly int _n, _a, _x, _b, _y;
        private int _currentDaniel, _currentVlad;

        public int CurrentDaniel => _currentDaniel;
        public int CurrentVlad => _currentVlad;

        public TickProcessor(int N, int A, int X, int B, int Y)
        {
            _n = N;
            _a = A;
            _x = X;
            _b = B;
            _y = Y;
            _currentDaniel = A;
            _currentVlad = B;
        }

        public bool CanNextTick()
        {
            return (_currentDaniel != _x && _currentVlad != _y);
        }
        public void NextTick()
        {
            if (_currentDaniel == _n)
                _currentDaniel = 1;
            else
                _currentDaniel++;
            if (_currentVlad == 1)
                _currentVlad = _n;
            else
                _currentVlad--;
        }
              
    }

    private static void OutputText(OutputTypeEnum _outputTypeEnum, string _outputText)
    {
        switch (_outputTypeEnum)
        {
            case OutputTypeEnum.Error:
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR");
                    Console.ResetColor();
                    
                    break;
                }
            case OutputTypeEnum.Help:
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    break;
                }
            case OutputTypeEnum.Information:
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                }
            default:
                break;
        }

        Console.WriteLine(_outputText);
        Console.ResetColor();
    }

    private static string GetHelpInformation()
    {
        StringBuilder stringBuilder = new StringBuilder();

        stringBuilder.AppendLine("JPA task #2 built on August 22 2022 by Andrey Ivanov");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine("General Options:");
        stringBuilder.AppendLine("JPA_task2 [n] [a] [x] [b] [y]");
        stringBuilder.AppendLine("  [n] \t: {integer} n");
        stringBuilder.AppendLine("  [a] \t: {integer} a");
        stringBuilder.AppendLine("  [x] \t: {integer} x");
        stringBuilder.AppendLine("  [b] \t: {integer} b");
        stringBuilder.AppendLine("  [y] \t: {integer} y");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine("Help Options:");
        stringBuilder.AppendLine("/help or --help or /? \t: This help information");

        return stringBuilder.ToString();
    }
    
    private static string InputParameter(string _title)
    {
        Console.Write(_title);
        string? tmp = Console.ReadLine();
        if (string.IsNullOrEmpty(tmp))
            tmp = "0";
        return tmp;
    }

    private static void InitParameters(string[] _args, ArgsTypeEnum _argsTypeEnum, out int _n, out int _a, out int _x, out int _b, out int _y)
    {
        if (_argsTypeEnum == ArgsTypeEnum.CommandLineArgs)
        {
            _n = Convert.ToInt32(_args[1]);
            _a = Convert.ToInt32(_args[2]);
            _x = Convert.ToInt32(_args[3]);
            _b = Convert.ToInt32(_args[4]);
            _y = Convert.ToInt32(_args[5]);
        }
        else
        {
            string[] _argsTmp = new string[] { "Manual input" };
            _argsTmp = _argsTmp.Append(InputParameter("How many stations does Roflyandia have? (n) ")).ToArray();
            _argsTmp = _argsTmp.Append(InputParameter("Daniel's start station (a): ")).ToArray();
            _argsTmp = _argsTmp.Append(InputParameter("Daniel's finish station (x): ")).ToArray();
            _argsTmp = _argsTmp.Append(InputParameter("Vlad's start station (b): ")).ToArray();
            _argsTmp = _argsTmp.Append(InputParameter("Vlad's finish station (y): ")).ToArray();

            Defender defender = new(_argsTmp);
            defender.CheckArgs();
            defender.CheckConstrains();

            if (defender.ErrorFlag)
            {
                OutputText(OutputTypeEnum.Error, defender.ErrorText);
                _n = 0; _a = 0; _x = 0; _b = 0; _y = 0;
            }
            else
            {
                InitParameters(_argsTmp, ArgsTypeEnum.CommandLineArgs, out _n, out _a, out _x, out _b, out _y);
            }
        }
    }

    private static string CalucalteTask(int _n, int _a, int _x, int _b, int _y)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("NO");

        TickProcessor tickProcessor = new(_n, _a, _x, _b, _y);
        while (tickProcessor.CanNextTick())
        {
            tickProcessor.NextTick();

            if (tickProcessor.CurrentDaniel == tickProcessor.CurrentVlad)
                return $"YES\nShared station: ({tickProcessor.CurrentDaniel}, {tickProcessor.CurrentVlad})";

            if (Math.Abs(tickProcessor.CurrentDaniel - tickProcessor.CurrentVlad) == 1)
                stringBuilder.AppendLine($"Closest station: ({tickProcessor.CurrentDaniel}, {tickProcessor.CurrentVlad})");
        }

        return stringBuilder.ToString();
    }

    public static void Main()
    {
        string[] args = Environment.GetCommandLineArgs();

        Defender defender = new(args);
        defender.CheckArgs();
        defender.CheckConstrains();

        if (defender.ErrorFlag)
        {
            OutputText(OutputTypeEnum.Error, defender.ErrorText);
            return;
        }

        if (defender.ArgsType == ArgsTypeEnum.HelpRequest)
        {
            OutputText(OutputTypeEnum.Help, GetHelpInformation());
            return;
        }

        InitParameters(args, defender.ArgsType, out int n, out int a, out int x, out int b, out int y);

        OutputText(OutputTypeEnum.Information, CalucalteTask(n, a, x, b, y));

    }
}