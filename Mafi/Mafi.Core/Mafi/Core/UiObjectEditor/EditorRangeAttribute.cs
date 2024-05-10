// Decompiled with JetBrains decompiler
// Type: Mafi.Core.UiObjectEditor.EditorRangeAttribute
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi.Core.UiObjectEditor
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
  public sealed class EditorRangeAttribute : Attribute, IEditorValidationAttribute
  {
    private readonly double m_min;
    private readonly double m_max;

    public EditorRangeAttribute(double min, double max)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_min = min;
      this.m_max = max;
    }

    public string ValidateObj(object obj)
    {
      switch (obj)
      {
        case Fix32 fix32:
          return checkDouble(fix32.ToDouble());
        case IConvertible convertible:
          return checkDouble(convertible.ToDouble((IFormatProvider) null));
        default:
          Log.Error(string.Format("Type {0} not supported for range check.", (object) obj?.GetType()));
          return string.Empty;
      }

      string checkDouble(double value)
      {
        return value < this.m_min || value > this.m_max ? string.Format("Value is out of range [{0}, {1}]", (object) this.m_min, (object) this.m_max) : string.Empty;
      }
    }
  }
}
