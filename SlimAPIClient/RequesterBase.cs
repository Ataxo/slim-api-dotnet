using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ataxo.SlimAPIClient.Entities;
using Ataxo.SlimAPIClient.Entities.Constants;
using Ataxo.SlimAPIClient.Entities.Response;
using Ataxo.SlimAPIClient.Entities.Request;
using Ataxo.SlimAPIClient.Entities.Request.Query;
using System.Net;

namespace Ataxo.SlimAPIClient
{
  /// <summary>
  /// determines, what requester should implement to make accessing slim data comfortable
  /// </summary>
  public interface IRequesterBase
  {
    IDataBase PropertiesData { get; }
    ResponseData SendRequestSync(RequestMethod requestMethod, string model, string additional_concretization = null, IDictionary<string, object> requestParams = null, IEnumerable<QueryItem> queryItems = null, Granularity granularity = null);
    void SendRequestAsync(RequestMethod requestMethod, string model, string additional_concretization = null, IDictionary<string, object> requestParams = null, Action<WebResponse, ResponseData> listener = null, IEnumerable<QueryItem> queryItems = null, Granularity granularity = null);
    void RespCallback(IAsyncResult asynchronousResult);
    Dictionary<string, object> GenerateModelDictionary(string model);
  }
}
