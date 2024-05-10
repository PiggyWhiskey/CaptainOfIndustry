// Decompiled with JetBrains decompiler
// Type: RTG.GLRenderer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace RTG
{
  public static class GLRenderer
  {
    public static void DrawQuads2D(List<Vector2> quadPoints, Camera camera)
    {
      int num = quadPoints.Count / 4;
      if (num < 1)
        return;
      GL.PushMatrix();
      GL.LoadOrtho();
      GL.Begin(7);
      for (int index1 = 0; index1 < num; ++index1)
      {
        int index2 = index1 * 4;
        GL.Vertex(camera.ScreenToViewportPoint((Vector3) quadPoints[index2]));
        GL.Vertex(camera.ScreenToViewportPoint((Vector3) quadPoints[index2 + 1]));
        GL.Vertex(camera.ScreenToViewportPoint((Vector3) quadPoints[index2 + 2]));
        GL.Vertex(camera.ScreenToViewportPoint((Vector3) quadPoints[index2 + 3]));
      }
      GL.End();
      GL.PopMatrix();
    }

    public static void DrawLineLoop2D(List<Vector2> linePoints, Camera camera)
    {
      if (linePoints.Count < 2)
        return;
      GL.PushMatrix();
      GL.LoadOrtho();
      GL.Begin(1);
      for (int index = 0; index < linePoints.Count; ++index)
      {
        Vector3 linePoint1 = (Vector3) linePoints[index];
        Vector3 linePoint2 = (Vector3) linePoints[(index + 1) % linePoints.Count];
        Vector3 viewportPoint1 = camera.ScreenToViewportPoint(linePoint1);
        Vector3 viewportPoint2 = camera.ScreenToViewportPoint(linePoint2);
        GL.Vertex(new Vector3(viewportPoint1.x, viewportPoint1.y, 0.0f));
        GL.Vertex(new Vector3(viewportPoint2.x, viewportPoint2.y, 0.0f));
      }
      GL.End();
      GL.PopMatrix();
    }

    public static void DrawLineLoop2D(
      List<Vector2> linePoints,
      Vector2 translation,
      Vector2 scale,
      Camera camera)
    {
      if (linePoints.Count < 2)
        return;
      GL.PushMatrix();
      GL.LoadOrtho();
      GL.Begin(1);
      for (int index = 0; index < linePoints.Count; ++index)
      {
        Vector3 position1 = (Vector3) (Vector2.Scale(linePoints[index], scale) + translation);
        Vector3 position2 = (Vector3) (Vector2.Scale(linePoints[(index + 1) % linePoints.Count], scale) + translation);
        Vector3 viewportPoint1 = camera.ScreenToViewportPoint(position1);
        Vector3 viewportPoint2 = camera.ScreenToViewportPoint(position2);
        GL.Vertex(new Vector3(viewportPoint1.x, viewportPoint1.y, 0.0f));
        GL.Vertex(new Vector3(viewportPoint2.x, viewportPoint2.y, 0.0f));
      }
      GL.End();
      GL.PopMatrix();
    }

    public static void DrawLines2D(List<Vector2> linePoints, Camera camera)
    {
      if (linePoints.Count < 2)
        return;
      GL.PushMatrix();
      GL.LoadOrtho();
      GL.Begin(1);
      for (int index = 0; index < linePoints.Count - 1; ++index)
      {
        Vector3 linePoint1 = (Vector3) linePoints[index];
        Vector3 linePoint2 = (Vector3) linePoints[index + 1];
        Vector3 viewportPoint1 = camera.ScreenToViewportPoint(linePoint1);
        Vector3 viewportPoint2 = camera.ScreenToViewportPoint(linePoint2);
        GL.Vertex(new Vector3(viewportPoint1.x, viewportPoint1.y, 0.0f));
        GL.Vertex(new Vector3(viewportPoint2.x, viewportPoint2.y, 0.0f));
      }
      GL.End();
      GL.PopMatrix();
    }

    public static void DrawLines2D(
      List<Vector2> linePoints,
      Vector2 translation,
      Vector2 scale,
      Camera camera)
    {
      if (linePoints.Count < 2)
        return;
      GL.PushMatrix();
      GL.LoadOrtho();
      GL.Begin(1);
      for (int index = 0; index < linePoints.Count - 1; ++index)
      {
        Vector3 position1 = (Vector3) (Vector2.Scale(linePoints[index], scale) + translation);
        Vector3 position2 = (Vector3) (Vector2.Scale(linePoints[index + 1], scale) + translation);
        Vector3 viewportPoint1 = camera.ScreenToViewportPoint(position1);
        Vector3 viewportPoint2 = camera.ScreenToViewportPoint(position2);
        GL.Vertex(new Vector3(viewportPoint1.x, viewportPoint1.y, 0.0f));
        GL.Vertex(new Vector3(viewportPoint2.x, viewportPoint2.y, 0.0f));
      }
      GL.End();
      GL.PopMatrix();
    }

    public static void DrawLine2D(Vector2 startPoint, Vector2 endPoint, Camera camera)
    {
      GL.PushMatrix();
      GL.LoadOrtho();
      GL.Begin(1);
      GL.Vertex(camera.ScreenToViewportPoint((Vector3) startPoint));
      GL.Vertex(camera.ScreenToViewportPoint((Vector3) endPoint));
      GL.End();
      GL.PopMatrix();
    }

    public static void DrawLine3D(Vector3 startPoint, Vector3 endPoint)
    {
      GL.Begin(1);
      GL.Vertex(startPoint);
      GL.Vertex(endPoint);
      GL.End();
    }

    public static void DrawLines3D(List<Vector3> linePoints)
    {
      if (linePoints.Count < 2)
        return;
      GL.Begin(1);
      for (int index = 0; index < linePoints.Count - 1; ++index)
      {
        Vector3 linePoint1 = linePoints[index];
        Vector3 linePoint2 = linePoints[index + 1];
        GL.Vertex(linePoint1);
        GL.Vertex(linePoint2);
      }
      GL.End();
    }

    public static void DrawLineLoop3D(List<Vector3> linePoints)
    {
      if (linePoints.Count < 2)
        return;
      GL.Begin(1);
      for (int index = 0; index < linePoints.Count; ++index)
      {
        Vector3 linePoint1 = linePoints[index];
        Vector3 linePoint2 = linePoints[(index + 1) % linePoints.Count];
        GL.Vertex(linePoint1);
        GL.Vertex(linePoint2);
      }
      GL.End();
    }

    public static void DrawLineStrip3D(List<Vector3> linePoints)
    {
      if (linePoints.Count < 2)
        return;
      GL.Begin(1);
      for (int index = 0; index < linePoints.Count - 1; ++index)
      {
        GL.Vertex(linePoints[index]);
        GL.Vertex(linePoints[index + 1]);
      }
      GL.End();
    }

    public static void DrawLineLoop3D(List<Vector3> linePoints, Vector3 pointOffset)
    {
      if (linePoints.Count < 2)
        return;
      GL.Begin(1);
      for (int index = 0; index < linePoints.Count; ++index)
      {
        Vector3 v1 = linePoints[index] + pointOffset;
        Vector3 v2 = linePoints[(index + 1) % linePoints.Count] + pointOffset;
        GL.Vertex(v1);
        GL.Vertex(v2);
      }
      GL.End();
    }

    public static void DrawLinePairs3D(List<Vector3> pairPoints)
    {
      if (pairPoints.Count < 2 || pairPoints.Count % 2 != 0)
        return;
      GL.Begin(1);
      for (int index = 0; index < pairPoints.Count; index += 2)
      {
        Vector3 pairPoint1 = pairPoints[index];
        Vector3 pairPoint2 = pairPoints[index + 1];
        GL.Vertex(pairPoint1);
        GL.Vertex(pairPoint2);
      }
      GL.End();
    }

    public static void DrawRectBorder2D(Rect rect, Camera camera)
    {
      GLRenderer.DrawLineLoop2D(rect.GetCornerPoints(), camera);
    }

    public static void DrawRect2D(Rect rect, Camera camera)
    {
      GL.PushMatrix();
      GL.LoadOrtho();
      GL.Begin(7);
      List<Vector2> cornerPoints = rect.GetCornerPoints();
      cornerPoints[0] = (Vector2) camera.ScreenToViewportPoint((Vector3) cornerPoints[0]);
      cornerPoints[1] = (Vector2) camera.ScreenToViewportPoint((Vector3) cornerPoints[1]);
      cornerPoints[2] = (Vector2) camera.ScreenToViewportPoint((Vector3) cornerPoints[2]);
      cornerPoints[3] = (Vector2) camera.ScreenToViewportPoint((Vector3) cornerPoints[3]);
      GL.Vertex((Vector3) cornerPoints[0]);
      GL.Vertex((Vector3) cornerPoints[1]);
      GL.Vertex((Vector3) cornerPoints[2]);
      GL.Vertex((Vector3) cornerPoints[3]);
      GL.End();
      GL.PopMatrix();
    }

    public static void DrawCircleBorder2D(
      Vector2 circleCenter,
      float circleRadius,
      int numPoints,
      Camera camera)
    {
      GLRenderer.DrawLineLoop2D(PrimitiveFactory.Generate2DCircleBorderPointsCW(circleCenter, circleRadius, numPoints), camera);
    }

    public static void DrawCircle2D(
      Vector2 circleCenter,
      float circleRadius,
      int numPoints,
      Camera camera)
    {
      List<Vector2> points = PrimitiveFactory.Generate2DCircleBorderPointsCW(circleCenter, circleRadius, numPoints);
      GLRenderer.DrawTriangleFan2D(circleCenter, points, camera);
    }

    public static void DrawCircleBorder3D(
      Vector3 circleCenter,
      float circleRadius,
      Vector3 circleRight,
      Vector3 circleUp,
      int numPoints)
    {
      GLRenderer.DrawLineLoop3D(PrimitiveFactory.Generate3DCircleBorderPoints(circleCenter, circleRadius, circleRight, circleUp, numPoints));
    }

    public static void DrawCircle3D(
      Vector2 circleCenter,
      float circleRadius,
      Vector3 circleRight,
      Vector3 circleUp,
      int numPoints)
    {
      List<Vector3> points = PrimitiveFactory.Generate3DCircleBorderPoints((Vector3) circleCenter, circleRadius, circleRight, circleUp, numPoints);
      GLRenderer.DrawTriangleFan3D((Vector3) circleCenter, points);
    }

    public static void DrawSphereBorder(
      Camera camera,
      Vector3 sphereCenter,
      float sphereRadius,
      int numPoints)
    {
      GLRenderer.DrawLineLoop3D(PrimitiveFactory.GenerateSphereBorderPoints(camera, sphereCenter, sphereRadius, numPoints));
    }

    public static void DrawTriangleFan2D(
      Vector2 origin,
      List<Vector2> points,
      Vector2 translation,
      Vector2 scale,
      Camera camera)
    {
      int num = points.Count - 1;
      if (num < 1)
        return;
      GL.PushMatrix();
      GL.LoadOrtho();
      GL.Begin(4);
      origin = (Vector2) camera.ScreenToViewportPoint((Vector3) (Vector2.Scale(origin, scale) + translation));
      for (int index = 0; index < num; ++index)
      {
        GL.Vertex((Vector3) origin);
        GL.Vertex(camera.ScreenToViewportPoint((Vector3) (Vector2.Scale(points[index], scale) + translation)));
        GL.Vertex(camera.ScreenToViewportPoint((Vector3) (Vector2.Scale(points[index + 1], scale) + translation)));
      }
      GL.End();
      GL.PopMatrix();
    }

    public static void DrawTriangleFan2D(Vector2 origin, List<Vector2> points, Camera camera)
    {
      int num = points.Count - 1;
      if (num < 1)
        return;
      GL.PushMatrix();
      GL.LoadOrtho();
      GL.Begin(4);
      origin = (Vector2) camera.ScreenToViewportPoint((Vector3) origin);
      for (int index = 0; index < num; ++index)
      {
        GL.Vertex((Vector3) origin);
        GL.Vertex(camera.ScreenToViewportPoint((Vector3) points[index]));
        GL.Vertex(camera.ScreenToViewportPoint((Vector3) points[index + 1]));
      }
      GL.End();
      GL.PopMatrix();
    }

    public static void DrawTriangleFan3D(
      Vector3 origin,
      List<Vector3> points,
      Vector3 translation,
      Vector3 scale)
    {
      int num = points.Count - 1;
      if (num < 1)
        return;
      GL.Begin(4);
      origin = Vector3.Scale(origin, scale) + translation;
      for (int index = 0; index < num; ++index)
      {
        GL.Vertex(origin);
        GL.Vertex(Vector3.Scale(points[index], scale) + translation);
        GL.Vertex(Vector3.Scale(points[index + 1], scale) + translation);
      }
      GL.End();
    }

    public static void DrawTriangleFan3D(Vector3 origin, List<Vector3> points)
    {
      int num = points.Count - 1;
      if (num < 1)
        return;
      GL.Begin(4);
      for (int index = 0; index < num; ++index)
      {
        GL.Vertex(origin);
        GL.Vertex(points[index]);
        GL.Vertex(points[index + 1]);
      }
      GL.End();
    }
  }
}
