// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MapEditor.CustomEditors.Polygon3fEditorFactory
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiToolkit.Library.ObjectEditor;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MapEditor.CustomEditors
{
  public class Polygon3fEditorFactory : ICustomObjEditorFactory
  {
    private readonly IResolver m_resolver;

    public Polygon3fEditorFactory(IResolver resolver)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_resolver = resolver;
    }

    public ICustomObjEditor Create(object obj)
    {
      Polygon3fEditor polygon3fEditor = this.m_resolver.Instantiate<Polygon3fEditor>();
      polygon3fEditor.StartEdit(obj);
      return (ICustomObjEditor) polygon3fEditor;
    }
  }
}
