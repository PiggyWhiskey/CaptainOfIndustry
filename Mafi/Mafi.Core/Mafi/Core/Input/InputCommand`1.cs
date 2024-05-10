// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Input.InputCommand`1
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Input
{
  [GenerateSerializer(false, null, 0)]
  public abstract class InputCommand<TResult> : IInputCommand, IInputCommandFriend where TResult : struct
  {
    private TResult? m_result;
    private Option<string> m_errorMessage;

    /// <summary>Whether this command was processed.</summary>
    public bool IsProcessed => this.ProcessedAtStep.IsPositive;

    /// <summary>
    /// Whether this command was processed and sync was performed.
    /// </summary>
    public bool IsProcessedAndSynced { get; private set; }

    /// <summary>Sim step of when this command was processed.</summary>
    public SimStep ProcessedAtStep { get; private set; }

    public bool ResultSet => this.m_result.HasValue;

    public virtual bool AffectsSaveState => true;

    public virtual bool IsVerificationCmd => false;

    public TResult Result
    {
      get
      {
        if (this.m_result.HasValue)
          return this.m_result.Value;
        Log.Error(string.Format("Getting result which was not set yet on: {0}", (object) this));
        return default (TResult);
      }
    }

    public bool HasError
    {
      get
      {
        Assert.That<bool>(this.ResultSet).IsTrue<InputCommand<TResult>>("Result was not set yet on: {0}", this);
        return this.m_errorMessage.HasValue;
      }
    }

    public string ErrorMessage
    {
      get
      {
        Assert.That<bool>(this.ResultSet).IsTrue<InputCommand<TResult>>("Result was not set yet on: {0}", this);
        return this.m_errorMessage.ValueOr("");
      }
    }

    public void SetResultSuccess(TResult result)
    {
      Assert.That<bool>(this.ResultSet).IsFalse<InputCommand<TResult>>("Result was already set on: {0}", this);
      this.m_result = new TResult?(result);
      this.m_errorMessage = Option<string>.None;
    }

    public void SetResultError(TResult result, string errorMessage)
    {
      Assert.That<bool>(this.ResultSet).IsFalse<InputCommand<TResult>>("Result was already set on: {0}", this);
      this.m_result = new TResult?(result);
      this.m_errorMessage = (Option<string>) (errorMessage ?? "");
    }

    public object GetResultObject()
    {
      Assert.That<bool>(this.ResultSet).IsTrue<InputCommand<TResult>>("Result was not set on: {0}", this);
      return (object) this.m_result;
    }

    /// <summary>
    /// Marks this input command processed. This should be called only by the <see cref="T:Mafi.Core.Input.InputScheduler" />.
    /// </summary>
    void IInputCommandFriend.MarkProcessed(SimStep step)
    {
      Assert.That<SimStep>(step).IsPositive("Processed step must be positive.");
      Assert.That<bool>(this.IsProcessed).IsFalse("Input command processed more than once.");
      Assert.That<bool>(this.IsProcessedAndSynced).IsFalse("Already synced?");
      Assert.That<bool>(this.ResultSet).IsTrue<InputCommand<TResult>>("Result was not set on: {0}", this);
      this.ProcessedAtStep = step;
    }

    void IInputCommandFriend.MarkFailed(SimStep step)
    {
      Assert.That<SimStep>(step).IsPositive("Processed step must be positive.");
      Assert.That<bool>(this.IsProcessed).IsFalse("Input command processed more than once.");
      Assert.That<bool>(this.IsProcessedAndSynced).IsFalse("Already synced?");
      this.ProcessedAtStep = step;
    }

    void IInputCommandFriend.Sync()
    {
      Assert.That<bool>(this.IsProcessed).IsTrue("Calling sync when command is not processed!");
      Assert.That<bool>(this.IsProcessedAndSynced).IsFalse("Calling sync more than once!");
      this.IsProcessedAndSynced = true;
    }

    public void EraseProcessedInfo()
    {
      this.m_result = new TResult?();
      this.m_errorMessage = Option<string>.None;
      this.ProcessedAtStep = new SimStep();
      this.IsProcessedAndSynced = false;
    }

    public IInputCommand InitializeAsProcessed(SimStep step, TResult result, string resultMessage)
    {
      Assert.That<SimStep>(step).IsPositive("Processed step must be positive.");
      Assert.That<bool>(this.IsProcessed).IsFalse("Input command processed more than once.");
      Assert.That<bool>(this.IsProcessedAndSynced).IsFalse("Already synced?");
      Assert.That<bool>(this.ResultSet).IsFalse<InputCommand<TResult>>("Result was already set on: {0}", this);
      this.ProcessedAtStep = step;
      this.m_result = new TResult?(result);
      this.m_errorMessage = (Option<string>) resultMessage;
      return (IInputCommand) this;
    }

    public virtual bool ResultEqualTo(TResult other) => this.Result.Equals((object) other);

    public bool ResultObjectEqualTo(object other)
    {
      return other is TResult other1 && this.ResultEqualTo(other1);
    }

    public bool ResultEqualTo(IInputCommand other)
    {
      return other is InputCommand<TResult> inputCommand && this.ResultEqualTo(inputCommand.Result);
    }

    public IInputCommand ShallowCloneWithoutResult()
    {
      InputCommand<TResult> inputCommand = (InputCommand<TResult>) this.MemberwiseClone();
      inputCommand.EraseProcessedInfo();
      return (IInputCommand) inputCommand;
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteBool(this.IsProcessedAndSynced);
      Option<string>.Serialize(this.m_errorMessage, writer);
      writer.WriteNullableStruct<TResult>(this.m_result);
      SimStep.Serialize(this.ProcessedAtStep, writer);
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.IsProcessedAndSynced = reader.ReadBool();
      this.m_errorMessage = Option<string>.Deserialize(reader);
      this.m_result = reader.ReadNullableStruct<TResult>();
      this.ProcessedAtStep = SimStep.Deserialize(reader);
    }

    protected InputCommand()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
