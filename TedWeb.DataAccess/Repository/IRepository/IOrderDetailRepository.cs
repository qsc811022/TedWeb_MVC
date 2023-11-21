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
    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {
        void Update(OrderDetail obj);


    }
}
