// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Prototypes.IProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Mods;

#nullable disable
namespace Mafi.Core.Prototypes
{
  public interface IProto
  {
    Proto.Str Strings { get; }

    Proto.ID Id { get; }

    bool IsAvailable { get; }

    bool IsNotAvailable { get; }

    IMod Mod { get; }

    bool TryGetParam<T>(out T paramValue) where T : class;
  }
}
