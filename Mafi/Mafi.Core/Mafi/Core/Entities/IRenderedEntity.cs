// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.IRenderedEntity
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Entities
{
  /// <summary>
  /// Entity that should be rendered in the world meaning a renderer should be auto-created by EntitiesRenderer.
  /// </summary>
  public interface IRenderedEntity : IEntity, IIsSafeAsHashKey
  {
    /// <summary>
    /// Data used solely by renderers. This should not be touched from sim or other places.
    /// </summary>
    /// <remarks>
    /// This data is an ulong to be able to store any data renderers need, usage of struct would be better but
    /// currently limiting. TODO: Rethink this and improve it once we get rid of MB-based rendering.
    /// </remarks>
    ulong RendererData { get; set; }
  }
}
