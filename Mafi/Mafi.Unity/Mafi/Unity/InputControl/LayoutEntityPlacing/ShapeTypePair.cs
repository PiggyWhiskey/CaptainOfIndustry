// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.LayoutEntityPlacing.ShapeTypePair
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Ports.Io;
using Mafi.Core.Prototypes;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.LayoutEntityPlacing
{
  public readonly struct ShapeTypePair : IEquatable<ShapeTypePair>
  {
    public readonly IoPortShapeProto Shape;
    public readonly IoPortType Type;

    public ShapeTypePair(IoPortShapeProto shape, IoPortType type)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.Shape = shape;
      this.Type = type;
    }

    public bool Matches(IoPort p)
    {
      return (Proto) this.Shape == (Proto) p.ShapePrototype && this.Type.IsCompatibleWith(p.Type);
    }

    public bool Equals(ShapeTypePair other)
    {
      return (Proto) this.Shape == (Proto) other.Shape && this.Type == other.Type;
    }

    public override bool Equals(object obj) => obj is ShapeTypePair other && this.Equals(other);

    public override int GetHashCode()
    {
      return Hash.Combine<IoPortShapeProto, IoPortType>(this.Shape, this.Type);
    }
  }
}
