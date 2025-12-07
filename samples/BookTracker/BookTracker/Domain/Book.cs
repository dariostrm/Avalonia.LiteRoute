using System;

namespace BookTracker.Domain;

public record Book(
    Guid Id,
    string Title,
    string Author,
    int Pages,
    bool IsRead
);