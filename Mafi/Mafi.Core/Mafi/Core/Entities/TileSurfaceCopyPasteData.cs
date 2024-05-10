// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.TileSurfaceCopyPasteData
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Terrain;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Entities
{
  [GenerateSerializer(false, null, 0)]
  public struct TileSurfaceCopyPasteData
  {
    [ThreadStatic]
    private static Dict<ushort, Lyst<Tile2iSlim>> s_decalPositionsForSerialization;
    [ThreadStatic]
    private static Dict<Tile2iSlim, TileSurfaceData> s_surfaceMapForDeserialization;
    [ThreadStatic]
    private static Lyst<(ushort, Tile2iSlim)> s_decalsTmp;
    [ThreadStatic]
    private static Lyst<(TileSurfaceSlimId, RectangleTerrainArea2i)> s_areasTmp;
    public readonly TileSurfaceData SurfaceData;
    public readonly Tile2i Position;

    public TileSurfaceCopyPasteData(TileSurfaceData surfaceData, Tile2i position)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.SurfaceData = surfaceData;
      this.Position = position;
    }

    [MustUseReturnValue]
    public TileSurfaceCopyPasteData NormalizePosition(RelTile2i offset)
    {
      return new TileSurfaceCopyPasteData(this.SurfaceData, this.Position + offset);
    }

    public static void SerializeAllForBlueprints(
      ImmutableArray<TileSurfaceCopyPasteData> surfaces,
      BlobWriter writer)
    {
      TileSurfaceCopyPasteData.serializeSurfacesNewV2(surfaces, writer);
      TileSurfaceCopyPasteData.serializeDecalsNewV2(surfaces, writer);
    }

    public static ImmutableArray<TileSurfaceCopyPasteData> DeserializeAllForBlueprints(
      BlobReader reader)
    {
      Dict<Tile2iSlim, TileSurfaceData> surfacesTmp = TileSurfaceCopyPasteData.s_surfaceMapForDeserialization;
      if (surfacesTmp == null)
        TileSurfaceCopyPasteData.s_surfaceMapForDeserialization = surfacesTmp = new Dict<Tile2iSlim, TileSurfaceData>();
      surfacesTmp.Clear();
      ImmutableArrayBuilder<TileSurfaceCopyPasteData> immutableArrayBuilder = new ImmutableArrayBuilder<TileSurfaceCopyPasteData>(TileSurfaceCopyPasteData.deserializeSurfacesNewV2(reader, surfacesTmp) + TileSurfaceCopyPasteData.deserializeDecalsNewV2(reader, surfacesTmp));
      int i = 0;
      foreach (KeyValuePair<Tile2iSlim, TileSurfaceData> keyValuePair in surfacesTmp)
      {
        immutableArrayBuilder[i] = new TileSurfaceCopyPasteData(keyValuePair.Value, (Tile2i) keyValuePair.Key);
        ++i;
      }
      return immutableArrayBuilder.GetImmutableArrayAndClear();
    }

    [MustUseReturnValue]
    public TileSurfaceCopyPasteData ApplyTransformDelta(
      RelTile3i deltaPosition,
      Rotation90 deltaRotation,
      bool deltaReflection,
      Tile2i pivot)
    {
      TileTransform transform = new TileTransform(this.Position.ExtendHeight(HeightTilesI.Zero), this.SurfaceData.DecalRotation.ToRotation(), this.SurfaceData.IsDecalFlipped);
      Rotation90 rotation90 = transform.Rotation + deltaRotation;
      bool flag = transform.IsReflected ^ deltaReflection;
      if (deltaReflection && rotation90.Is90Or270Deg)
        rotation90 += Rotation90.Deg180;
      RelTile2i relTile2i = transformRelative(RelTile2i.Zero, transform);
      RelTile2i v = transform.Position.Xy + relTile2i - pivot;
      Matrix2i m = Matrix2i.FromRotationFlip(deltaRotation, deltaReflection);
      Tile3i tile3i = (pivot + m.Transform(v) - transformRelative(RelTile2i.Zero, new TileTransform(Tile3i.Zero, rotation90, flag))).ExtendZ(transform.Position.Z);
      return new TileSurfaceCopyPasteData(new TileSurfaceData(this.SurfaceData.RawValue, this.SurfaceData.DecalSlimId, flag, rotation90, this.SurfaceData.ColorKey), tile3i.Xy + deltaPosition.Xy);

      static RelTile2i transformRelative(RelTile2i relTile, TileTransform transform)
      {
        RelTile2f relTile2f = relTile.RelTile2f;
        return new RelTile2f(transform.TransformMatrix.Transform(relTile2f.Vector2f)).RelTile2iFloored;
      }
    }

    private static void serializeSurfacesNew(
      ImmutableArray<TileSurfaceCopyPasteData> surfaces,
      BlobWriter writer)
    {
      Tile2i tile2i1 = Tile2i.MaxValue;
      Tile2i tile2i2 = Tile2i.MinValue;
      int num1 = 0;
      foreach (TileSurfaceCopyPasteData surface in surfaces)
      {
        if (!surface.SurfaceData.IsAutoPlaced)
        {
          tile2i1 = tile2i1.Min(surface.Position);
          tile2i2 = tile2i2.Max(surface.Position);
          ++num1;
        }
      }
      writer.WriteIntNotNegative(num1);
      if (num1 == 0)
        return;
      RelTile2i size = tile2i2 - tile2i1 + RelTile2i.One;
      int stride = size.X;
      bool flag = size.X < (int) byte.MaxValue && size.Y < (int) byte.MaxValue;
      writer.WriteBool(flag);
      writer.WriteUShortVariable((ushort) tile2i1.X);
      writer.WriteUShortVariable((ushort) tile2i1.Y);
      TileSurfaceSlimId[] remainingIdsRectangle = new TileSurfaceSlimId[size.ProductInt];
      foreach (TileSurfaceCopyPasteData surface in surfaces)
      {
        if (!surface.SurfaceData.IsAutoPlaced)
        {
          RelTile2i relTile2i = surface.Position - tile2i1;
          remainingIdsRectangle[relTile2i.X + relTile2i.Y * stride] = surface.SurfaceData.SurfaceSlimId;
        }
      }
      Lyst<(TileSurfaceSlimId, RectangleTerrainArea2i)> source = TileSurfaceCopyPasteData.s_areasTmp;
      if (source == null)
        TileSurfaceCopyPasteData.s_areasTmp = source = new Lyst<(TileSurfaceSlimId, RectangleTerrainArea2i)>(true);
      source.Clear();
      int num2 = 0;
      int num3 = 0;
      while (num2 < size.Y)
      {
        for (int index1 = 0; index1 < size.X; ++index1)
        {
          int index2 = num3 + index1;
          TileSurfaceSlimId tileSurfaceSlimId = remainingIdsRectangle[index2];
          if (tileSurfaceSlimId.Value != (byte) 0)
          {
            RelTile2i largestRect = findLargestRect(index1, num2);
            source.Add((tileSurfaceSlimId, new RectangleTerrainArea2i(new Tile2i(index1, num2), largestRect)));
            int num4 = 0;
            while (num4 < largestRect.Y)
            {
              for (int index3 = 0; index3 < largestRect.X; ++index3)
                remainingIdsRectangle[index2 + index3] = new TileSurfaceSlimId();
              ++num4;
              index2 += stride;
            }
          }
        }
        ++num2;
        num3 += stride;
      }
      Lyst<IGrouping<TileSurfaceSlimId, (TileSurfaceSlimId, RectangleTerrainArea2i)>> lyst1 = source.GroupBy<(TileSurfaceSlimId, RectangleTerrainArea2i), TileSurfaceSlimId>((Func<(TileSurfaceSlimId, RectangleTerrainArea2i), TileSurfaceSlimId>) (x => x.Item1)).ToLyst<IGrouping<TileSurfaceSlimId, (TileSurfaceSlimId, RectangleTerrainArea2i)>>();
      writer.WriteIntNotNegative(lyst1.Count);
      foreach (IGrouping<TileSurfaceSlimId, (TileSurfaceSlimId, RectangleTerrainArea2i)> enumerable in lyst1)
      {
        writer.WriteByte(enumerable.Key.Value);
        Lyst<(TileSurfaceSlimId, RectangleTerrainArea2i)> lyst2 = enumerable.ToLyst<(TileSurfaceSlimId, RectangleTerrainArea2i)>();
        writer.WriteIntNotNegative(lyst2.Count);
        foreach ((TileSurfaceSlimId _, RectangleTerrainArea2i rectangleTerrainArea2i) in lyst2)
        {
          if (flag)
          {
            writer.WriteByte((byte) rectangleTerrainArea2i.Origin.X);
            writer.WriteByte((byte) rectangleTerrainArea2i.Origin.Y);
          }
          else
          {
            writer.WriteUShortVariable((ushort) rectangleTerrainArea2i.Origin.X);
            writer.WriteUShortVariable((ushort) rectangleTerrainArea2i.Origin.Y);
          }
          writer.WriteByte((byte) rectangleTerrainArea2i.Size.X);
          writer.WriteByte((byte) rectangleTerrainArea2i.Size.Y);
        }
      }

      RelTile2i findLargestRect(int originX, int originY)
      {
        TileSurfaceSlimId tileSurfaceSlimId = remainingIdsRectangle[originX + originY * stride];
        int x = 1;
        int y = 1;
        while (true)
        {
          int num1;
          int num2;
          bool flag1;
          do
          {
            num1 = originX + x;
            bool flag2;
            if (num1 < (int) byte.MaxValue && num1 < size.X)
            {
              flag2 = true;
              int index = num1 + originY * stride;
              int num3 = 0;
              while (num3 < y)
              {
                flag2 &= remainingIdsRectangle[index] == tileSurfaceSlimId;
                ++num3;
                index += stride;
              }
            }
            else
              flag2 = false;
            int num4 = originY + y;
            num2 = num4 * stride;
            if (num4 < (int) byte.MaxValue && num4 < size.Y)
            {
              flag1 = true;
              int num5 = originX + num2;
              for (int index = 0; index < x; ++index)
                flag1 &= remainingIdsRectangle[num5 + index] == tileSurfaceSlimId;
            }
            else
              flag1 = false;
            if (flag2)
              ++x;
            else
              goto label_14;
          }
          while (!flag1 || !(remainingIdsRectangle[num1 + num2] == tileSurfaceSlimId));
          ++y;
          continue;
label_14:
          if (flag1)
            ++y;
          else
            break;
        }
        return new RelTile2i(x, y);
      }
    }

    private static int deserializeSurfacesNew(
      BlobReader reader,
      Dict<Tile2iSlim, TileSurfaceData> surfacesTmp)
    {
      int other = reader.ReadIntNotNegative();
      if (other == 0)
        return 0;
      bool flag = reader.ReadBool();
      Tile2i tile2i1 = new Tile2i((int) reader.ReadUShortVariable(), (int) reader.ReadUShortVariable());
      int num1 = reader.ReadIntNotNegative();
      int num2 = 0;
      for (int index1 = 0; index1 < num1; ++index1)
      {
        byte rawValue = reader.ReadByte();
        int num3 = reader.ReadIntNotNegative();
        for (int index2 = 0; index2 < num3; ++index2)
        {
          Tile2i tile2i2 = tile2i1 + (flag ? new RelTile2i((int) reader.ReadByte(), (int) reader.ReadByte()) : new RelTile2i((int) reader.ReadUShortVariable(), (int) reader.ReadUShortVariable()));
          int num4 = (int) reader.ReadByte();
          int num5 = (int) reader.ReadByte();
          num2 += num4 * num5;
          for (int y = 0; y < num5; ++y)
          {
            for (int x = 0; x < num4; ++x)
              surfacesTmp.Add((tile2i2 + new RelTile2i(x, y)).AsSlim, new TileSurfaceData((int) rawValue, 0));
          }
        }
      }
      Assert.That<int>(num2).IsEqualTo(other);
      return num2;
    }

    private static void serializeSurfacesNewV2(
      ImmutableArray<TileSurfaceCopyPasteData> surfaces,
      BlobWriter writer)
    {
      Tile2i tile2i1 = Tile2i.MaxValue;
      Tile2i tile2i2 = Tile2i.MinValue;
      int num1 = 0;
      foreach (TileSurfaceCopyPasteData surface in surfaces)
      {
        if (!surface.SurfaceData.IsAutoPlaced)
        {
          tile2i1 = tile2i1.Min(surface.Position);
          tile2i2 = tile2i2.Max(surface.Position);
          ++num1;
        }
      }
      writer.WriteIntNotNegative(num1);
      if (num1 == 0)
        return;
      RelTile2i size = tile2i2 - tile2i1 + RelTile2i.One;
      int stride = size.X;
      bool flag1 = size.X <= (int) byte.MaxValue;
      bool flag2 = size.Y <= (int) byte.MaxValue;
      writer.WriteBool(flag1);
      writer.WriteBool(flag2);
      writer.WriteUShortVariable((ushort) tile2i1.X);
      writer.WriteUShortVariable((ushort) tile2i1.Y);
      TileSurfaceSlimId[] remainingIdsRectangle = new TileSurfaceSlimId[size.ProductInt];
      foreach (TileSurfaceCopyPasteData surface in surfaces)
      {
        if (!surface.SurfaceData.IsAutoPlaced)
        {
          RelTile2i relTile2i = surface.Position - tile2i1;
          remainingIdsRectangle[relTile2i.X + relTile2i.Y * stride] = surface.SurfaceData.SurfaceSlimId;
        }
      }
      Lyst<(TileSurfaceSlimId, RectangleTerrainArea2i)> source = TileSurfaceCopyPasteData.s_areasTmp;
      if (source == null)
        TileSurfaceCopyPasteData.s_areasTmp = source = new Lyst<(TileSurfaceSlimId, RectangleTerrainArea2i)>(true);
      source.Clear();
      int num2 = 0;
      int num3 = 0;
      while (num2 < size.Y)
      {
        for (int index1 = 0; index1 < size.X; ++index1)
        {
          int index2 = num3 + index1;
          TileSurfaceSlimId tileSurfaceSlimId = remainingIdsRectangle[index2];
          if (tileSurfaceSlimId.Value != (byte) 0)
          {
            RelTile2i largestRect = findLargestRect(index1, num2);
            source.Add((tileSurfaceSlimId, new RectangleTerrainArea2i(new Tile2i(index1, num2), largestRect)));
            int num4 = 0;
            while (num4 < largestRect.Y)
            {
              for (int index3 = 0; index3 < largestRect.X; ++index3)
                remainingIdsRectangle[index2 + index3] = new TileSurfaceSlimId();
              ++num4;
              index2 += stride;
            }
          }
        }
        ++num2;
        num3 += stride;
      }
      Lyst<IGrouping<TileSurfaceSlimId, (TileSurfaceSlimId, RectangleTerrainArea2i)>> lyst1 = source.GroupBy<(TileSurfaceSlimId, RectangleTerrainArea2i), TileSurfaceSlimId>((Func<(TileSurfaceSlimId, RectangleTerrainArea2i), TileSurfaceSlimId>) (x => x.Item1)).ToLyst<IGrouping<TileSurfaceSlimId, (TileSurfaceSlimId, RectangleTerrainArea2i)>>();
      writer.WriteIntNotNegative(lyst1.Count);
      foreach (IGrouping<TileSurfaceSlimId, (TileSurfaceSlimId, RectangleTerrainArea2i)> enumerable in lyst1)
      {
        writer.WriteByte(enumerable.Key.Value);
        Lyst<(TileSurfaceSlimId, RectangleTerrainArea2i)> lyst2 = enumerable.ToLyst<(TileSurfaceSlimId, RectangleTerrainArea2i)>();
        writer.WriteIntNotNegative(lyst2.Count);
        foreach ((TileSurfaceSlimId _, RectangleTerrainArea2i rectangleTerrainArea2i) in lyst2)
        {
          if (flag1)
            writer.WriteByte((byte) rectangleTerrainArea2i.Origin.X);
          else
            writer.WriteUShortVariable((ushort) rectangleTerrainArea2i.Origin.X);
          if (flag2)
            writer.WriteByte((byte) rectangleTerrainArea2i.Origin.Y);
          else
            writer.WriteUShortVariable((ushort) rectangleTerrainArea2i.Origin.Y);
          writer.WriteByte((byte) rectangleTerrainArea2i.Size.X);
          writer.WriteByte((byte) rectangleTerrainArea2i.Size.Y);
        }
      }

      RelTile2i findLargestRect(int originX, int originY)
      {
        TileSurfaceSlimId tileSurfaceSlimId = remainingIdsRectangle[originX + originY * stride];
        int x = 1;
        int y = 1;
        while (true)
        {
          int num1;
          int num2;
          bool flag1;
          do
          {
            num1 = originX + x;
            bool flag2;
            if (x < (int) byte.MaxValue && num1 < size.X)
            {
              flag2 = true;
              int index = num1 + originY * stride;
              int num3 = 0;
              while (num3 < y)
              {
                flag2 &= remainingIdsRectangle[index] == tileSurfaceSlimId;
                ++num3;
                index += stride;
              }
            }
            else
              flag2 = false;
            int num4 = originY + y;
            num2 = num4 * stride;
            if (y < (int) byte.MaxValue && num4 < size.Y)
            {
              flag1 = true;
              int num5 = originX + num2;
              for (int index = 0; index < x; ++index)
                flag1 &= remainingIdsRectangle[num5 + index] == tileSurfaceSlimId;
            }
            else
              flag1 = false;
            if (flag2)
              ++x;
            else
              goto label_14;
          }
          while (!flag1 || !(remainingIdsRectangle[num1 + num2] == tileSurfaceSlimId));
          ++y;
          continue;
label_14:
          if (flag1)
            ++y;
          else
            break;
        }
        return new RelTile2i(x, y);
      }
    }

    private static int deserializeSurfacesNewV2(
      BlobReader reader,
      Dict<Tile2iSlim, TileSurfaceData> surfacesTmp)
    {
      int other = reader.ReadIntNotNegative();
      if (other == 0)
        return 0;
      bool flag1 = reader.ReadBool();
      bool flag2 = reader.ReadBool();
      Tile2i tile2i1 = new Tile2i((int) reader.ReadUShortVariable(), (int) reader.ReadUShortVariable());
      int num1 = reader.ReadIntNotNegative();
      int num2 = 0;
      for (int index1 = 0; index1 < num1; ++index1)
      {
        byte rawValue = reader.ReadByte();
        int num3 = reader.ReadIntNotNegative();
        for (int index2 = 0; index2 < num3; ++index2)
        {
          int x1 = flag1 ? (int) reader.ReadByte() : (int) reader.ReadUShortVariable();
          int y1 = flag2 ? (int) reader.ReadByte() : (int) reader.ReadUShortVariable();
          Tile2i tile2i2 = tile2i1 + new RelTile2i(x1, y1);
          int num4 = (int) reader.ReadByte();
          int num5 = (int) reader.ReadByte();
          num2 += num4 * num5;
          for (int y2 = 0; y2 < num5; ++y2)
          {
            for (int x2 = 0; x2 < num4; ++x2)
              surfacesTmp.Add((tile2i2 + new RelTile2i(x2, y2)).AsSlim, new TileSurfaceData((int) rawValue, 0));
          }
        }
      }
      Assert.That<int>(num2).IsEqualTo(other);
      return num2;
    }

    private static void serializeDecalsNewV2(
      ImmutableArray<TileSurfaceCopyPasteData> surfaces,
      BlobWriter writer)
    {
      Lyst<(ushort, Tile2iSlim)> source = TileSurfaceCopyPasteData.s_decalsTmp;
      if (source == null)
        TileSurfaceCopyPasteData.s_decalsTmp = source = new Lyst<(ushort, Tile2iSlim)>(true);
      source.Clear();
      Set<ushort> set1 = new Set<ushort>();
      Set<ushort> set2 = new Set<ushort>();
      Tile2i tile2i1 = Tile2i.MaxValue;
      Tile2i tile2i2 = Tile2i.MinValue;
      foreach (TileSurfaceCopyPasteData surface in surfaces)
      {
        ushort num1 = (ushort) surface.SurfaceData.DecalSlimId.Value;
        if (num1 != (ushort) 0)
        {
          ushort num2 = (ushort) ((uint) (ushort) ((uint) (ushort) ((uint) num1 | (uint) (ushort) ((uint) surface.SurfaceData.IsDecalFlipped << 8)) | (uint) (ushort) (surface.SurfaceData.DecalRotation.DirectionIndex << 9)) | (uint) (ushort) (surface.SurfaceData.ColorKey << 11));
          source.Add((num2, surface.Position.AsSlim));
          tile2i1 = tile2i1.Min(surface.Position);
          tile2i2 = tile2i2.Max(surface.Position);
          set1.Add(surface.Position.AsSlim.X);
          set2.Add(surface.Position.AsSlim.Y);
        }
      }
      writer.WriteIntNotNegative(source.Count);
      if (source.Count == 0)
        return;
      writer.WriteUShortVariable((ushort) tile2i1.X);
      writer.WriteUShortVariable((ushort) tile2i1.Y);
      bool flag = set1.Count < set2.Count;
      writer.WriteBool(flag);
      bool useByteCoordsX = tile2i2.X - tile2i1.X < (int) byte.MaxValue;
      bool useByteCoordsY = tile2i2.Y - tile2i1.Y < (int) byte.MaxValue;
      writer.WriteBool(useByteCoordsX);
      writer.WriteBool(useByteCoordsY);
      if (flag)
      {
        writeX(set1.Count);
        foreach (IGrouping<ushort, (ushort, Tile2iSlim)> enumerable in source.GroupBy<(ushort, Tile2iSlim), ushort>((Func<(ushort, Tile2iSlim), ushort>) (x => x.Item2.X)))
        {
          writeX((int) enumerable.Key - tile2i1.X);
          Lyst<(ushort, Tile2iSlim)> lyst = enumerable.ToLyst<(ushort, Tile2iSlim)>();
          writeY(lyst.Count);
          foreach ((ushort, Tile2iSlim) tuple in lyst)
          {
            ushort num = tuple.Item1;
            writeY((int) tuple.Item2.Y - tile2i1.Y);
            writer.WriteUShort(num);
          }
        }
      }
      else
      {
        writeY(set2.Count);
        foreach (IGrouping<ushort, (ushort, Tile2iSlim)> enumerable in source.GroupBy<(ushort, Tile2iSlim), ushort>((Func<(ushort, Tile2iSlim), ushort>) (x => x.Item2.Y)))
        {
          writeY((int) enumerable.Key - tile2i1.Y);
          Lyst<(ushort, Tile2iSlim)> lyst = enumerable.ToLyst<(ushort, Tile2iSlim)>();
          writeX(lyst.Count);
          foreach ((ushort, Tile2iSlim) tuple in lyst)
          {
            ushort num = tuple.Item1;
            writeX((int) tuple.Item2.X - tile2i1.X);
            writer.WriteUShort(num);
          }
        }
      }

      void writeX(int val)
      {
        if (useByteCoordsX)
          writer.WriteByte((byte) val);
        else
          writer.WriteUShortVariable((ushort) val);
      }

      void writeY(int val)
      {
        if (useByteCoordsY)
          writer.WriteByte((byte) val);
        else
          writer.WriteUShortVariable((ushort) val);
      }
    }

    private static int deserializeDecalsNewV2(
      BlobReader reader,
      Dict<Tile2iSlim, TileSurfaceData> surfacesTmp)
    {
      int other = reader.ReadIntNotNegative();
      if (other <= 0)
        return 0;
      int newItemsCount = 0;
      int deserializedCount = 0;
      Tile2i tile2i = new Tile2i((int) reader.ReadUShortVariable(), (int) reader.ReadUShortVariable());
      bool flag = reader.ReadBool();
      bool useByteCoordsX = reader.ReadBool();
      bool useByteCoordsY = reader.ReadBool();
      if (flag)
      {
        int num1 = readX();
        for (int index1 = 0; index1 < num1; ++index1)
        {
          int x = tile2i.X + readX();
          int num2 = readY();
          for (int index2 = 0; index2 < num2; ++index2)
          {
            int y = tile2i.Y + readY();
            setDecal(reader.ReadUShort(), new Tile2i(x, y).AsSlim);
          }
        }
      }
      else
      {
        int num3 = readY();
        for (int index3 = 0; index3 < num3; ++index3)
        {
          int y = tile2i.Y + readY();
          int num4 = readX();
          for (int index4 = 0; index4 < num4; ++index4)
          {
            int x = tile2i.X + readX();
            setDecal(reader.ReadUShort(), new Tile2i(x, y).AsSlim);
          }
        }
      }
      Assert.That<int>(deserializedCount).IsEqualTo(other);
      return newItemsCount;

      void setDecal(ushort decalRawData, Tile2iSlim position)
      {
        TileSurfaceData tileSurfaceData;
        if (!surfacesTmp.TryGetValue(position, out tileSurfaceData))
        {
          tileSurfaceData = new TileSurfaceData(0, 0);
          ++newItemsCount;
        }
        surfacesTmp[position] = new TileSurfaceData(tileSurfaceData.RawValue, (int) decalRawData);
        ++deserializedCount;
      }

      int readX() => !useByteCoordsX ? (int) reader.ReadUShortVariable() : (int) reader.ReadByte();

      int readY() => !useByteCoordsY ? (int) reader.ReadUShortVariable() : (int) reader.ReadByte();
    }

    private static void serializeDecalsNew(
      ImmutableArray<TileSurfaceCopyPasteData> surfaces,
      BlobWriter writer)
    {
      Lyst<(ushort, Tile2iSlim)> source = TileSurfaceCopyPasteData.s_decalsTmp;
      if (source == null)
        TileSurfaceCopyPasteData.s_decalsTmp = source = new Lyst<(ushort, Tile2iSlim)>(true);
      source.Clear();
      Set<ushort> set1 = new Set<ushort>();
      Set<ushort> set2 = new Set<ushort>();
      Tile2i tile2i1 = Tile2i.MaxValue;
      Tile2i tile2i2 = Tile2i.MinValue;
      foreach (TileSurfaceCopyPasteData surface in surfaces)
      {
        ushort num1 = (ushort) surface.SurfaceData.DecalSlimId.Value;
        if (num1 != (ushort) 0)
        {
          ushort num2 = (ushort) ((uint) (ushort) ((uint) (ushort) ((uint) num1 | (uint) (ushort) ((uint) surface.SurfaceData.IsDecalFlipped << 8)) | (uint) (ushort) (surface.SurfaceData.DecalRotation.DirectionIndex << 9)) | (uint) (ushort) (surface.SurfaceData.ColorKey << 11));
          source.Add((num2, surface.Position.AsSlim));
          tile2i1 = tile2i1.Min(surface.Position);
          tile2i2 = tile2i2.Max(surface.Position);
          set1.Add(surface.Position.AsSlim.X);
          set2.Add(surface.Position.AsSlim.Y);
        }
      }
      writer.WriteIntNotNegative(source.Count);
      if (source.Count == 0)
        return;
      writer.WriteUShortVariable((ushort) tile2i1.X);
      writer.WriteUShortVariable((ushort) tile2i1.Y);
      bool flag = set1.Count < set2.Count;
      writer.WriteBool(flag);
      if (flag)
      {
        writer.WriteIntNotNegative(set1.Count);
        foreach (IGrouping<ushort, (ushort, Tile2iSlim)> enumerable in source.GroupBy<(ushort, Tile2iSlim), ushort>((Func<(ushort, Tile2iSlim), ushort>) (x => x.Item2.X)))
        {
          writer.WriteUShortVariable((ushort) ((uint) enumerable.Key - (uint) tile2i1.X));
          Lyst<(ushort, Tile2iSlim)> lyst = enumerable.ToLyst<(ushort, Tile2iSlim)>();
          writer.WriteIntNotNegative(lyst.Count);
          foreach ((ushort num, Tile2iSlim tile2iSlim) in lyst)
          {
            writer.WriteUShortVariable((ushort) ((uint) tile2iSlim.Y - (uint) tile2i1.Y));
            writer.WriteUShort(num);
          }
        }
      }
      else
      {
        writer.WriteIntNotNegative(set2.Count);
        foreach (IGrouping<ushort, (ushort, Tile2iSlim)> enumerable in source.GroupBy<(ushort, Tile2iSlim), ushort>((Func<(ushort, Tile2iSlim), ushort>) (x => x.Item2.Y)))
        {
          writer.WriteUShortVariable((ushort) ((uint) enumerable.Key - (uint) tile2i1.Y));
          Lyst<(ushort, Tile2iSlim)> lyst = enumerable.ToLyst<(ushort, Tile2iSlim)>();
          writer.WriteIntNotNegative(lyst.Count);
          foreach ((ushort num, Tile2iSlim tile2iSlim) in lyst)
          {
            writer.WriteUShortVariable((ushort) ((uint) tile2iSlim.X - (uint) tile2i1.X));
            writer.WriteUShort(num);
          }
        }
      }
    }

    private static int deserializeDecalsNew(
      BlobReader reader,
      Dict<Tile2iSlim, TileSurfaceData> surfacesTmp)
    {
      int other = reader.ReadIntNotNegative();
      if (other <= 0)
        return 0;
      int newItemsCount = 0;
      int deserializedCount = 0;
      Tile2i tile2i = new Tile2i((int) reader.ReadUShortVariable(), (int) reader.ReadUShortVariable());
      if (reader.ReadBool())
      {
        int num1 = reader.ReadIntNotNegative();
        for (int index1 = 0; index1 < num1; ++index1)
        {
          int x = tile2i.X + (int) reader.ReadUShortVariable();
          int num2 = reader.ReadIntNotNegative();
          for (int index2 = 0; index2 < num2; ++index2)
          {
            int y = tile2i.Y + (int) reader.ReadUShortVariable();
            setDecal(reader.ReadUShort(), new Tile2i(x, y).AsSlim);
          }
        }
      }
      else
      {
        int num3 = reader.ReadIntNotNegative();
        for (int index3 = 0; index3 < num3; ++index3)
        {
          int y = tile2i.Y + (int) reader.ReadUShortVariable();
          int num4 = reader.ReadIntNotNegative();
          for (int index4 = 0; index4 < num4; ++index4)
          {
            int x = tile2i.X + (int) reader.ReadUShortVariable();
            setDecal(reader.ReadUShort(), new Tile2i(x, y).AsSlim);
          }
        }
      }
      Assert.That<int>(deserializedCount).IsEqualTo(other);
      return newItemsCount;

      void setDecal(ushort decalRawData, Tile2iSlim position)
      {
        TileSurfaceData tileSurfaceData;
        if (!surfacesTmp.TryGetValue(position, out tileSurfaceData))
        {
          tileSurfaceData = new TileSurfaceData(0, 0);
          ++newItemsCount;
        }
        surfacesTmp[position] = new TileSurfaceData(tileSurfaceData.RawValue, (int) decalRawData);
        ++deserializedCount;
      }
    }

    public static void Serialize(TileSurfaceCopyPasteData value, BlobWriter writer)
    {
      TileSurfaceData.Serialize(value.SurfaceData, writer);
      Tile2i.Serialize(value.Position, writer);
    }

    public static TileSurfaceCopyPasteData Deserialize(BlobReader reader)
    {
      return new TileSurfaceCopyPasteData(TileSurfaceData.Deserialize(reader), Tile2i.Deserialize(reader));
    }
  }
}
