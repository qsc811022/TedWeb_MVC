using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TedWeb.DataAccess.Data;
using TedWeb.DataAccess.Repository.IRepository;
using TedWeb.Model.Models;
using TedWeb.DataAccess.Repository.IRepository;
using TedWeb.Model;

namespace TedWeb.DataAccess.Repository
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private ApplicationDbContext _db;
        public OrderDetailRepository(ApplicationDbContext db):base(db)
        {
            _db=db;
        }   



        public void Update(OrderDetail obj)
        {
            _db.OrderDetails.Update(obj);
        }
    }
}
