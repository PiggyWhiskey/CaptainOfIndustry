// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Prototypes.IProtoWithIconAndName
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Localization.Quantity;

#nullable disable
namespace Mafi.Core.Prototypes
{
  public interface IProtoWithIconAndName : IProtoWithIcon, IProto
  {
    /// <summary>
    /// Allows to formats quantity of the current proto with proper units so it can displayed to the player.
    /// </summary>
    QuantityFormatter QuantityFormatter { get; }
  }
}
