using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReGreenShop.Domain.Entities;

namespace ReGreenShop.Application.Products.Models;
public class ProductScoreModel
{
    public Product Product { get; set; }
    public int Score { get; set; }
}
