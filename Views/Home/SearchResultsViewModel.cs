using System.Collections.Generic;

namespace Smartphone_Store.Models
{
    public class SearchResultsViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public List<string> Brands { get; set; }
        public string[] SelectedBrands { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public string SearchQuery { get; set; }
        public string SortOrder { get; set; }
        public bool IsUserSpecifiedMinPrice { get; set; }
        public bool IsUserSpecifiedMaxPrice { get; set; }
    }
}