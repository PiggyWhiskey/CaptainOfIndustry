// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Themes.TmpIcon
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Themes
{
  /// <summary>Icons in our atlas for TextMeshPro</summary>
  public struct TmpIcon
  {
    public static TmpIcon InfoIcon;
    public readonly string SpriteTag;

    public TmpIcon(int index, int extraHeight)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      string str = string.Format("<sprite={0} tint=1>", (object) index);
      if (extraHeight > 0)
        str = string.Format("<size=+{0}>{1}</size>", (object) extraHeight, (object) str);
      this.SpriteTag = str;
    }

    static TmpIcon()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      TmpIcon.InfoIcon = new TmpIcon(0, 2);
    }
  }
}
