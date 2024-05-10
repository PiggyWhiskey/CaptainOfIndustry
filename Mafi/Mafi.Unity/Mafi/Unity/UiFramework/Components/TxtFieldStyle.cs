// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.TxtFieldStyle
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components
{
  public struct TxtFieldStyle
  {
    public static readonly TextStyle DEFAULT;

    public TextStyle TextStyle { get; private set; }

    public TextStyle PlaceHolderStyle { get; private set; }

    public TxtFieldStyle(TextStyle textStyle, TextStyle placeHolderStyle)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.TextStyle = textStyle;
      this.PlaceHolderStyle = placeHolderStyle;
    }

    static TxtFieldStyle()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      TxtFieldStyle.DEFAULT = new TextStyle(ColorRgba.Black);
    }
  }
}
