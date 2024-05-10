// Decompiled with JetBrains decompiler
// Type: RTG.PlaneDescriptor
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public struct PlaneDescriptor
  {
    private PlaneId _id;
    private PlaneQuadrantId _quadrant;
    private AxisDescriptor _firstAxisDescriptor;
    private AxisDescriptor _secondAxisDescriptor;

    public PlaneId Id => this._id;

    public PlaneQuadrantId Quadrant => this._quadrant;

    public AxisSign FirstAxisSign => this._firstAxisDescriptor.Sign;

    public AxisSign SecondAxisSign => this._secondAxisDescriptor.Sign;

    public int FirstAxisIndex => this._firstAxisDescriptor.Index;

    public int SecondAxisIndex => this._secondAxisDescriptor.Index;

    public AxisDescriptor FirstAxisDescriptor => this._firstAxisDescriptor;

    public AxisDescriptor SecondAxisDescriptor => this._secondAxisDescriptor;

    public PlaneDescriptor(PlaneId planeId, PlaneQuadrantId planeQuadrant)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._id = planeId;
      this._quadrant = planeQuadrant;
      this._firstAxisDescriptor = PlaneIdHelper.GetFirstAxisDescriptor(planeId, planeQuadrant);
      this._secondAxisDescriptor = PlaneIdHelper.GetSecondAxisDescriptor(planeId, planeQuadrant);
    }
  }
}
