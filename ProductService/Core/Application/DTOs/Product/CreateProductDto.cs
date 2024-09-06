﻿namespace Application.DTOs.Product
{
    public class CreateProductDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public Guid ProducerId { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
