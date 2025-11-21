using Ardalis.GuardClauses;
using TickerQ.Utilities.Interfaces.Managers;
using TickerQ.Utilities.Models;
using TickerQ.Utilities.Models.Ticker;

namespace BookingApp.Utils.Stubs;

public class FakeTickerManager<TTicker> : ITimeTickerManager<TTicker> where TTicker : TimeTicker
{
    public readonly List<TTicker> AddedTickers = [];

    public FakeTickerManager<TTicker> Clear()
    {
        AddedTickers.Clear();
        return this;
    }
    
    public Task<TickerResult<TTicker>> AddAsync(TTicker entity, CancellationToken cancellationToken = new())
    {
        AddedTickers.Add(entity);
        var result = new TickerResult<TTicker>(entity);
        return Task.FromResult(result);
    }

    public Task<TickerResult<TTicker>> UpdateAsync(Guid id, Action<TTicker> updateAction, CancellationToken cancellationToken = new())
    {
        var entity = AddedTickers.FirstOrDefault(x => x.Id == id);
        if (entity is null)
        {
            var ex = new NotFoundException("not found", nameof(TTicker));
            return Task.FromResult(new TickerResult<TTicker>(ex));
        }
        
        updateAction.Invoke(entity);
        
        var result = new TickerResult<TTicker>(entity);
        return Task.FromResult(result);
    }

    public Task<TickerResult<TTicker>> DeleteAsync(Guid id, CancellationToken cancellationToken = new())
    {
        var entity = AddedTickers.FirstOrDefault(x => x.Id == id);
        
        if (entity is null)
        {
            var ex = new NotFoundException("not found", nameof(TTicker));
            return Task.FromResult(new TickerResult<TTicker>(ex));
        }

        AddedTickers.Remove(entity);
        var result = new TickerResult<TTicker>(entity);
        return Task.FromResult(result);
    }
}