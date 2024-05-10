// Decompiled with JetBrains decompiler
// Type: Mafi.Core.GameTime
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core
{
  public class GameTime
  {
    public const int DEFAULT_SIM_STEP_DURATION_MS = 100;

    /// <summary>
    /// Absolute time from the start of the game in milliseconds.
    /// 
    /// NOTE: This does not count in the time spent in game pause!
    /// </summary>
    public Fix64 TimeSinceStartMs { get; private set; }

    /// <summary>
    /// Absolute time from the game load. Is not saved and resets every load. This is useful for shaders.
    /// 
    /// NOTE: This does not count in the time spent in game pause!
    /// </summary>
    public Fix64 TimeSinceLoadMs { get; private set; }

    public Fix64 TotalElapsedSeconds => this.TimeSinceStartMs / 1000;

    /// <summary>Total number of sim steps.</summary>
    public int SimStepsCount { get; private set; }

    public int SimStepsSinceLoad { get; private set; }

    /// <summary>
    /// Total number of elapsed sim ticks from game start. This includes fractions.
    /// </summary>
    public Fix64 TotalElapsedSimStepsSmooth => this.SimStepsCount + this.AbsoluteT.ToFix64();

    /// <summary>
    /// Approximate number of sim steps that passed during last frame. This is not affected by sped mult.
    /// </summary>
    public Percent DeltaSimStepsApprox { get; private set; }

    /// <summary>
    /// Time from the last sim update in milliseconds. Can be zero if the sim update occurred right at the instant of
    /// the frame.
    /// </summary>
    public Fix32 TimeSinceLastSimUpdateMs { get; private set; }

    /// <summary>Duration of the last frame in milliseconds.</summary>
    public float DeltaTimeMs { get; private set; }

    public float FrameTimeSec => this.DeltaTimeMs * (1f / 1000f);

    /// <summary>
    /// Absolute interpolation parameter that represents time between last two sim updates. Values are from 0 to 1.
    /// Intended usage:
    /// <code>
    /// // Interpolation:
    /// float value = MafiMath.Lerp(lastState, currentState, AbsoluteT);
    /// 
    /// // On update-state:
    /// lastState = currentState;
    /// currentState = newState;
    /// </code>
    /// Advantage of this interpolation is that is does not have to store current value, it is directly computed.
    /// However, your implementation have to store previous simulation state, current (latest) simulation state, and
    /// those two can produce interpolated value (that is usually stored, too).
    /// 
    /// On the other hand <see cref="P:Mafi.Core.GameTime.RelativeT" /> does not require to store previous simulation state.
    /// </summary>
    public float AbsoluteT { get; private set; }

    /// <summary>
    /// Relative interpolation parameter that represents amount of change between last state (not simulation state)
    /// and current simulation state. Values are from 0 to 1. Intended usage:
    /// <code>
    /// // Interpolation:
    /// float value = MafiMath.Lerp(value, currentState, RelativeT);
    /// 
    /// // On update-state:
    /// currentState = newState;
    /// // Do not do: 'value = currentState;' before the assignment of the new state value.
    /// </code>
    /// A big advantage of this interpolation is that you have to store only the last simulation state. Together with
    /// current value you can compute a new value. Unlike with <see cref="P:Mafi.Core.GameTime.AbsoluteT" /> you do not have to keep two
    /// states in order to do the interpolation. If the state is large this can create large savings in memory and
    /// CPU.
    /// 
    /// Please keep in mind that the logic behind this interpolation parameter is smart to automatically adjust the
    /// values to varying lengths of frames and sim updates. Do not set new value on copy-state!
    /// </summary>
    public float RelativeT { get; private set; }

    /// <summary>
    /// Changed amount from last frame. This is difference between last frames and this frames <see cref="P:Mafi.Core.GameTime.AbsoluteT" />. The first frame after sync this is equal to <see cref="P:Mafi.Core.GameTime.AbsoluteT" />.
    /// </summary>
    public float DeltaT { get; private set; }

    /// <summary>
    /// Whether the current simulation is paused. Means that render, input and sync updates are still being processed
    /// including commands but the simulation itself is paused.
    /// </summary>
    public bool IsGamePaused { get; private set; }

    /// <summary>
    /// Current game speed multiplier. This should be for example used as a speed multiplier for all animations.
    /// It's zero during pause.
    /// </summary>
    public int GameSpeedMult { get; private set; }

    /// <summary>Current duration of a sim update.</summary>
    public Fix32 CurrSimUpdateDurationMs { get; private set; }

    internal void Update(
      float deltaTimeMs,
      Fix64 msFromStart,
      Fix64 msFromLoad,
      Fix32 msFromSimUpdate,
      Percent absoluteT,
      Percent relativeT,
      int gameSpeedMult,
      int simStepsCount,
      int simStepsSinceLoad,
      Percent deltaStepsApprox)
    {
      this.IsGamePaused = false;
      this.GameSpeedMult = gameSpeedMult;
      this.DeltaTimeMs = deltaTimeMs;
      this.TimeSinceStartMs = msFromStart;
      this.TimeSinceLoadMs = msFromLoad;
      this.TimeSinceLastSimUpdateMs = msFromSimUpdate;
      this.DeltaT = absoluteT.ToFloat() - this.AbsoluteT;
      if ((double) this.DeltaT < 0.0)
        this.DeltaT = absoluteT.ToFloat();
      this.AbsoluteT = absoluteT.ToFloat();
      this.RelativeT = relativeT.ToFloat();
      this.SimStepsCount = simStepsCount;
      this.SimStepsSinceLoad = simStepsSinceLoad;
      this.DeltaSimStepsApprox = deltaStepsApprox;
    }

    internal void UpdatePaused(int simStepsCount, int simStepsSinceLoad)
    {
      this.IsGamePaused = true;
      this.GameSpeedMult = 0;
      this.DeltaTimeMs = 0.0f;
      this.TimeSinceLastSimUpdateMs = Fix32.Zero;
      this.AbsoluteT = 1f;
      this.RelativeT = 0.0f;
      this.SimStepsCount = simStepsCount;
      this.SimStepsSinceLoad = simStepsSinceLoad;
      this.DeltaSimStepsApprox = Percent.Zero;
    }

    public void UpdateForSync(
      float deltaTimeMs,
      bool isGamePaused,
      int gameSpeedMult,
      int simStepsCount,
      int simStepsSinceLoad,
      Fix32 currSimUpdateDurationMs)
    {
      this.IsGamePaused = isGamePaused;
      this.GameSpeedMult = isGamePaused ? 0 : gameSpeedMult;
      this.DeltaTimeMs = deltaTimeMs;
      this.TimeSinceLastSimUpdateMs = Fix32.Zero;
      this.CurrSimUpdateDurationMs = currSimUpdateDurationMs;
      this.DeltaT = 0.0f;
      this.AbsoluteT = 0.0f;
      this.RelativeT = 0.0f;
      this.SimStepsCount = simStepsCount;
      this.SimStepsSinceLoad = simStepsSinceLoad;
      this.DeltaSimStepsApprox = Percent.Zero;
    }

    public GameTime()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: reference to a compiler-generated field
      this.\u003CCurrSimUpdateDurationMs\u003Ek__BackingField = (Fix32) 100;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
