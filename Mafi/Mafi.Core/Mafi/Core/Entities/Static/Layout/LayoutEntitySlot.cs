// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.Layout.LayoutEntitySlot
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Entities.Static.Layout
{
  public struct LayoutEntitySlot
  {
    public readonly TileTransform Transform;
    public readonly bool AllowAnyRotationAndReflection;

    public LayoutEntitySlot(TileTransform transform, bool allowAnyRotationAndReflection = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Transform = transform;
      this.AllowAnyRotationAndReflection = allowAnyRotationAndReflection;
    }

    public bool IsCompatibleWith(TileTransform t)
    {
      return !this.AllowAnyRotationAndReflection ? this.Transform == t : this.Transform.Position == t.Position;
    }

    public TileTransform GetSlotTransform(TileTransform transform)
    {
      return !this.AllowAnyRotationAndReflection ? this.Transform : new TileTransform(this.Transform.Position, transform.Rotation, transform.IsReflected);
    }
  }
}
