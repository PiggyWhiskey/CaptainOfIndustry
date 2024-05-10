// Decompiled with JetBrains decompiler
// Type: Mafi.EditorRangeAttribute
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
  public class EditorRangeAttribute : Attribute, IEditorValidationAttribute
  {
    private readonly double m_min;
    private readonly double m_max;

    public EditorRangeAttribute(double min, double max)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_min = min;
      this.m_max = max;
    }

    public string ValidateObj(object obj)
    {
      if (obj is Fix32)
        return checkDouble(((Fix32) obj).ToDouble());
      if (obj is Fix64)
        return checkDouble(((Fix64) obj).ToDouble());
      if (obj is Percent)
        return checkDouble(((Percent) obj).ToDouble());
      if (obj is IConvertible convertible)
        return checkDouble(convertible.ToDouble((IFormatProvider) null));
      Log.Error(string.Format("Type {0} not supported for range check.", (object) obj?.GetType()));
      return string.Empty;

      string checkDouble(double value)
      {
        if (value >= this.m_min && value <= this.m_max)
          return string.Empty;
        return obj is Percent ? string.Format("Value is out of range [{0}%, {1}%]", (object) (this.m_min * 100.0), (object) (this.m_max * 100.0)) : string.Format("Value is out of range [{0}, {1}]", (object) this.m_min, (object) this.m_max);
      }
    }
  }
}
