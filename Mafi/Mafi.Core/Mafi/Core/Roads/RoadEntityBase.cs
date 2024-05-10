// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Roads.RoadEntityBase
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Roads
{
  public abstract class RoadEntityBase : 
    LayoutEntity,
    IRoadGraphEntity,
    ILayoutEntity,
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IEntity,
    IIsSafeAsHashKey,
    IAreaSelectableEntity
  {
    public int RoadLanesCount => this.RoadProto.LanesData.Length;

    public RoadEntityProtoBase RoadProto { get; private set; }

    protected RoadEntityBase(
      EntityId id,
      RoadEntityProtoBase proto,
      TileTransform transform,
      EntityContext context)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (LayoutEntityProto) proto, transform, context);
      this.RoadProto = proto;
    }

    public RoadLaneMetadata GetRawRoadLaneMetadata(int laneIndex)
    {
      return this.RoadProto.LanesData[laneIndex];
    }

    public RoadLaneTrajectory GetTransformedRoadLane(int laneIndex)
    {
      return this.RoadProto.GetTransformedLane(this.Transform, laneIndex);
    }

    public void GetLaneNodes(
      int laneIndex,
      out RoadGraphNodeKey startNodeKey,
      out RoadGraphNodeKey endNodeKey)
    {
      RoadLaneMetadata roadLaneMetadata = this.RoadProto.LanesData[laneIndex];
      startNodeKey = this.RoadProto.GetTransformedRoadGraphNode(roadLaneMetadata.StartPosition, roadLaneMetadata.StartDirection, this.Transform);
      endNodeKey = this.RoadProto.GetTransformedRoadGraphNode(roadLaneMetadata.EndPosition, roadLaneMetadata.EndDirection, this.Transform);
    }

    public override string ToString()
    {
      return string.Format("{0} #{1} {2} {3}", (object) this.GetType().Name, (object) this.Id, (object) this.RoadProto.Id, this.IsDestroyed ? (object) " (destroyed)" : (object) "");
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteGeneric<RoadEntityProtoBase>(this.RoadProto);
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.RoadProto = reader.ReadGenericAs<RoadEntityProtoBase>();
    }
  }
}
