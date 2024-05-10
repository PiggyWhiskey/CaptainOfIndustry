// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Console.GameCommandsExecutor
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Mafi.Core.Console
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class GameCommandsExecutor
  {
    private readonly Dict<string, GameCommand> m_commands;
    private readonly Dict<Type, TypeConverter> m_converters;
    private readonly Lyst<string> m_paramsTmp;

    public IReadOnlyDictionary<string, GameCommand> Commands
    {
      get => (IReadOnlyDictionary<string, GameCommand>) this.m_commands;
    }

    public GameCommandsExecutor()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_commands = new Dict<string, GameCommand>();
      this.m_converters = new Dict<Type, TypeConverter>();
      this.m_paramsTmp = new Lyst<string>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public void RegisterCommand(
      object target,
      MethodInfo method,
      string documentation = "",
      string customCommandName = null,
      bool invokeOnMainThread = false,
      bool invokeDuringSync = false)
    {
      string name = customCommandName ?? method.Name;
      if (string.IsNullOrWhiteSpace(name))
      {
        Log.Error("Failed to register null or whitespace command name.");
      }
      else
      {
        string[] strArray = GameCommandsExecutor.tokenizeCmdName(name);
        string str = ((IEnumerable<string>) strArray).JoinStrings("_");
        if (this.m_commands.ContainsKey(str))
        {
          Log.Error("Multiple commands with the same name: " + str);
        }
        else
        {
          GameCommand gameCommand = new GameCommand(str, documentation, target, method, invokeOnMainThread, invokeDuringSync);
          this.m_commands[str] = gameCommand;
          this.m_commands[((IEnumerable<string>) strArray).JoinStrings()] = gameCommand;
        }
      }
    }

    private static string[] tokenizeCmdName(string name)
    {
      Assert.That<string>(name).IsNotNullOrEmpty();
      Lyst<string> lyst = new Lyst<string>(5);
      if (string.IsNullOrWhiteSpace(name))
        return lyst.ToArray();
      int startIndex = 0;
      int index = 1;
      int num = getCarType(name[0]);
      bool flag = true;
      for (; index < name.Length; ++index)
      {
        int carType = getCarType(name[index]);
        if (carType == num)
        {
          flag = false;
        }
        else
        {
          num = carType;
          if (flag)
          {
            flag = false;
          }
          else
          {
            lyst.Add(name.Substring(startIndex, index - startIndex));
            startIndex = index;
            if (name[index] == '_')
            {
              ++startIndex;
              ++index;
            }
            flag = true;
          }
        }
      }
      if (startIndex != index)
        lyst.Add(name.Substring(startIndex, index - startIndex));
      return lyst.ToArray<string>((Func<string, string>) (x => x.ToLowerInvariant()));

      static int getCarType(char c)
      {
        if (char.IsLower(c))
          return 1;
        if (char.IsUpper(c))
          return 2;
        return char.IsDigit(c) ? 3 : 4;
      }
    }

    /// <summary>
    /// Attempts to invoke method specified in the <paramref name="commandText" /> argument.
    /// <paramref name="commandText" /> has to have format: method_name arg1 arg2 (number of arguments is defined by
    /// method).
    /// </summary>
    public GameCommandResult TryExecute(string commandText)
    {
      GameCommand command;
      object[] parameters;
      string errorMessage;
      string unparsedStr;
      if (!this.TryParseCommand(commandText, out command, out parameters, out errorMessage, out unparsedStr))
        return new GameCommandResult(Option<object>.None, (Option<string>) errorMessage);
      Assert.That<int>(unparsedStr.Length).IsEqualTo(0);
      try
      {
        object obj = command.Method.Invoke(command.Target, parameters);
        return obj is GameCommandResult gameCommandResult ? gameCommandResult : GameCommandResult.Success(obj);
      }
      catch (Exception ex)
      {
        Log.Exception(ex, "Console command execution threw an exception: `" + commandText + "`");
        return GameCommandResult.Error(string.Format("Command invocation failed with an exception: {0}", (object) ex));
      }
    }

    public bool TryParseCommand(
      string commandStr,
      out GameCommand command,
      out string errorMessage,
      out string unparsedStr)
    {
      return this.TryParseCommand(commandStr, out command, out object[] _, out errorMessage, out unparsedStr, true);
    }

    /// <summary>Returns whether command parsing succeeded.</summary>
    /// <param name="commandStr">Text of command to be parsed.</param>
    /// <param name="command">Command if parsing was successful, null otherwise.</param>
    /// <param name="parameters">Parsed command parameters if parsing succeeded, null otherwise.</param>
    /// <param name="errorMessage">Error message if command parsing failed, null otherwise.</param>
    public bool TryParseCommand(
      string commandStr,
      out GameCommand command,
      out object[] parameters,
      out string errorMessage,
      out string unparsedStr,
      bool skipParamsParsing = false)
    {
      command = (GameCommand) null;
      parameters = (object[]) null;
      unparsedStr = commandStr.TrimStart();
      if (string.IsNullOrWhiteSpace(unparsedStr))
      {
        errorMessage = "Empty command.";
        return false;
      }
      if (unparsedStr[0] == '=' && this.m_commands.TryGetValue("=", out command) && command.Parameters.Length == 1)
      {
        unparsedStr = unparsedStr.Substring(1);
        if (!skipParamsParsing)
        {
          parameters = new object[1]
          {
            (object) unparsedStr.Trim()
          };
          unparsedStr = "";
        }
        errorMessage = "";
        return true;
      }
      string nextToken1 = GameCommandsExecutor.getNextToken(ref unparsedStr);
      if (!this.m_commands.TryGetValue(nextToken1, out command))
      {
        errorMessage = "Command '" + nextToken1 + "' not found.";
        return false;
      }
      if (!skipParamsParsing)
      {
        unparsedStr = unparsedStr.Trim();
        if (command.Parameters.Length == 1 && command.Parameters.First.ParameterType == typeof (string) && unparsedStr.Length > 0)
        {
          parameters = new object[1]{ (object) unparsedStr };
          unparsedStr = "";
          errorMessage = "";
          return true;
        }
        this.m_paramsTmp.Clear();
        while (unparsedStr.Length > 0)
        {
          string nextToken2 = GameCommandsExecutor.getNextToken(ref unparsedStr);
          if (nextToken2.Length > 0)
            this.m_paramsTmp.Add(nextToken2);
        }
        if (this.m_paramsTmp.Count < command.MandatoryParametersCount)
        {
          errorMessage = string.Format("Not enough arguments, expected at least {0}, ", (object) command.MandatoryParametersCount) + string.Format("passed: {0}.", (object) this.m_paramsTmp.Count);
          return false;
        }
        if (this.m_paramsTmp.Count > command.Parameters.Length)
        {
          errorMessage = string.Format("Too many parameters passed, expected at most {0}, ", (object) command.Parameters.Length) + string.Format("passed {0}.", (object) this.m_paramsTmp.Count);
          return false;
        }
        parameters = new object[command.Parameters.Length];
        for (int index = 0; index < command.Parameters.Length; ++index)
        {
          if (index >= this.m_paramsTmp.Count)
          {
            parameters[index] = Type.Missing;
          }
          else
          {
            TypeConverter converter = this.getConverter(command.Parameters[index].ParameterType);
            try
            {
              parameters[index] = converter.ConvertFromString(this.m_paramsTmp[index]);
            }
            catch (Exception ex)
            {
              errorMessage = string.Format("Unable to parse argument number {0} (expected type: ", (object) index) + string.Format("{0}): '{1}'. ", (object) command.Parameters[index].ParameterType, (object) this.m_paramsTmp[index]) + "Exception message: '" + ex.Message + "'";
              return false;
            }
          }
        }
      }
      errorMessage = "";
      return true;
    }

    private TypeConverter getConverter(Type type)
    {
      TypeConverter converter1;
      if (this.m_converters.TryGetValue(type, out converter1))
        return converter1;
      TypeConverter converter2 = TypeDescriptor.GetConverter(type);
      this.m_converters.Add(type, converter2);
      return converter2;
    }

    public string GetHelpFor(string commandStr, out string newCommandStr)
    {
      GameCommand command;
      string unparsedStr;
      if (this.TryParseCommand(commandStr, out command, out string _, out unparsedStr))
      {
        newCommandStr = command.CanonicalName + " " + unparsedStr.TrimStart();
        return command.CanonicalName + " " + this.getCommandHelp(command);
      }
      commandStr = commandStr.Trim();
      Lyst<string> lyst1 = this.m_commands.Where<KeyValuePair<string, GameCommand>>((Func<KeyValuePair<string, GameCommand>, bool>) (x => x.Key.StartsWith(commandStr, StringComparison.OrdinalIgnoreCase))).Select<KeyValuePair<string, GameCommand>, GameCommand>((Func<KeyValuePair<string, GameCommand>, GameCommand>) (x => x.Value)).Distinct<GameCommand>().Select<GameCommand, string>((Func<GameCommand, string>) (x => x.CanonicalName)).OrderBy<string, string>((Func<string, string>) (x => x)).ToLyst<string>();
      if (lyst1.IsEmpty)
      {
        newCommandStr = commandStr;
        Lyst<string> lyst2 = this.m_commands.Where<KeyValuePair<string, GameCommand>>((Func<KeyValuePair<string, GameCommand>, bool>) (x => x.Key.Contains(commandStr, StringComparison.OrdinalIgnoreCase))).Select<KeyValuePair<string, GameCommand>, GameCommand>((Func<KeyValuePair<string, GameCommand>, GameCommand>) (x => x.Value)).Distinct<GameCommand>().Select<GameCommand, string>((Func<GameCommand, string>) (x => x.CanonicalName)).OrderBy<string, string>((Func<string, string>) (x => x)).ToLyst<string>();
        if (lyst2.IsEmpty)
          return "No command match '" + commandStr + "'.";
        if (lyst2.Count == 1)
        {
          newCommandStr = lyst2[0] + " ";
          return "";
        }
        string commonPrefix = GameCommandsExecutor.getCommonPrefix(lyst2);
        if (string.IsNullOrEmpty(commonPrefix))
          return "Did you mean any of these:\n" + lyst2.JoinStrings("\n");
        newCommandStr = commonPrefix;
        return lyst2.JoinStrings("\n");
      }
      if (lyst1.Count == 1)
      {
        newCommandStr = lyst1[0] + " ";
        return "";
      }
      string commonPrefix1 = GameCommandsExecutor.getCommonPrefix(lyst1);
      if (commonPrefix1.Equals(commandStr, StringComparison.OrdinalIgnoreCase))
      {
        newCommandStr = commonPrefix1;
        return lyst1.JoinStrings("\n");
      }
      newCommandStr = commonPrefix1;
      return "";
    }

    private static string getCommonPrefix(Lyst<string> names)
    {
      int length = names.Min<string>((Func<string, int>) (x => x.Length));
      for (int index1 = 0; index1 < length; ++index1)
      {
        char ch = names[0][index1];
        for (int index2 = 1; index2 < names.Count; ++index2)
        {
          if ((int) names[index2][index1] != (int) ch)
            return names[0].Substring(0, index1);
        }
      }
      return names[0].Substring(0, length);
    }

    private string getCommandHelp(GameCommand command)
    {
      return !string.IsNullOrWhiteSpace(command.Documentation) ? command.Documentation + "\n" + command.ParametersDocStr : command.ParametersDocStr;
    }

    private static string getNextToken(ref string str)
    {
      str = str.TrimStart();
      int num1;
      int startIndex;
      if (str.StartsWith("\""))
      {
        num1 = str.IndexOf('"', 1);
        startIndex = 1;
      }
      else
      {
        int self = str.IndexOf(' ');
        int num2 = str.IndexOf(',');
        num1 = self >= 0 ? (num2 >= 0 ? self.Min(num2) : self) : num2;
        startIndex = 0;
      }
      string nextToken;
      if (num1 > 0)
      {
        nextToken = str.Substring(startIndex, num1 - startIndex);
        str = str.Substring(num1 + 1).Trim();
      }
      else
      {
        nextToken = str;
        str = "";
      }
      return nextToken;
    }
  }
}
