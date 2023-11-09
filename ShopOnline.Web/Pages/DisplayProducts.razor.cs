using Microsoft.AspNetCore.Components;
using ShopOnline.Models;

namespace ShopOnline.Web.Pages
{
    public class DisplayProductsBase : ComponentBase
    {
        [Parameter]
        public IEnumerable<ProductDto> ProductList { get; set; }

        
    }
}
