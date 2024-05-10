// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.SimpleProgressBar
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiFramework.Styles;
using Mafi.Unity.UserInterface;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components
{
  public class SimpleProgressBar : IUiElement
  {
    private readonly bool m_isVertical;
    private readonly Panel m_progressBarContainer;
    private readonly Panel m_progressBar;
    private readonly UiBuilder m_builder;
    /// <summary>Value in [0, 1].</summary>
    private float m_progress;

    public GameObject GameObject => this.m_progressBarContainer.GameObject;

    public RectTransform RectTransform => this.m_progressBarContainer.RectTransform;

    public SimpleProgressBar(IUiElement parent, UiBuilder builder, bool isVertical = false)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_builder = builder;
      this.m_isVertical = isVertical;
      this.m_progressBarContainer = builder.NewPanel("ProgressBarHolder", parent);
      this.m_progressBar = builder.NewPanel("ProgressBar", (IUiElement) this.m_progressBarContainer).SetBackground((ColorRgba) 4558930);
      if (isVertical)
        this.m_progressBar.PutToBottomOf<Panel>((IUiElement) this.m_progressBarContainer, 0.0f);
      else
        this.m_progressBar.PutToLeftOf<Panel>((IUiElement) this.m_progressBarContainer, 0.0f);
    }

    public SimpleProgressBar SetProgress(float progress)
    {
      this.m_progress = progress.Clamp01();
      this.UpdateProgressSize();
      return this;
    }

    public SimpleProgressBar SetProgress(Percent progress) => this.SetProgress(progress.ToFloat());

    public SimpleProgressBar SetProgress(int progress) => this.SetProgress(progress.Percent());

    /// <summary>
    /// Updates progress visualisation according to current progress. To be called after size change.
    /// </summary>
    public SimpleProgressBar UpdateProgressSize()
    {
      if (this.m_isVertical)
        this.m_progressBar.SetHeight<Panel>(this.m_progressBarContainer.GetHeight() * this.m_progress);
      else
        this.m_progressBar.SetWidth<Panel>(this.m_progressBarContainer.GetWidth() * this.m_progress);
      return this;
    }

    public SimpleProgressBar SetBackgroundColor(ColorRgba color)
    {
      this.m_progressBarContainer.SetBackground(color);
      return this;
    }

    public SimpleProgressBar AddBorder(BorderStyle borderStyle)
    {
      this.m_builder.NewPanel("Border").SetBorderStyle(borderStyle).PutTo<Panel>((IUiElement) this.m_progressBarContainer);
      return this;
    }

    public SimpleProgressBar SetColor(ColorRgba color)
    {
      this.m_progressBar.SetBackground(color);
      return this;
    }

    public SimpleProgressBar SetSide(bool fromLeft)
    {
      if (fromLeft)
        this.m_progressBar.PutToLeftOf<Panel>((IUiElement) this.m_progressBarContainer, 0.0f);
      else
        this.m_progressBar.PutToRightOf<Panel>((IUiElement) this.m_progressBarContainer, 0.0f);
      this.UpdateProgressSize();
      return this;
    }
  }
}
