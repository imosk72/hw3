using hw3;

IStack<int> st = new MyConcurrentStack<int>();

var readThread = new Thread(() =>
{
    Thread.Sleep(1000);
    int x;
    for (int i = 0; i < 3; i++)
    {
        st.TryPop(out x);
        Console.WriteLine(x);
    }
});

var writeThread = new Thread(() =>
{
    for (int i = 0; i < 3; i++)
    {
        st.Push(i);
    }
});

readThread.Start();
writeThread.Start();
readThread.Join();
writeThread.Join();

st.Push(12351235);
Console.WriteLine(st.Count);