global using AwesomeAssertions;
global using MyApi.Shared.Support;
global using Serilog.Sinks.InMemory.Assertions;
global using System.Net;
global using System.Text;
global using System.Text.Json;
global using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]