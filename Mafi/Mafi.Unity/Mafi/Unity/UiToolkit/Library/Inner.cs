// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.Inner
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiToolkit.Component;
using UnityEngine.UIElements;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library
{
  public class Inner : UiComponent
  {
    public VisualElement ElementToPlace => this.Element;

    public static Inner GlowAll => new Inner(Cls.inGlow, Cls.glowAll);

    public static Inner GlowError => new Inner(Cls.inGlow, Cls.glowError);

    public static Inner ShadowLtr => new Inner(Cls.inShadow, Cls.shadowLtr);

    public Inner(string className1, string className2)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Class<Inner>(className1, className2).IgnoreInputPicking<Inner>();
    }
  }
}
