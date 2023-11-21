using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TedWeb.DataAccess.Repository.IRepository;
using TedWeb.Model;
using TedWeb.Model.Models;

namespace TedWeb.DataAccess.Repository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void Update(OrderHeader obj);
		void UpdateStatus(int id, string orderStatus, string? paymentStatus = null);
		void UpdateStripePaymentID(int id, string sessionId, string paymentIntentId);


	}
}
