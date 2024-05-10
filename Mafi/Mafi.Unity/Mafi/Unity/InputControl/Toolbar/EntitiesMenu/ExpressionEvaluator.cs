// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Toolbar.EntitiesMenu.ExpressionEvaluator
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Console;
using System;
using System.Data;
using System.Text.RegularExpressions;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Toolbar.EntitiesMenu
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class ExpressionEvaluator
  {
    private readonly DataTable m_dataTable;
    private readonly Regex m_expressionMatcher;
    private readonly Regex m_digitMatcher;
    private readonly Regex m_operatorMatcher;

    public ExpressionEvaluator()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_dataTable = new DataTable();
      this.m_expressionMatcher = new Regex("^[ \\.,\\d()/*+%-]+$", RegexOptions.Compiled);
      this.m_digitMatcher = new Regex("\\d+", RegexOptions.Compiled);
      this.m_operatorMatcher = new Regex("[/*+%-]+", RegexOptions.Compiled);
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public bool TryEvaluate(string input, out string result)
    {
      result = "";
      if (!this.m_digitMatcher.IsMatch(input) || !this.m_operatorMatcher.IsMatch(input))
        return false;
      if (!this.m_expressionMatcher.IsMatch(input))
        return false;
      try
      {
        input = input.Replace(',', '.');
        object obj = this.m_dataTable.Compute(input, "");
        switch (obj)
        {
          case int _:
            result = obj.ToString();
            break;
          case Decimal num1:
            result = num1.ToString("F");
            break;
          case double num2:
            result = num2.ToString("F");
            break;
          default:
            result = obj.ToString();
            break;
        }
        return true;
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    [ConsoleCommand(false, false, null, "=")]
    private string calculate(string input)
    {
      string result;
      return this.TryEvaluate(input, out result) ? result : "N/A";
    }
  }
}
