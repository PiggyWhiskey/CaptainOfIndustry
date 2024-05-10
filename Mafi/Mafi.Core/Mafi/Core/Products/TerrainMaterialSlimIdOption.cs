// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Products.TerrainMaterialSlimIdOption
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Products
{
  /// <summary>
  /// Wrapped <see cref="T:Mafi.Core.Products.TerrainMaterialSlimId" /> that uses <see cref="P:Mafi.Core.Products.TerrainMaterialSlimId.PhantomId" /> as None.
  /// Use this instead of nullable type <c>TerrainMaterialSlimId?</c>.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public readonly struct TerrainMaterialSlimIdOption : IEquatable<TerrainMaterialSlimIdOption>
  {
    private readonly TerrainMaterialSlimId m_value;

    public static TerrainMaterialSlimIdOption None => new TerrainMaterialSlimIdOption();

    public static TerrainMaterialSlimIdOption Phantom => new TerrainMaterialSlimIdOption();

    public TerrainMaterialSlimId Value => this.m_value;

    public bool HasValue => this.m_value.IsNotPhantom;

    public bool IsNone => this.m_value.IsPhantom;

    public TerrainMaterialSlimIdOption(TerrainMaterialSlimId value)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_value = value;
    }

    public override string ToString()
    {
      return !this.m_value.IsPhantom ? this.m_value.ToString() : "None(phantom)";
    }

    public static implicit operator TerrainMaterialSlimIdOption(TerrainMaterialSlimId value)
    {
      return new TerrainMaterialSlimIdOption(value);
    }

    public static bool operator ==(
      TerrainMaterialSlimIdOption left,
      TerrainMaterialSlimIdOption right)
    {
      return left.m_value == right.m_value;
    }

    public static bool operator !=(
      TerrainMaterialSlimIdOption left,
      TerrainMaterialSlimIdOption right)
    {
      return left.m_value != right.m_value;
    }

    public bool Equals(TerrainMaterialSlimIdOption other) => this.m_value == other.m_value;

    public override bool Equals(object obj)
    {
      return obj is TerrainMaterialSlimIdOption other && this.Equals(other);
    }

    public override int GetHashCode() => this.m_value.GetHashCode();

    public static void Serialize(TerrainMaterialSlimIdOption value, BlobWriter writer)
    {
      TerrainMaterialSlimId.Serialize(value.m_value, writer);
    }

    public static TerrainMaterialSlimIdOption Deserialize(BlobReader reader)
    {
      return new TerrainMaterialSlimIdOption(TerrainMaterialSlimId.Deserialize(reader));
    }
  }
}
