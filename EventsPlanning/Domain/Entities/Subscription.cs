﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Subscription
    {
        public int Id { get; set; }
        public int ParticipantId { get; set; }
        public int ActivityId { get; set; }

        public Activity Activity { get; set; }

        public Participant Participant { get; set; }
    }
}
