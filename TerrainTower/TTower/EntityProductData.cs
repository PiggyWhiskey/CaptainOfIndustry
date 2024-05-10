using Mafi;
using Mafi.Core.Entities.Static;
using Mafi.Core.Products;
using Mafi.Serialization;

using System;
using System.Linq;

namespace TerrainTower.TTower
{
    public sealed partial class TerrainTowerEntity
    {
        /// <summary>
        /// UnsortedQuantity = Result from Mining
        /// SortedQuantity = Trickled from UnsortedQuantity, and then moved to Buffer
        /// Buffer = Storage to output to external entities (Conveyors/Storages)
        /// </summary>
        [GenerateSerializer]
        internal class TerrainTowerProductData
        {
            /// <summary>
            /// Buffer to handle movement to External
            /// </summary>
            public readonly GlobalOutputBuffer Buffer;

            /// <summary>
            /// Bool Flag if the product can be affected by ConversionLoss
            /// </summary>
            public readonly bool CanBeWasted;

            private static readonly string[] s_defaultAlert = { "Rock", "Dirt" };

            private static readonly string[] s_lossSafeProducts = { "Rock", "Gravel", "Slag", "Dirt" };

            public TerrainTowerProductData(GlobalOutputBuffer buffer, char outputPort)
            {
                Buffer = buffer;
                OutputPort = outputPort;
                ProductProto product = buffer.Product;
                SortedLastMonth = Quantity.Zero;
                SortedThisMonth = Quantity.Zero;
                //Set defaults
                //Rock/Gravel/Slag/Dirt cannot have conversion loss
                //IsWaste cannot have conversion loss
                //Automatically set FullOutput alert for Rock & Dirt

                CanBeWasted = !product.IsWaste && !s_lossSafeProducts.Contains(product.Id.Value);
                NotifyIfFullOutput = s_defaultAlert.Contains(product.Id.Value);
            }

            /// <summary>
            /// Flag to notify if the product is blocked
            /// </summary>
            public bool NotifyIfFullOutput { get; set; }

            /// <summary>
            /// CHAR identifier of the selected output port
            /// </summary>
            public char OutputPort { get; set; }

            public Quantity SortedLastMonth { get; set; }

            /// <summary>
            /// Processed Quantity pending move to buffer (Should be 0 or close to)
            /// </summary>
            public Quantity SortedQuantity { get; set; }

            public Quantity SortedThisMonth { get; set; }

            /// <summary>
            /// Quantity lost from ConversionLoss
            /// </summary>
            public PartialQuantity ToWaste { get; set; }

            /// <summary>
            /// Storage of Mining Result
            /// </summary>
            public Quantity UnsortedQuantity { get; set; }

            public void OnNewMonth()
            {
                SortedLastMonth = SortedThisMonth;
                SortedThisMonth = Quantity.Zero;
            }

            /// <summary>
            /// Move from SortedQuantity to Buffer
            /// </summary>
            /// <returns>TRUE if any product remains to be sorted</returns>
            internal bool MoveSortedQuantityToBuffer()
            {
                Quantity quantity = Buffer.StoreAsMuchAsReturnStored(SortedQuantity);
                if (quantity.IsPositive)
                {
                    SortedQuantity -= quantity;
                }
                return SortedQuantity.IsPositive;
            }

            /// <summary>
            /// Move from Mining Result (UnsortedQuantity) to SortedQuantity
            /// Called externally to trickle sort via SimUpdate
            /// </summary>
            /// <param name="quantity">Quantity to move</param>
            internal void SortQuantity(Quantity quantity)
            {
                UnsortedQuantity -= quantity;
                SortedQuantity += quantity;
            }

            #region SERIALISATION

            private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
            private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;

            static TerrainTowerProductData()
            {
                s_serializeDataDelayedAction = (obj, writer) => ((TerrainTowerProductData)obj).SerializeData(writer);
                s_deserializeDataDelayedAction = (obj, reader) => ((TerrainTowerProductData)obj).DeserializeData(reader);
            }

            public static TerrainTowerProductData Deserialize(BlobReader reader)
            {
                if (reader.TryStartClassDeserialization(out TerrainTowerProductData productData))
                {
                    reader.EnqueueDataDeserialization(productData, s_deserializeDataDelayedAction);
                }
                return productData;
            }

            public static void Serialize(TerrainTowerProductData productData, BlobWriter writer)
            {
                if (writer.TryStartClassSerialization(productData))
                {
                    writer.EnqueueDataSerialization(productData, s_serializeDataDelayedAction);
                }
            }

            protected virtual void DeserializeData(BlobReader reader)
            {
                reader.SetField(this, nameof(Buffer), GlobalOutputBuffer.Deserialize(reader));
                SortedQuantity = Quantity.Deserialize(reader);
                UnsortedQuantity = Quantity.Deserialize(reader);
                reader.SetField(this, nameof(CanBeWasted), reader.ReadBool());
                NotifyIfFullOutput = reader.ReadBool();
                OutputPort = reader.ReadChar();
                SortedThisMonth = Quantity.Deserialize(reader);
                SortedLastMonth = Quantity.Deserialize(reader);
            }

            protected virtual void SerializeData(BlobWriter writer)
            {
                GlobalOutputBuffer.Serialize(Buffer, writer);
                Quantity.Serialize(SortedQuantity, writer);
                Quantity.Serialize(UnsortedQuantity, writer);
                writer.WriteBool(CanBeWasted);
                writer.WriteBool(NotifyIfFullOutput);
                writer.WriteChar(OutputPort);
                Quantity.Serialize(SortedThisMonth, writer);
                Quantity.Serialize(SortedLastMonth, writer);
            }

            #endregion SERIALISATION
        }
    }
}