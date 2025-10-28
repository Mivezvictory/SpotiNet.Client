namespace SpotiNet.Client;

/// <summary>Shape for /v1/me.</summary>
public sealed record CurrentUserProfile(string Id, string? DisplayName);
