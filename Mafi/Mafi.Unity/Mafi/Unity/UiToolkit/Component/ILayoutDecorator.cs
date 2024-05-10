// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Component.ILayoutDecorator
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine.UIElements;

#nullable disable
namespace Mafi.Unity.UiToolkit.Component
{
  public interface ILayoutDecorator
  {
    void SetSize(StyleLength? width = null, StyleLength? height = null);

    void SetMinWidth(StyleLength width);

    void SetMinHeight(StyleLength height);

    void SetMaxSize(StyleLength? width = null, StyleLength? height = null);

    void SetMargin(Px? top = null, Px? right = null, Px? bottom = null, Px? left = null);

    void SetMargin(Percent? top = null, Percent? right = null, Percent? bottom = null, Percent? left = null);

    void AddMarginLeft(float value);

    void AddMarginTop(float value);

    void AddMarginLeftRight(float value);

    void AddMarginTopBottom(float value);

    void Fill();

    void FlexGrow(float flexGrow);

    void FlexShrink(float flexShrink);

    void RelativePosition();

    void SetAbsolutePosition(
      StyleLength? top = null,
      StyleLength? right = null,
      StyleLength? bottom = null,
      StyleLength? left = null);

    void AlignSelf(Align alignSelf);

    void Opacity(float value);

    void SetRotate(int? degrees);

    void SetScale(float x = 1f, float y = 1f);

    void SetTranslate(int x = 0, int y = 0);

    void SetTranslate(Percent x, Percent y);

    void SetTransformOrigin(Percent x, Percent y);
  }
}
