using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ataxo.SlimAPIClient.Entities.Constants;

namespace Ataxo.SlimAPIClient.Entities.Request.Query
{
  /// <summary>
  /// specifies sub parameter of query
  /// </summary>
  public class QueryItem
  {
    public string Parameter { get; set; }
    public string QueryOperator { get; set; }
    public IEnumerable<object> Values { get; set; }

    /// <summary>
    /// contructor
    /// </summary>
    /// <param name="parameter">parameter anme</param>
    /// <param name="queryOperator">operator of condition</param>
    /// <param name="values">at least one value of parameter, if more than one is set, values are joined by ','</param>
    public QueryItem(string parameter, string queryOperator, IEnumerable<object> values)
    {
      Parameter = parameter;
      QueryOperator = queryOperator;
      Values = values;
    }

    public QueryItem() { }

    public QueryItem(string parameter, string queryOperator, object value) : this(parameter, queryOperator, new List<object> { value }) { }

    /// <summary>
    /// constructs query string from this object
    /// </summary>
    /// <returns></returns>
    public string GenerateSubQuery()
    {
      if (QueryOperator == QueryOperators.Equals)
      {
        return string.Format("{0}={1}", Parameter, string.Join(",", Values));
      }
      else
      {
        return string.Format("{0}.{1}={2}", Parameter, QueryOperator, string.Join(",", Values));
      }
    }
  }
}
