using Shouldly;

namespace LightFixture.Tests;

[DataFactory(typeof(Collections))]
public sealed partial class CollectionResolutionTests
{
    [Fact]
    public void CollectionResolution()
    {
        var provider = new DataProviderBuilder()
            .Customize(this)
            .Build();
        
        provider.Resolve<Collections>().Value.ShouldSatisfyAllConditions(
            c => c.List.ShouldNotBeEmpty(),
            c => c.IList.ShouldNotBeEmpty(),
            c => c.IReadOnlyList.ShouldNotBeEmpty(),
            c => c.IEnumerable.ShouldNotBeEmpty(),
            c => c.ICollection.ShouldNotBeEmpty(),
            c => c.IReadOnlyCollection.ShouldNotBeEmpty(),
            c => c.HashSet.ShouldNotBeEmpty(),
            c => c.ISet.ShouldNotBeEmpty(),
            c => c.IReadOnlySet.ShouldNotBeEmpty(),
            c => c.Stack.ShouldNotBeEmpty(),
            c => c.Queue.ShouldNotBeEmpty());
    }
    
    private sealed class Collections
    {
        public List<int>? List { get; set; }
        
        public IList<int>? IList { get; set; }
        
        public IReadOnlyList<int>? IReadOnlyList { get; set; }
        
        public IEnumerable<int>? IEnumerable { get; set; }
        
        public ICollection<int>? ICollection { get; set; }
        
        public IReadOnlyCollection<int>? IReadOnlyCollection { get; set; }
        
        public HashSet<int>? HashSet { get; set; }
        
        public ISet<int>? ISet { get; set; }
        
        public IReadOnlySet<int>?  IReadOnlySet { get; set; }
        
        public Stack<int>?  Stack { get; set; }
        
        public Queue<int>?  Queue { get; set; }
    }
}