﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Geofy.Domain.Commands.Chart;
using Geofy.Infrastructure.ServiceBus.Interfaces;
using Geofy.ReadModels;
using Geofy.ReadModels.Services.Chart;
using Geofy.Shared.Mongo;
using Geofy.Shared.Resources;
using Geofy.WebAPi.ViewModels.Chart;
using Geofy.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Geofy.WebAPi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ChartController : BaseController
    {
        private readonly ChartReadModelService _chartReadModelService;

        public ChartController(ChartReadModelService chartReadModelService, ICommandBus commandBus, IdGenerator idGenerator) 
            : base(commandBus, idGenerator)
        {
            _chartReadModelService = chartReadModelService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody]CreateChartPostModel model)
        {
            if(!ModelState.IsValid) return new GeofyBadRequest(ModelState);
            await SendAsync(new CreateChart
            {
                ChartId = IdGenerator.Generate(),
                Title = model.Title,
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                Radius = model.Radius,
                OwnerId = UserId,
                Description = model.Description
            });
            return Ok();
        }

        [HttpGet("inlocation")]
        public async Task<List<ChartViewModelShort>> GetCharts(Location location)
        {
            return (await _chartReadModelService.GetInLocationCharts(location))
                .Select(Map)
                .ToList();
        }

        [HttpGet("{id}")]
        public async Task<ChartViewModel> GetById(string id)
        {
            return MapToView(await _chartReadModelService.GetByIdAsync(id));
        }

        [HttpPost("changeName")]
        public async Task<IActionResult> ChangeName([FromBody]ParticipantChangeNameModel model)
        {
            var chart = await _chartReadModelService.GetByIdAsync(model.ChatId);
            if (chart == null || chart.Participants.All(x => x.UserId != UserId))
            {
                ModelState.AddModelError(nameof(chart), MessageConstants.Errors.UserNotInChat);
                return new GeofyBadRequest(ModelState);
            }
            if (chart.Participants.Any(x => x.UserName == model.Name))
            {
                ModelState.AddModelError(nameof(chart), MessageConstants.Errors.UserAlreadyInChat);
                return new GeofyBadRequest(ModelState);
            }

            await SendAsync(new ChangeParticipantName
            {
                ChatId = model.ChatId,
                Name = model.Name,
                UserId = UserId
            });
            return Ok();
        }

        private ChartViewModel MapToView(ChartReadModel model)
        {
            return new ChartViewModel
            {
                AdminIds = model.AdminIds,
                Id = model.Id,
                Location = new Location
                {
                    Latitude = model.Location.Coordinates.Latitude,
                    Longitude = model.Location.Coordinates.Longitude
                },
                OwnerId = model.OwnerId,
                Participants = model.Participants,
                Radius = model.Radius,
                Title = model.Title,
                Description = model.Description,
                Messages = model.Messages
            };
        }

        private ChartViewModelShort Map(ChartReadModel model)
        {
            return new ChartViewModelShort
            {
                AdminIds = model.AdminIds,
                Id = model.Id,
                Location = new Location
                {
                    Latitude = model.Location.Coordinates.Latitude,
                    Longitude = model.Location.Coordinates.Longitude
                },
                OwnerId = model.OwnerId,
                Participants = model.Participants,
                Radius = model.Radius,
                Title = model.Title,
                LastMessage = model.LastMessage
            };
        }
    }
}