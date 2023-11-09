using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ShopOnline.Models;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ShoppingCartBase : ComponentBase
    {
        [Inject]
        public IJSRuntime JS { set; get; }
        [Inject]
        public IShoppingCartService shoppingCartService { get; set; }
        public List<CartItemDto> ShoppingCartItems { set; get; }
        public string ErrorMessage { get; set; }
        protected string TotalPrice { get; set; }
        protected int TotalQuantity { get; set; }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await shoppingCartService.GetItems(HardCode.UserId);
                CartChanged();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        protected async Task DeleteCartItem_Click(int id)
        {
            try
            {
                var cartItemDto = shoppingCartService.DeleteItem(id);
                RemoveCartItem(id);
                CartChanged();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        protected async Task UpdateQtyCartItem_Click(int id, int qty)
        {
            try
            {
                if (qty > 0)
                {
                    var updateItemDto = new CartItemQtyUpdateDto
                    {
                        CartItemId = id,
                        Qty = qty
                    };
                    var returnUpdateQtyCartItem = await shoppingCartService.UpdateQty(updateItemDto);

                    UpdateItemTotalPrice(returnUpdateQtyCartItem);
                    
                    await MakeUpdateQtyButtonVisible(id, false);

                    CartChanged();
                }
                else
                {
                    var item = ShoppingCartItems.FirstOrDefault(x => x.Id == id);
                    if (item != null)
                    {
                        item.Qty = 1;
                        item.Price = item.Price;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        private CartItemDto GetCartItem(int id)
        {
            return ShoppingCartItems.FirstOrDefault(x => x.Id == id);
        }
        private void RemoveCartItem(int id)
        {
            var removeCartItem = GetCartItem(id);
            ShoppingCartItems.Remove(removeCartItem);
        }

        private void CalculateCartSummaryTotals()
        {
            SetTotalPrice();
            SetTotalQuantity();
        }

        private void SetTotalPrice()
        {
            TotalPrice = this.ShoppingCartItems.Sum(p => p.TotalPrice).ToString("C");
        }
        private void SetTotalQuantity()
        {
            TotalQuantity = this.ShoppingCartItems.Sum(p => p.Qty);
        }

        private void UpdateItemTotalPrice(CartItemDto cartItemDto)
        {
            var item = GetCartItem(cartItemDto.Id);

            if (item != null)
            {
                item.TotalPrice = cartItemDto.Price * cartItemDto.Qty;
            }
        }

        public async Task UpdateQty_OnChange(int id)
        {
            await MakeUpdateQtyButtonVisible(id, true);
        }

        public async Task MakeUpdateQtyButtonVisible(int id,bool visible)
        {
            await JS.InvokeVoidAsync("MakeUpdateQtyButtonVisible", id, visible);
        }

        private void CartChanged()
        {
            CalculateCartSummaryTotals();
            shoppingCartService.RaiseEventOnShoppingCartChanged(TotalQuantity);
        }
    }
}
