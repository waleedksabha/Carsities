﻿namespace AuctionService.DTOs;

public class AuctionDto
{
    public Guid Id { get; set; }
        public int ReservePrice { get; set; }  
        public string Seller { get; set; }
        public string Winner { get; set; }
        public int SoldAmount { get; set; }
        public int CurrentHighBid { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public DateTime AuctionDate { get; set; }
        public string MyProperty { get; set; }  //status
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
        public int Mileage { get; set; }
        public string ImageUrl { get; set; }
}
