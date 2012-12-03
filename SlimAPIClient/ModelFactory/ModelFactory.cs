using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ataxo.SlimAPIClient.Entities.Constants;
using System.Reflection;

namespace Ataxo.SlimAPIClient.ModelFactory
{
  /// <summary>
  /// singelton class for generating empty model name-value collection
  /// uses lazy values and depends on KeysAttributte attributtes used in model parameters mapping
  /// </summary>
  public class ModelGenerator
  {
    private static ModelGenerator _instance;
    private static Dictionary<string, Lazy<Dictionary<string, object>>> _modelGenerators;
    private static Dictionary<string, Lazy<Dictionary<string, object>>> ModelGenerators
    {
      get
      {
        if (_instance == null)
        {
          _instance = new ModelGenerator();
        }

        return _modelGenerators;
      }
    }

    private ModelGenerator()
    {
      RegisterModelDictionaries();
    }

    /// <summary>
    /// basicly initialises name-value collections as a reflection of classes using KeysAttributte attributte
    /// </summary>
    protected virtual void RegisterModelDictionaries()
    {
      List<Type> types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetCustomAttributes(typeof(KeysAttributte), false).Length > 0
                                                                            && ((KeysAttributte)t.GetCustomAttributes(typeof(KeysAttributte), false)[0]).factoriable).ToList();

      _modelGenerators = new Dictionary<string, Lazy<Dictionary<string, object>>>();

      foreach (Type type in types)
      {
        string modelName = ((KeysAttributte)type.GetCustomAttributes(typeof(KeysAttributte), false)[0]).model;
        _modelGenerators.Add(modelName, GenerateLazyModelDictionary(type));
      }
    }

    private Lazy<Dictionary<string, object>> GenerateLazyModelDictionary(Type type)
    {
      return new Lazy<Dictionary<string, object>>(() =>
        type.GetFields().Select(f => new KeyValuePair<string, object>((string)f.GetValue(null), null)).ToDictionary(k => k.Key, k => k.Value)
      );
    }

    /// <summary>
    /// generates empty model name-value collection
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public static Dictionary<string, object> GenerateModelDictionary(string model)
    {
      if (!ModelGenerators.ContainsKey(model))
      {
        throw new NotImplementedException(string.Format("There is no attributted constant class for model: {0}, pls create one for factoriation", model));
      }

      return ModelGenerators[model].Value;
    }
  }
}
