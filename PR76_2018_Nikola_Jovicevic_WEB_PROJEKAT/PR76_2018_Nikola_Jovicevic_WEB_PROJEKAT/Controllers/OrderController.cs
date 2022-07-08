using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.DTOs;
using PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private readonly IOrder _orderService;

        public OrderController(IOrder orderService)
        {
            this._orderService = orderService;
        }


        // POST api/<OrderController>
        [HttpPost("create-order")]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> AnounceOrder([FromBody] OrderDTO orderDto)
        {
            await _orderService.AnounceOrder(orderDto);
            return Ok();
        }

        [HttpGet("get-all")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllOrders()
        {
            List<OrderDTO> retOrders = await _orderService.GetAllOrders();
            if(retOrders == null)
            {
                return NotFound();
            }
            return Ok(retOrders);
        }

        [HttpGet("{email}")]
        [Authorize(Roles = "customer")]
        public async Task<ActionResult> GetOrdersForUser(string email)
        {
            List<OrderDTO> retOrders = await _orderService.GetOrdersForUser(email);
            if (retOrders == null)
            {
                return NotFound();
            }
            return Ok(retOrders);
        }

        [HttpGet("current-orders/{email}")]
        [Authorize(Roles = "customer")]
        public async Task<ActionResult> GetUndeliveredOrder(string email)
        {
            OrderDTO retOrder = await _orderService.GetUndeliveredOrder(email);
            if(retOrder == null)
            {
                return NotFound();
            }
            return Ok(retOrder);
        }

        [HttpGet("available-orders")]
        [Authorize(Roles = "deliverer")]
        public async Task<ActionResult> GetAvailableOrders()
        {
            List<OrderDTO> retOrders = await _orderService.GetAvailableOrders();
            if(retOrders == null)
            {
                return NotFound();
            }
            return Ok(retOrders);
        }

        [HttpPost("take-order")]
        [Authorize(Roles = "deliverer")]
        public async Task<IActionResult> TakeOrder([FromBody] OrderTakeDTO data)
        {
            //await _orderService.AnounceOrder(orderDto);
            OrderDTO order = await _orderService.TakeOrder(data);
            return Ok(order);
        }

        [HttpGet("taken-order/{email}")]
        [Authorize(Roles = "deliverer")]
        public async Task<ActionResult> GetTakenOrder(string email)
        {
            OrderDTO retOrder = await _orderService.GetTakenOrder(email);
            if (retOrder == null)
            {
                return NotFound();
            }
            return Ok(retOrder);
        }

        [HttpGet("time-remaining/{deliveryId}")]
        [Authorize(Roles = "deliverer, customer")]
        public async Task<ActionResult> GetSecondsUntilDelivery(int deliveryId)
        {
            double? retVal = await _orderService.GetSecondsUntilDelivery(deliveryId);
            if (retVal == null)
            {
                return NotFound();
            }
            return Ok(retVal);
        }

        [HttpPost("finish-delivery")]
        [Authorize(Roles ="deliverer")]
        public async Task<ActionResult> FinishDelivery(OrderDTO orderDto)
        {
            bool retVal = await _orderService.FinishDelivery(orderDto);
            if (retVal != true)
            {
                return NotFound();
            }
            return Ok(retVal);
        }

        [HttpGet("my-deliveries/{email}")]
        [Authorize(Roles = "deliverer")]
        public async Task<ActionResult> GetMyDeliveries(string email)
        {
            List<OrderDTO> orders = await _orderService.GetMyDeliveries(email);
            if(orders == null)
            {
                return NotFound();
            }
            return Ok(orders);
        }
    }
}
