using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions;
using DapperExtensions.Mapper;

namespace FreshFruit.Data.Ext.Dapper
{
    public static class DbOperatorExtensions
    {
        public static IPredicate GetPredicate(IClassMapper classMap, object predicate)
        {
            var wherePredicate = predicate as IPredicate;
            if (wherePredicate == null && predicate != null)
            {
                Type predicateType = typeof(FieldPredicate<>).MakeGenericType(classMap.EntityType);
                IList<IPredicate> predicates = new List<IPredicate>();
                foreach (var kvp in ReflectionHelper.GetObjectValues(predicate))
                {
                    var fieldPredicate = Activator.CreateInstance(predicateType) as IFieldPredicate;
                    fieldPredicate.Not = false;
                    fieldPredicate.Operator = Operator.Eq;
                    fieldPredicate.PropertyName = kvp.Key;
                    fieldPredicate.Value = kvp.Value;
                    predicates.Add(fieldPredicate);
                }

                return predicates.Count == 1
                           ? predicates[0]
                           : new PredicateGroup
                           {
                               Operator = GroupOperator.And,
                               Predicates = predicates
                           };
            }

            return wherePredicate;
        }
    }
}
