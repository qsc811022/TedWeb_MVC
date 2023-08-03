using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Diagnostics;
using TedWeb.DataAccess.Repository.IRepository;
using TedWeb.Model;


namespace TedWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {


        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;


        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList=_unitOfWork.Product.GetAll(includeProperties:"Category");
            return View(productList);
        }


        public IActionResult Details(int productId)
        {
            //IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category");
            ShoppingCart cart = new()
            {
                Product=_unitOfWork.Product.Get(u=>u.Id==productId,includeProperties:"Category"),
                Count=1,
                ProductId=productId
            };
            //Product product=_unitOfWork.Product.Get(u=>u.Id== productId, includeProperties: "Category");
            return View(cart);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}