// Decompiled with JetBrains decompiler
// Type: RTG.AxisDescriptor
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class AxisDescriptor
  {
    private AxisSign _sign;
    private int _index;

    public AxisSign Sign => this._sign;

    public int Index => this._index;

    public bool IsPositive => this._sign == AxisSign.Positive;

    public bool IsNegative => this._sign == AxisSign.Negative;

    public AxisDescriptor(int axisIndex, AxisSign axisSign)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._sign = axisSign;
      this._index = axisIndex;
    }

    public AxisDescriptor(int axisIndex, bool isNegative)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._sign = isNegative ? AxisSign.Negative : AxisSign.Positive;
      this._index = axisIndex;
    }

    public BoxFace GetAssociatedBoxFace()
    {
      if (this._sign == AxisSign.Negative)
      {
        if (this._index == 0)
          return BoxFace.Left;
        return this._index == 1 ? BoxFace.Bottom : BoxFace.Front;
      }
      if (this._index == 0)
        return BoxFace.Right;
      return this._index == 1 ? BoxFace.Top : BoxFace.Back;
    }
  }
}
