// Decompiled with JetBrains decompiler
// Type: RTG.PlaneIdHelper
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using System.Collections.Generic;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public static class PlaneIdHelper
  {
    private static List<PlaneIdHelper.PlaneInfo> _planeInfo;
    private static PlaneId[] _allPlaneIds;

    static PlaneIdHelper()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      PlaneIdHelper._planeInfo = new List<PlaneIdHelper.PlaneInfo>(3)
      {
        new PlaneIdHelper.PlaneInfo(),
        new PlaneIdHelper.PlaneInfo(),
        new PlaneIdHelper.PlaneInfo()
      };
      PlaneIdHelper._allPlaneIds = new PlaneId[3]
      {
        PlaneId.XY,
        PlaneId.ZX,
        PlaneId.YZ
      };
      PlaneIdHelper._planeInfo[0] = new PlaneIdHelper.PlaneInfo()
      {
        PlaneId = PlaneId.XY,
        QuadrantInfo = new List<PlaneIdHelper.PlaneQuadrantInfo>()
        {
          new PlaneIdHelper.PlaneQuadrantInfo()
          {
            Quadrant = PlaneQuadrantId.First,
            FirstAxisSign = AxisSign.Positive,
            SecondAxisSign = AxisSign.Positive
          },
          new PlaneIdHelper.PlaneQuadrantInfo()
          {
            Quadrant = PlaneQuadrantId.Second,
            FirstAxisSign = AxisSign.Negative,
            SecondAxisSign = AxisSign.Positive
          },
          new PlaneIdHelper.PlaneQuadrantInfo()
          {
            Quadrant = PlaneQuadrantId.Third,
            FirstAxisSign = AxisSign.Negative,
            SecondAxisSign = AxisSign.Negative
          },
          new PlaneIdHelper.PlaneQuadrantInfo()
          {
            Quadrant = PlaneQuadrantId.Fourth,
            FirstAxisSign = AxisSign.Positive,
            SecondAxisSign = AxisSign.Negative
          }
        }
      };
      PlaneIdHelper._planeInfo[1] = new PlaneIdHelper.PlaneInfo()
      {
        PlaneId = PlaneId.YZ,
        QuadrantInfo = new List<PlaneIdHelper.PlaneQuadrantInfo>()
        {
          new PlaneIdHelper.PlaneQuadrantInfo()
          {
            Quadrant = PlaneQuadrantId.First,
            FirstAxisSign = AxisSign.Positive,
            SecondAxisSign = AxisSign.Negative
          },
          new PlaneIdHelper.PlaneQuadrantInfo()
          {
            Quadrant = PlaneQuadrantId.Second,
            FirstAxisSign = AxisSign.Positive,
            SecondAxisSign = AxisSign.Positive
          },
          new PlaneIdHelper.PlaneQuadrantInfo()
          {
            Quadrant = PlaneQuadrantId.Third,
            FirstAxisSign = AxisSign.Negative,
            SecondAxisSign = AxisSign.Positive
          },
          new PlaneIdHelper.PlaneQuadrantInfo()
          {
            Quadrant = PlaneQuadrantId.Fourth,
            FirstAxisSign = AxisSign.Negative,
            SecondAxisSign = AxisSign.Negative
          }
        }
      };
      PlaneIdHelper._planeInfo[2] = new PlaneIdHelper.PlaneInfo()
      {
        PlaneId = PlaneId.ZX,
        QuadrantInfo = new List<PlaneIdHelper.PlaneQuadrantInfo>()
        {
          new PlaneIdHelper.PlaneQuadrantInfo()
          {
            Quadrant = PlaneQuadrantId.First,
            FirstAxisSign = AxisSign.Positive,
            SecondAxisSign = AxisSign.Positive
          },
          new PlaneIdHelper.PlaneQuadrantInfo()
          {
            Quadrant = PlaneQuadrantId.Second,
            FirstAxisSign = AxisSign.Negative,
            SecondAxisSign = AxisSign.Positive
          },
          new PlaneIdHelper.PlaneQuadrantInfo()
          {
            Quadrant = PlaneQuadrantId.Third,
            FirstAxisSign = AxisSign.Negative,
            SecondAxisSign = AxisSign.Negative
          },
          new PlaneIdHelper.PlaneQuadrantInfo()
          {
            Quadrant = PlaneQuadrantId.Fourth,
            FirstAxisSign = AxisSign.Positive,
            SecondAxisSign = AxisSign.Negative
          }
        }
      };
    }

    public static PlaneId[] AllPlaneIds => PlaneIdHelper._allPlaneIds.Clone() as PlaneId[];

    public static AxisDescriptor GetFirstAxisDescriptor(
      PlaneId planeId,
      PlaneQuadrantId planeQuadrant)
    {
      return new AxisDescriptor(PlaneIdHelper.PlaneIdToFirstAxisIndex(planeId), PlaneIdHelper.GetFirstAxisSign(planeId, planeQuadrant));
    }

    public static AxisDescriptor GetSecondAxisDescriptor(
      PlaneId planeId,
      PlaneQuadrantId planeQuadrant)
    {
      return new AxisDescriptor(PlaneIdHelper.PlaneIdToSecondAxisIndex(planeId), PlaneIdHelper.GetSecondAxisSign(planeId, planeQuadrant));
    }

    public static AxisSign GetFirstAxisSign(PlaneId planeId, PlaneQuadrantId planeQuadrant)
    {
      return PlaneIdHelper._planeInfo[(int) planeId].QuadrantInfo.FindAll((Predicate<PlaneIdHelper.PlaneQuadrantInfo>) (item => item.Quadrant == planeQuadrant))[0].FirstAxisSign;
    }

    public static AxisSign GetSecondAxisSign(PlaneId planeId, PlaneQuadrantId planeQuadrant)
    {
      return PlaneIdHelper._planeInfo[(int) planeId].QuadrantInfo.FindAll((Predicate<PlaneIdHelper.PlaneQuadrantInfo>) (item => item.Quadrant == planeQuadrant))[0].SecondAxisSign;
    }

    public static PlaneQuadrantId GetQuadrantFromAxesSigns(
      PlaneId planeId,
      AxisSign firstAxisSign,
      AxisSign secondAxisSign)
    {
      return PlaneIdHelper._planeInfo[(int) planeId].QuadrantInfo.FindAll((Predicate<PlaneIdHelper.PlaneQuadrantInfo>) (item => item.FirstAxisSign == firstAxisSign && item.SecondAxisSign == secondAxisSign))[0].Quadrant;
    }

    public static int PlaneIdToFirstAxisIndex(PlaneId planeId)
    {
      if (planeId == PlaneId.XY)
        return 0;
      return planeId == PlaneId.ZX ? 2 : 1;
    }

    public static int PlaneIdToSecondAxisIndex(PlaneId planeId)
    {
      if (planeId == PlaneId.XY)
        return 1;
      return planeId == PlaneId.ZX ? 0 : 2;
    }

    public static PlaneId NormalAxisIndexToPlaneId(int axisIndex)
    {
      if (axisIndex == 0)
        return PlaneId.YZ;
      return axisIndex == 1 ? PlaneId.ZX : PlaneId.XY;
    }

    private struct PlaneQuadrantInfo
    {
      public PlaneQuadrantId Quadrant;
      public AxisSign FirstAxisSign;
      public AxisSign SecondAxisSign;
    }

    private struct PlaneInfo
    {
      public PlaneId PlaneId;
      public List<PlaneIdHelper.PlaneQuadrantInfo> QuadrantInfo;
    }
  }
}
