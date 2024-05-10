// Decompiled with JetBrains decompiler
// Type: Mafi.EditorCollectionAttribute
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
  public sealed class EditorCollectionAttribute : Attribute
  {
    /// <summary>Allows to reorder items. Only supported by Lyst.</summary>
    public readonly bool AllowReorder;
    /// <summary>
    /// Add delete button for each item. Only supported by Lyst.
    /// </summary>
    public readonly bool AllowRemoval;

    public EditorCollectionAttribute(bool allowReorder = false, bool allowRemoval = false)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.AllowReorder = allowReorder;
      this.AllowRemoval = allowRemoval;
    }
  }
}
