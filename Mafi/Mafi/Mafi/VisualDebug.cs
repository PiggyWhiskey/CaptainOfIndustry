// Decompiled with JetBrains decompiler
// Type: Mafi.VisualDebug
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Numerics;
using System;
using System.Linq;

#nullable disable
namespace Mafi
{
  /// <summary>
  /// Class that allows to debug visual data. This class just logs all debug event. Graphics backend has to query the
  /// events and show them.
  /// </summary>
  public static class VisualDebug
  {
    private static readonly Dict<string, VisualDebug.Data> s_data;
    private static readonly VisualDebug.Data m_emptyData;

    public static bool IsEmpty { get; private set; }

    public static string[] Categories { get; private set; }

    public static VisualDebug.IData GetData(string key)
    {
      lock (VisualDebug.s_data)
      {
        VisualDebug.Data data;
        if (VisualDebug.s_data.TryGetValue(key, out data))
          return (VisualDebug.IData) data;
      }
      return (VisualDebug.IData) VisualDebug.m_emptyData;
    }

    public static VisualDebug.Data Log(string key)
    {
      VisualDebug.Data data;
      lock (VisualDebug.s_data)
      {
        if (!VisualDebug.s_data.TryGetValue(key, out data))
        {
          data = new VisualDebug.Data();
          VisualDebug.s_data[key] = data;
          VisualDebug.Categories = VisualDebug.s_data.Keys.OrderBy<string, string>((Func<string, string>) (x => x)).ToArray<string>();
        }
      }
      data.SyncData();
      return data;
    }

    static VisualDebug()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      VisualDebug.s_data = new Dict<string, VisualDebug.Data>();
      VisualDebug.m_emptyData = new VisualDebug.Data();
    }

    public interface IData
    {
      Vector3f[] Tiles { get; }

      Vector4f[] Spheres { get; }

      Line3f[] Lines { get; }
    }

    public class Data : VisualDebug.IData
    {
      private Vector3f[] m_cachedTiles;
      private readonly Lyst<Vector3f> m_tiles;
      private Vector4f[] m_cachedSpheres;
      private readonly Lyst<Vector4f> m_spheres;
      private Line3f[] m_cachedLines;
      private readonly Lyst<Line3f> m_lines;

      Vector3f[] VisualDebug.IData.Tiles => this.m_cachedTiles;

      Vector4f[] VisualDebug.IData.Spheres => this.m_cachedSpheres;

      Line3f[] VisualDebug.IData.Lines => this.m_cachedLines;

      public void Tile(Vector3f pt)
      {
        lock (this.m_tiles)
          this.m_tiles.Add(pt);
      }

      public void Sphere(Vector3f centerTiles, Fix32 radiusTiles)
      {
        lock (this.m_spheres)
          this.m_spheres.Add(centerTiles.ExtendW(radiusTiles));
      }

      public void Line(Vector3f fromTile, Vector3f toTile)
      {
        lock (this.m_lines)
          this.m_lines.Add(new Line3f(fromTile, toTile));
      }

      internal void SyncData()
      {
        if (this.m_cachedTiles.Length != this.m_tiles.Count)
        {
          lock (this.m_tiles)
            this.m_cachedTiles = this.m_tiles.ToArray();
        }
        if (this.m_cachedSpheres.Length != this.m_spheres.Count)
        {
          lock (this.m_spheres)
            this.m_cachedSpheres = this.m_spheres.ToArray();
        }
        if (this.m_cachedLines.Length == this.m_lines.Count)
          return;
        lock (this.m_lines)
          this.m_cachedLines = this.m_lines.ToArray();
      }

      public Data()
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        this.m_cachedTiles = Array.Empty<Vector3f>();
        this.m_tiles = new Lyst<Vector3f>();
        this.m_cachedSpheres = Array.Empty<Vector4f>();
        this.m_spheres = new Lyst<Vector4f>();
        this.m_cachedLines = Array.Empty<Line3f>();
        this.m_lines = new Lyst<Line3f>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }
  }
}
