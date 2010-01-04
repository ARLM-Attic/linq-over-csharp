using System.Collections.Generic;
using System.Linq;
using CSharpTreeBuilder.CSharpSemanticGraph;

namespace CSharpTreeBuilder.CSharpSemanticGraphBuilder
{
  // ================================================================================================
  /// <summary>
  /// This class implements the overload resolution algorithm described in the spec (§7.4.3).
  /// Overload resolution is a compile-time mechanism for selecting the best function member 
  /// to invoke given an argument list and a set of candidate function members.
  /// </summary>
  // ================================================================================================
  public static class OverloadResolver
  {
    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Selects the best function member from a set of candidates, given a list of arguments.
    /// </summary>
    /// <param name="candidates">The collection of candidate function members.</param>
    /// <param name="arguments">The collection of arguments.</param>
    /// <returns>The best function member from the candidates</returns>
    // ----------------------------------------------------------------------------------------------
    public static FunctionMemberEntity SelectBest(IEnumerable<FunctionMemberEntity> candidates, IEnumerable<ArgumentEntity> arguments)
    {
      FunctionMemberEntity result = null;

      foreach(var candidate in candidates)
      {
        if (candidate is IOverloadableEntity)
        {
          ((IOverloadableEntity) candidate).IsApplicable(arguments);
        }
      }

      return result;
    }

    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// Returns a value indicating whether an overloadable entity is compatible with a given argument list.
    /// </summary>
    /// <param name="entity">An overloadable entity.</param>
    /// <param name="arguments">The collection of arguments.</param>
    /// <returns>True if the entity is compatible with the argument list.</returns>
    /// <remarks>
    /// See spec 7.4.3.1 Applicable function member. The algorithm in the spec applies to all 
    /// function members, but in our model it's the IOverloadableEntity that is known to have parameters.
    /// </remarks>
    // ----------------------------------------------------------------------------------------------
    public static bool IsApplicable(this IOverloadableEntity entity, IEnumerable<ArgumentEntity> arguments)
    {
      bool isApplicable = false;

      // A function member is said to be an applicable function member with respect to an argument list A 
      // when all of the following are true:

      // The number of arguments in A is identical to the number of parameters in the function member declaration.
      if (arguments.Count() == entity.Parameters.Count())
      {
        var parameters = entity.Parameters.ToList();
        var i = 0;

        // For each argument in A, 
        foreach (var argument in arguments)
        {
          var argumentIsApplicable = false;

          // the parameter passing mode of the argument (i.e., value, ref, or out) 
          // is identical to the parameter passing mode of the corresponding parameter, and ...
          if (parameters[i].Kind == argument.Kind)
          {
            // for a value parameter or TODO: a parameter array, 
            // an implicit conversion (§6.1) exists from the argument to the type of the corresponding parameter, or
            if (argument.Kind == ParameterKind.Value
              && TypeConverter.ImplicitConversionExists(argument.Expression, parameters[i].Type))
            {
              argumentIsApplicable = true;
            }
            else
            {
              // for a ref or out parameter, 
              // the type of the argument is identical to the type of the corresponding parameter. 
              // After all, a ref or out parameter is an alias for the argument passed.
              if (argument.Expression.ExpressionResult is TypedExpressionResult
                && (argument.Expression.ExpressionResult as TypedExpressionResult).Type == parameters[i].Type)
              {
                argumentIsApplicable = true;
              }
            }
          }

          if (!argumentIsApplicable)
          {
            break;
          }

          i++;
        }

        if (i == parameters.Count)
        {
          isApplicable = true;
        }
      }

      // TODO: function member with a parameter array, see spec 7.4.3.1 Applicable function member

      return isApplicable;
    }
  }
}
