using System.ComponentModel.DataAnnotations;

namespace Smartphone_Store.Models
{
    public class Product
    {
        public int ProductID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Brand { get; set; }

        [StringLength(50)]
        public string ModelName { get; set; }

        [Required]
        [Range(0, 10000)]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        public ICollection<Image> Images { get; set; } = new List<Image>();

        [StringLength(2000)]
        public string Description { get; set; }

        // Specifications
        [StringLength(20)]
        public string SimType { get; set; }

        [Range(0, 256)]
        public int RAMSize { get; set; }

        [Range(0, 1024)]
        public int ROMSize { get; set; }

        public bool MicroSDsupport { get; set; }

        [StringLength(50)]
        public string ProcessorType { get; set; }

        [StringLength(50)]
        public string OperatingSystem { get; set; }

        [StringLength(50)]
        public string DisplayType { get; set; }

        [StringLength(20)]
        public string DisplayResolution { get; set; }

        [StringLength(20)]
        public string BluetoothType { get; set; }

        [StringLength(20)]
        public string WiFiType { get; set; }

        public bool NFCsupport { get; set; }

        [StringLength(100)]
        public string BatterySpecs { get; set; }

        [StringLength(100)]
        public string BackCameraSpecs { get; set; }

        [StringLength(100)]
        public string FrontCameraSpecs { get; set; }

        [StringLength(50)]
        public string Dimensions { get; set; }

        [StringLength(500)]
        public string ExtraDetails { get; set; }
    }
}