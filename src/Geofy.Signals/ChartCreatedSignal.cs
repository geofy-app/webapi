﻿using System.Collections.Generic;
using Geofy.Infrastructure.ServiceBus.Messages;
using Geofy.ReadModels;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Geofy.Signals
{
    public class ChartCreatedSignal : Message
    {
        public string ChartId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Location Location { get; set; }
        public double Radius { get; set; }
        public string OwnerId { get; set; }
        public IList<string> AdminIds { get; set; } = new List<string>();
        public IList<Participant> Participants { get; set; } = new List<Participant>();
    }
}