using System;
using System.Collections.Generic;
using System.Threading.Tasks;

/***
 * Pretended use:
 * New QueryableValue(propertyClass( BaseValue))
 *  .From(source)
 *  .To(target)
 *  .Result();
 * 
 * 
 * new QueryableValue<int>(baseValue)
 *      .evolve<Character>("from",source)
 *      .evolve<Character>("to",target)
 *      .value()
 * 
 */


public abstract class QueryableValue<VarType>
{
    public delegate VarType Derivator(VarType input);
    public static string Symbol { get; set; }
    protected VarType value;
    // Verb -> PropertyType -> entity -> Event

    private static Dictionary<string, 
                   Dictionary<Type, 
                   Dictionary<object, 
                   List<Derivator>>>> evolutions = new Dictionary<string, Dictionary<Type, Dictionary<object, List<Derivator>>>>();
    
    public QueryableValue(VarType baseValue){
        value = baseValue;
    }

    public VarType Value()
    {
        return this.value;
    }


    public QueryableValue<VarType> D<EntityType>(string verb, EntityType entity)
    {
        return Derivate(verb, entity);
    }


    public QueryableValue<VarType> Derivate<EntityType>(string verb, EntityType entity)
    {
        if (entity == null)
        {
            return this;
        }

        List<Derivator> derivations = GetPropertyEvolution(verb, entity);

        foreach(Derivator derivation in derivations)
        {
             this.value = derivation(value);
        }

        return this;
    }

    public static Task SetDerivator<EntityType>(string verb, EntityType entity, Derivator action)
    {
        var derivatorList = GetPropertyEvolution(verb, entity);
        derivatorList.Add(action);
        return new Task(() => derivatorList.Remove(action));
    }

    private static List<Derivator> GetPropertyEvolution<EntityType>(string verb, EntityType entity) {
        Dictionary<Type, Dictionary<object, List<Derivator>>> verbDictionary = GetElementFormDictionary(evolutions, verb);
        Dictionary<object, List<Derivator>> typeDictionary = GetElementFormDictionary(verbDictionary, typeof(EntityType));
        return GetElementFormDictionary(typeDictionary, entity);
    }

    private static EntityType GetElementFormDictionary<KeyType,EntityType>(Dictionary<KeyType, EntityType> dictionary, KeyType elementKey) where EntityType : new() {
        EntityType obtainedElement = default(EntityType);

        if (dictionary.TryGetValue(elementKey, out obtainedElement))
        {
            return obtainedElement;
        }
        else
        {
            obtainedElement = new EntityType();
            dictionary.Add(elementKey, obtainedElement);
            return obtainedElement;
        }
    }
}