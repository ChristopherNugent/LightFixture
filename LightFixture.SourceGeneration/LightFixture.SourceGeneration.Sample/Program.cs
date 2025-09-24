using System;
using System.Text.Json;
using LightFixture;
using LightFixture.SourceGeneration.Sample;

var dataProvider = new DataProviderBuilder()
    .Customize(new SampleDataProvider())
    .Build();

var data = dataProvider.Resolve<SampleDataOuter>();

var json = JsonSerializer.Serialize(
    data,
    new JsonSerializerOptions()
    {
        WriteIndented = true,
    });
Console.WriteLine(json);