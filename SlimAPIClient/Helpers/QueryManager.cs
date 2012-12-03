using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ataxo.SlimAPIClient.Entities.Request.Query;
using Ataxo.SlimAPIClient.Helpers;

namespace Ataxo.SlimAPIClient.Helpers
{
  public static class QueryManager
  {
    /// <summary>
    /// generates query string from array of query items and granularity
    /// </summary>
    /// <param name="queryItems"></param>
    /// <param name="granularity"></param>
    /// <returns></returns>
    public static string GenerateQuery(IEnumerable<QueryItem> queryItems, Granularity granularity)
    {
      IEnumerable<QueryItem> allQueries = granularity == null ? queryItems : JoinArrays(queryItems, ((Granularity)granularity).ToQueryItems());

      return allQueries == null ? null : string.Join("&", allQueries.ToList().Select(q => q.GenerateSubQuery()));
    }

    /// <summary>
    /// connects arrays of QueryItems together
    /// </summary>
    /// <param name="arrays"></param>
    /// <returns></returns>
    private static IEnumerable<QueryItem> JoinArrays(params IEnumerable<QueryItem>[] arrays)
    {
      foreach (IEnumerable<QueryItem> array in arrays)
      {
        foreach (QueryItem queryItem in array)
        {
          yield return queryItem;
        }
      }
    }
  }
}
