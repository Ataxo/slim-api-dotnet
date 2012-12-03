using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ataxo.SlimAPIClient.Entities.Constants
{
  /// <summary>
  /// common operators for request query
  /// </summary>
  public class QueryOperators
  {
    public static string Equals = "=";
    public static string Not_equals = "not_eq";
    public static string Like = "matches";
    public static string NotLike = "does_not_match";
    public static string LowerThan = "lt";
    public static string LowerThanOrEqual = "lte";
    public static string GreatherThan = "gt";
    public static string GreatherThanOrEqual = "gte";
    public static string Containing = "in";
    public static string NotContaining = "not_in";
    public static string From = "from";
    public static string To = "to";
  }
}
