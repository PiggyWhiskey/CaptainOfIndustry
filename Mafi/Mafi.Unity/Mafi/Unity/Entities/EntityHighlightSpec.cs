// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.EntityHighlightSpec
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Entities;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities
{
  public readonly struct EntityHighlightSpec
  {
    public readonly IRenderedEntity Entity;
    public readonly ColorRgba Color;

    public bool IsValid => this.Entity != null;

    public EntityHighlightSpec(IRenderedEntity entity, ColorRgba color)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.Entity = entity;
      this.Color = color;
    }
  }
}
