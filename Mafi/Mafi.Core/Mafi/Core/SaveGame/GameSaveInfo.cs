// Decompiled with JetBrains decompiler
// Type: Mafi.Core.SaveGame.GameSaveInfo
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.SaveGame
{
  [GenerateSerializer(false, null, 0)]
  public class GameSaveInfo
  {
    public static readonly GameSaveInfo Empty;
    public readonly GameDate GameDate;
    public string MapName;
    public readonly int Population;
    public readonly int ResearchNodesUnlocked;
    public readonly int ResearchNodesTotal;
    public readonly int LaunchCount;
    public readonly string Notes;
    public readonly Vector2i ThumbnailImageSize;
    public readonly byte[] ThumbnailImageBytes;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public bool IsEmpty => this.GameDate.RelGameDate.TotalDays == 0;

    public GameSaveInfo(
      GameDate gameDate,
      string mapName,
      int population,
      int researchNodesUnlocked,
      int researchNodesTotal,
      int launchCount,
      string notes,
      Vector2i thumbnailImageSize,
      byte[] thumbnailImageBytes)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.GameDate = gameDate;
      this.MapName = mapName;
      this.Population = population;
      this.ResearchNodesUnlocked = researchNodesUnlocked;
      this.ResearchNodesTotal = researchNodesTotal;
      this.LaunchCount = launchCount;
      this.Notes = notes;
      this.ThumbnailImageSize = thumbnailImageSize;
      this.ThumbnailImageBytes = thumbnailImageBytes ?? Array.Empty<byte>();
    }

    public static void Serialize(GameSaveInfo value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GameSaveInfo>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GameSaveInfo.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      GameDate.Serialize(this.GameDate, writer);
      writer.WriteInt(this.LaunchCount);
      writer.WriteString(this.MapName);
      writer.WriteString(this.Notes);
      writer.WriteInt(this.Population);
      writer.WriteInt(this.ResearchNodesTotal);
      writer.WriteInt(this.ResearchNodesUnlocked);
      writer.WriteArray<byte>(this.ThumbnailImageBytes);
      Vector2i.Serialize(this.ThumbnailImageSize, writer);
    }

    public static GameSaveInfo Deserialize(BlobReader reader)
    {
      GameSaveInfo gameSaveInfo;
      if (reader.TryStartClassDeserialization<GameSaveInfo>(out gameSaveInfo))
        reader.EnqueueDataDeserialization((object) gameSaveInfo, GameSaveInfo.s_deserializeDataDelayedAction);
      return gameSaveInfo;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<GameSaveInfo>(this, "GameDate", (object) GameDate.Deserialize(reader));
      reader.SetField<GameSaveInfo>(this, "LaunchCount", (object) reader.ReadInt());
      this.MapName = reader.ReadString();
      reader.SetField<GameSaveInfo>(this, "Notes", (object) reader.ReadString());
      reader.SetField<GameSaveInfo>(this, "Population", (object) reader.ReadInt());
      reader.SetField<GameSaveInfo>(this, "ResearchNodesTotal", (object) reader.ReadInt());
      reader.SetField<GameSaveInfo>(this, "ResearchNodesUnlocked", (object) reader.ReadInt());
      reader.SetField<GameSaveInfo>(this, "ThumbnailImageBytes", (object) reader.ReadArray<byte>());
      reader.SetField<GameSaveInfo>(this, "ThumbnailImageSize", (object) Vector2i.Deserialize(reader));
    }

    static GameSaveInfo()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GameSaveInfo.Empty = new GameSaveInfo(new GameDate(), "", 0, 0, 0, 0, "", Vector2i.Zero, Array.Empty<byte>());
      GameSaveInfo.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((GameSaveInfo) obj).SerializeData(writer));
      GameSaveInfo.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((GameSaveInfo) obj).DeserializeData(reader));
    }
  }
}
