namespace MapSystem
{
    public interface IMapper<in TKey, TValue>
    {
        public void AddValue(TKey key, TValue value);
        public void RemoveByKey(TKey key);
        public bool TryGetValue(TKey key, out TValue value);
    }
}