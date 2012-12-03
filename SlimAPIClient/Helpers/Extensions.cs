using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using Ataxo.SlimAPIClient.Entities.Constants;
using Ataxo.SlimAPIClient.Entities.Request.Query;
using System.Net;
using System.IO;

using System.Threading;
using System.Globalization;

namespace Ataxo.SlimAPIClient.Helpers
{
  public static class Extensions
  {
    private static JavaScriptSerializer _jsonSerializer = new JavaScriptSerializer() { MaxJsonLength = int.MaxValue };

    public static IDictionary<string, object> ToDict(this string jsonString)
    {
      Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
      return (IDictionary<string, object>)_jsonSerializer.DeserializeObject(jsonString);
    }

    public static string ToJson(this IDictionary<string, object> data)
    {
      return (string)_jsonSerializer.Serialize(data);
    }

    /// <summary>
    /// alows convert granularity object to aueryitem array equivalent
    /// </summary>
    /// <param name="granularity"></param>
    /// <returns></returns>
    public static IEnumerable<QueryItem> ToQueryItems(this Granularity granularity)
    {
      if (granularity != null)
      {
        if (granularity.Offset != null)
        {
          yield return new QueryItem(GranularityQuery.Offset, QueryOperators.Equals, new List<object>() { granularity.Offset });
        }
        if (granularity.Limit != null)
        {
          yield return new QueryItem(GranularityQuery.Limit, QueryOperators.Equals, new List<object>() { granularity.Limit });
        }
        if (granularity.PropertyOrders != null)
        {
          yield return new QueryItem(GranularityQuery.Order, QueryOperators.Equals, string.Join(",", granularity.PropertyOrders.Select(po => po.ToString())));
        }
      }
    }

    public static void SetStreamData(this HttpWebRequest request, IDictionary<string, object> data)
    {
      if (data == null)
      {
        return;
      }

      string json = data.ToJson();
      byte[] dataStream = Encoding.UTF8.GetBytes(json);

      using (Stream requestStream = request.GetRequestStream())
      {
        requestStream.Write(dataStream, 0, dataStream.Length);
      }
    }
  }
}
