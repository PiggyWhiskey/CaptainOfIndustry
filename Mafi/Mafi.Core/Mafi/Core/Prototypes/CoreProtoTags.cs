// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Prototypes.CoreProtoTags
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Ports.Io;

#nullable disable
namespace Mafi.Core.Prototypes
{
  public static class CoreProtoTags
  {
    /// <summary>
    /// For <see cref="T:Mafi.Core.Ports.Io.IoPortShapeProto" />. Identifies a port shape proto that represents a mechanical shaft.
    /// TODO: Kill this if possible.
    /// </summary>
    public static readonly Tag MechanicalShaft;

    static CoreProtoTags()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      CoreProtoTags.MechanicalShaft = Tag.Create<IoPortShapeProto>(nameof (MechanicalShaft));
    }
  }
}
