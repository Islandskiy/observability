using System.Diagnostics.Metrics;

namespace Observability.API.Metrics;

public class JustACounter
{
    private readonly Counter<long> _counter;
    
    public JustACounter(Meter meter)
    {
        _counter = meter.CreateCounter<long>("just-a-counter");
        _counter.Add(0);
    }

    public void Increment()
    {
        _counter.Add(1);
    }
}