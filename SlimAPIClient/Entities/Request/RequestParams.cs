using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ataxo.SlimAPIClient.Entities;

namespace Ataxo.SlimAPIClient.Entities.Request
{
  /// <summary>
  /// class of slim api connecting parameters
  /// </summary>
  public class RequestParams : IDataBase
  {
    private readonly string _acessToken;
    public string AccessToken
    {
      get
      {
        return _acessToken;
      }
    }

    private readonly string _domainUrl;
    public string DomainUrl
    {
      get
      {
        return _domainUrl;
      }
    }

    private readonly string _apiVersion;
    public string ApiVersion
    {
      get
      {
        return _apiVersion;
      }
    }

    private readonly string _taxonomy;
    public string Taxonomy
    {
      get
      {
        return _taxonomy;
      }
    }

    public RequestParams(string accessToken, string domainUrl, string apiVersion, string taxonomy)
    {
      _acessToken = accessToken;
      _domainUrl = domainUrl;
      _apiVersion = apiVersion;
      _taxonomy = taxonomy;
    }
  }
}
