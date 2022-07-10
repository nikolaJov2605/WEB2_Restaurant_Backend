using PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.Interfaces
{
    public interface IOrder
    {
        Task AnounceOrder(OrderDTO order);
        Task<double> GetDeliveryFee();
        Task<List<OrderDTO>> GetAllOrders();
        Task<List<OrderDTO>> GetOrdersForUser(string email);
        Task<OrderDTO> GetUndeliveredOrder(string email);
        Task<List<OrderDTO>> GetAvailableOrders();
        Task<OrderDTO> TakeOrder(OrderTakeDTO data);
        Task<OrderDTO> GetTakenOrder(string email);
        Task<double?> GetSecondsUntilDelivery(int deliveryId);
        Task<bool> FinishDelivery(OrderDTO order);
        Task<List<OrderDTO>> GetMyDeliveries(string email);
    }
}
