using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ataxo.SlimAPIClient.Entities.Constants
{
  /// <summary>
  /// basic ordering
  /// </summary>
  public enum Ordering
  {
    Ascending,
    Descending
  }

  public class PropertyOrder
  {
    public string PropertyName { get; set; }
    public Ordering Order { get; set; }

    public override string ToString()
    {
      return string.Format("{0} {1}", PropertyName, Order == Ordering.Ascending ? "asc" : "desc");
    }
  }
}
