using DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Models;
namespace DELMUNDOMARCKEVIN_241639_FUNDAMENTAL_OF_PROGRAMMING.Services;

// Immutable records make snapshots safe to share after releasing the lock.
public sealed class TypedCrudStore<T> where T : DELMUNDOMARCKEVIN_241639_CrudRecord
{
    private readonly object _lock = new();
    private readonly Dictionary<int, T> _records;
    private int _nextId;
    public TypedCrudStore(IEnumerable<T> seeds)
    {
        _records = seeds.ToDictionary(record => record.Id);
        _nextId = _records.Keys.DefaultIfEmpty().Max();
    }
    public IReadOnlyList<T> GetAll()
    {
        lock (_lock) return _records.Values.ToArray();
    }

    public T? GetById(int id)
    {
        lock (_lock) return _records.GetValueOrDefault(id);
    }

    public T Insert(T record)
    {
        lock (_lock)
        {
            var saved = (T)(record with { Id = ++_nextId });
            _records.Add(saved.Id, saved);
            return saved;
        }
    }
    public bool Update(int id, T record)
    {
        lock (_lock)
        {
            if (!_records.ContainsKey(id)) return false;
            _records[id] = (T)(record with { Id = id });
            return true;
        }
    }
    public bool Delete(int id)
    {
        lock (_lock) return _records.Remove(id);
    }

    public bool Change(int id, Func<T, T> change)
    {
        lock (_lock)
        {
            if (!_records.TryGetValue(id, out var record)) return false;
            _records[id] = (T)(change(record) with { Id = id });
            return true;
        }
    }
}
