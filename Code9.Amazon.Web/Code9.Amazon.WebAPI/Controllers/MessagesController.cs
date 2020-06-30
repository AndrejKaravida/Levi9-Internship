using AutoMapper;
using Code9.Amazon.WebAPI.Core.IService;
using Code9.Amazon.WebAPI.Core.Models;
using Code9.Amazon.WebAPI.Dto;
using Code9.Amazon.WebAPI.Helpers;
using Code9.Amazon.WebAPI.HubConfig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Code9.Amazon.WebAPI.Controllers
{
    [Route("api/users/{userId}/[controller]")]
    [ApiController]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _serviceMessage;
        private readonly IService<User> _serviceUser;
        private readonly IMapper _mapper;
        private readonly IHubContext<ChatHub> _hub;

        public MessagesController(IMessageService serviceMessage, IService<User> serviceUser, IMapper mapper, IHubContext<ChatHub> hub)
        {
            _serviceMessage = serviceMessage;
            _serviceUser = serviceUser;
            _mapper = mapper;
            _hub = hub;
        }

        [HttpGet("{id}", Name = "GetMessage")]
        public async Task<IActionResult> GetMessage(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var message = await _serviceMessage.GetByIdAsync(id);

            if (message == null)
                return NotFound();

            return Ok(_mapper.Map<MessageToReturnDto>(message));
        }

        [HttpGet]
        public IActionResult GetMessagesForUser(int userId,
            [FromQuery]MessageParams messageParams)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            messageParams.UserId = userId;

            var messagesFromRepo = _serviceMessage.GetMessagesForUser(messageParams);

            var messages = _mapper.Map<IEnumerable<MessageToReturnDto>>(messagesFromRepo);

            Response.AddPagination(messagesFromRepo.CurrentPage, messagesFromRepo.PageSize,
                messagesFromRepo.TotalCount, messagesFromRepo.TotalPages);

            return Ok(messages);
        }

        [HttpGet("thread/{recipientId}")]
        public async Task<IActionResult> GetMessageThread(int userId, int recipientId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messagesFromRepo = await _serviceMessage.GetMessageThread(userId, recipientId);

            var messageThread = _mapper.Map<IEnumerable<MessageToReturnDto>>(messagesFromRepo);

            return Ok(messageThread);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(int userId, MessageToSaveDto messageForCreationDto)
        {
            var sender = await _serviceUser.GetByIdAsync(userId);

            if (sender.Id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            messageForCreationDto.SenderId = userId;

            var recipient = await _serviceUser.GetByIdAsync(messageForCreationDto.RecipientId);

            if (recipient == null)
                return BadRequest("Could not find user");

            var message = _mapper.Map<Message>(messageForCreationDto);

            await _serviceMessage.AddAsync(message);

            var result = _mapper.Map<MessageToReturnDto>(message);

            await _hub.Clients.All.SendAsync("RecieveMessage " + result.RecipientId, result);

            return CreatedAtRoute("GetMessage", new { userId = userId, id = message.Id }, result);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteMessage(int id, int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var message = await _serviceMessage.GetByIdAsync(id);

            if (message.SenderId == userId)
                message.SenderDeleted = true;

            if (message.RecipientId == userId)
                message.RecipientDeleted = true;

            if (message.SenderDeleted && message.RecipientDeleted)
            {
                await _serviceMessage.DeleteAsync(message);
                return Ok(id);
            }

            if(await _serviceMessage.SaveAsync())
                return Ok(id);



            return BadRequest("Comment could not be deleted");
        }


        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkMessageAsRead(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var message = await _serviceMessage.GetByIdAsync(id);

            if (message.RecipientId != userId)
                return Unauthorized();

            message.IsRead = true;
            message.DateRead = DateTime.Now;

            await _serviceMessage.SaveAsync();

            return NoContent();
        }



    }
}
