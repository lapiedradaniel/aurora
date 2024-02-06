﻿using Backend.Domain.Entities.Base;
using Backend.Domain.Entities.Categories;
using Backend.Domain.Entities.ProductTypes;
using Backend.Domain.Entities.SubCategories;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Backend.Domain.Entities.Products
{
    [Table("Product")]
    public class Product : Model
    {
        [Required]
        public Guid TenantId { get; set; }
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string SKU { get; set; }
        public string GTIN { get; set; }
        
        public string Name { get; set; }
        public string? Description { get; set; }
        public double Value { get; set; }
        public double? TotalWeight { get; set; }
        public double? LiquidWeight { get; set; }

        public int ProductTypeId { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? SubCategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }
        [ForeignKey("SubCategoryId")]
        public virtual SubCategory? SubCategory { get; set; }
        [ForeignKey("ProductTypeId")]
        public virtual ProductType? ProductType { get; set; }


        public Product Create(Product product, Guid userId)
        {
            return new Product()
            {
                Id = Guid.NewGuid(),
                TenantId = product.TenantId,
                SKU = product.SKU, 
                GTIN = product.GTIN,
                Name = product.Name,
                Description = product.Description,
                ProductTypeId = product.ProductTypeId,
                Value = product.Value,
                TotalWeight = product.TotalWeight,
                LiquidWeight = product.LiquidWeight,
                CreatedBy = userId,
                Created = DateTime.UtcNow,
                Updated = null,
                UpdatedBy = null,
                Active = true
            };
        }

        public Product Update(Product product, Guid userId)
        {
            return new Product() // Updating the header info from the product.
            {
                TenantId = product.TenantId,
                Id = product.Id,
                Name = product.Name,
                SKU = product.SKU,
                Description = product.Description,
                ProductTypeId = product.ProductTypeId,
                Active = true,
                Updated = DateTime.Now,
                UpdatedBy = userId
            };
        }
    }

    public class ProductDetail : Product
    {
        public string? CategoryName { get; set; }
        public string? SubCategoryName { get; set; }
        public string ProductTypeName { get; set; }
    }
}
