// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Ports.Io.IoPortShapeProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Mafi.Core.Ports.Io
{
  /// <summary>
  /// Io port shape specifies an interface between transport and entity. Two <see cref="T:Mafi.Core.Ports.Io.IoPort" /> s can be only
  /// connected if they have identical <see cref="T:Mafi.Core.Ports.Io.IoPortShapeProto" />.
  /// 
  /// Shape does not contain information about transport so we can have multiple different transports with different
  /// <see cref="!:TransportCrossSectionProto" /> that have the same port shape.
  /// </summary>
  public class IoPortShapeProto : Proto
  {
    /// <summary>Representation of this shape in entity layout.</summary>
    public readonly char LayoutChar;
    /// <summary>
    /// Allowed product type for this shape. Only one product per shape is allowed.
    /// </summary>
    public readonly ProductType AllowedProductType;
    /// <summary>
    /// Graphics-only properties that does not affect game simulation and are not needed or accessed by the game
    /// simulation.
    /// </summary>
    public readonly IoPortShapeProto.Gfx Graphics;

    public IoPortShapeProto(
      IoPortShapeProto.ID id,
      Proto.Str strings,
      char layoutChar,
      ProductType allowedProductType,
      IoPortShapeProto.Gfx graphics,
      IEnumerable<Tag> tags = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((Proto.ID) id, strings, tags);
      this.LayoutChar = layoutChar;
      this.AllowedProductType = allowedProductType;
      this.Graphics = graphics;
    }

    public IoPortShapeProto.ID Id => new IoPortShapeProto.ID(base.Id.Value);

    [DebuggerDisplay("{Value,nq}")]
    [ManuallyWrittenSerialization]
    [DebuggerStepThrough]
    public new readonly struct ID : IEquatable<IoPortShapeProto.ID>, IComparable<IoPortShapeProto.ID>
    {
      /// <summary>Underlying string value of this Id.</summary>
      public readonly string Value;

      public ID(string value)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Value = value;
      }

      /// <summary>
      /// Implicit conversion to parent <see cref="T:Mafi.Core.Prototypes.Proto.ID" />.
      /// </summary>
      public static implicit operator Proto.ID(IoPortShapeProto.ID id) => new Proto.ID(id.Value);

      public static bool operator ==(Proto.ID lhs, IoPortShapeProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(IoPortShapeProto.ID lhs, Proto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(Proto.ID lhs, IoPortShapeProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(IoPortShapeProto.ID lhs, Proto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator ==(IoPortShapeProto.ID lhs, IoPortShapeProto.ID rhs)
      {
        return string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public static bool operator !=(IoPortShapeProto.ID lhs, IoPortShapeProto.ID rhs)
      {
        return !string.Equals(lhs.Value, rhs.Value, StringComparison.Ordinal);
      }

      public override bool Equals(object other)
      {
        return other is IoPortShapeProto.ID other1 && this.Equals(other1);
      }

      public bool Equals(IoPortShapeProto.ID other)
      {
        return string.Equals(this.Value, other.Value, StringComparison.Ordinal);
      }

      public int CompareTo(IoPortShapeProto.ID other)
      {
        return string.CompareOrdinal(this.Value, other.Value);
      }

      public override string ToString() => this.Value ?? string.Empty;

      public override int GetHashCode()
      {
        string str = this.Value;
        return str == null ? 0 : str.GetHashCode();
      }

      public static void Serialize(IoPortShapeProto.ID value, BlobWriter writer)
      {
        writer.WriteString(value.Value);
      }

      public static IoPortShapeProto.ID Deserialize(BlobReader reader)
      {
        return new IoPortShapeProto.ID(reader.ReadString());
      }
    }

    public new class Gfx : Proto.Gfx
    {
      public static readonly IoPortShapeProto.Gfx Empty;
      /// <summary>Prefab of this port.</summary>
      public readonly string ConnectedPortPrefabPath;
      public readonly string ConnectedPortPrefabPathLod3;
      /// <summary>
      /// Prefab of special end model that is shown when the port is not connected. May be the same path as <see cref="F:Mafi.Core.Ports.Io.IoPortShapeProto.Gfx.ConnectedPortPrefabPath" />.
      /// </summary>
      public readonly string DisconnectedPortPrefabPath;
      public readonly string DisconnectedPortPrefabPathLod3;
      /// <summary>
      /// Helper fields solely for use by `IoPortsRenderer` to avoid dict lookup. Do not touch them.
      /// </summary>
      public int RendererIndexConnected;
      public int RendererIndexDisconnected;
      public bool ShowWhenTwoTransportsConnect;

      public Gfx(
        string connectedPrefabPath,
        string connectedPrefabPathLod3,
        bool showWhenTwoTransportsConnect = false,
        Option<string> disconnectedPrefabPath = default (Option<string>),
        Option<string> disconnectedPrefabPathLod3 = default (Option<string>))
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.ShowWhenTwoTransportsConnect = showWhenTwoTransportsConnect;
        this.ConnectedPortPrefabPath = connectedPrefabPath.CheckNotNull<string>();
        this.ConnectedPortPrefabPathLod3 = connectedPrefabPathLod3.CheckNotNull<string>();
        this.DisconnectedPortPrefabPath = disconnectedPrefabPath.ValueOrNull ?? connectedPrefabPath;
        this.DisconnectedPortPrefabPathLod3 = disconnectedPrefabPathLod3.ValueOrNull ?? connectedPrefabPathLod3;
      }

      static Gfx()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        IoPortShapeProto.Gfx.Empty = new IoPortShapeProto.Gfx("EMPTY", "EMPTY");
      }
    }
  }
}
