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
        public async Task<ActionResult> GetUndeliveredOrders(string email)
        {
            List<OrderDTO> retOrders = await _orderService.GetUndeliveredOrders(email);
            if(retOrders == null)
            {
                return NotFound();
            }
            return Ok(retOrders);
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

    }
}
