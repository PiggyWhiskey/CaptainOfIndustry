// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PathFinding.ExploredPfNode
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.PathFinding
{
  public readonly struct ExploredPfNode
  {
    public readonly Tile2i Node;
    public readonly Tile2i ParentNode;
    public readonly Fix32 Cost;
    public readonly bool IsProcessed;
    public readonly bool IsVisitedFromStart;

    public ExploredPfNode(
      Tile2i node,
      Tile2i parentNode,
      Fix32 cost,
      bool isProcessed,
      bool isVisitedFromStart)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Node = node;
      this.ParentNode = parentNode;
      this.Cost = cost;
      this.IsProcessed = isProcessed;
      this.IsVisitedFromStart = isVisitedFromStart;
    }

    public override string ToString()
    {
      return string.Format("{0} cost={1}, fromStart={2}", (object) this.Node, (object) this.Cost.ToStringRounded(), (object) this.IsVisitedFromStart);
    }
  }
}
