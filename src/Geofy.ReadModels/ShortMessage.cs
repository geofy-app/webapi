﻿using System;

namespace Geofy.ReadModels
{
    public class ShortMessage
    {
        public string MessageId { get; set; }
        public string UserId { get; set; }
        public DateTime Created { get; set; }
        public string Message { get; set; }
    }
}