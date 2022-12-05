namespace hw3;

// храним связный список элементов в стеке и длину этого списка
public class Item<T>
{
    public readonly T Value;
    public readonly Item<T>? Previous;
    public readonly int Length;
    
    public Item(T value, Item<T>? previous, int length)
    {
        this.Value = value;
        this.Previous = previous;
        this.Length = length;
    }
}

public class MyConcurrentStack<T>: IStack<T>
{
    private Item<T>? back; // указатель на последний элемент

    public int Count
    {
        get
        {
            if (back == null)
            {
                return 0;
            }
            return back.Length;
        }
    }

    public void Push(T item)
    {
        while (true)
        {
            var currentItem = back;
            var newItem = new Item<T>(item, back, currentItem?.Length + 1 ?? 1);
            if (Interlocked.CompareExchange(ref back, newItem, currentItem) == currentItem)
            {
                break;
            }
        }
    }

    public bool TryPop(out T? item)
    {
        while (true)
        {
            var currentItem = back;

            if (currentItem == null)
            {
                item = default;
                return false;
            }
             
            if (Interlocked.CompareExchange(ref back, currentItem.Previous, currentItem) == currentItem)
            {
                item = currentItem.Value;
                return true;
            }
        }
    }

}