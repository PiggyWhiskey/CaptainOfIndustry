﻿// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Ports.Io.PortHighlightSpec
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.Utils;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Ports.Io
{
  public struct PortHighlightSpec
  {
    public readonly ColorRgba? HighlightColor;
    public readonly Mafi.Unity.Ports.Io.ArrowSpec? ArrowSpec;
    public readonly MultiIconSpec? IconSpec;
    public readonly bool ActivatePortCollider;

    public PortHighlightSpec(
      ColorRgba? highlightColor = null,
      Mafi.Unity.Ports.Io.ArrowSpec? arrowSpec = null,
      MultiIconSpec? iconSpec = null,
      bool activatePortCollider = false)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.HighlightColor = highlightColor;
      this.ArrowSpec = arrowSpec;
      this.IconSpec = iconSpec;
      this.ActivatePortCollider = activatePortCollider;
    }
  }
}
