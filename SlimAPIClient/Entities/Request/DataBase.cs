using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ataxo.SlimAPIClient.Entities.Request
{
  /// <summary>
  /// basic parameters to connect slim api
  /// </summary>
  public interface IDataBase
  {
    string AccessToken { get; }
    string DomainUrl { get; }
    string ApiVersion { get; }
    string Taxonomy { get; }
  }
}
