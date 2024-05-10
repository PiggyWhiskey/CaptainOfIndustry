// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.Charts.PointHighlightMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using Mafi.Unity.UserInterface;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components.Charts
{
  public class PointHighlightMb : MonoBehaviour
  {
    private RectTransform m_transform;
    private Txt m_text;
    private CanvasRenderer m_renderer;
    private float m_halfPointSize;
    private Panel m_textHolder;

    public Vector2? Position { get; private set; }

    public void Initialize(UiBuilder builder, float pointSize, int fontSize)
    {
      this.m_transform = (RectTransform) this.transform;
      this.m_textHolder = builder.NewPanel("TxtHolder").SetBackground(ColorRgba.Black);
      this.m_textHolder.RectTransform.SetParent((Transform) this.m_transform, false);
      this.m_text = builder.NewTxt("Point label").SetColor(ColorRgba.White).AllowHorizontalOverflow().SetTextStyle(new TextStyle(ColorRgba.White, fontSize)).PutTo<Txt>((IUiElement) this.m_textHolder);
      this.m_renderer = this.gameObject.AddComponent<CanvasRenderer>();
      this.m_renderer.materialCount = 1;
      BuildableCanvasMb buildableMb = this.gameObject.AddComponent<BuildableCanvasMb>();
      MeshBuilder instance = MeshBuilder.Instance;
      this.m_halfPointSize = pointSize / 2f;
      instance.AddQuad((Vector3) new Vector2(-this.m_halfPointSize, -this.m_halfPointSize), (Vector3) new Vector2(-this.m_halfPointSize, this.m_halfPointSize), (Vector3) new Vector2(this.m_halfPointSize, this.m_halfPointSize), (Vector3) new Vector2(this.m_halfPointSize, -this.m_halfPointSize), (Color32) Color.white);
      instance.UpdateMbAndClear((IBuildable) buildableMb);
    }

    public void Show(Vector2 position, Material pointMaterial, LocStrFormatted text)
    {
      this.m_transform.localPosition = (Vector3) position;
      this.m_renderer.SetMaterial(pointMaterial, 0);
      this.m_text.SetText(text);
      Vector2 preferedSize = this.m_text.GetPreferedSize();
      this.m_textHolder.SetSize<Panel>(preferedSize);
      this.m_textHolder.SetLocalPosition<Panel>((Vector3) new Vector2(0.0f, preferedSize.y / 2f + this.m_halfPointSize));
      this.Position = new Vector2?(position);
    }

    public void Hide() => this.Position = new Vector2?();

    public PointHighlightMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
