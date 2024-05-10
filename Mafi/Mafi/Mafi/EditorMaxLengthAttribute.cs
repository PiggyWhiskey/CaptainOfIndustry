// Decompiled with JetBrains decompiler
// Type: Mafi.EditorMaxLengthAttribute
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
  public sealed class EditorMaxLengthAttribute : Attribute, IEditorValidationAttribute
  {
    public readonly int MaxLength;

    public EditorMaxLengthAttribute(int maxLength)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.MaxLength = maxLength;
    }

    public string ValidateObj(object obj)
    {
      string str = (string) obj;
      return str.Length > this.MaxLength ? string.Format("Max chars allowed {0} (is {1})", (object) this.MaxLength, (object) str.Length) : string.Empty;
    }
  }
}
