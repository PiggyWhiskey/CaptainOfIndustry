// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.TerrainGenerationContext
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Products;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  public class TerrainGenerationContext : ITerrainExtraDataRegistrator
  {
    /// <summary>
    /// Size of the entire terrain. Note that the data might be a susbet of this.
    /// </summary>
    public readonly RelTile2i TerrainSize;
    public readonly int InitialMapCreationSaveVersion;
    public readonly Tile2i DataOrigin;
    public readonly Chunk64Area ChunkArea;
    public TerrainManager.TerrainData Data;
    public readonly TerrainMaterialProto BedrockMaterial;
    private readonly Dict<Type, ITerrainGenerationExtraData> m_extraData;
    private readonly IResolver m_resolver;

    public RectangleTerrainArea2i Area
    {
      get => new RectangleTerrainArea2i(this.DataOrigin, this.Data.Size);
    }

    public IReadOnlyDictionary<Type, ITerrainGenerationExtraData> ExtraData
    {
      get => (IReadOnlyDictionary<Type, ITerrainGenerationExtraData>) this.m_extraData;
    }

    public TerrainGenerationContext(
      RelTile2i terrainSize,
      Tile2i dataOrigin,
      TerrainManager.TerrainData data,
      TerrainMaterialProto bedrockMaterial,
      int initialMapCreationSaveVersion,
      IResolver resolver)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_extraData = new Dict<Type, ITerrainGenerationExtraData>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.TerrainSize = terrainSize;
      this.DataOrigin = dataOrigin;
      this.Data = data;
      this.BedrockMaterial = bedrockMaterial;
      this.m_resolver = resolver;
      this.InitialMapCreationSaveVersion = initialMapCreationSaveVersion;
      this.ChunkArea = this.Area.Chunk64Area;
    }

    public int GetTileIndex(Tile2i tile)
    {
      RelTile2i relTile2i = tile - this.DataOrigin;
      return relTile2i.X + relTile2i.Y * this.Data.Width;
    }

    public bool TryGetExtraData<T>(out T data) where T : class, ITerrainGenerationExtraData
    {
      ITerrainGenerationExtraData generationExtraData;
      if (this.m_extraData.TryGetValue(typeof (T), out generationExtraData))
      {
        data = (T) generationExtraData;
        return true;
      }
      data = default (T);
      return false;
    }

    public bool TryGetOrCreateExtraData<T>(out T data) where T : class, ITerrainGenerationExtraData
    {
      ITerrainGenerationExtraData generationExtraData;
      if (this.m_extraData.TryGetValue(typeof (T), out generationExtraData))
      {
        data = (T) generationExtraData;
        return true;
      }
      data = this.m_resolver.Instantiate<T>();
      data.Initialize(this.ChunkArea);
      this.m_extraData.Add(typeof (T), (ITerrainGenerationExtraData) data);
      return true;
    }

    public void SetOrReplaceExtraData<T>(T data) where T : class, ITerrainGenerationExtraData
    {
      this.m_extraData[typeof (T)] = (ITerrainGenerationExtraData) data;
    }

    public void SetBoundaryFlag()
    {
      ushort[] flags = this.Data.Flags;
      int height = this.Data.Height;
      int width = this.Data.Width;
      if (height <= 0 || width <= 0)
      {
        Log.Error(string.Format("Attempting to set boundary flags with non-positive height ({0}) or width ({1})", (object) height, (object) width));
      }
      else
      {
        int num1 = (height - 1) * width;
        for (int index = 0; index < width; ++index)
        {
          flags[index] |= (ushort) 1;
          flags[num1 + index] |= (ushort) 1;
        }
        int num2 = width - 1;
        int num3 = 0;
        int index1 = 0;
        while (num3 < height)
        {
          flags[index1] |= (ushort) 1;
          flags[num2 + index1] |= (ushort) 1;
          ++num3;
          index1 += width;
        }
      }
    }

    /// <summary>
    /// Sets off-limits tile flag of tiles, based on the given actual terrain size (assuming
    /// that the actual terrain origin is at tile [0, 0]).
    /// Note: Terrain data must have the given area allocated.
    /// </summary>
    public void SetOffLimitsFlag(RelTile2i actualTerrainSize, MapOffLimitsSize offLimitsSize)
    {
      Tile2i tile2i1 = new Tile2i(offLimitsSize.MinusX.Value - 1, offLimitsSize.MinusY.Value - 1);
      RelTile2i size = this.Data.Size;
      int x = this.Data.Size.X;
      ushort[] flags = this.Data.Flags;
      if (this.DataOrigin.X <= tile2i1.X)
      {
        int num1 = tile2i1.X - this.DataOrigin.X + 1;
        int num2 = 0;
        int num3 = 0;
        while (num2 < size.Y)
        {
          for (int index = 0; index < num1; ++index)
            flags[num3 + index] |= (ushort) 2;
          ++num2;
          num3 += x;
        }
      }
      if (this.DataOrigin.Y <= tile2i1.Y)
      {
        int num4 = tile2i1.Y - this.DataOrigin.Y + 1;
        int num5 = 0;
        int num6 = 0;
        while (num5 < num4)
        {
          for (int index = 0; index < size.X; ++index)
            flags[num6 + index] |= (ushort) 2;
          ++num5;
          num6 += x;
        }
      }
      Tile2i tile2i2 = this.DataOrigin + this.Data.Size - 1;
      Tile2i tile2i3 = new Tile2i(actualTerrainSize.X - offLimitsSize.PlusX.Value, actualTerrainSize.Y - offLimitsSize.PlusY.Value);
      if (tile2i2.X >= tile2i3.X)
      {
        int num7 = tile2i2.X - tile2i3.X + 1;
        int num8 = 0;
        int num9 = size.X - num7;
        while (num8 < size.Y)
        {
          for (int index = 0; index < num7; ++index)
            flags[num9 + index] |= (ushort) 2;
          ++num8;
          num9 += x;
        }
      }
      if (tile2i2.Y < tile2i3.Y)
        return;
      int num10 = tile2i2.Y - tile2i3.Y + 1;
      int num11 = 0;
      int num12 = (size.Y - num10) * x;
      while (num11 < num10)
      {
        for (int index = 0; index < size.X; ++index)
          flags[num12 + index] |= (ushort) 2;
        ++num11;
        num12 += x;
      }
    }
  }
}
