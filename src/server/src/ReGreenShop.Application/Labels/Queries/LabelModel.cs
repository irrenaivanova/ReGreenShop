using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReGreenShop.Application.Common.Mappings;
using ReGreenShop.Domain.Entities;

namespace ReGreenShop.Application.Labels.Queries;
public class LabelModel : IMapFrom<Label>
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
}
