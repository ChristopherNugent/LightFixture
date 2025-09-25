using System;
using System.Text.Json;
using AutoFixture;
using LightFixture;
using LightFixture.SourceGeneration.Sample;

var dataProvider = new DataProviderBuilder()
    .Customize(new SampleDataProvider())
    .Build();

var fixture = new Fixture();

var data = dataProvider.Resolve<SampleDataOuter>().Value;
var data2 = fixture.Create<SampleDataOuter>();

// var json = JsonSerializer.Serialize(
//     data,
//     new JsonSerializerOptions()
//     {
//         WriteIndented = true,
//     });
Console.WriteLine();