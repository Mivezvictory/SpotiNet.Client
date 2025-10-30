using System.Net;

namespace SpotiNet.Client.Models;

/// <summary>
/// Represents the result of an API call, encapsulating the response data, status code, and raw JSON response.  
/// </summary>
/// <typeparam name="T">The type of the data returned by the API call.</typeparam>
/// <param name="Data">The data returned</param>
/// <param name="StatusCode">The Status code of an API call</param>
/// <param name="RawJson">The raw JSON</param>
public sealed record ApiResult<T>(T Data, HttpStatusCode StatusCode, string? RawJson);
