// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.BulletedList
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiToolkit.Component;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library
{
  public class BulletedList : Column
  {
    public BulletedList()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Class<BulletedList>(Cls.bulletedList);
      this.Gap<BulletedList>(new Px?(1.pt()));
    }

    public BulletedList NoIndent() => this.Class<BulletedList>(Cls.noIndent);

    public override void InsertAt(int index, [CanBeNull] UiComponent child)
    {
      if (child != null)
        child = child.Class<UiComponent>(Cls.bulletedList_item);
      base.InsertAt(index, child);
    }
  }
}
