// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Messages.MessagesExtensions
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using Mafi.Core.Buildings.Cargo;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Research;

#nullable disable
namespace Mafi.Base.Prototypes.Messages
{
  internal static class MessagesExtensions
  {
    public static string TranslatedName(this Mafi.Core.Prototypes.Proto.ID protoId, ProtosDb protosDb)
    {
      return protosDb.GetOrThrow<Mafi.Core.Prototypes.Proto>(protoId).Strings.Name.TranslatedString;
    }

    public static string TranslatedName(this DynamicEntityProto.ID protoId, ProtosDb protosDb)
    {
      return protosDb.GetOrThrow<Mafi.Core.Prototypes.Proto>((Mafi.Core.Prototypes.Proto.ID) protoId).Strings.Name.TranslatedString;
    }

    public static string TranslatedName(this StaticEntityProto.ID protoId, ProtosDb protosDb)
    {
      return protosDb.GetOrThrow<Mafi.Core.Prototypes.Proto>((Mafi.Core.Prototypes.Proto.ID) protoId).Strings.Name.TranslatedString;
    }

    public static string TranslatedName(this ResearchNodeProto.ID protoId, ProtosDb protosDb)
    {
      return protosDb.GetOrThrow<ResearchNodeProto>((Mafi.Core.Prototypes.Proto.ID) protoId).Strings.Name.TranslatedString;
    }

    public static string TranslatedName(this MachineProto.ID protoId, ProtosDb protosDb)
    {
      return protosDb.GetOrThrow<Mafi.Core.Prototypes.Proto>((Mafi.Core.Prototypes.Proto.ID) protoId).Strings.Name.TranslatedString;
    }

    public static string TranslatedName(this CargoDepotProto.ID protoId, ProtosDb protosDb)
    {
      return protosDb.GetOrThrow<Mafi.Core.Prototypes.Proto>((Mafi.Core.Prototypes.Proto.ID) protoId).Strings.Name.TranslatedString;
    }

    public static string TranslatedName(this ProductProto.ID protoId, ProtosDb protosDb)
    {
      return protosDb.GetOrThrow<Mafi.Core.Prototypes.Proto>((Mafi.Core.Prototypes.Proto.ID) protoId).Strings.Name.TranslatedString;
    }

    public static string MakeBoldColored(this string stringIn) => "<bc>" + stringIn + "</bc>";

    public static string MakeBold(this string stringIn) => "<b>" + stringIn + "</b>";

    public static string Quote(this string stringIn) => "\"" + stringIn + "\"";
  }
}
