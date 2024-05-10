using Mafi;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;

namespace TerrainTower.TTower
{
    /// <summary>
    /// Class is used to show Product icon at selected Output Port
    /// </summary>
    [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
    internal class TerrainTowerPortProductResolver : PortProductResolverBase<TerrainTowerEntity>
    {
        public TerrainTowerPortProductResolver(ProtosDb protosDb) : base(protosDb)
        {
        }

        public override ImmutableArray<ProductProto> GetPortProduct(
            IEntityProto proto,
            PortSpec portSpec)
        {
            return ImmutableArray<ProductProto>.Empty;
        }

        protected override ImmutableArray<ProductProto> GetPortProduct(
            TerrainTowerEntity entity,
            IoPort port,
            bool considerAllUnlockedRecipes,
            bool fallbackToUnlockedIfNoRecipesAssigned)
        {
            return port.Type == IoPortType.Output ? entity.GetPortProducts(port) : ImmutableArray<ProductProto>.Empty;
        }
    }
}