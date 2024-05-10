// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Console.Commands.GenerateBlueprintsCommand
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Console;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Prototypes;
using Mafi.Unity.Factory.Transports;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Console.Commands
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal class GenerateBlueprintsCommand
  {
    private readonly ProtosDb m_db;
    private readonly TransportModelFactory m_transportModelFactory;
    private readonly LayoutEntityBlueprintGenerator m_layoutEntityBlueprintGenerator;
    private readonly IFileSystemHelper m_fshFileSystemHelper;

    public GenerateBlueprintsCommand(
      ProtosDb db,
      TransportModelFactory transportModelFactory,
      LayoutEntityBlueprintGenerator layoutEntityBlueprintGenerator,
      IFileSystemHelper fshFileSystemHelper)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_db = db;
      this.m_transportModelFactory = transportModelFactory;
      this.m_layoutEntityBlueprintGenerator = layoutEntityBlueprintGenerator;
      this.m_fshFileSystemHelper = fshFileSystemHelper;
    }

    [ConsoleCommand(true, false, null, null)]
    private GameCommandResult generateLayoutEntityMeshTemplates(string idSubstring)
    {
      return this.generateMeshes(idSubstring, true, false);
    }

    [ConsoleCommand(true, false, null, null)]
    private GameCommandResult generateTransportsMeshTemplates()
    {
      return this.generateMeshes("", false, true);
    }

    private GameCommandResult generateMeshes(
      string idSubstring,
      bool genEntities,
      bool genTransports)
    {
      try
      {
        int num = 0;
        if (genEntities)
          num += this.generateLayoutEntities(idSubstring, "GeneratedMeshTemplates");
        if (genTransports)
          num += this.generateTransports("GeneratedMeshTemplates");
        return GameCommandResult.Success((object) string.Format("Generated {0} templates, search for '{1}' directory.", (object) num, (object) "GeneratedMeshTemplates"));
      }
      catch (Exception ex)
      {
        Log.Exception(ex, "Failed to generate meshes.");
        return GameCommandResult.Error("Failed: " + ex?.ToString());
      }
    }

    private int generateLayoutEntities(string substring, string basePath)
    {
      string path = Path.Combine(basePath, "LayoutEntity");
      Directory.CreateDirectory(path);
      MeshBuilder builder = new MeshBuilder();
      int layoutEntities = 0;
      foreach (LayoutEntityProto layoutEntityProto in this.m_db.All<LayoutEntityProto>())
      {
        Assert.That<bool>(builder.IsEmpty).IsTrue();
        if (string.IsNullOrEmpty(substring) || layoutEntityProto.Id.Value.Contains(substring, StringComparison.OrdinalIgnoreCase))
        {
          this.m_layoutEntityBlueprintGenerator.GenerateTemplate(builder, layoutEntityProto.Layout);
          ObjMeshExporter.ExportObjAndClear(path, layoutEntityProto.Id.Value, builder);
          ++layoutEntities;
        }
      }
      return layoutEntities;
    }

    private int generateTransports(string basePath)
    {
      string dirPath = Path.Combine(basePath, "Transports");
      Directory.CreateDirectory(dirPath);
      Vector3 alignOffset = new Tile3f(-Fix32.Half, -Fix32.Half, (Fix32) 0).ToVector3();
      int count = 0;
      foreach (TransportProto proto in this.m_db.All<TransportProto>())
      {
        if (!proto.Graphics.CrossSection.MovingCrossSectionParts.IsEmpty || !proto.Graphics.CrossSection.StaticCrossSectionParts.IsEmpty)
        {
          foreach (TransportPillarAttachmentType attachmentType in Enum.GetValues(typeof (TransportPillarAttachmentType)))
          {
            TransportTrajectory traj;
            if (TransportTrajectory.TryCreateForAttachment(attachmentType, proto, out traj, out string _))
              genTransport(string.Format("Att-{0}", (object) attachmentType), proto, new Tile3i[1]
              {
                new Tile3i(0, 0, 0)
              }, new RelTile3i?(traj.StartDirection), new RelTile3i?(traj.EndDirection), true);
          }
          genTransport("00_Straight_2m", proto, new Tile3i[1]
          {
            new Tile3i(0, 0, 0)
          });
          genTransport("00_Straight_2m", proto, new Tile3i[1]
          {
            new Tile3i(0, 0, 0)
          });
          genTransport("01_Straight_4m", proto, new Tile3i[2]
          {
            new Tile3i(0, 0, 0),
            new Tile3i(1, 0, 0)
          });
          genTransport("02_Straight_8m", proto, new Tile3i[2]
          {
            new Tile3i(0, 0, 0),
            new Tile3i(3, 0, 0)
          });
          genTransport("03_Turn", proto, new Tile3i[1]
          {
            new Tile3i(0, 0, 0)
          }, new RelTile3i?(new RelTile3i(-1, 0, 0)), new RelTile3i?(new RelTile3i(0, 1, 0)));
          genTransport("04_Sss", proto, new Tile3i[4]
          {
            new Tile3i(-2, 0, 0),
            new Tile3i(0, 0, 0),
            new Tile3i(0, 1, 0),
            new Tile3i(2, 1, 0)
          });
          if (proto.CanGoUpDown)
          {
            int num = proto.ZStepLength.Value;
            if (num != 0)
            {
              genTransport("05_Ramp", proto, new Tile3i[2]
              {
                new Tile3i(0, 0, 0),
                new Tile3i(num, 0, 1)
              });
              genTransport("06_RampTurn", proto, new Tile3i[2]
              {
                new Tile3i(0, 0, 0),
                new Tile3i(num, 0, 1)
              }, new RelTile3i?(new RelTile3i(-1, 0, 0)), new RelTile3i?(new RelTile3i(0, 1, 0)));
              genTransport("07_DoubleRamp", proto, new Tile3i[4]
              {
                new Tile3i(-num, 0, 0),
                new Tile3i(0, 0, 1),
                new Tile3i(1, 0, 1),
                new Tile3i(1 + num, 0, 1)
              });
              genTransport("08_DoubleRampTurn", proto, new Tile3i[3]
              {
                new Tile3i(-num, 0, 0),
                new Tile3i(0, 0, 1),
                new Tile3i(0, num, 2)
              });
              genTransport("09_Serpentine", proto, new Tile3i[4]
              {
                new Tile3i(0, 0, 0),
                new Tile3i(num, 0, 1),
                new Tile3i(num, 1, 1),
                new Tile3i(0, 1, 2)
              });
            }
          }
        }
      }
      return count;

      void genTransport(
        string name,
        TransportProto proto,
        Tile3i[] pivots,
        RelTile3i? startDir = null,
        RelTile3i? endDir = null,
        bool noAlignOnGrid = false)
      {
        RelTile3i startDirection;
        RelTile3i endDirection;
        TransportTrajectory.ComputeStartAndEndDirections(pivots.AsSlice<Tile3i>(), startDir, endDir, out startDirection, out endDirection);
        string error;
        GameObject go = this.m_transportModelFactory.CreateModel(proto, ((ICollection<Tile3i>) pivots).ToImmutableArray<Tile3i>(), out error, new RelTile3i?(startDirection), new RelTile3i?(endDirection), true, true, true, true).ValueOrThrow(error);
        MeshBuilder instance = MeshBuilder.Instance;
        instance.SetTransform(noAlignOnGrid ? Vector3.zero : alignOffset, Quaternion.identity, 1f);
        instance.AddAllMeshes(go, true);
        go.DestroyImmediate();
        ObjMeshExporter.ExportObjAndClear(dirPath, string.Format("{0}--{1}", (object) proto.Id, (object) name), instance);
        ++count;
      }
    }
  }
}
