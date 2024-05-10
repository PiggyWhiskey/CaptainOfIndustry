// Decompiled with JetBrains decompiler
// Type: RTG.MeshPool
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class MeshPool : Singleton<MeshPool>
  {
    private Mesh _unitTorus;
    private Mesh _unitCylindricalTorus;
    private Mesh _unitBox;
    private Mesh _unitWireBox;
    private Mesh _unitPyramid;
    private Mesh _unitWirePyramid;
    private Mesh _unitTriangularPrism;
    private Mesh _unitWireTriangularPrism;
    private Mesh _unitCone;
    private Mesh _unitCylinder;
    private Mesh _unitSphere;
    private Mesh _unitCoordSystem;
    private Mesh _unitSegmentX;
    private Mesh _unitQuadXY;
    private Mesh _unitQuadXZ;
    private Mesh _unitWireQuadXY;
    private Mesh _unitCircleXY;
    private Mesh _unitWireCircleXY;
    private Mesh _unitRightAngledTriangleXY;
    private Mesh _unitWireRightAngledTriangleXY;
    private Mesh _unitEqTriangleXY;
    private Mesh _unitWireEqTriangleXY;

    public Mesh UnitTorus
    {
      get
      {
        if ((Object) this._unitTorus == (Object) null)
          this._unitTorus = TorusMesh.CreateTorus(Vector3.zero, 1f, 1f, 80, 80, Color.white);
        return this._unitTorus;
      }
    }

    public Mesh UnitCylindricalTorus
    {
      get
      {
        if ((Object) this._unitCylindricalTorus == (Object) null)
          this._unitCylindricalTorus = TorusMesh.CreateCylindricalTorus(Vector3.zero, 1f, 1f, 1f, 80, Color.white);
        return this._unitCylindricalTorus;
      }
    }

    public Mesh UnitQuadXY
    {
      get
      {
        if ((Object) this._unitQuadXY == (Object) null)
          this._unitQuadXY = QuadMesh.CreateQuadXY(1f, 1f, Color.white);
        return this._unitQuadXY;
      }
    }

    public Mesh UnitQuadXZ
    {
      get
      {
        if ((Object) this._unitQuadXZ == (Object) null)
          this._unitQuadXZ = QuadMesh.CreateQuadXZ(1f, 1f, Color.white);
        return this._unitQuadXZ;
      }
    }

    public Mesh UnitBox
    {
      get
      {
        if ((Object) this._unitBox == (Object) null)
          this._unitBox = BoxMesh.CreateBox(1f, 1f, 1f, Color.white);
        return this._unitBox;
      }
    }

    public Mesh UnitWireBox
    {
      get
      {
        if ((Object) this._unitWireBox == (Object) null)
          this._unitWireBox = BoxMesh.CreateWireBox(1f, 1f, 1f, Color.white);
        return this._unitWireBox;
      }
    }

    public Mesh UnitPyramid
    {
      get
      {
        if ((Object) this._unitPyramid == (Object) null)
          this._unitPyramid = PyramidMesh.CreatePyramid(Vector3.zero, 1f, 1f, 1f, Color.white);
        return this._unitPyramid;
      }
    }

    public Mesh UnitWirePyramid
    {
      get
      {
        if ((Object) this._unitWirePyramid == (Object) null)
          this._unitWirePyramid = PyramidMesh.CreateWirePyramid(Vector3.zero, 1f, 1f, 1f, Color.white);
        return this._unitWirePyramid;
      }
    }

    public Mesh UnitTriangularPrism
    {
      get
      {
        if ((Object) this._unitTriangularPrism == (Object) null)
          this._unitTriangularPrism = PrismMesh.CreateTriangularPrism(Vector3.zero, 1f, 1f, 1f, 1f, 1f, Color.white);
        return this._unitTriangularPrism;
      }
    }

    public Mesh UnitWireTriangularPrism
    {
      get
      {
        if ((Object) this._unitWireTriangularPrism == (Object) null)
          this._unitWireTriangularPrism = PrismMesh.CreateWireTriangularPrism(Vector3.zero, 1f, 1f, 1f, 1f, 1f, Color.white);
        return this._unitWireTriangularPrism;
      }
    }

    public Mesh UnitCone
    {
      get
      {
        if ((Object) this._unitCone == (Object) null)
          this._unitCone = CylinderMesh.CreateCylinder(1f, 0.0f, 1f, 30, 30, 1, 1, Color.white);
        return this._unitCone;
      }
    }

    public Mesh UnitCylinder
    {
      get
      {
        if ((Object) this._unitCylinder == (Object) null)
          this._unitCylinder = CylinderMesh.CreateCylinder(1f, 1f, 1f, 30, 30, 1, 1, Color.white);
        return this._unitCylinder;
      }
    }

    public Mesh UnitSphere
    {
      get
      {
        if ((Object) this._unitSphere == (Object) null)
          this._unitSphere = SphereMesh.CreateSphere(1f, 30, 30, Color.white);
        return this._unitSphere;
      }
    }

    public Mesh UnitCoordSystem
    {
      get
      {
        if ((Object) this._unitCoordSystem == (Object) null)
          this._unitCoordSystem = LineMesh.CreateCoordSystemAxesLines(1f, Color.white);
        return this._unitCoordSystem;
      }
    }

    public Mesh UnitSegmentX
    {
      get
      {
        if ((Object) this._unitSegmentX == (Object) null)
          this._unitSegmentX = LineMesh.CreateLine(Vector3.zero, new Vector3(1f, 0.0f, 0.0f), Color.white);
        return this._unitSegmentX;
      }
    }

    public Mesh UnitWireQuadXY
    {
      get
      {
        if ((Object) this._unitWireQuadXY == (Object) null)
          this._unitWireQuadXY = QuadMesh.CreateWireQuadXY(Vector3.zero, new Vector2(1f, 1f), Color.white);
        return this._unitWireQuadXY;
      }
    }

    public Mesh UnitCircleXY
    {
      get
      {
        if ((Object) this._unitCircleXY == (Object) null)
          this._unitCircleXY = CircleMesh.CreateCircleXY(1f, 200, Color.white);
        return this._unitCircleXY;
      }
    }

    public Mesh UnitWireCircleXY
    {
      get
      {
        if ((Object) this._unitWireCircleXY == (Object) null)
          this._unitWireCircleXY = CircleMesh.CreateWireCircleXY(1f, 200, Color.white);
        return this._unitWireCircleXY;
      }
    }

    public Mesh UnitRightAngledTriangleXY
    {
      get
      {
        if ((Object) this._unitRightAngledTriangleXY == (Object) null)
          this._unitRightAngledTriangleXY = TriangleMesh.CreateRightAngledTriangleXY(Vector3.zero, 1f, 1f, Color.white);
        return this._unitRightAngledTriangleXY;
      }
    }

    public Mesh UnitWireRightAngledTriangleXY
    {
      get
      {
        if ((Object) this._unitWireRightAngledTriangleXY == (Object) null)
          this._unitWireRightAngledTriangleXY = TriangleMesh.CreateWireRightAngledTriangleXY(Vector3.zero, 1f, 1f, Color.white);
        return this._unitWireRightAngledTriangleXY;
      }
    }

    public Mesh UnitEqTriangleXY
    {
      get
      {
        if ((Object) this._unitEqTriangleXY == (Object) null)
          this._unitEqTriangleXY = TriangleMesh.CreateEqXY(Vector3.zero, 1f, Color.white);
        return this._unitEqTriangleXY;
      }
    }

    public Mesh UnitWireEqTriangleXY
    {
      get
      {
        if ((Object) this._unitWireEqTriangleXY == (Object) null)
          this._unitWireEqTriangleXY = TriangleMesh.CreateWireEqXY(Vector3.zero, 1f, Color.white);
        return this._unitWireEqTriangleXY;
      }
    }

    public MeshPool()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
