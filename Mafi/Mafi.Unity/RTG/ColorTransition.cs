// Decompiled with JetBrains decompiler
// Type: RTG.ColorTransition
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class ColorTransition
  {
    private ColorRef _colorRef;
    private Color _fadeInColor;
    private Color _fadeOutColor;
    private ColorTransition.State _state;
    private float _durationInSeconds;
    private float _elapsedTimeInSeconds;
    private bool _isActive;

    public event ColorTransition.ColorTransitionBeginHandler TransitionBegin;

    public event ColorTransition.ColorTransitionEndHandler TransitionEnd;

    public ColorTransition.State TransitionState => this._state;

    public Color FadeInColor
    {
      get => this._fadeInColor;
      set => this._fadeInColor = value;
    }

    public Color FadeOutColor
    {
      get => this._fadeOutColor;
      set => this._fadeOutColor = value;
    }

    public float DurationInSeconds
    {
      get => this._durationInSeconds;
      set => this._durationInSeconds = Mathf.Max(value, 0.0f);
    }

    public bool IsActive => this._isActive;

    public ColorTransition(ColorRef colorRef)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._state = ColorTransition.State.Ready;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._colorRef = colorRef;
      this._fadeInColor = colorRef.Value;
      this._fadeOutColor = colorRef.Value;
    }

    public void BeginFadeIn(bool startFromCurrentColor)
    {
      if (startFromCurrentColor)
        this.FadeOutColor = this._colorRef.Value;
      this._state = ColorTransition.State.FadingIn;
      this._isActive = true;
      this._elapsedTimeInSeconds = 0.0f;
      if (this.TransitionBegin == null)
        return;
      this.TransitionBegin(this);
    }

    public void BeginFadeOut(bool startFromCurrentColor)
    {
      if (startFromCurrentColor)
        this.FadeInColor = this._colorRef.Value;
      this._state = ColorTransition.State.FadingOut;
      this._isActive = true;
      this._elapsedTimeInSeconds = 0.0f;
      if (this.TransitionBegin == null)
        return;
      this.TransitionBegin(this);
    }

    public void Update(float elapsedTime)
    {
      if (!this._isActive)
        return;
      Color a = this._fadeOutColor;
      Color b = this._fadeInColor;
      if (this._state == ColorTransition.State.FadingOut)
      {
        a = this._fadeInColor;
        b = this._fadeOutColor;
      }
      this._elapsedTimeInSeconds += elapsedTime;
      this._elapsedTimeInSeconds = Mathf.Clamp(this._elapsedTimeInSeconds, 0.0f, this._durationInSeconds);
      float num = this._elapsedTimeInSeconds / this._durationInSeconds;
      if (MathEx.AlmostEqual(num, 1f, 0.0001f))
      {
        this._colorRef.Value = b;
        this.End();
      }
      else
        this._colorRef.Value = Color.Lerp(a, b, num);
    }

    private void End()
    {
      if (!this._isActive)
        return;
      this._isActive = false;
      if (this._state == ColorTransition.State.FadingOut)
        this._state = ColorTransition.State.CompleteFadeOut;
      else if (this._state == ColorTransition.State.FadingIn)
        this._state = ColorTransition.State.CompleteFadeIn;
      if (this.TransitionEnd == null)
        return;
      this.TransitionEnd(this);
    }

    public enum State
    {
      /// <summary>
      /// This is the state the transition exists in after it has
      /// completed a fade-in operation.
      /// </summary>
      CompleteFadeIn,
      /// <summary>
      /// This is the state the transition exists in after it has
      /// completed a fade-out operation.
      /// </summary>
      CompleteFadeOut,
      /// <summary>The transition is fading in.</summary>
      FadingIn,
      /// <summary>The transition is fading out.</summary>
      FadingOut,
      /// <summary>
      /// The transition is in a ready state. This is the state in which the
      /// transition exists when it is created. After a fade is performed,
      /// it can never go back to this state.
      /// </summary>
      Ready,
    }

    public delegate void ColorTransitionBeginHandler(ColorTransition colorTransition);

    public delegate void ColorTransitionEndHandler(ColorTransition colorTransition);
  }
}
