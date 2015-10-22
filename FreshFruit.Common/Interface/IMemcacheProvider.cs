namespace FreshFruit.Common.Interface
{
    public interface IMemcacheProvider
    {
        bool Add(string key, object value);

        bool Add(string key, object value, double expiry);

        bool Add(string key, object value, int hashCode);

        bool Add(string key, object value, int hashCode, double expiry);

        long Decrement(string key);

        long Decrement(string key, long inc);

        long Decrement(string key, long inc, int hashCode);

        bool Delete(string key);

        bool Delete(string key, double expiry);

        bool Delete(string key, object hashCode, double expiry);

        bool FlushAll();

        bool FlushAll(System.Collections.ArrayList servers);

        object Get(string key);

        object Get(string key, int hashCode);

        object Get(string key, object hashCode, bool asString);

        long GetCounter(string key);

        long GetCounter(string key, object hashCode);

        System.Collections.Hashtable GetMultiple(string[] keys);

        System.Collections.Hashtable GetMultiple(string[] keys, int[] hashCodes);

        System.Collections.Hashtable GetMultiple(string[] keys, int[] hashCodes, bool asString);

        object[] GetMultipleArray(string[] keys);

        object[] GetMultipleArray(string[] keys, int[] hashCodes);

        object[] GetMultipleArray(string[] keys, int[] hashCodes, bool asString);

        long Increment(string key);

        long Increment(string key, long inc);

        long Increment(string key, long inc, int hashCode);

        bool Exists(string key);

        bool Replace(string key, object value);

        bool Replace(string key, object value, double expiry);

        bool Replace(string key, object value, int hashCode);

        bool Replace(string key, object value, int hashCode, double expiry);

        bool Set(string key, object value);

        bool Set(string key, object value, double expiry);

        bool Set(string key, object value, int hashCode);

        bool Set(string key, object value, int hashCode, double expiry);

        System.Collections.Hashtable Stats();

        System.Collections.Hashtable Stats(System.Collections.ArrayList servers);

        bool StoreCounter(string key, long counter);

        bool StoreCounter(string key, long counter, int hashCode);
    }
}
