// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.UniqueId
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Serialization
{
  /// <summary>
  /// Represents an identifier saved at the start of each class serialized class.
  /// Merges identification of objects used by reference serialized types and identification of object type used by
  /// copy serialized types.
  /// </summary>
  public struct UniqueId
  {
    public readonly int Id;
    private readonly UniqueId.SpaceId m_space;

    public bool IsObjectId => this.m_space == UniqueId.SpaceId.Objects;

    public bool IsTypeId => this.m_space == UniqueId.SpaceId.Types;

    private UniqueId(int id, UniqueId.SpaceId spaceId)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.Id = id;
      this.m_space = spaceId;
    }

    /// <summary>Serialization into 4 bytes.</summary>
    public uint ToUint()
    {
      int space = (int) this.m_space;
      Assert.That<int>(space >> 1).IsZero();
      Assert.That<int>(this.Id >> 31).IsZero();
      return (uint) (this.Id << 1 | space);
    }

    /// <summary>Deserialization.</summary>
    public static UniqueId ParseUint(uint i)
    {
      return new UniqueId((int) (i >> 1), (UniqueId.SpaceId) ((int) i & 1));
    }

    public static UniqueId CreateObjectId(int objectId)
    {
      return new UniqueId(objectId, UniqueId.SpaceId.Objects);
    }

    public static UniqueId CreateTypeId(int typeId) => new UniqueId(typeId, UniqueId.SpaceId.Types);

    private enum SpaceId
    {
      Types,
      Objects,
    }
  }
}
