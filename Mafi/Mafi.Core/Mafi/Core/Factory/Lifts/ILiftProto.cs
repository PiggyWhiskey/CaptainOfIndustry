// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Lifts.ILiftProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Ports.Io;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Factory.Lifts
{
  public interface ILiftProto : ILayoutEntityProto, IStaticEntityProto, IEntityProto, IProto
  {
    /// <summary>
    /// Change in "output" height compared to initial placement position.
    /// </summary>
    ThicknessTilesI HeightDelta { get; }

    /// <summary>
    /// The minimum height delta across all protos of this type.
    /// </summary>
    ThicknessTilesI MinHeightDelta { get; }

    IoPortShapeProto PortsShape { get; }

    bool TryGetHeightReversedProto(ProtosDb protosDb, out ILiftProto newProto);

    bool TryGetProtoForHeightDelta(ProtosDb protosDb, int heightChange, out ILiftProto newProto);
  }
}
