using System;
using System.Collections.Generic;
using System.Text;

namespace Fileforge.Domain.Entities;

public class MessageSource
{
    public Guid MessageId { get; private set; }
    public Guid ChunkId { get; private set; }
    public double Score { get; private set; }
}
