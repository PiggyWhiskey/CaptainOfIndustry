// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Terrain.PostProcessors.TerrainChunk64BitMap
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Base.Terrain.PostProcessors
{
  [GenerateSerializer(false, null, 0)]
  public struct TerrainChunk64BitMap
  {
    private BitMap m_bitmap;
    private int m_stride;

    public static void Serialize(TerrainChunk64BitMap value, BlobWriter writer)
    {
      BitMap.Serialize(value.m_bitmap, writer);
      writer.WriteInt(value.m_stride);
    }

    public static TerrainChunk64BitMap Deserialize(BlobReader reader)
    {
      return new TerrainChunk64BitMap(BitMap.Deserialize(reader), reader.ReadInt());
    }

    public readonly ulong[] BackingArray => this.m_bitmap.BackingArray;

    [LoadCtor]
    private TerrainChunk64BitMap(BitMap bitmap, int stride)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      this.m_bitmap = bitmap;
      this.m_stride = stride;
    }

    public readonly bool Contains(Chunk2i chunk)
    {
      return this.m_bitmap.IsSet(chunk.X + chunk.Y * this.m_stride);
    }

    public readonly void Add(Chunk2i chunk)
    {
      this.m_bitmap.SetBit(chunk.X + chunk.Y * this.m_stride);
    }

    public readonly void Remove(Chunk2i chunk)
    {
      this.m_bitmap.ClearBit(chunk.X + chunk.Y * this.m_stride);
    }

    public readonly void Clear() => this.m_bitmap.ClearAllBits();

    public void EnsureCorrectSize(RelTile2i terrainSize)
    {
      int num = terrainSize.X >> 6;
      int size = num * (terrainSize.Y >> 6);
      if (this.m_stride == num && this.m_bitmap.Size == size)
        return;
      this.m_stride = num;
      this.m_bitmap = new BitMap(size);
    }
  }
}
