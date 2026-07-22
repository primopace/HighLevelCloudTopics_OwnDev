using System;
using System.Collections.Generic;
using System.Text;

namespace Exam.API.Outbox
{
    public class OutboxMessage
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
        public DateTime OccurredOn { get; set; }
        public DateTime? ProcessedOn { get; set; }
    }
}
