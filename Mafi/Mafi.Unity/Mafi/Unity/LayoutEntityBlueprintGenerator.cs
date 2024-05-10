// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.LayoutEntityBlueprintGenerator
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Console;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Ports.Io;
using Mafi.Core.Prototypes;
using Mafi.Unity.Entities.Static;
using Mafi.Unity.Factory.Transports;
using Mafi.Unity.Ports.Io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class LayoutEntityBlueprintGenerator
  {
    private readonly LayoutEntityModelFactory m_layoutEntityModelFactory;
    private readonly IoPortModelFactory m_portModelFactory;
    private readonly TransportModelFactory m_transportModelFactory;
    private readonly ProtosDb m_protosDb;
    private readonly Dict<IoPortShapeProto, TransportProto> m_shapeToTransport;

    public LayoutEntityBlueprintGenerator(AssetsDb assetsDb, ProtosDb protosDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_protosDb = protosDb;
      this.m_layoutEntityModelFactory = new LayoutEntityModelFactory(assetsDb);
      this.m_portModelFactory = new IoPortModelFactory(assetsDb);
      this.m_transportModelFactory = new TransportModelFactory(assetsDb);
      this.m_shapeToTransport = protosDb.All<TransportProto>().Where<TransportProto>((Func<TransportProto, bool>) (x => x.Upgrade.NextTier.IsNone)).ToDict<TransportProto, IoPortShapeProto, TransportProto>((Func<TransportProto, IoPortShapeProto>) (x => x.PortsShape), (Func<TransportProto, TransportProto>) (x => x));
    }

    [ConsoleCommand(false, false, null, null)]
    private GameCommandResult printEntityLayout(string name)
    {
      Lyst<LayoutEntityProto> lyst = this.m_protosDb.All<LayoutEntityProto>().Where<LayoutEntityProto>((Func<LayoutEntityProto, bool>) (x => x.Id.Value.Contains(name, StringComparison.OrdinalIgnoreCase))).ToLyst<LayoutEntityProto>();
      if (lyst.IsEmpty)
        return GameCommandResult.Error("No matching entity.");
      if (lyst.Count > 8)
        return GameCommandResult.Error("Too many matching entities, be more specific:\n" + ((IEnumerable<string>) lyst.Select<string>((Func<LayoutEntityProto, string>) (x => x.Id.Value))).JoinStrings("\n"));
      StringBuilder stringBuilder = new StringBuilder(1024);
      foreach (LayoutEntityProto layoutEntityProto in lyst)
      {
        stringBuilder.AppendLine(layoutEntityProto.Id.Value);
        stringBuilder.AppendLine(layoutEntityProto.Layout.SourceLayoutStr);
        stringBuilder.AppendLine();
      }
      return GameCommandResult.Success((object) stringBuilder.ToString());
    }

    public void GenerateTemplate(
      MeshBuilder builder,
      EntityLayout layout,
      bool omitPortsAndTransports = false)
    {
      Assert.That<bool>(builder.IsEmpty).IsTrue();
      Vector3 vector3 = layout.TransformF_Fixed(RelTile3f.Zero, TileTransform.Identity).ToVector3();
      if (layout.OriginTile.HasValue)
        vector3 -= layout.TransformF_Fixed(layout.OriginTile.Value.ExtendZ(Fix32.Zero), TileTransform.Identity).ToVector3();
      builder.SetTransform(vector3);
      this.m_layoutEntityModelFactory.AppendLayoutMesh(layout, builder);
      if (omitPortsAndTransports)
        return;
      foreach (IoPortTemplate port in layout.Ports)
      {
        Tile3i tile = port.RelativePosition + Tile3i.Zero;
        GameObject modelFor = this.m_portModelFactory.CreateModelFor(port.Shape, false);
        modelFor.transform.position = tile.ToGroundCenterVector3() + vector3;
        modelFor.transform.Rotate(Vector3.up, port.RelativeDirection.ToRotation().Angle.ToUnityAngleDegrees());
        builder.AddAllMeshes(modelFor, true, true);
        modelFor.DestroyImmediate();
        Assert.That<Dict<IoPortShapeProto, TransportProto>>(this.m_shapeToTransport).ContainsKey<IoPortShapeProto, TransportProto>(port.Shape, "No transport for shape.");
        RelTile2i relTile2i = new RelTile2i(port.RelativeDirection.DirectionVector);
        string error;
        GameObject go = this.m_transportModelFactory.CreateModel(this.m_shapeToTransport[port.Shape], ImmutableArray.Create<Tile3i>(tile + port.RelativeDirection.ToTileDirection().ExtendZ(0)), out error, new RelTile3i?(-relTile2i.ExtendZ(0)), new RelTile3i?(relTile2i.ExtendZ(0)), noColliders: true, singleMesh: true, noFlowIndicators: true).ValueOrThrow(error);
        go.transform.position += vector3;
        builder.AddAllMeshes(go, true, true);
        go.DestroyImmediate();
      }
    }
  }
}
