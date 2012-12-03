using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ataxo.SlimAPIClient.Entities;
using System.Web.Script.Serialization;
using Ataxo.SlimAPIClient.Helpers;

namespace Ataxo.SlimAPIClient.Entities.Response
{
  /// <summary>
  /// request result representation
  /// </summary>
  public class ResponseData
  {
    public string ResponseString { get; set; }
    public IDictionary<string, object> ResponseParams { get; set; }

    /// <summary>
    /// inits json from request as name-value collection
    /// </summary>
    /// <param name="jsonString"></param>
    public ResponseData(string jsonString)
    {
      ResponseParams = ParseJson(jsonString);
    }

    protected virtual IDictionary<string, object> ParseJson(string jsonString)
    {
      ResponseString = jsonString;
      return jsonString.ToDict();
    }
  }
}
