﻿using Microsoft.AspNetCore.Authorization;
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
        public async Task<ActionResult> GetFood(string email)
        {
            List<OrderDTO> retOrders = await _orderService.GetOrdersForUser(email);
            return Ok(retOrders);
        }

    }
}
