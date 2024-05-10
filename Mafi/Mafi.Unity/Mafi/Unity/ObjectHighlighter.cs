// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.ObjectHighlighter
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Unity.Camera;
using Mafi.Unity.StandardAssets.Effects.ImageEffects.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class ObjectHighlighter : IDisposable
  {
    private readonly Dict<GameObject, ObjectHighlighter.HighlightedObject> m_highlightedObjects;
    private readonly Set<GameObject> m_ignoredObjects;
    private bool m_highlightedObjectsChanged;
    private bool m_ignoredObjectsChanged;
    private readonly ObjectPool<ObjectHighlighter.HighlightedObject> m_highlightedPool;
    private readonly Set<ICustomHighlightsRenderer> m_customRenderers;
    private readonly ObjectHighlighter.ObjectHighlighterMb m_highlighterMb;

    public IEnumerable<GameObject> HighlightedObjects
    {
      get => (IEnumerable<GameObject>) this.m_highlightedObjects.Keys;
    }

    public bool HasObjectsToHighlight
    {
      get => this.m_highlightedObjects.Count + this.m_customRenderers.Count > 0;
    }

    public bool IsActive { get; private set; }

    public static ObjectHighlighter CreateForCamera(
      UnityEngine.Camera camera,
      AssetsDb assetsDb,
      float blurSize = 5f,
      int blurDownsample = 2,
      int blurIterations = 1)
    {
      return new ObjectHighlighter(camera, assetsDb, blurSize, blurDownsample, blurIterations);
    }

    /// <summary>
    /// The primary constructor is private to allow special ctor for dependency resolver. Use static factory method
    /// <see cref="M:Mafi.Unity.ObjectHighlighter.CreateForCamera(UnityEngine.Camera,Mafi.Unity.AssetsDb,System.Single,System.Int32,System.Int32)" /> to construct this object.
    /// </summary>
    private ObjectHighlighter(
      UnityEngine.Camera camera,
      AssetsDb assetsDb,
      float blurSize = 5f,
      int blurDownsample = 2,
      int blurIterations = 1)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_highlightedObjects = new Dict<GameObject, ObjectHighlighter.HighlightedObject>();
      this.m_ignoredObjects = new Set<GameObject>();
      // ISSUE: reference to a compiler-generated field
      this.\u003CIsActive\u003Ek__BackingField = true;
      this.m_highlightedPool = new ObjectPool<ObjectHighlighter.HighlightedObject>(32, (Func<ObjectHighlighter.HighlightedObject>) (() => new ObjectHighlighter.HighlightedObject()), (Action<ObjectHighlighter.HighlightedObject>) (x => { }));
      this.m_customRenderers = new Set<ICustomHighlightsRenderer>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Shader sharedAssetOrThrow1 = assetsDb.GetSharedAssetOrThrow<Shader>("Assets/Shaders/ObjectHighlight.shader");
      Shader sharedAssetOrThrow2 = assetsDb.GetSharedAssetOrThrow<Shader>("Assets/Shaders/BlurFast.shader");
      this.m_highlighterMb = camera.gameObject.AddComponent<ObjectHighlighter.ObjectHighlighterMb>();
      this.m_highlighterMb.Initialize(this, sharedAssetOrThrow1, sharedAssetOrThrow2, blurSize, blurDownsample, blurIterations);
    }

    public ObjectHighlighter(CameraController camera, AssetsDb assetsDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      this.\u002Ector(camera.Camera, assetsDb);
    }

    public void Dispose() => this.m_highlighterMb.Dispose();

    public void ForceSetActive(bool isActive) => this.IsActive = isActive;

    public void Highlight(ObjectHighlightSpec highlightSpec)
    {
      this.Highlight(highlightSpec.Go, highlightSpec.Color);
    }

    /// <summary>
    /// Highlights given game object. Multiple overlay colors are supported and are added. Each color is added and
    /// removed independently.
    /// </summary>
    /// <remarks>
    /// The object will glow with given color and itself it will be covered by color intensity equal to transparency
    /// of the color (inverted alpha). For example opaque color results in no overlay color, only glow. 25%
    /// transparent color results in 25% overlay of the object and 75% glow.
    /// </remarks>
    public void Highlight(GameObject go, ColorRgba color)
    {
      Assert.That<GameObject>(go).IsValidUnityObject<GameObject>();
      Assert.That<bool>(color.IsEmpty).IsFalse();
      ObjectHighlighter.HighlightedObject highlightedObject;
      if (this.m_highlightedObjects.TryGetValue(go, out highlightedObject))
      {
        if (!highlightedObject.AddColor(color))
          return;
        this.m_highlightedObjectsChanged = true;
      }
      else
      {
        ObjectHighlighter.HighlightedObject pooledInstance = ObjectHighlighter.HighlightedObject.GetPooledInstance(this.m_highlightedPool, color, go);
        this.m_highlightedObjects[go] = pooledInstance;
        this.m_highlightedObjectsChanged = true;
      }
    }

    /// <summary>
    /// Indicates that the object has changed in some way so we may need to re-highlight.
    /// </summary>
    public void SetHasChanged(GameObject go)
    {
      Assert.That<GameObject>(go).IsValidUnityObject<GameObject>();
      if (!this.m_highlightedObjects.ContainsKey(go))
        return;
      this.m_highlightedObjectsChanged = true;
    }

    public void HighlightNewAndAssert(GameObject go, ColorRgba color)
    {
      Assert.That<GameObject>(go).IsValidUnityObject<GameObject>();
      Assert.That<bool>(color.IsEmpty).IsFalse();
      ObjectHighlighter.HighlightedObject highlightedObject;
      if (this.m_highlightedObjects.TryRemove(go, out highlightedObject))
      {
        Assert.Fail<GameObject>("GO '{0}' was already highlighted.", go);
        highlightedObject.ClearAndReturnToPool();
      }
      ObjectHighlighter.HighlightedObject pooledInstance = ObjectHighlighter.HighlightedObject.GetPooledInstance(this.m_highlightedPool, color, go);
      this.m_highlightedObjects[go] = pooledInstance;
      this.m_highlightedObjectsChanged = true;
    }

    public void RemoveHighlight(ObjectHighlightSpec highlightSpec)
    {
      this.RemoveHighlight(highlightSpec.Go, highlightSpec.Color);
    }

    /// <summary>
    /// Removes highlight color from given <see cref="T:UnityEngine.GameObject" />.
    /// </summary>
    public void RemoveHighlight(GameObject go, ColorRgba color)
    {
      ObjectHighlighter.HighlightedObject highlightedObject;
      if (!this.m_highlightedObjects.TryGetValue(go, out highlightedObject) || !highlightedObject.RemoveColor(color))
        return;
      if (!highlightedObject.HasColor)
      {
        highlightedObject.ClearAndReturnToPool();
        this.m_highlightedObjects.Remove(go);
      }
      this.m_highlightedObjectsChanged = true;
    }

    public void RemoveHighlightAndAssert(GameObject go, ColorRgba color)
    {
      ObjectHighlighter.HighlightedObject highlightedObject;
      if (!this.m_highlightedObjects.TryGetValue(go, out highlightedObject))
        Assert.Fail<string>("Failed to remove highlight from GO '{0}'.", go.name);
      else if (!highlightedObject.RemoveColor(color))
      {
        Assert.Fail<ColorRgba, string>("Failed to remove highlight color {0} from GO '{1}'.", color, go.name);
      }
      else
      {
        if (!highlightedObject.HasColor)
        {
          highlightedObject.ClearAndReturnToPool();
          this.m_highlightedObjects.Remove(go);
        }
        this.m_highlightedObjectsChanged = true;
      }
    }

    /// <summary>
    /// Removes all highlighted colors from given <see cref="T:UnityEngine.GameObject" />.
    /// </summary>
    public void RemoveAllHighlights(GameObject go)
    {
      ObjectHighlighter.HighlightedObject highlightedObject;
      if (!this.m_highlightedObjects.TryRemove(go, out highlightedObject))
        return;
      highlightedObject.ClearAndReturnToPool();
      this.m_highlightedObjectsChanged = true;
    }

    private void validateHighlightedObjects()
    {
      this.m_highlightedObjectsChanged |= this.m_highlightedObjects.RemoveKeys((Predicate<GameObject>) (go => go.IsNullOrDestroyed()), (Action<KeyValuePair<GameObject, ObjectHighlighter.HighlightedObject>>) (kvp => kvp.Value.ClearAndReturnToPool())) > 0;
    }

    public void AddIgnoreHighlight(GameObject go)
    {
      Assert.That<GameObject>(go).IsValidUnityObject<GameObject>();
      this.m_ignoredObjectsChanged |= this.m_ignoredObjects.Add(go);
    }

    public void RemoveIgnoreHighlight(GameObject go)
    {
      Assert.That<GameObject>(go).IsValidUnityObject<GameObject>();
      this.m_ignoredObjectsChanged |= this.m_ignoredObjects.Remove(go);
    }

    public void AddCustomHighlightsRenderer(ICustomHighlightsRenderer value)
    {
      this.m_customRenderers.AddAndAssertNew(value);
      this.m_highlightedObjectsChanged = true;
    }

    public void RemoveCustomHighlightsRenderer(ICustomHighlightsRenderer value)
    {
      this.m_customRenderers.RemoveAndAssert(value);
      this.m_highlightedObjectsChanged = true;
    }

    private void validateIgnoredObjects()
    {
      this.m_ignoredObjectsChanged |= this.m_ignoredObjects.RemoveWhere((Predicate<GameObject>) (go => go.IsNullOrDestroyed())) > 0;
    }

    private class HighlightedObject
    {
      /// <summary>
      /// Helper list for allocation-free retrieval of renderers.
      /// </summary>
      private static readonly List<Renderer> s_renderersTmp;
      private ColorRgba m_color;
      public readonly Lyst<Renderer> Renderers;
      /// <summary>
      /// List that stores colors if more that one color is used for highlight. We create the list only if two or
      /// more colors are needed since more than one color is very rare.
      /// </summary>
      private Option<Lyst<ColorRgba>> m_colors;
      private ObjectPool<ObjectHighlighter.HighlightedObject> m_sourcePool;

      public bool HasColor => this.m_color.IsNotEmpty;

      public Color Color { get; private set; }

      public HighlightedObject()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Renderers = new Lyst<Renderer>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }

      public static ObjectHighlighter.HighlightedObject GetPooledInstance(
        ObjectPool<ObjectHighlighter.HighlightedObject> pool,
        ColorRgba color,
        GameObject go)
      {
        ObjectHighlighter.HighlightedObject instance = pool.GetInstance();
        Assert.That<int>(instance.Renderers.Count).IsZero();
        instance.m_sourcePool = pool;
        instance.m_color = color;
        instance.Color = color.ToColor();
        Assert.That<List<Renderer>>(ObjectHighlighter.HighlightedObject.s_renderersTmp).IsEmpty<Renderer>();
        go.GetComponentsInChildren<Renderer>(ObjectHighlighter.HighlightedObject.s_renderersTmp);
        Assert.That<int>(ObjectHighlighter.HighlightedObject.s_renderersTmp.Count).IsPositive("Trying to highlight object with no renderers ('" + go.name + "').");
        foreach (Renderer renderer in ObjectHighlighter.HighlightedObject.s_renderersTmp)
        {
          if (!renderer.gameObject.HasTag(UnityTag.NoHi) && !renderer.gameObject.HasTag(UnityTag.NoOnlyHi))
            instance.Renderers.Add(renderer);
        }
        ObjectHighlighter.HighlightedObject.s_renderersTmp.Clear();
        return instance;
      }

      public bool AddColor(ColorRgba color)
      {
        if (this.m_colors.HasValue)
        {
          if (this.m_colors.Value.Contains(color))
            return false;
          this.m_colors.Value.Add(color);
          this.recomputeCombinedColor();
          return true;
        }
        if (this.m_color == color)
          return false;
        this.m_colors = (Option<Lyst<ColorRgba>>) new Lyst<ColorRgba>(4, true);
        this.m_colors.Value.Add(this.m_color, color);
        this.recomputeCombinedColor();
        return true;
      }

      /// <summary>
      /// Tries to remove given highlighted color. Returns true when color was removed.
      /// </summary>
      public bool RemoveColor(ColorRgba color)
      {
        if (this.m_colors.HasValue)
        {
          if (!this.m_colors.Value.Contains(color))
            return false;
          this.m_colors.Value.Remove(color);
          if (this.m_colors.Value.IsEmpty)
          {
            this.m_colors = Option<Lyst<ColorRgba>>.None;
            this.m_color = ColorRgba.Empty;
          }
          else
            this.recomputeCombinedColor();
          return true;
        }
        if (!(this.m_color == color))
          return false;
        this.m_color = ColorRgba.Empty;
        return true;
      }

      private void recomputeCombinedColor()
      {
        Assert.That<Option<Lyst<ColorRgba>>>(this.m_colors).HasValue<Lyst<ColorRgba>>();
        Assert.That<Lyst<ColorRgba>>(this.m_colors.Value).IsNotEmpty<ColorRgba>();
        if (this.m_colors.Value.Count == 1)
        {
          this.Color = this.m_colors.Value.First.ToColor();
        }
        else
        {
          Color color = new Color();
          foreach (ColorRgba c in this.m_colors.Value)
            color += c.ToColor();
          color.r = Math.Min(1f, color.r);
          color.g = Math.Min(1f, color.g);
          color.b = Math.Min(1f, color.b);
          color.a = Math.Min(1f, color.a / (float) this.m_colors.Value.Count);
          this.Color = color;
        }
      }

      public void ClearAndReturnToPool()
      {
        ObjectPool<ObjectHighlighter.HighlightedObject> sourcePool = this.m_sourcePool;
        this.clear();
        sourcePool.ReturnInstance(this);
      }

      private void clear()
      {
        this.Color = new Color();
        this.m_colors = Option<Lyst<ColorRgba>>.None;
        this.Renderers.Clear();
        this.m_sourcePool = (ObjectPool<ObjectHighlighter.HighlightedObject>) null;
      }

      static HighlightedObject()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        ObjectHighlighter.HighlightedObject.s_renderersTmp = new List<Renderer>(6);
      }
    }

    public class ObjectHighlighterMb : MonoBehaviour, IDisposable
    {
      private static readonly List<Renderer> s_renderersTmp;
      private Option<ObjectHighlighter> m_highlighter;
      private CommandBuffer m_renderBuffer;
      private CommandBuffer m_ignoredRenderBuffer;
      private Material m_highlightMaterial;
      private BlurOptimized m_blur;
      private RenderTexture m_highlightTexture;

      public void Initialize(
        ObjectHighlighter highlighter,
        Shader highlightShader,
        Shader blurShader,
        float blurSize,
        int blurDownsample,
        int blurIterations)
      {
        this.m_highlighter = (Option<ObjectHighlighter>) highlighter;
        this.m_highlightMaterial = new Material(highlightShader.CheckNotNull<Shader>());
        Assert.That<int>(this.m_highlightMaterial.passCount).IsGreaterOrEqual(2);
        this.m_blur = this.gameObject.AddComponent<BlurOptimized>();
        this.m_blur.Initialize(blurShader);
        this.m_blur.enabled = false;
        this.m_blur.BlurIterations = blurIterations;
        this.m_blur.DownsamplePasses = blurDownsample;
        this.m_blur.BlurSize = blurSize;
        this.m_renderBuffer = new CommandBuffer();
        this.m_ignoredRenderBuffer = new CommandBuffer();
        this.m_highlightTexture = new RenderTexture(1, 1, 0);
      }

      public void Dispose() => this.m_highlightTexture.DestroyIfNotNull();

      [PublicAPI]
      public void OnRenderImage(RenderTexture source, RenderTexture destination)
      {
        if (this.m_highlighter.IsNone)
        {
          Log.Error("No highlighter found");
        }
        else
        {
          ObjectHighlighter objectHighlighter = this.m_highlighter.Value;
          if (!objectHighlighter.HasObjectsToHighlight || !objectHighlighter.IsActive)
            Graphics.Blit((Texture) source, destination);
          else if (!(bool) (UnityEngine.Object) this.m_highlightTexture)
          {
            Log.Error("Highlight texture is null or destroyed");
            Graphics.Blit((Texture) source, destination);
          }
          else if (!(bool) (UnityEngine.Object) this.m_blur)
          {
            Log.Error("Blue component is null or destroyed");
            Graphics.Blit((Texture) source, destination);
          }
          else
          {
            int width = source.width;
            int height = source.height;
            if (this.m_highlightTexture.width != width || this.m_highlightTexture.height != height)
            {
              this.m_highlightTexture.Release();
              this.m_highlightTexture = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32);
              this.m_highlightTexture.Create();
              objectHighlighter.m_highlightedObjectsChanged = true;
            }
            this.renderHighlights(this.m_highlightTexture, objectHighlighter.m_highlightedObjectsChanged);
            objectHighlighter.m_highlightedObjectsChanged = false;
            RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.ARGB32);
            this.m_blur.OnRenderImage(this.m_highlightTexture, temporary);
            this.renderIgnoredHighlights(this.m_highlightTexture);
            this.m_highlightMaterial.SetTexture("_MaskTex", (Texture) this.m_highlightTexture);
            this.m_highlightMaterial.SetTexture("_BlurredMaskTex", (Texture) temporary);
            Graphics.Blit((Texture) source, destination, this.m_highlightMaterial, 1);
            RenderTexture.ReleaseTemporary(temporary);
          }
        }
      }

      private void renderHighlights(RenderTexture targetTexture, bool recomputeRenderBuffer)
      {
        if (this.m_highlighter.IsNone)
        {
          Log.Error("No highlighter found");
        }
        else
        {
          ObjectHighlighter objectHighlighter = this.m_highlighter.Value;
          objectHighlighter.validateHighlightedObjects();
          foreach (ICustomHighlightsRenderer customRenderer in objectHighlighter.m_customRenderers)
            recomputeRenderBuffer |= customRenderer.UpdateDataIfNeeded();
          if (recomputeRenderBuffer)
          {
            this.m_renderBuffer.Clear();
            this.m_renderBuffer.SetRenderTarget(new RenderTargetIdentifier((Texture) targetTexture));
            foreach (ICustomHighlightsRenderer customRenderer in objectHighlighter.m_customRenderers)
              customRenderer.RegisterRendering(this.m_renderBuffer);
            Color color = Color.clear;
            foreach (KeyValuePair<GameObject, ObjectHighlighter.HighlightedObject> highlightedObject in objectHighlighter.m_highlightedObjects)
            {
              if (color != highlightedObject.Value.Color)
              {
                color = highlightedObject.Value.Color;
                this.m_renderBuffer.SetGlobalColor("_Color", color);
              }
              foreach (Renderer renderer in highlightedObject.Value.Renderers)
              {
                if (!((UnityEngine.Object) renderer == (UnityEngine.Object) null) && renderer.gameObject.activeInHierarchy)
                  this.m_renderBuffer.DrawRenderer(renderer, this.m_highlightMaterial, 0, 0);
              }
            }
          }
          RenderTexture.active = targetTexture;
          GL.Clear(true, true, Color.clear);
          Graphics.ExecuteCommandBuffer(this.m_renderBuffer);
          RenderTexture.active = (RenderTexture) null;
        }
      }

      private void renderIgnoredHighlights(RenderTexture targetTexture)
      {
        if (this.m_highlighter.IsNone)
        {
          Log.Error("No highlighter found");
        }
        else
        {
          this.m_highlighter.Value.validateIgnoredObjects();
          if (this.m_highlighter.Value.m_ignoredObjectsChanged)
          {
            this.m_ignoredRenderBuffer.Clear();
            this.m_ignoredRenderBuffer.SetRenderTarget(new RenderTargetIdentifier((Texture) targetTexture));
            Assert.That<List<Renderer>>(ObjectHighlighter.ObjectHighlighterMb.s_renderersTmp).IsEmpty<Renderer>();
            foreach (GameObject ignoredObject in this.m_highlighter.Value.m_ignoredObjects)
            {
              ignoredObject.GetComponentsInChildren<Renderer>(ObjectHighlighter.ObjectHighlighterMb.s_renderersTmp);
              foreach (Renderer renderer in ObjectHighlighter.ObjectHighlighterMb.s_renderersTmp)
                this.m_renderBuffer.DrawRenderer(renderer, renderer.material);
            }
            ObjectHighlighter.ObjectHighlighterMb.s_renderersTmp.Clear();
          }
          RenderTexture.active = targetTexture;
          Graphics.ExecuteCommandBuffer(this.m_ignoredRenderBuffer);
          RenderTexture.active = (RenderTexture) null;
        }
      }

      public ObjectHighlighterMb()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }

      static ObjectHighlighterMb()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        ObjectHighlighter.ObjectHighlighterMb.s_renderersTmp = new List<Renderer>(6);
      }
    }
  }
}
