using System;
using System.Collections.Generic;
using System.Text;

namespace Fileforge.Application.Features.Documents.GetDocument;

public sealed record DocumentDetailsResponse(
    Guid Id,
    string FileName,
    string ContentType,
    long SizeInBytes,
    string Status,
    int ChunkCount,
    string? FailureReason,
    DateTime CreatedAtUtc,
    DateTime? ProcessedAtUtc
);
