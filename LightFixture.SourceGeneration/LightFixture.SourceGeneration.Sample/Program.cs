
using LightFixture;
using LightFixture.SourceGeneration.Sample;

var dataProvider = new DataProviderBuilder()
    .Customize(new SampleDataProvider())
    .Build();
    
var data = dataProvider.Resolve<SampleDataOuter>();
var data2 = dataProvider.Resolve<SampleDataInner>();

var a = 1;