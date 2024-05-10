// Decompiled with JetBrains decompiler
// Type: RTG.YesNoAnswer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class YesNoAnswer
  {
    private bool _hasYes;
    private bool _hasNo;

    public bool HasYes => this._hasYes;

    public bool HasNo => this._hasNo;

    public bool HasOnlyYes => this.HasYes && !this.HasNo;

    public void Yes() => this._hasYes = true;

    public void No() => this._hasNo = true;

    public YesNoAnswer()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
