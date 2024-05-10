// Decompiled with JetBrains decompiler
// Type: Mafi.Core.SpaceProgram.IRocketOwnerExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.SpaceProgram
{
  public static class IRocketOwnerExtensions
  {
    public static bool TryTransferRocketTo(this IRocketOwner src, IRocketOwner dst)
    {
      if (src.AttachedRocketBase.IsNone || !dst.CanAttachRocket(src.AttachedRocketBase.Value))
        return false;
      Option<RocketEntityBase> option = src.DetachRocket();
      if (option.IsNone)
      {
        Log.Error("Lost rocket?");
        return false;
      }
      dst.AttachRocket(option.Value);
      return true;
    }
  }
}
