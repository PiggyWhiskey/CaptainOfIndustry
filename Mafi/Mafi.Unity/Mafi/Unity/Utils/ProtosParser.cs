// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Utils.ProtosParser
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Prototypes;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Utils
{
  internal class ProtosParser : IObjectEditorCustomParser
  {
    private readonly ProtosDb m_protosDb;

    public ProtosParser(ProtosDb protosDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_protosDb = protosDb;
    }

    public bool CanParse(Type t) => t.IsAssignableTo<Proto>();

    public Option<string> SerializeToStr(Type type, object value)
    {
      if (type.IsAssignableTo<Proto>())
        return (Option<string>) ((Proto) value).Id.Value;
      Log.Error(string.Format("Unexpected type for serialization: {0}", (object) type));
      return Option<string>.None;
    }

    public bool TryParseFromStrAs(string value, Type type, out object obj)
    {
      obj = (object) null;
      if (type.IsAssignableTo<Proto>())
      {
        Proto proto;
        if (!this.m_protosDb.TryGetProto<Proto>(new Proto.ID(value), out proto))
          return false;
        Assert.That<Type>(proto.GetType()).IsAssignableTo(type);
        obj = (object) proto;
        return true;
      }
      Log.Error(string.Format("Unexpected type for parsing: {0}", (object) type));
      return false;
    }
  }
}
