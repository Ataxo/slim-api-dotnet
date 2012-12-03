using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ataxo.SlimAPIClient.Entities.Constants
{
  /// <summary>
  /// Attributte for marking constant class as factoriable in reflection
  /// </summary>
  public class KeysAttributte : Attribute
  {
    public string model;
    public bool factoriable;
  }
}
