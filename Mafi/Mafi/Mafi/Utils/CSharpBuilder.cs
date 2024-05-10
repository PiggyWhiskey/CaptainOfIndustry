// Decompiled with JetBrains decompiler
// Type: Mafi.Utils.CSharpBuilder
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;
using System.Text;

#nullable disable
namespace Mafi.Utils
{
  public class CSharpBuilder
  {
    private readonly StringBuilder m_sb;

    public string GetCode() => this.m_sb.ToString();

    public void AppendSemicolon() => this.m_sb.AppendLine(";");

    public void Append(string str) => this.m_sb.Append(str);

    public void AppendCtorCall<T>(params string[] parameters)
    {
      this.m_sb.Append("new ");
      this.m_sb.Append(typeof (T).Name);
      this.m_sb.Append("(");
      this.m_sb.Append(string.Join(", ", parameters));
      this.m_sb.Append(")");
    }

    public static string FormatEnum<TEnum>(TEnum value) where TEnum : struct
    {
      Type enumType = typeof (TEnum);
      Assert.That<bool>(enumType.IsEnum).IsTrue();
      return (enumType.DeclaringType == (Type) null ? string.Empty : enumType.DeclaringType.Name + ".") + enumType.Name + "." + Enum.GetName(enumType, (object) value);
    }

    public static string FormatBool(bool value) => !value ? "false" : "true";

    public CSharpBuilder()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.m_sb = new StringBuilder();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
