// Decompiled with JetBrains decompiler
// Type: RTG.BoxFaceAreaDesc
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public struct BoxFaceAreaDesc
  {
    public BoxFaceAreaType AreaType;
    public float Area;

    public BoxFaceAreaDesc(BoxFaceAreaType areaType, float area)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.AreaType = areaType;
      this.Area = area;
    }

    public static BoxFaceAreaDesc GetInvalid()
    {
      return new BoxFaceAreaDesc(BoxFaceAreaType.Invalid, 0.0f);
    }
  }
}
