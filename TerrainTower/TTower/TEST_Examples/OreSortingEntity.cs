using Mafi;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Static;
using Mafi.Core.Products;
using Mafi.Serialization;

using System;

#pragma warning disable

namespace OreSortingPlant
{
    [GenerateSerializer(false, null, 0)]
    public class OreSortingPlantProductData
    {
        public readonly GlobalOutputBuffer Buffer;
        public readonly bool CanBeWasted;

        [NewInSaveVersion(180, null, null, null, null)]
        public ImmutableArray<char> OutputPorts;

        public OreSortingPlantProductData(GlobalOutputBuffer buffer, ImmutableArray<char> ports) : base()
        {
            this.Buffer = buffer;
            ProductProto product = buffer.Product;
            this.OutputPorts = ports;
            this.CanBeWasted = !product.IsWaste && !product.Id.Value.Contains("Rock") && !product.Id.Value.Contains("Gravel") && !product.Id.Value.Contains("Slag") && !product.Id.Value.Contains("Dirt");
            this.NotifyIfFullOutput = product.Id.Value.Contains("Rock") || product.Id.Value.Contains("Dirt");
        }

        public Quantity BeingSorted { get; set; }

        public bool CanAcceptMoreTrucks
        {
            get
            {
                return this.Buffer.Quantity + this.UnsortedQuantity + this.Reserved <= this.Buffer.Capacity;
            }
        }

        public bool CanAcceptMoreTrucksForUi
        {
            get => this.Buffer.Quantity + this.UnsortedQuantity <= this.Buffer.Capacity;
        }

        [NewInSaveVersion(156, null, null, null, null)]
        public bool NotifyIfFullOutput { get; set; }

        [Obsolete]
        [DoNotSave(180, null)]
        public char OutputPort { get; set; }

        [DoNotSave(0, null)]
        public Quantity Reserved { get; set; }

        public Quantity SortedLastMonth { get; set; }
        public Quantity SortedThisMonth { get; set; }
        public PartialQuantity ToWaste { get; set; }
        public Quantity UnsortedQuantity { get; set; }

        public void OnNewMonth()
        {
            this.SortedLastMonth = this.SortedThisMonth;
            this.SortedThisMonth = Quantity.Zero;
        }

        [InitAfterLoad(InitPriority.High)]
        private void initSelf(int saveVersion)
        {
            if (saveVersion >= 180)
                return;
            this.OutputPorts = ImmutableArray.Create<char>(this.OutputPort);
        }

        #region SERIALISATION

        private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

        private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;

        static OreSortingPlantProductData()
        {
            OreSortingPlant.OreSortingPlantProductData.s_serializeDataDelayedAction = (Action<object, BlobWriter>)((obj, writer) => ((OreSortingPlant.OreSortingPlantProductData)obj).SerializeData(writer));
            OreSortingPlant.OreSortingPlantProductData.s_deserializeDataDelayedAction = (Action<object, BlobReader>)((obj, reader) => ((OreSortingPlant.OreSortingPlantProductData)obj).DeserializeData(reader));
        }

        public static OreSortingPlant.OreSortingPlantProductData Deserialize(BlobReader reader)
        {
            OreSortingPlant.OreSortingPlantProductData plantProductData;
            if (reader.TryStartClassDeserialization<OreSortingPlant.OreSortingPlantProductData>(out plantProductData))
                reader.EnqueueDataDeserialization((object)plantProductData, OreSortingPlant.OreSortingPlantProductData.s_deserializeDataDelayedAction);
            return plantProductData;
        }

        public static void Serialize(
    OreSortingPlant.OreSortingPlantProductData value,
    BlobWriter writer)
        {
            if (!writer.TryStartClassSerialization<OreSortingPlant.OreSortingPlantProductData>(value))
                return;
            writer.EnqueueDataSerialization((object)value, OreSortingPlant.OreSortingPlantProductData.s_serializeDataDelayedAction);
        }

        protected virtual void DeserializeData(BlobReader reader)
        {
            this.BeingSorted = Quantity.Deserialize(reader);

            reader.SetField<OreSortingPlant.OreSortingPlantProductData>(this, "Buffer", (object)GlobalOutputBuffer.Deserialize(reader));
            reader.SetField<OreSortingPlant.OreSortingPlantProductData>(this, "CanBeWasted", (object)reader.ReadBool());

            this.NotifyIfFullOutput = reader.LoadedSaveVersion >= 156 && reader.ReadBool();

            if (reader.LoadedSaveVersion < 180)
                this.OutputPort = reader.ReadChar();
            this.OutputPorts = reader.LoadedSaveVersion >= 180 ? ImmutableArray<char>.Deserialize(reader) : new ImmutableArray<char>();

            this.SortedLastMonth = Quantity.Deserialize(reader);
            this.SortedThisMonth = Quantity.Deserialize(reader);
            this.ToWaste = PartialQuantity.Deserialize(reader);
            this.UnsortedQuantity = Quantity.Deserialize(reader);

            reader.RegisterInitAfterLoad<OreSortingPlant.OreSortingPlantProductData>(this, "initSelf", InitPriority.High);
        }

        protected virtual void SerializeData(BlobWriter writer)
        {
            Quantity.Serialize(this.BeingSorted, writer);
            GlobalOutputBuffer.Serialize(this.Buffer, writer);
            writer.WriteBool(this.CanBeWasted);
            writer.WriteBool(this.NotifyIfFullOutput);
            ImmutableArray<char>.Serialize(this.OutputPorts, writer);
            Quantity.Serialize(this.SortedLastMonth, writer);
            Quantity.Serialize(this.SortedThisMonth, writer);
            PartialQuantity.Serialize(this.ToWaste, writer);
            Quantity.Serialize(this.UnsortedQuantity, writer);
        }

        #endregion SERIALISATION
    }
}