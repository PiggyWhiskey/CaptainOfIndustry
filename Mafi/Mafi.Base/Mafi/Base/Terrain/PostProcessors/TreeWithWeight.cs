// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Terrain.PostProcessors.TreeWithWeight
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Terrain.Trees;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Base.Terrain.PostProcessors
{
  [GenerateSerializer(false, null, 0)]
  public readonly struct TreeWithWeight
  {
    public readonly TreeProto TreeProto;
    [EditorRange(0.0, 1000.0)]
    public readonly int Weight;

    public static void Serialize(TreeWithWeight value, BlobWriter writer)
    {
      writer.WriteGeneric<TreeProto>(value.TreeProto);
      writer.WriteInt(value.Weight);
    }

    public static TreeWithWeight Deserialize(BlobReader reader)
    {
      return new TreeWithWeight(reader.ReadGenericAs<TreeProto>(), reader.ReadInt());
    }

    public TreeWithWeight(TreeProto treeProto, int weight)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      this.TreeProto = treeProto;
      this.Weight = weight;
    }
  }
}
