using Microsoft.AspNetCore.Mvc;

using TedWeb.DataAccess.Repository.IRepository;
using TedWeb.Model;

namespace TedWeb.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class OrderController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork=unitOfWork;
        }

        public IActionResult Index()
		{
			return View();
		}

		#region API Calls
		[HttpGet]
		public IActionResult GetAll() {
			List<OrderHeader> objOrderHeader = _unitOfWork.OrderHeader.GetAll(includeProperties:"ApplicationUser").ToList();
			return Json(new {data=objOrderHeader});
		}

		#endregion



	}
}