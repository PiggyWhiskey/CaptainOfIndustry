// Decompiled with JetBrains decompiler
// Type: Mafi.Numerics.LineRasterizer
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Numerics
{
  /// <summary>
  /// Helper struct that allows allocation-free line rasterization on integer coordinates.
  /// </summary>
  /// <remarks>
  /// Uses Bresenham's Line Algorithm. Source: http://www.roguebasin.com/index.php?title=Bresenham%27s_Line_Algorithm
  /// </remarks>
  public readonly struct LineRasterizer
  {
    public readonly Vector2i From;
    public readonly Vector2i To;
    public readonly bool SkipFirst;
    private readonly Vector2i m_delta;
    private readonly int m_ystep;
    private readonly bool m_steep;
    private readonly bool m_isForwards;

    /// <summary>
    /// Creates iterator for line points. Use foreach to iterate. Iterated points are always form <paramref name="from" /> to <paramref name="to" />. This method produces the same points if arguments are swapped, just
    /// in opposite order.
    /// </summary>
    public LineRasterizer(Vector2i from, Vector2i to, bool skipFirstPoint)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.From = from;
      this.To = to;
      this.SkipFirst = skipFirstPoint;
      this.m_delta = to - from;
      this.m_steep = this.m_delta.Y.Abs() > this.m_delta.X.Abs();
      if (this.m_steep)
      {
        this.From = from.Yx;
        this.To = to.Yx;
        this.m_delta = this.m_delta.Yx;
      }
      else
      {
        this.From = from;
        this.To = to;
      }
      if (this.m_delta.X >= 0)
      {
        this.m_isForwards = true;
      }
      else
      {
        this.m_isForwards = false;
        this.m_delta = this.m_delta.SetX(-this.m_delta.X);
      }
      if (this.m_delta.Y >= 0)
      {
        this.m_ystep = 1;
      }
      else
      {
        this.m_ystep = -1;
        this.m_delta = this.m_delta.SetY(-this.m_delta.Y);
      }
    }

    public LineRasterizer.Enumerator GetEnumerator() => new LineRasterizer.Enumerator(this);

    public struct Enumerator
    {
      private readonly LineRasterizer m_rasterizer;
      private int m_x;
      private int m_y;
      private int m_err;
      private bool m_isFirstCall;

      internal Enumerator(LineRasterizer rasterizer)
      {
        MBiHIp97M4MqqbtZOh.RFLpSOptx();
        this.m_rasterizer = rasterizer;
        this.m_x = rasterizer.From.X;
        this.m_y = rasterizer.From.Y;
        this.m_err = !rasterizer.m_isForwards ? (rasterizer.m_delta.X / 2 - rasterizer.m_delta.X * rasterizer.m_delta.Y).Modulo(rasterizer.m_delta.X) : rasterizer.m_delta.X / 2;
        this.m_isFirstCall = !rasterizer.SkipFirst;
      }

      public bool MoveNext()
      {
        if (this.m_isFirstCall)
        {
          this.m_isFirstCall = false;
          return true;
        }
        if (this.m_rasterizer.m_isForwards)
        {
          if (this.m_x >= this.m_rasterizer.To.X)
            return false;
          ++this.m_x;
          this.m_err -= this.m_rasterizer.m_delta.Y;
          if (this.m_err < 0)
          {
            this.m_y += this.m_rasterizer.m_ystep;
            this.m_err += this.m_rasterizer.m_delta.X;
          }
        }
        else
        {
          if (this.m_x <= this.m_rasterizer.To.X)
            return false;
          --this.m_x;
          this.m_err += this.m_rasterizer.m_delta.Y;
          if (this.m_err >= this.m_rasterizer.m_delta.X)
          {
            this.m_y += this.m_rasterizer.m_ystep;
            this.m_err -= this.m_rasterizer.m_delta.X;
          }
        }
        return true;
      }

      public Vector2i Current
      {
        get
        {
          return !this.m_rasterizer.m_steep ? new Vector2i(this.m_x, this.m_y) : new Vector2i(this.m_y, this.m_x);
        }
      }
    }
  }
}
