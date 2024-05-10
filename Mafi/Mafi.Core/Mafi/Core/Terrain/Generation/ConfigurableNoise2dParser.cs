// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.ConfigurableNoise2dParser
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Random.Noise;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class ConfigurableNoise2dParser
  {
    private static readonly char[] PARAM_SPLIT_CHAR;
    private readonly Dict<string, ConfigurableNoise2dParser.InitialStatementData> m_initialStatements;
    private readonly Dict<string, ConfigurableNoise2dParser.TransformStatementData> m_transformStatements;
    private readonly Dict<string, Type> m_parameterTypeLookup;
    private readonly Dict<Type, Func<object>> m_parametersFactories;

    public IReadOnlyDictionary<string, ConfigurableNoise2dParser.InitialStatementData> InitialStatements
    {
      get
      {
        return (IReadOnlyDictionary<string, ConfigurableNoise2dParser.InitialStatementData>) this.m_initialStatements;
      }
    }

    public IReadOnlyDictionary<string, ConfigurableNoise2dParser.TransformStatementData> TransformStatements
    {
      get
      {
        return (IReadOnlyDictionary<string, ConfigurableNoise2dParser.TransformStatementData>) this.m_transformStatements;
      }
    }

    public IReadOnlyDictionary<string, Type> ParameterTypeLookup
    {
      get => (IReadOnlyDictionary<string, Type>) this.m_parameterTypeLookup;
    }

    public void RegisterParameterType(Type type) => this.m_parameterTypeLookup[type.Name] = type;

    public bool TryRegisterInitialStatement(
      string name,
      string description,
      ImmutableArray<ConfigurableNoise2dParamSpec> parameters,
      Func<object[], INoise2D> factoryFn)
    {
      name = name.Trim();
      if (this.m_initialStatements.ContainsKey(name))
        return false;
      foreach (ConfigurableNoise2dParamSpec parameter in parameters)
        this.RegisterParameterType(parameter.Type);
      this.m_initialStatements.Add(name, new ConfigurableNoise2dParser.InitialStatementData(name, description.NoneIfNullOrWhiteSpace(), parameters, factoryFn));
      return true;
    }

    public void RegisterInitialStatement(
      string name,
      string description,
      ImmutableArray<ConfigurableNoise2dParamSpec> parameters,
      Func<object[], INoise2D> factoryFn)
    {
      if (this.TryRegisterInitialStatement(name, description, parameters, factoryFn))
        return;
      Log.Error("Failed to register initial statement '" + name + "'.");
    }

    public bool TryRegisterTransformStatement(
      string name,
      string description,
      ImmutableArray<ConfigurableNoise2dParamSpec> parameters,
      Func<INoise2D, object[], INoise2D> factoryFn)
    {
      name = name.Trim();
      if (this.m_transformStatements.ContainsKey(name))
        return false;
      foreach (ConfigurableNoise2dParamSpec parameter in parameters)
        this.RegisterParameterType(parameter.Type);
      this.m_transformStatements.Add(name, new ConfigurableNoise2dParser.TransformStatementData(name, description.NoneIfNullOrWhiteSpace(), parameters, factoryFn));
      return true;
    }

    public void RegisterTransformStatement(
      string name,
      string description,
      ImmutableArray<ConfigurableNoise2dParamSpec> parameters,
      Func<INoise2D, object[], INoise2D> factoryFn)
    {
      if (this.TryRegisterTransformStatement(name, description, parameters, factoryFn))
        return;
      Log.Error("Failed to register transform statement '" + name + "'.");
    }

    public void RegisterDefaultParameterFactory(Func<object> factory)
    {
      this.m_parametersFactories[factory().GetType()] = factory;
    }

    public bool TryGetDefaultParameter(Type t, out object value)
    {
      Func<object> func;
      if (this.m_parametersFactories.TryGetValue(t, out func))
      {
        value = func();
        return true;
      }
      value = (object) null;
      return false;
    }

    public object GetDefaultParameter(Type t)
    {
      Func<object> func;
      return !this.m_parametersFactories.TryGetValue(t, out func) ? Activator.CreateInstance(t) : func();
    }

    public bool TryParseNoiseFactorySpec(
      IEnumerable<string> lines,
      out ConfigurableNoise2dFactorySpec factorySpec,
      out string error)
    {
      factorySpec = (ConfigurableNoise2dFactorySpec) null;
      Dict<string, ImmutableArray<string>> outBlocks = new Dict<string, ImmutableArray<string>>();
      if (!this.TryParseBlocks(lines, outBlocks, out error))
        return false;
      ImmutableArray<string> empty;
      if (outBlocks.TryGetValue("parameters", out empty))
        outBlocks.Remove("parameters");
      else
        empty = ImmutableArray<string>.Empty;
      Dict<string, Type> parameters = new Dict<string, Type>();
      if (!this.TryParseParameters(empty.AsEnumerable(), parameters, out error))
        return false;
      Lyst<ConfigurableNoise2dFactorySpec.Block> lyst1 = new Lyst<ConfigurableNoise2dFactorySpec.Block>();
      Lyst<Pair<string, ImmutableArray<string>>> lyst2 = new Lyst<Pair<string, ImmutableArray<string>>>();
      foreach (KeyValuePair<string, ImmutableArray<string>> keyValuePair in outBlocks)
      {
        lyst2.Clear();
        int index = 0;
        while (true)
        {
          int num = index;
          ImmutableArray<string> immutableArray = keyValuePair.Value;
          int length = immutableArray.Length;
          if (num < length)
          {
            immutableArray = keyValuePair.Value;
            string str = immutableArray[index].Trim();
            if (str.StartsWith("|>"))
            {
              if (index != 0)
                str = str.SubstringSafe(2).Trim();
              else
                break;
            }
            else if (index > 0)
              goto label_15;
            Pair<string, ImmutableArray<string>> statement;
            if (this.TryParseStatement(str, out statement, out error))
            {
              lyst2.Add(statement);
              ++index;
            }
            else
              goto label_17;
          }
          else
            goto label_20;
        }
        error = string.Format("Unexpected pipeline operator on line '{0}'.", (object) lyst2);
        return false;
label_15:
        error = string.Format("Missing pipeline operator on line '{0}'.", (object) lyst2);
        return false;
label_17:
        error = "Failed to parse statement: " + error;
        return false;
label_20:
        lyst1.Add(new ConfigurableNoise2dFactorySpec.Block(keyValuePair.Key, lyst2.ToImmutableArray()));
      }
      factorySpec = new ConfigurableNoise2dFactorySpec(parameters, lyst1.ToImmutableArray());
      return true;
    }

    /// <summary>
    /// Tries to build the noise. All noise blocks will be added to the <paramref name="argsDict" />.
    /// </summary>
    public bool TryBuildNoise(
      ConfigurableNoise2dFactorySpec factorySpec,
      Dict<string, object> argsDict,
      out INoise2D resultingNoise,
      out string error)
    {
      resultingNoise = (INoise2D) null;
      if (factorySpec.StatementsBlocks.IsEmpty)
      {
        error = "No blocks to process.";
        return false;
      }
      foreach (ConfigurableNoise2dFactorySpec.Block statementsBlock in factorySpec.StatementsBlocks)
      {
        if (argsDict.ContainsKey(statementsBlock.Name))
        {
          error = "Block name '" + statementsBlock.Name + "' clashes with parameter.";
          return false;
        }
      }
      for (int index = 0; index < factorySpec.StatementsBlocks.Length; ++index)
      {
        ConfigurableNoise2dFactorySpec.Block statementsBlock = factorySpec.StatementsBlocks[index];
        if (!this.TryBuildNoiseBlock(statementsBlock.Statements, argsDict, out resultingNoise, out error))
          return false;
        argsDict.Add(statementsBlock.Name, (object) resultingNoise);
      }
      error = "";
      return true;
    }

    public bool TryBuildNoiseBlock(
      ImmutableArray<Pair<string, ImmutableArray<string>>> statements,
      Dict<string, object> argsDict,
      out INoise2D resultingNoise,
      out string error)
    {
      resultingNoise = (INoise2D) null;
      if (statements.IsEmpty)
      {
        error = "No statements";
        return false;
      }
      ConfigurableNoise2dParser.InitialStatementData initialStatementData;
      if (!this.m_initialStatements.TryGetValue(statements.First.First, out initialStatementData))
      {
        error = "Failed to find initial function '" + statements.First.First + "'.";
        return false;
      }
      object[] result1;
      if (!tryCollectArgsPooled(initialStatementData.Parameters, statements.First.Second, out result1, out error))
        return false;
      try
      {
        resultingNoise = initialStatementData.FactoryFn(result1);
      }
      catch (InvalidCastException ex)
      {
        error = "Failed to invoke factory function '" + initialStatementData.Name + "', argument cast likely failed. Args: " + ((IEnumerable<string>) result1.MapArray<object, string>((Func<object, string>) (x => x.GetType().Name))).JoinStrings(", ");
        return false;
      }
      catch (Exception ex)
      {
        error = "Failed to invoke factory function '" + initialStatementData.Name + "': " + ex.Message;
        return false;
      }
      finally
      {
        result1.ReturnToPool<object>();
      }
      for (int index = 1; index < statements.Length; ++index)
      {
        Pair<string, ImmutableArray<string>> statement = statements[index];
        ConfigurableNoise2dParser.TransformStatementData transformStatementData;
        if (!this.m_transformStatements.TryGetValue(statement.First, out transformStatementData))
        {
          error = "Failed to find function '" + statement.First + "'.";
          return false;
        }
        object[] result2;
        if (!tryCollectArgsPooled(transformStatementData.Parameters, statement.Second, out result2, out error))
          return false;
        resultingNoise = transformStatementData.FactoryFn(resultingNoise, result2);
        result2.ReturnToPool<object>();
      }
      return true;

      bool tryCollectArgsPooled(
        ImmutableArray<ConfigurableNoise2dParamSpec> parameters,
        ImmutableArray<string> paramNames,
        out object[] result,
        out string error)
      {
        result = (object[]) null;
        if (parameters.Length != paramNames.Length)
        {
          error = string.Format("Function '{0}' requires {1} parameters", (object) statements.First.First, (object) parameters.Length) + string.Format("but {0} were provided.", (object) statements.First.Second.Length);
          return false;
        }
        result = ArrayPool<object>.Get(parameters.Length);
        for (int index = 0; index < parameters.Length; ++index)
        {
          ConfigurableNoise2dParamSpec parameter = parameters[index];
          string paramName = paramNames[index];
          if (!argsDict.TryGetValue(paramName, out result[index]))
          {
            error = "Failed to find parameter '" + paramName + "' of type '" + parameter.Type.Name + "'.";
            result.ReturnToPool<object>();
            return false;
          }
          if (!result[index].GetType().IsAssignableTo(parameter.Type))
          {
            error = string.Format("Unexpected type '{0}' of parameter '{1}',", (object) result[index].GetType(), (object) paramName) + " expected type '" + parameter.Type.Name + "'.";
            result.ReturnToPool<object>();
            return false;
          }
        }
        error = "";
        return true;
      }
    }

    public bool TryParseBlocks(
      IEnumerable<string> lines,
      Dict<string, ImmutableArray<string>> outBlocks,
      out string error)
    {
      string currentBlockName = (string) null;
      Lyst<string> currentBlockLines = new Lyst<string>();
      foreach (string line in lines)
      {
        string str = line.Trim();
        if (!string.IsNullOrEmpty(str) && str[0] != '#')
        {
          if (line[0] == ' ' || line[0] == '\t')
          {
            if (currentBlockName == null)
            {
              error = "Failed to parse block, statement without block header: '" + line + "'.";
              return false;
            }
            currentBlockLines.Add(str);
          }
          else
          {
            if (currentBlockName != null && !tryAddNewBlock(out error))
              return false;
            currentBlockName = str;
          }
        }
      }
      if (currentBlockName != null && !tryAddNewBlock(out error))
        return false;
      error = "";
      return true;

      bool tryAddNewBlock(out string error)
      {
        if (outBlocks.ContainsKey(currentBlockName))
        {
          error = "Duplicate block name '" + currentBlockName + "'.";
          return false;
        }
        error = "";
        outBlocks.Add(currentBlockName, currentBlockLines.ToImmutableArrayAndClear());
        return true;
      }
    }

    public bool TryParseParameters(
      IEnumerable<string> lines,
      Dict<string, Type> parameters,
      out string error)
    {
      foreach (string line in lines)
      {
        KeyValuePair<string, Type> parameter;
        if (!this.TryParseParameter(line, out parameter, out error))
        {
          error = "Failed to parse parameter: " + error;
          return false;
        }
        if (!parameters.TryAdd(parameter.Key, parameter.Value))
        {
          error = "Duplicate parameter '" + parameter.Key + "'.";
          return false;
        }
      }
      error = "";
      return true;
    }

    public bool TryParseParameter(
      string line,
      out KeyValuePair<string, Type> parameter,
      out string error)
    {
      parameter = new KeyValuePair<string, Type>();
      int num = line.IndexOf(':');
      if (num < 0)
      {
        error = "The ':' character was not found in '" + line + "'.";
        return false;
      }
      string key1 = line.SubstringSafe(0, new int?(num)).Trim();
      if (key1.Length == 0)
      {
        error = "Name (before ':') is empty in '" + line + "'.";
        return false;
      }
      string key2 = line.SubstringSafe(num + 1).Trim();
      if (key2.Length == 0)
      {
        error = "Type (after ':') is empty in '" + line + "'.";
        return false;
      }
      Type type;
      if (!this.m_parameterTypeLookup.TryGetValue(key2, out type))
      {
        error = "Failed to parse '" + key2 + "' as a type. The type name is either invalid or not registered.";
        return false;
      }
      error = "";
      parameter = new KeyValuePair<string, Type>(key1, type);
      return true;
    }

    public bool TryParseStatement(
      string line,
      out Pair<string, ImmutableArray<string>> statement,
      out string error)
    {
      statement = new Pair<string, ImmutableArray<string>>();
      line = line.Trim();
      int num = line.IndexOf('(');
      if (num < 0)
      {
        if (line.IndexOf(')') >= 0 || line.IndexOf(',') >= 0)
        {
          error = string.Format("Invalid statement, missing '{0}' in '{1}'.", (object) '(', (object) line);
          return false;
        }
        statement = Pair.Create<string, ImmutableArray<string>>(line, ImmutableArray<string>.Empty);
        error = "";
        return true;
      }
      if (line[line.Length - 1] != ')')
      {
        error = string.Format("Invalid statement, missing '{0}' at the end of '{1}'.", (object) ')', (object) line);
        return false;
      }
      string first = line.SubstringSafe(0, new int?(num)).Trim();
      if (first.Length == 0)
      {
        error = "Name is empty in '" + line + "'.";
        return false;
      }
      string[] strArray = line.SubstringSafe(num + 1, new int?(line.Length - num - 2)).Trim().Split(ConfigurableNoise2dParser.PARAM_SPLIT_CHAR, StringSplitOptions.RemoveEmptyEntries);
      if (strArray.Length == 0)
      {
        statement = Pair.Create<string, ImmutableArray<string>>(first, ImmutableArray<string>.Empty);
        error = "";
        return true;
      }
      ImmutableArrayBuilder<string> immutableArrayBuilder = new ImmutableArrayBuilder<string>(strArray.Length);
      for (int i = 0; i < strArray.Length; ++i)
      {
        immutableArrayBuilder[i] = strArray[i].Trim();
        if (immutableArrayBuilder[i].Length == 0)
        {
          error = string.Format("Argument {0} is empty in '{1}'.", (object) i, (object) line);
          return false;
        }
      }
      statement = Pair.Create<string, ImmutableArray<string>>(first, immutableArrayBuilder.GetImmutableArrayAndClear());
      error = "";
      return true;
    }

    public ConfigurableNoise2dParser()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_initialStatements = new Dict<string, ConfigurableNoise2dParser.InitialStatementData>();
      this.m_transformStatements = new Dict<string, ConfigurableNoise2dParser.TransformStatementData>();
      this.m_parameterTypeLookup = new Dict<string, Type>();
      this.m_parametersFactories = new Dict<Type, Func<object>>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static ConfigurableNoise2dParser()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ConfigurableNoise2dParser.PARAM_SPLIT_CHAR = new char[1]
      {
        ','
      };
    }

    public readonly struct InitialStatementData
    {
      public readonly string Name;
      public readonly Option<string> Description;
      public readonly ImmutableArray<ConfigurableNoise2dParamSpec> Parameters;
      public readonly Func<object[], INoise2D> FactoryFn;

      public InitialStatementData(
        string name,
        Option<string> description,
        ImmutableArray<ConfigurableNoise2dParamSpec> parameters,
        Func<object[], INoise2D> factoryFn)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Name = name;
        this.Description = description;
        this.Parameters = parameters;
        this.FactoryFn = factoryFn;
      }
    }

    public readonly struct TransformStatementData
    {
      public readonly string Name;
      public readonly Option<string> Description;
      public readonly ImmutableArray<ConfigurableNoise2dParamSpec> Parameters;
      public readonly Func<INoise2D, object[], INoise2D> FactoryFn;

      public TransformStatementData(
        string name,
        Option<string> description,
        ImmutableArray<ConfigurableNoise2dParamSpec> parameters,
        Func<INoise2D, object[], INoise2D> factoryFn)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Name = name;
        this.Description = description;
        this.Parameters = parameters;
        this.FactoryFn = factoryFn;
      }
    }
  }
}
