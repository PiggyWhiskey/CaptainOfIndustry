// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Vehicles.TracksAnimator
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Vehicles
{
  internal class TracksAnimator
  {
    private static readonly int TEX_OFFSET_SHADER_ID;
    private readonly Renderer m_leftRenderer;
    private readonly Renderer m_rightRenderer;
    private float m_leftUvOffset;
    private float m_rightUvOffset;
    private float m_leftNextUvOffset;
    private float m_rightNextUvOffset;
    private readonly float m_speedToUv;
    private readonly float m_degreesToUv;
    private readonly MaterialPropertyBlock m_leftMaterialBlock;
    private readonly MaterialPropertyBlock m_rightMaterialBlock;
    private bool m_tracksMoved;

    /// <param name="spacingBetweenTracksM">Distance between tracks. This is used to convert angular velocity to track movement.</param>
    /// <param name="textureLengthM">How many meters is one texture length of the track.</param>
    public TracksAnimator(
      Transform baseTransform,
      string leftTrackObjectPath,
      string rightTrackObjectPath,
      float spacingBetweenTracksM,
      float textureLengthM)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Transform resultTransform1;
      if (baseTransform.TryFindChild(leftTrackObjectPath, out resultTransform1))
      {
        this.m_leftRenderer = (Renderer) resultTransform1.gameObject.GetComponentOrCreateNewAndLogError<MeshRenderer>();
      }
      else
      {
        Log.Error("Failed to find left track object at '" + leftTrackObjectPath + "'.");
        this.m_leftRenderer = (Renderer) new GameObject("TrackDummy").AddComponent<MeshRenderer>();
      }
      Transform resultTransform2;
      if (baseTransform.TryFindChild(rightTrackObjectPath, out resultTransform2))
      {
        this.m_rightRenderer = (Renderer) resultTransform2.gameObject.GetComponentOrCreateNewAndLogError<MeshRenderer>();
      }
      else
      {
        Log.Error("Failed to find right track object at '" + rightTrackObjectPath + "'.");
        this.m_rightRenderer = (Renderer) new GameObject("TrackDummy").AddComponent<MeshRenderer>();
      }
      this.m_speedToUv = 1f / textureLengthM;
      this.m_degreesToUv = (float) (3.1415927410125732 * (double) spacingBetweenTracksM / 360.0) * this.m_speedToUv;
      this.m_leftMaterialBlock = new MaterialPropertyBlock();
      this.m_rightMaterialBlock = new MaterialPropertyBlock();
    }

    /// <summary>
    /// Updates position of the tracks using speed of the vehicle they belong to.
    /// </summary>
    public void Sync(GameTime time, RelTile1f speed, AngleDegrees1f steeringSpeed)
    {
      if (time.IsGamePaused || speed.IsZero && steeringSpeed.IsZero)
      {
        this.m_tracksMoved = false;
      }
      else
      {
        float num1 = speed.ToUnityUnits() * this.m_speedToUv;
        float num2 = steeringSpeed.Degrees.ToFloat() * this.m_degreesToUv;
        this.m_tracksMoved = true;
        this.m_leftUvOffset = this.m_leftNextUvOffset;
        this.m_leftNextUvOffset += (num1 + num2) * (float) time.GameSpeedMult;
        if ((double) this.m_leftNextUvOffset > 1024.0)
        {
          this.m_leftUvOffset -= 1024f;
          this.m_leftNextUvOffset -= 1024f;
        }
        this.m_rightUvOffset = this.m_rightNextUvOffset;
        this.m_rightNextUvOffset += (num1 - num2) * (float) time.GameSpeedMult;
        if ((double) this.m_rightNextUvOffset <= 1024.0)
          return;
        this.m_rightUvOffset -= 1024f;
        this.m_rightNextUvOffset -= 1024f;
      }
    }

    public void RenderUpdate(GameTime time)
    {
      if (!this.m_tracksMoved)
        return;
      this.m_leftMaterialBlock.SetFloat(TracksAnimator.TEX_OFFSET_SHADER_ID, this.m_leftUvOffset.Lerp(this.m_leftNextUvOffset, time.AbsoluteT));
      this.m_leftRenderer.SetPropertyBlock(this.m_leftMaterialBlock);
      this.m_rightMaterialBlock.SetFloat(TracksAnimator.TEX_OFFSET_SHADER_ID, this.m_rightUvOffset.Lerp(this.m_rightNextUvOffset, time.AbsoluteT));
      this.m_rightRenderer.SetPropertyBlock(this.m_rightMaterialBlock);
    }

    static TracksAnimator()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      TracksAnimator.TEX_OFFSET_SHADER_ID = Shader.PropertyToID("_TexOffset");
    }
  }
}
