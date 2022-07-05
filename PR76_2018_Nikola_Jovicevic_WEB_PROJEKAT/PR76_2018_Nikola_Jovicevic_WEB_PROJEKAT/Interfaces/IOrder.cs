using PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.Interfaces
{
    public interface IOrder
    {
        Task AnounceOrder(OrderDTO order);
        Task<List<OrderDTO>> GetOrdersForUser(string email);
        Task<List<OrderDTO>> GetUndeliveredOrders(string email);
    }
}
