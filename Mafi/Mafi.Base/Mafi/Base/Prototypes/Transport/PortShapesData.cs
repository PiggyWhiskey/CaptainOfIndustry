// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Transport.PortShapesData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Mods;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Base.Prototypes.Transport
{
  public class PortShapesData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      ProtosDb prototypesDb = registrator.PrototypesDb;
      prototypesDb.Add<IoPortShapeProto>(new IoPortShapeProto(Ids.IoPortShapes.FlatConveyor, Proto.Str.Empty, '#', CountableProductProto.ProductType, new IoPortShapeProto.Gfx("Assets/Base/Transports/ConveyorUnit/Port.prefab", "Assets/Base/Transports/ConveyorUnit/Port-Lod3.prefab", disconnectedPrefabPath: (Option<string>) "Assets/Base/Transports/ConveyorUnit/PortEnd.prefab", disconnectedPrefabPathLod3: (Option<string>) "Assets/Base/Transports/ConveyorUnit/PortEnd-Lod3.prefab")));
      prototypesDb.Add<IoPortShapeProto>(new IoPortShapeProto(Ids.IoPortShapes.LooseMaterialConveyor, Proto.Str.Empty, '~', LooseProductProto.ProductType, new IoPortShapeProto.Gfx("Assets/Base/Transports/ConveyorLoose/Port.prefab", "Assets/Base/Transports/ConveyorLoose/Port-Lod3.prefab", disconnectedPrefabPath: (Option<string>) "Assets/Base/Transports/ConveyorLoose/PortEnd.prefab", disconnectedPrefabPathLod3: (Option<string>) "Assets/Base/Transports/ConveyorLoose/PortEnd-Lod3.prefab")));
      prototypesDb.Add<IoPortShapeProto>(new IoPortShapeProto(Ids.IoPortShapes.MoltenMetalChannel, Proto.Str.Empty, '\'', MoltenProductProto.ProductType, new IoPortShapeProto.Gfx("Assets/Base/Transports/MoltenChannel/Port.prefab", "Assets/Base/Transports/MoltenChannel/Port-Lod3.prefab", disconnectedPrefabPath: (Option<string>) "Assets/Base/Transports/MoltenChannel/PortEnd.prefab", disconnectedPrefabPathLod3: (Option<string>) "Assets/Base/Transports/MoltenChannel/PortEnd-Lod3.prefab")));
      prototypesDb.Add<IoPortShapeProto>(new IoPortShapeProto(Ids.IoPortShapes.Pipe, Proto.Str.Empty, '@', FluidProductProto.ProductType, new IoPortShapeProto.Gfx("Assets/Base/Transports/Pipes/Port.prefab", "Assets/Base/Transports/Pipes/Port-Lod3.prefab", true)));
      prototypesDb.Add<IoPortShapeProto>(new IoPortShapeProto(Ids.IoPortShapes.Shaft, Proto.Str.Empty, '|', ProductType.NONE, new IoPortShapeProto.Gfx("Assets/Base/Transports/Shaft/Port.prefab", "Assets/Base/Transports/Shaft/Port-Lod3.prefab"), (IEnumerable<Tag>) new Tag[1]
      {
        CoreProtoTags.MechanicalShaft
      }));
    }

    public PortShapesData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
