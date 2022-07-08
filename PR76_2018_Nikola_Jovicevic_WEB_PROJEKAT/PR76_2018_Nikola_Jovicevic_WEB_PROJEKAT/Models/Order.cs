using System;
using System.Collections.Generic;

namespace PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.Models
{
    public class Order
    {
        public Order() { }
        public int Id { get; set; }
        public string UserEmail { get; set; }
        public string Comment { get; set; }
        public string Address { get; set; }
        public double Price { get; set; }
        public bool Accepted { get; set; }
        //public int DelivererId { get; set; }
        public User Deliverer { get; set; }
        public string DelivererEmail { get; set; }
        public DateTime TimePosted { get; set; }
        public DateTime? TimeAccepted { get; set; }
        public DateTime? TimeDelivered { get; set; }
        public bool Delivered { get; set; }
        // public List<Food> OrderedFood { get; set; }
    }
}
