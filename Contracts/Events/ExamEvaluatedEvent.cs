using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Events
{
    public record ExamEvaluatedEvent
    {
        public string StudentId { get; set; }
        public int Score { get; set; }

    }
}

