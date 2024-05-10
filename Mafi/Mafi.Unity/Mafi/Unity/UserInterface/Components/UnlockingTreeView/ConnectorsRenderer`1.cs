// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Components.UnlockingTreeView.ConnectorsRenderer`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.UnlockingTree;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface.Style;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Components.UnlockingTreeView
{
  /// <summary>
  /// Takes care of rendering of lines and arrows between the research nodes.
  /// </summary>
  internal class ConnectorsRenderer<TTreeNode> where TTreeNode : class, IUnlockingNode<TTreeNode>
  {
    private readonly UiBuilder m_builder;
    private readonly Func<TTreeNode, Vector2i> m_positionProvider;
    private readonly Func<TTreeNode, IEnumerable<TTreeNode>> m_childrenProvider;
    private readonly Panel m_linesContainer;
    private readonly Sprite m_lineH;
    private readonly Sprite m_lineV;
    private readonly Sprite m_arrow;
    private readonly Sprite m_cornerBR;
    private readonly Sprite m_cornerRB;
    private readonly Sprite m_cornerRT;
    private readonly Sprite m_cornerTR;
    private bool m_inHighlightMode;
    private readonly Lyst<Panel> m_tempHighlight;

    private ColorRgba LineColor
    {
      get
      {
        return !this.m_inHighlightMode ? this.m_builder.Style.Research.LineColor : this.m_builder.Style.Research.LineColorHighlight;
      }
    }

    public ConnectorsRenderer(
      UiBuilder builder,
      Panel container,
      Func<TTreeNode, Vector2i> positionProvider,
      Func<TTreeNode, IEnumerable<TTreeNode>> childrenProvider)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_tempHighlight = new Lyst<Panel>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_positionProvider = positionProvider;
      this.m_childrenProvider = childrenProvider;
      this.m_linesContainer = container;
      this.m_builder = builder.CheckNotNull<UiBuilder>();
      UiStyle style = builder.Style;
      this.m_lineH = builder.AssetsDb.GetSharedSprite(style.Icons.ResLineH);
      this.m_lineV = builder.AssetsDb.GetSharedSprite(style.Icons.ResLineV);
      this.m_arrow = builder.AssetsDb.GetSharedSprite(style.Icons.ResArrow);
      this.m_cornerBR = builder.AssetsDb.GetSharedSprite(style.Icons.ResCornerBR);
      this.m_cornerRB = builder.AssetsDb.GetSharedSprite(style.Icons.ResCornerRB);
      this.m_cornerRT = builder.AssetsDb.GetSharedSprite(style.Icons.ResCornerRT);
      this.m_cornerTR = builder.AssetsDb.GetSharedSprite(style.Icons.ResCornerTR);
    }

    public void Highlight(TTreeNode parent, TTreeNode child)
    {
      this.m_inHighlightMode = true;
      this.createLine(parent, child);
      this.createArrow(child);
      this.m_inHighlightMode = false;
    }

    public void ClearAllHighlights()
    {
      foreach (Panel panel in this.m_tempHighlight)
        panel.GameObject.Destroy();
      this.m_tempHighlight.Clear();
    }

    public void BuildConnectors(ImmutableArray<TTreeNode> nodes)
    {
      foreach (TTreeNode node in nodes)
      {
        if (!node.IsNotAvailable())
        {
          foreach (TTreeNode to in this.m_childrenProvider(node))
          {
            if (!to.IsNotAvailable())
              this.createLine(node, to);
          }
          if (node.Parents.IsNotEmpty)
            this.createArrow(node);
        }
      }
    }

    public void Clear()
    {
      for (int index = 0; index < this.m_linesContainer.RectTransform.childCount; ++index)
        this.m_linesContainer.RectTransform.GetChild(index).gameObject.Destroy();
      this.m_tempHighlight.Clear();
    }

    private void createArrow(TTreeNode node)
    {
      if (!node.Parents.IsNotEmpty)
        return;
      Vector2i vector2i = this.m_positionProvider(node);
      ResearchWindowUiStyle research = this.m_builder.Style.Research;
      Vector2 vector2 = vector2i.Vector2f.ToVector2() + new Vector2(-research.ArrowSize.x, (float) ((double) research.NodeSize.Y / 2.0 - (double) research.ArrowSize.y / 2.0));
      Panel leftTopOf = this.m_builder.NewPanel("Arrow", (IUiElement) this.m_linesContainer).SetBackground(this.m_arrow, new ColorRgba?(this.LineColor)).PutToLeftTopOf<Panel>((IUiElement) this.m_linesContainer, research.ArrowSize, Offset.TopLeft(vector2.y, vector2.x));
      if (!this.m_inHighlightMode)
        return;
      this.m_tempHighlight.Add(leftTopOf);
    }

    private void createLine(TTreeNode from, TTreeNode to)
    {
      ResearchWindowUiStyle research = this.m_builder.Style.Research;
      Vector2i vector2i1 = this.m_positionProvider(from);
      Vector2i vector2i2 = this.m_positionProvider(to);
      float width = (float) (vector2i2.X - vector2i1.X - research.NodeSize.X) - research.ArrowSize.x - (float) research.LineLeftOffset;
      float num1 = (float) research.NodeSize.X / 3f / 2f;
      bool flag = vector2i1.Y < vector2i2.Y;
      int num2 = research.CornerSize / 2;
      int x = research.LineWidth / 2;
      Vector2 vector2_1 = vector2i1.Vector2f.ToVector2() + new Vector2((float) (research.NodeSize.X + research.LineLeftOffset), (float) (research.NodeSize.Y / 2 - x));
      if (vector2i1.Y == vector2i2.Y)
        this.createLineHorizontal(vector2_1.x, vector2_1.y, width);
      else if (vector2i1.X + research.NodeSize.X >= vector2i2.X)
      {
        float num3 = (float) ((vector2i2 - vector2i1).Y.Abs() - research.NodeSize.Y / 2);
        Vector2 vector2_2 = new Vector2((float) (vector2i1.X + 30), (float) vector2i1.Y + (!flag ? -num3 + (float) num2 : (float) research.NodeSize.Y));
        Vector2 vector2_3 = vector2_2 - new Vector2((float) x, 0.0f);
        Vector2 vector2_4 = new Vector2(vector2_2.x, (float) (vector2i2.Y + research.NodeSize.Y / 2 - x));
        this.createLineVertical(vector2_3.x, vector2_2.y, num3 - (float) num2);
        this.createCornerGraphics(vector2_3.x, vector2_4.y, flag ? this.m_cornerBR : this.m_cornerTR);
        this.createLineHorizontal(vector2_4.x + (float) num2, vector2_4.y, (float) vector2i2.X - vector2_2.x - research.ArrowSize.x - (float) num2);
      }
      else
      {
        float num4 = (float) (vector2i2 - vector2i1).Y.Abs();
        Vector2 vector2_5 = vector2_1 + new Vector2(num1 - (float) num2, !flag ? -num4 + (float) research.CornerSize : (float) research.CornerSize);
        Vector2 vector2_6 = new Vector2(vector2_1.x + num1, (float) (vector2i2.Y + research.NodeSize.Y / 2 - x));
        this.createLineHorizontal(vector2_1.x, vector2_1.y, num1 - (float) num2);
        this.createCornerGraphics(vector2_5.x, vector2_1.y, flag ? this.m_cornerRB : this.m_cornerRT);
        this.createLineVertical(vector2_5.x, vector2_5.y, num4 - (float) research.CornerSize);
        this.createCornerGraphics(vector2_5.x, vector2_6.y, flag ? this.m_cornerBR : this.m_cornerTR);
        this.createLineHorizontal(vector2_6.x + (float) num2, vector2_6.y, width - num1 - (float) num2);
      }
    }

    private void createCornerGraphics(float x, float y, Sprite cornerSprite)
    {
      Panel leftTopOf = this.m_builder.NewPanel("Corner", (IUiElement) this.m_linesContainer).SetBackground(cornerSprite, new ColorRgba?(this.LineColor)).PutToLeftTopOf<Panel>((IUiElement) this.m_linesContainer, new Vector2((float) this.m_builder.Style.Research.CornerSize, (float) this.m_builder.Style.Research.CornerSize), Offset.TopLeft(y, x));
      if (!this.m_inHighlightMode)
        return;
      this.m_tempHighlight.Add(leftTopOf);
    }

    private void createLineVertical(float x, float y, float height)
    {
      Panel leftTopOf = this.m_builder.NewPanel("Line", (IUiElement) this.m_linesContainer).SetBackground(this.m_lineV, new ColorRgba?(this.LineColor)).PutToLeftTopOf<Panel>((IUiElement) this.m_linesContainer, new Vector2((float) this.m_builder.Style.Research.LineWidth, height), Offset.TopLeft(y, x));
      if (!this.m_inHighlightMode)
        return;
      this.m_tempHighlight.Add(leftTopOf);
    }

    private void createLineHorizontal(float x, float y, float width)
    {
      Panel leftTopOf = this.m_builder.NewPanel("Line", (IUiElement) this.m_linesContainer).SetBackground(this.m_lineH, new ColorRgba?(this.LineColor)).PutToLeftTopOf<Panel>((IUiElement) this.m_linesContainer, new Vector2(width, (float) this.m_builder.Style.Research.LineWidth), Offset.TopLeft(y, x));
      if (!this.m_inHighlightMode)
        return;
      this.m_tempHighlight.Add(leftTopOf);
    }
  }
}
