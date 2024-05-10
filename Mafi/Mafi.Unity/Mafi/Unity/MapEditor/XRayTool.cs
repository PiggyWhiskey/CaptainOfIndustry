// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MapEditor.XRayTool
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.GameLoop;
using Mafi.Core.Input;
using Mafi.Core.Terrain;
using Mafi.Unity.InputControl;
using Mafi.Unity.Terrain;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MapEditor
{
  public class XRayTool : IUnityInputController, IRegisterInMapEditor
  {
    public static readonly RelTile1i TOOL_OUTER_RADIUS;
    public static readonly RelTile1i TOOL_INNER_RADIUS;
    public static readonly ThicknessTilesI TOOL_BASE_DEPTH;
    private readonly TerrainManager m_terrainManager;
    private readonly TerrainRenderer m_terrainRenderer;
    private readonly TerrainCursor m_terrainCursor;
    private readonly IGameLoopEvents m_gameLoop;
    private Tile2i m_oldXRayPos;
    private ThicknessTilesI m_oldDepthOffset;
    private readonly RelTile1i m_radius;
    private ThicknessTilesI m_depthOffset;
    private readonly XRayToolbox m_toolbox;
    private readonly TerrainCircleRenderer m_topCircleRenderer;
    private readonly TerrainCircleRenderer m_bottomCircleRenderer;

    public ControllerConfig Config => ControllerConfig.ToolBlockingCamera;

    public bool IsEnabled { get; private set; }

    public XRayTool(
      NewInstanceOf<XRayToolbox> toolbox,
      TerrainManager terrainManager,
      TerrainRenderer terrainRenderer,
      LinesFactory linesFactory,
      NewInstanceOf<TerrainCursor> terrainCursor,
      IGameLoopEvents gameLoop)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_terrainManager = terrainManager;
      this.m_terrainRenderer = terrainRenderer;
      this.m_terrainCursor = terrainCursor.Instance;
      this.m_gameLoop = gameLoop;
      this.m_toolbox = toolbox.Instance;
      this.m_toolbox.SetOnUp(new Func<bool?>(this.onUp));
      this.m_toolbox.SetOnDown(new Func<bool?>(this.onDown));
      this.m_topCircleRenderer = new TerrainCircleRenderer(linesFactory);
      this.m_topCircleRenderer.SetColor(new Color(0.8f, 0.8f, 0.8f, 0.3f));
      this.m_topCircleRenderer.SetWidth(0.5f);
      this.m_topCircleRenderer.Hide();
      this.m_bottomCircleRenderer = new TerrainCircleRenderer(linesFactory);
      this.m_bottomCircleRenderer.SetColor(new Color(0.6f, 0.6f, 0.6f, 0.3f));
      this.m_bottomCircleRenderer.SetWidth(0.5f);
      this.m_bottomCircleRenderer.Hide();
      this.m_radius = XRayTool.TOOL_OUTER_RADIUS;
      this.m_depthOffset = ThicknessTilesI.Zero;
    }

    public void Activate()
    {
      if (this.IsEnabled)
        return;
      this.IsEnabled = true;
      this.m_terrainCursor.Activate();
      this.m_topCircleRenderer.Show();
      this.m_bottomCircleRenderer.Show();
      this.m_toolbox.Show();
      this.m_gameLoop.SyncUpdate.AddNonSaveable<XRayTool>(this, new Action<GameTime>(this.syncUpdate));
    }

    public void Deactivate()
    {
      if (!this.IsEnabled)
        return;
      this.IsEnabled = false;
      this.m_terrainCursor.Deactivate();
      this.m_terrainRenderer.DisableXRay();
      this.m_toolbox.Hide();
      this.m_topCircleRenderer.Hide();
      this.m_bottomCircleRenderer.Hide();
    }

    private bool? onUp()
    {
      if (this.m_depthOffset.Value >= this.m_radius.Value / 2)
        return new bool?(false);
      this.m_depthOffset += ThicknessTilesI.One;
      return new bool?(true);
    }

    private bool? onDown()
    {
      this.m_depthOffset -= ThicknessTilesI.One;
      return new bool?(true);
    }

    public bool InputUpdate(IInputScheduler inputScheduler)
    {
      if (!UnityEngine.Input.GetKey(KeyCode.LeftControl) && !UnityEngine.Input.GetKey(KeyCode.RightControl))
        return false;
      this.m_depthOffset += (-10f * UnityEngine.Input.GetAxis("MouseScroll")).CeilToInt().TilesThick();
      this.m_depthOffset = this.m_depthOffset.Min((this.m_radius.Value / 2).TilesThick());
      return true;
    }

    private void syncUpdate(GameTime time)
    {
      if (!this.IsEnabled)
      {
        int num = this.m_radius.Value.Squared();
        for (int index1 = -this.m_radius.Value; index1 <= this.m_radius.Value; ++index1)
        {
          for (int index2 = -this.m_radius.Value; index2 <= this.m_radius.Value; ++index2)
          {
            if (index2.Squared() + index1.Squared() <= num)
            {
              Tile2i tile2i = this.m_oldXRayPos + new RelTile2i(index2, index1);
              if (!this.m_terrainManager.IsOffLimitsOrInvalid(this.m_oldXRayPos))
                this.m_terrainRenderer.TileChanged(tile2i.ExtendIndex(this.m_terrainManager));
            }
          }
        }
        this.m_gameLoop.SyncUpdate.RemoveNonSaveable<XRayTool>(this, new Action<GameTime>(this.syncUpdate));
      }
      else
      {
        Tile2i tile2i = this.m_terrainCursor.Tile2i;
        if (!(tile2i != this.m_oldXRayPos) && !(this.m_oldDepthOffset != this.m_depthOffset))
          return;
        HeightTilesF height1 = this.m_terrainManager.GetHeight(tile2i);
        HeightTilesF height2 = height1 + (this.m_depthOffset.Value - XRayTool.TOOL_BASE_DEPTH.Value).TilesHigh();
        this.m_topCircleRenderer.SetCircle(tile2i.CornerTile2f, XRayTool.TOOL_OUTER_RADIUS, height1);
        this.m_bottomCircleRenderer.SetCircle(tile2i.CornerTile2f, XRayTool.TOOL_INNER_RADIUS, height2);
        this.m_terrainRenderer.SetXRayData(this.m_terrainCursor.Tile2i, this.m_radius, this.m_depthOffset);
        int num = this.m_radius.Value.Squared();
        for (int index3 = -this.m_radius.Value; index3 <= this.m_radius.Value; ++index3)
        {
          for (int index4 = -this.m_radius.Value; index4 <= this.m_radius.Value; ++index4)
          {
            if (index4.Squared() + index3.Squared() <= num)
            {
              Tile2i coord = this.m_oldXRayPos + new RelTile2i(index4, index3);
              if (!this.m_terrainManager.IsOffLimitsOrInvalid(this.m_oldXRayPos))
                this.m_terrainRenderer.TileChanged(coord.ExtendIndex(this.m_terrainManager));
              coord = this.m_terrainCursor.Tile2i + new RelTile2i(index4, index3);
              if (!this.m_terrainManager.IsOffLimitsOrInvalid(coord))
                this.m_terrainRenderer.TileChanged(coord.ExtendIndex(this.m_terrainManager));
            }
          }
        }
        this.m_oldXRayPos = this.m_terrainCursor.Tile2i;
        this.m_oldDepthOffset = this.m_depthOffset;
      }
    }

    static XRayTool()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      XRayTool.TOOL_OUTER_RADIUS = new RelTile1i(20);
      XRayTool.TOOL_INNER_RADIUS = new RelTile1i(15);
      XRayTool.TOOL_BASE_DEPTH = new ThicknessTilesI(10);
    }
  }
}
