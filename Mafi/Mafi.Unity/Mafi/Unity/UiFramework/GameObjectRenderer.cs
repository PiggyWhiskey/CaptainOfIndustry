// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.GameObjectRenderer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Unity.Camera;
using Mafi.Unity.StandardAssets.Effects.ImageEffects.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework
{
  public class GameObjectRenderer
  {
    private readonly GameObject m_rootGo;
    private readonly UnityEngine.Camera m_mainCamera;
    private readonly float m_fieldOfView;
    private UnityEngine.Camera m_camera;
    private Texture2D m_texture;
    private Texture2D m_finalTexture;
    private RenderTexture m_renderTexture;
    private AngleDegrees1f m_cameraPitch;
    private Vector2f m_cameraDirection;
    private AngleDegrees1f m_modelRotation;
    private Light m_mainLight;
    private ImmutableArray<GameObject> m_sceneLights;
    private bool m_isInitialized;
    private Bloom m_bloom;
    private Vignette m_vignette;

    public GameObjectRenderer(GameObject rootGo, float fieldOfView, UnityEngine.Camera mainCamera)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_rootGo = rootGo.CheckNotNull<GameObject>();
      this.m_mainCamera = mainCamera;
      this.m_fieldOfView = fieldOfView;
    }

    public void SetImageSize(Vector2i imageSize)
    {
      Assert.That<int>(imageSize.X).IsPositive();
      Assert.That<int>(imageSize.Y).IsPositive();
      this.m_texture.Reinitialize(imageSize.X, imageSize.Y);
      this.m_finalTexture.Reinitialize(imageSize.X * 2, imageSize.Y * 2);
    }

    public void SetCamera(
      AngleDegrees1f pitch,
      AngleDegrees1f rotation,
      AngleDegrees1f fieldOfView)
    {
      this.m_cameraPitch = pitch;
      this.m_modelRotation = rotation;
      this.m_camera.fieldOfView = fieldOfView.Degrees.ToFloat();
      this.m_cameraDirection = this.m_cameraPitch.DirectionVector;
    }

    public void SetLight(Color color, AngleDegrees1f pitch, AngleDegrees1f yaw)
    {
      this.m_mainLight.color = color;
      this.m_mainLight.transform.rotation = Quaternion.Euler(pitch.Degrees.ToFloat(), yaw.Degrees.ToFloat(), 0.0f);
    }

    public void SetUpRendering()
    {
      if (this.m_isInitialized)
        throw new InvalidOperationException("Renderer already set up. Forgot to tear down?");
      this.m_sceneLights = ((IEnumerable<Light>) UnityEngine.Object.FindObjectsOfType<Light>()).Select<Light, GameObject>((Func<Light, GameObject>) (x => x.gameObject)).Where<GameObject>((Func<GameObject, bool>) (x => x.gameObject.activeSelf)).ToImmutableArray<GameObject>();
      foreach (GameObject sceneLight in this.m_sceneLights)
        sceneLight.SetActive(false);
      this.m_isInitialized = true;
      Texture2D texture2D1 = new Texture2D(32, 32, TextureFormat.ARGB32, false);
      texture2D1.wrapMode = TextureWrapMode.Clamp;
      texture2D1.filterMode = FilterMode.Bilinear;
      this.m_texture = texture2D1;
      Texture2D texture2D2 = new Texture2D(32, 32, TextureFormat.ARGB32, false);
      texture2D2.wrapMode = TextureWrapMode.Clamp;
      texture2D2.filterMode = FilterMode.Bilinear;
      this.m_finalTexture = texture2D2;
      this.m_renderTexture = new RenderTexture(32, 32, 24, RenderTextureFormat.ARGB32)
      {
        antiAliasing = 8
      };
      this.m_camera = UnityEngine.Object.Instantiate<UnityEngine.Camera>(this.m_mainCamera);
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_camera.GetComponent<ObjectHighlighter.ObjectHighlighterMb>());
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_camera.GetComponent<BlurOptimized>());
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_camera.GetComponent<AudioListener>());
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_camera.GetComponent<GameCameraControllerMb>());
      GameObject result;
      if (this.m_camera.gameObject.TryFindNameInHierarchy("FogQuad", out result))
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) result);
      this.m_camera.transform.SetParent(this.m_rootGo.transform, false);
      this.m_camera.clearFlags = CameraClearFlags.Color;
      this.m_camera.backgroundColor = Color.clear;
      this.m_camera.fieldOfView = this.m_fieldOfView;
      this.m_camera.renderingPath = RenderingPath.Forward;
      this.m_camera.GetComponent<PostProcessLayer>().antialiasingMode = PostProcessLayer.Antialiasing.None;
      this.m_vignette = this.m_camera.GetComponentInChildren<PostProcessVolume>().profile.GetSetting<Vignette>();
      this.m_vignette.enabled.Override(false);
      this.m_mainLight = new GameObject("GoRendererLight").AddComponent<Light>();
      this.m_mainLight.gameObject.SetActive(true);
      this.m_mainLight.gameObject.transform.SetParent(this.m_rootGo.transform, false);
      this.m_mainLight.type = LightType.Directional;
      this.m_mainLight.intensity = 1.1f;
      this.m_mainLight.shadows = LightShadows.Soft;
      this.m_mainLight.renderMode = LightRenderMode.ForcePixel;
    }

    public void TearDownRendering()
    {
      this.m_isInitialized = this.m_isInitialized ? false : throw new InvalidOperationException("Renderer not set up yet.");
      this.m_vignette.enabled.Override(true);
      this.m_vignette = (Vignette) null;
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_camera.GetComponent<PostProcessLayer>());
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_camera.transform.GetChild(0).gameObject);
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_camera.gameObject);
      this.m_camera = (UnityEngine.Camera) null;
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_mainLight.gameObject);
      this.m_mainLight = (Light) null;
      foreach (GameObject sceneLight in this.m_sceneLights)
      {
        if (!(bool) (UnityEngine.Object) sceneLight)
          Log.Error(string.Format("Failed to restore light '{0}'", (object) sceneLight));
        else
          sceneLight.SetActive(true);
      }
    }

    public IEnumerable<int> RenderToPng(GameObject go, string outPath)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<int>) new GameObjectRenderer.\u003CRenderToPng\u003Ed__23(-2)
      {
        \u003C\u003E4__this = this,
        \u003C\u003E3__go = go,
        \u003C\u003E3__outPath = outPath
      };
    }

    /// <summary>
    /// Sets camera distance from given game object. <paramref name="originOffset" /> should be relative offset form
    /// game object's origin at which the camera is pointed.
    /// </summary>
    private void setCameraDistance(GameObject go, Vector3 originOffset, float distance)
    {
      Vector2f vector2f = Fix32.FromFloat(distance) * this.m_cameraDirection;
      this.m_camera.transform.localPosition = originOffset + new Vector3(-vector2f.X.ToFloat(), vector2f.Y.ToFloat(), 0.0f);
      this.m_camera.transform.LookAt(go.transform.position + originOffset);
    }

    /// <summary>Computes bounding box of given game object.</summary>
    private static Bounds getBoundingBox(GameObject go)
    {
      Assert.That<Vector3>(go.transform.lossyScale).IsNotEqualTo<Vector3>(Vector3.zero);
      Renderer[] componentsInChildren = go.GetComponentsInChildren<Renderer>();
      if (componentsInChildren.Length == 0)
        throw new InvalidOperationException("Failed to render GameObject '" + go.name + "', no component of type `Renderer` was found.");
      bool flag = true;
      Bounds boundingBox = new Bounds();
      foreach (Renderer renderer in componentsInChildren)
      {
        Bounds bounds = renderer.bounds;
        if (!bounds.center.IsFinite() || !bounds.size.IsFinite())
          Assert.Fail(string.Format("GO '{0}' has not finite bounds: {1}.", (object) go.name, (object) bounds));
        else if (flag)
        {
          flag = false;
          boundingBox = bounds;
        }
        else
          boundingBox.Encapsulate(bounds);
      }
      Assert.That<bool>(flag).IsFalse("No finite bounds found!");
      Assert.That<Vector3>(boundingBox.center).IsFinite();
      Assert.That<Vector3>(boundingBox.size).IsFinite();
      return boundingBox;
    }

    /// <summary>Renders current scene to a given texture.</summary>
    private void renderTo(Texture2D texture)
    {
      if (texture.width != this.m_renderTexture.width || texture.height != this.m_renderTexture.height)
      {
        UnityEngine.Object.Destroy((UnityEngine.Object) this.m_renderTexture);
        this.m_renderTexture = new RenderTexture(texture.width, texture.height, 32, RenderTextureFormat.ARGB32);
      }
      this.m_camera.aspect = (float) texture.width / (float) texture.height;
      this.m_camera.targetTexture = this.m_renderTexture;
      this.m_camera.Render();
      this.m_camera.targetTexture = (RenderTexture) null;
      RenderTexture.active = this.m_renderTexture;
      texture.ReadPixels(new Rect(0.0f, 0.0f, (float) this.m_renderTexture.width, (float) this.m_renderTexture.height), 0, 0);
      texture.Apply();
      RenderTexture.active = (RenderTexture) null;
    }

    /// <summary>
    /// Performs binary search of camera distance to find smallest distance such that all pixels on the image
    /// boundary are still not filled.
    /// </summary>
    private IEnumerable<int> findOptimalDistance(
      GameObject go,
      Vector3 originOffset,
      float initialDistance,
      float step,
      float distEpsilon,
      string outPath,
      Action<float> setOptimalDistance)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<int>) new GameObjectRenderer.\u003CfindOptimalDistance\u003Ed__27(-2)
      {
        \u003C\u003E4__this = this,
        \u003C\u003E3__go = go,
        \u003C\u003E3__originOffset = originOffset,
        \u003C\u003E3__initialDistance = initialDistance,
        \u003C\u003E3__step = step,
        \u003C\u003E3__distEpsilon = distEpsilon,
        \u003C\u003E3__outPath = outPath,
        \u003C\u003E3__setOptimalDistance = setOptimalDistance
      };
    }

    /// <summary>
    /// Returns whether given texture has non-empty pixel on the boundary. Empty pixel is defined as transparent
    /// black.
    /// </summary>
    private static bool hasNonemptyPixelOnBoundary(Texture2D texture)
    {
      Color32[] pixels32 = texture.GetPixels32();
      int num = (texture.height - 1) * texture.width;
      for (int index = 0; index < texture.width; ++index)
      {
        if (pixels32[index].a >= (byte) 2 || pixels32[num + index].a >= (byte) 2)
          return true;
      }
      for (int index = 0; index < texture.height; ++index)
      {
        if (pixels32[index * texture.width].a >= (byte) 2 || pixels32[(index + 1) * texture.width - 1].a >= (byte) 2)
          return true;
      }
      return false;
    }

    private IEnumerable<int> findOptimalCameraOffset(
      GameObject go,
      Vector3 originOffset,
      float distance,
      float step,
      float minStep,
      string outPath,
      Action<Vector3> setOptimalOffset)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<int>) new GameObjectRenderer.\u003CfindOptimalCameraOffset\u003Ed__29(-2)
      {
        \u003C\u003E4__this = this,
        \u003C\u003E3__go = go,
        \u003C\u003E3__originOffset = originOffset,
        \u003C\u003E3__distance = distance,
        \u003C\u003E3__step = step,
        \u003C\u003E3__minStep = minStep,
        \u003C\u003E3__outPath = outPath,
        \u003C\u003E3__setOptimalOffset = setOptimalOffset
      };
    }

    private static Vector2i computeTextureClearance(Texture2D texture)
    {
      Color32[] pixels32 = texture.GetPixels32();
      int num1 = 0;
      for (int index1 = 0; index1 < texture.height / 2; ++index1)
      {
        int num2 = index1 * texture.width;
        for (int index2 = 0; index2 < texture.width; ++index2)
        {
          if (pixels32[index2 + num2].a >= (byte) 2)
          {
            index1 = texture.height;
            break;
          }
        }
        ++num1;
      }
      int num3 = (texture.height - 1) * texture.width;
      int num4 = 0;
      for (int index3 = 0; index3 < texture.height / 2; ++index3)
      {
        int num5 = num3 - index3 * texture.width;
        for (int index4 = 0; index4 < texture.width; ++index4)
        {
          if (pixels32[index4 + num5].a >= (byte) 2)
          {
            index3 = texture.height;
            break;
          }
        }
        ++num4;
      }
      int num6 = 0;
      for (int index5 = 0; index5 < texture.width / 2; ++index5)
      {
        for (int index6 = 0; index6 < texture.height; ++index6)
        {
          if (pixels32[index5 + index6 * texture.width].a >= (byte) 2)
          {
            index5 = texture.width;
            break;
          }
        }
        ++num6;
      }
      int num7 = 0;
      for (int index7 = 0; index7 < texture.width / 2; ++index7)
      {
        int num8 = texture.width - 1 - index7;
        for (int index8 = 0; index8 < texture.height; ++index8)
        {
          if (pixels32[num8 + index8 * texture.width].a >= (byte) 2)
          {
            index7 = texture.width;
            break;
          }
        }
        ++num7;
      }
      return new Vector2i(num6 - num7, num4 - num1);
    }
  }
}
