using Microsoft.AspNetCore.Components;
using ShopOnline.Models;
using ShopOnline.Web.Services.Contract;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ProductBase : ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; }
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
        public IEnumerable<ProductDto> Products { get; set; }
        public IOrderedEnumerable<IGrouping<int, ProductDto>> GetGroupedProductByCategpry()
        {
            var groupedProductByCategory = from product in Products
                                           group product by product.CategoryId into prodByCatGroup
                                           orderby prodByCatGroup.Key
                                           select prodByCatGroup;
            return groupedProductByCategory;
        }
        public string GetCategoryName(IGrouping<int, ProductDto> groupItem)
        {
            return groupItem.FirstOrDefault(x => x.CategoryId == groupItem.Key).Name;
        }
        protected override async Task OnInitializedAsync()
        {
            Products = await ProductService.GetItems();
            var shopCartItems = await ShoppingCartService.GetItems(HardCode.UserId);
            var totalQty = shopCartItems.Sum(x => x.Qty);

            ShoppingCartService.RaiseEventOnShoppingCartChanged(totalQty);
        }


    }
}
