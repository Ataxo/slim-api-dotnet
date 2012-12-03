using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ataxo.SlimAPIClient.Entities.Constants;

namespace Ataxo.SlimAPIClient.Entities.Request.Query
{
  /// <summary>
  /// specifies granularity for request
  /// if any parameter is not set default value appears
  /// </summary>
  public class Granularity
  {
    public int? Limit;
    public int? Offset;
    //public Ordering? Ordering;
    public List<PropertyOrder> PropertyOrders;
  }
}