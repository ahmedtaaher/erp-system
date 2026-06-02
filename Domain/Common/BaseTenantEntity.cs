using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Common
{
  public abstract class BaseTenantEntity : BaseEntity
  {
    public Guid TenantId { get; set; }
  }
}