// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Prototypes.IdFactoryBase`1
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Prototypes
{
  [GenerateSerializer(false, null, 0)]
  public abstract class IdFactoryBase<TId> where TId : struct
  {
    /// <summary>
    /// Counter for unique IDs. Current value represents last scheduled ID. Initial value is 0 which is invalid
    /// ID.
    /// </summary>
    private TId m_lastUsedId;

    /// <summary>Return next valid ID for a new object.</summary>
    public TId GetNextId()
    {
      this.m_lastUsedId = this.GetNextIdInternal(this.m_lastUsedId);
      return this.m_lastUsedId;
    }

    protected abstract TId GetNextIdInternal(TId lastUsed);

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<TId>(this.m_lastUsedId);
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.m_lastUsedId = reader.ReadGenericAs<TId>();
    }

    protected IdFactoryBase()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
