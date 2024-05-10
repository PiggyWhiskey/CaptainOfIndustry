// Decompiled with JetBrains decompiler
// Type: Mafi.Core.UiState.UiCameraState
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;
using System.Globalization;
using System.Text;

#nullable disable
namespace Mafi.Core.UiState
{
  /// <summary>Saveable state of the primary camera.</summary>
  [GenerateSerializer(false, null, 0)]
  [SkipDuringDeterminismValidation]
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class UiCameraState
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public static void Serialize(UiCameraState value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<UiCameraState>(value))
        return;
      writer.EnqueueDataSerialization((object) value, UiCameraState.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      RelTile1f.Serialize(this.OrbitRadius, writer);
      AngleDegrees1f.Serialize(this.PitchAngle, writer);
      HeightTilesF.Serialize(this.PivotHeight, writer);
      Tile2f.Serialize(this.PivotPosition, writer);
      Tile2f.Serialize(this.SavedPosition1, writer);
      Tile2f.Serialize(this.SavedPosition2, writer);
      Tile2f.Serialize(this.SavedPosition3, writer);
      Tile2f.Serialize(this.SavedPosition4, writer);
      AngleDegrees1f.Serialize(this.YawAngle, writer);
    }

    public static UiCameraState Deserialize(BlobReader reader)
    {
      UiCameraState uiCameraState;
      if (reader.TryStartClassDeserialization<UiCameraState>(out uiCameraState))
        reader.EnqueueDataDeserialization((object) uiCameraState, UiCameraState.s_deserializeDataDelayedAction);
      return uiCameraState;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.OrbitRadius = RelTile1f.Deserialize(reader);
      this.PitchAngle = AngleDegrees1f.Deserialize(reader);
      this.PivotHeight = HeightTilesF.Deserialize(reader);
      this.PivotPosition = Tile2f.Deserialize(reader);
      this.SavedPosition1 = reader.LoadedSaveVersion >= 115 ? Tile2f.Deserialize(reader) : new Tile2f();
      this.SavedPosition2 = reader.LoadedSaveVersion >= 115 ? Tile2f.Deserialize(reader) : new Tile2f();
      this.SavedPosition3 = reader.LoadedSaveVersion >= 115 ? Tile2f.Deserialize(reader) : new Tile2f();
      this.SavedPosition4 = reader.LoadedSaveVersion >= 115 ? Tile2f.Deserialize(reader) : new Tile2f();
      this.YawAngle = AngleDegrees1f.Deserialize(reader);
    }

    public Tile2f PivotPosition { get; set; }

    public HeightTilesF PivotHeight { get; set; }

    public RelTile1f OrbitRadius { get; set; }

    public AngleDegrees1f YawAngle { get; set; }

    public AngleDegrees1f PitchAngle { get; set; }

    public Tile3f PivotPosition3f => this.PivotPosition.ExtendHeight(this.PivotHeight);

    [NewInSaveVersion(115, null, null, null, null)]
    public Tile2f SavedPosition1 { get; set; }

    [NewInSaveVersion(115, null, null, null, null)]
    public Tile2f SavedPosition2 { get; set; }

    [NewInSaveVersion(115, null, null, null, null)]
    public Tile2f SavedPosition3 { get; set; }

    [NewInSaveVersion(115, null, null, null, null)]
    public Tile2f SavedPosition4 { get; set; }

    public string SerializeToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(string.Format("{0},{1}", (object) this.PivotPosition.X, (object) this.PivotPosition.Y));
      stringBuilder.Append(string.Format(",{0}", (object) this.PivotHeight.Value));
      stringBuilder.Append(string.Format(",{0}", (object) this.OrbitRadius.Value));
      stringBuilder.Append(string.Format(",{0}", (object) this.YawAngle.Degrees));
      stringBuilder.Append(string.Format(",{0}", (object) this.PitchAngle.Degrees));
      return stringBuilder.ToString();
    }

    public bool TryDeserializeFromString(string value)
    {
      if (string.IsNullOrEmpty(value))
        return false;
      string[] strArray = value.Split(',');
      if (strArray.Length != 6)
        return false;
      Fix32[] fix32Array = new Fix32[6];
      for (int index = 0; index < 6; ++index)
      {
        float result;
        if (!float.TryParse(strArray[index], NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result))
          return false;
        fix32Array[index] = result.ToFix32();
      }
      this.PivotPosition = new Tile2f(fix32Array[0], fix32Array[1]);
      this.PivotHeight = new HeightTilesF(fix32Array[2]);
      this.OrbitRadius = new RelTile1f(fix32Array[3]);
      this.YawAngle = AngleDegrees1f.FromDegrees(fix32Array[4]);
      this.PitchAngle = AngleDegrees1f.FromDegrees(fix32Array[5]);
      return true;
    }

    public UiCameraState()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static UiCameraState()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      UiCameraState.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((UiCameraState) obj).SerializeData(writer));
      UiCameraState.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((UiCameraState) obj).DeserializeData(reader));
    }
  }
}
