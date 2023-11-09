using Microsoft.AspNetCore.Components;
using ShopOnline.Models;
using ShopOnline.Web.Services;
using ShopOnline.Web.Services.Contract;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ProductDetailsBase : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }
        [Inject]
        public IProductService ProductService { get; set; }
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        public ProductDto Product { get; set; }
        public string ErrorMessage { get; set; }
        private List<CartItemDto> ShoppingCartItems { get; set; }

        //[Inject]
        //public IManageProductsLocalStorageService ManageProductsLocalStorageService { get; set; }

        //[Inject]
        //public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService { get; set; }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                Product = await ProductService.GetItem(Id);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        protected async Task AddToCart_Click(CartItemToAddDto cartItemToAddDto)
        {
            try
            {
                var cartItemDto = await ShoppingCartService.AddItem(cartItemToAddDto);

                //if (cartItemToAddDto != null)
                //{
                //    ShoppingCartItems.Add(cartItemToAddDto);
                //    //await ManageCartItemsLocalStorageService.SaveCollection(ShoppingCartItems);
                //}

                NavigationManager.NavigateTo("/ShoppingCart");
            }
            catch (Exception ex)
            {

                //Log Exception
            }
        }

        //private async Task<ProductDto> GetProductById(int id)
        //{
        //    var productDtos = await ManageProductsLocalStorageService.GetCollection();

        //    if (productDtos != null)
        //    {
        //        return productDtos.SingleOrDefault(p => p.Id == id);
        //    }
        //    return null;
        //}
    }
}
