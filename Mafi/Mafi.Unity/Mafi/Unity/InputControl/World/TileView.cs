// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.World.TileView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.World
{
  internal class TileView : IUiElement
  {
    private readonly LocStrFormatted m_label;
    private readonly StackContainer m_parent;
    private readonly Offset m_extraOffset;
    private readonly Panel m_container;
    private readonly Txt m_text;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public TileView(
      UiBuilder builder,
      LocStrFormatted label,
      LocStrFormatted tooltip,
      StackContainer parent,
      Offset? extraOffset = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_label = label;
      this.m_parent = parent;
      this.m_extraOffset = extraOffset ?? Offset.Zero;
      this.m_container = builder.NewPanel("Tile").SetBackground((ColorRgba) 2236962).AppendTo<Panel>(parent);
      if (tooltip.IsNotEmpty)
        this.m_container.AddToolTip(tooltip.Value);
      Txt txt = builder.NewTxt("");
      TextStyle text = builder.Style.Global.Text;
      ref TextStyle local = ref text;
      bool? nullable = new bool?(true);
      ColorRgba? color = new ColorRgba?();
      FontStyle? fontStyle = new FontStyle?();
      int? fontSize = new int?();
      bool? isCapitalized = nullable;
      TextStyle textStyle = local.Extend(color, fontStyle, fontSize, isCapitalized);
      this.m_text = txt.SetTextStyle(textStyle).SetAlignment(TextAnchor.MiddleLeft).EnableRichText().SetText("").PutTo<Txt>((IUiElement) this.m_container, Offset.LeftRight(10f) + this.m_extraOffset);
    }

    public Tooltip AddToolTipAndReturn() => this.m_container.AddToolTipAndReturn();

    public void SetValue(string value)
    {
      this.m_text.SetText(string.Format("<size=16><b>{0}</b></size>\n{1}", (object) value, (object) this.m_label));
      this.m_container.SetWidth<Panel>(this.m_text.GetPreferedWidth() + 20f);
      this.m_parent.UpdateItemWidth((IUiElement) this.m_container, this.m_text.GetPreferedWidth() + 20f + this.m_extraOffset.LeftRightOffset);
    }
  }
}
