using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TedWeb.DataAccess.Repository.IRepository;
using TedWeb.Model;
using TedWeb.Model.ViewModels;
using TedWeb.Utility;


namespace TedWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
		[BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            ShoppingCartVM = new ShoppingCartVM()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u=>u.ApplicationUserId==userId, includeProperties:"Product"),
                OrderHeader =new()
            };
			foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            return View(ShoppingCartVM);
        }

        private double GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
        {
            if (shoppingCart.Count <= 50)
            {
                return shoppingCart.Product.Price;
            }
            else
            {
                if (shoppingCart.Count <= 100)
                {
                    return shoppingCart.Product.Price50;
                }
                else
                {
                    return shoppingCart.Product.Price100;
                }
            }
        }

        public IActionResult Plus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            cartFromDb.Count += 1;
            _unitOfWork.ShoppingCart.Update(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            if (cartFromDb.Count <= 1)
            {
                //remove that from cart
                _unitOfWork.ShoppingCart.Remove(cartFromDb);
            }
            else
            {
                cartFromDb.Count -= 1;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
            }

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Summary() 
        {
			var claimIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
			ShoppingCartVM = new ShoppingCartVM()
			{
				ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product"),
				OrderHeader = new()
			};

			ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
			ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
			ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
			ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
			ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
			ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
			ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;
			foreach (var cart in ShoppingCartVM.ShoppingCartList)
			{
				cart.Price = GetPriceBasedOnQuantity(cart);
				ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
			}
			return View(ShoppingCartVM);
        }

        public IActionResult Remove(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);

            _unitOfWork.ShoppingCart.Remove(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

		[HttpPost]
		[ActionName("Summary")]
		public IActionResult SummaryPOST(ShoppingCartVM shoppingCartVM)
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

			ShoppingCartVM.ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
				includeProperties: "Product");

			ShoppingCartVM.OrderHeader.OrderDate = System.DateTime.Now;
			ShoppingCartVM.OrderHeader.ApplicationUserId = userId;

			ApplicationUser applicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);


			foreach (var cart in ShoppingCartVM.ShoppingCartList)
			{
				cart.Price = GetPriceBasedOnQuantity(cart);
				ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
			}

			//if (applicationUser.CompanyId.GetValueOrDefault() == 0)
			//{
			//	//it is a regular customer 
			//	ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
			//	ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
			//}
			//else
			//{
			//	//it is a company user
			//	ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
			//	ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusApproved;
			//}
			//_unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
			//_unitOfWork.Save();
			//foreach (var cart in ShoppingCartVM.ShoppingCartList)
			//{
			//	OrderDetail orderDetail = new()
			//	{
			//		ProductId = cart.ProductId,
			//		OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
			//		Price = cart.Price,
			//		Count = cart.Count
			//	};
			//	_unitOfWork.OrderDetail.Add(orderDetail);
			//	_unitOfWork.Save();
			//}

			//if (applicationUser.CompanyId.GetValueOrDefault() == 0)
			//{
			//	//it is a regular customer account and we need to capture payment
			//	//stripe logic
			//	var domain = Request.Scheme + "://" + Request.Host.Value + "/";
			//	var options = new SessionCreateOptions
			//	{
			//		SuccessUrl = domain + $"customer/cart/OrderConfirmation?id={ShoppingCartVM.OrderHeader.Id}",
			//		CancelUrl = domain + "customer/cart/index",
			//		LineItems = new List<SessionLineItemOptions>(),
			//		Mode = "payment",
			//	};

				foreach (var cart in ShoppingCartVM.ShoppingCartList)
				{
					cart.Price = GetPriceBasedOnQuantity(cart);
					shoppingCartVM.OrderHeader.OrderTotal +=(cart.Price*cart.Count);
					//var sessionLineItem = new SessionLineItemOptions
					//{
					//	PriceData = new SessionLineItemPriceDataOptions
					//	{
					//		UnitAmount = (long)(item.Price * 100), // $20.50 => 2050
					//		Currency = "usd",
					//		ProductData = new SessionLineItemPriceDataProductDataOptions
					//		{
					//			Name = item.Product.Title
					//		}
					//	},
					//	Quantity = item.Count
					//};
					//options.LineItems.Add(sessionLineItem);
				}
				if (ShoppingCartVM.OrderHeader.ApplicationUser.CompanyId.GetValueOrDefault()==0)
				{
				//it is a regular customer account and we need to capture payment
				ShoppingCartVM.OrderHeader.PaymentStatus = SD.StatusPending;
				ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;

			}
			else
				{
				//it is a company user
				ShoppingCartVM.OrderHeader.PaymentStatus = SD.StatusPending;
				ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;

			}
				_unitOfWork.OrderHeader.Add(shoppingCartVM.OrderHeader);
				_unitOfWork.Save();

            foreach (var cart in shoppingCartVM.ShoppingCartList)
            {
				OrderDetail orderDetail = new OrderDetail()
				{
					ProductId=cart.ProductId,
					OrderHeaderId=ShoppingCartVM.OrderHeader.Id,
					Price=cart.Price,
					Count=cart.Count,
					
				};
                _unitOfWork.OrderDetail.Add(orderDetail);
				_unitOfWork.Save();
            }


            //var service = new SessionService();
            //Session session = service.Create(options);
            //_unitOfWork.OrderHeader.UpdateStripePaymentID(ShoppingCartVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
            //_unitOfWork.Save();
            //Response.Headers.Add("Location", session.Url);
            //return new StatusCodeResult(303);
            return View(shoppingCartVM);

			}
		public IActionResult OrderConfirmation(int id)
		{
			return View(id);
		}
	}

}

