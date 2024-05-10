// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PathFinding.IVehiclePathSegmentExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.PathFinding
{
  public static class IVehiclePathSegmentExtensions
  {
    public static bool IsValidWithoutSelfLoops(this IVehiclePathSegment segment)
    {
      int num = 1;
      Option<IVehiclePathSegment> nextSegment = segment.NextSegment;
      while (nextSegment.HasValue)
      {
        nextSegment = nextSegment.Value.NextSegment;
        ++num;
        if (num > 10000)
          return false;
      }
      return true;
    }

    public static IVehiclePathSegment FindLastSegment(this IVehiclePathSegment segment)
    {
      int num = 1;
      IVehiclePathSegment lastSegment = segment;
      while (lastSegment.NextSegment.HasValue)
      {
        lastSegment = lastSegment.NextSegment.Value;
        ++num;
        if (num > 10000)
        {
          Log.Error("Path segments form a loop?");
          break;
        }
      }
      return lastSegment;
    }

    public static int ComputeSegmentsCount(this IVehiclePathSegment segment)
    {
      int segmentsCount = 1;
      Option<IVehiclePathSegment> nextSegment = segment.NextSegment;
      while (nextSegment.HasValue)
      {
        nextSegment = nextSegment.Value.NextSegment;
        ++segmentsCount;
        if (segmentsCount > 10000)
        {
          Log.Error("Path segments form a loop?");
          break;
        }
      }
      return segmentsCount;
    }
  }
}
