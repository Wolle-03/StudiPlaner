namespace StudiPlaner.Core.Data;

public interface IForEachAble<T>
{
    public delegate void Del(T obj);

    void ForEach(Del del);
}
