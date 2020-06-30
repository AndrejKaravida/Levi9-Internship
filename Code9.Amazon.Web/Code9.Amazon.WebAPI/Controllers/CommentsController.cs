using AutoMapper;
using Code9.Amazon.WebAPI.Core.IService;
using Code9.Amazon.WebAPI.Core.Models;
using Code9.Amazon.WebAPI.Dto;
using Code9.Amazon.WebAPI.HubConfig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Code9.Amazon.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;
        private readonly IHubContext<CommentHub> _hub;

        public CommentsController(ICommentService commentService, IMapper mapper,
                                  IHubContext<CommentHub> hub)
        {
            _commentService = commentService;
            _mapper = mapper;
            _hub = hub;
        }

        [HttpGet]
        public async Task<IActionResult> GetComments()
        {
            var comments = await _commentService.GetAllAsync();

            return Ok(_mapper.Map<IEnumerable<CommentDto>>(comments));
        }

        [HttpGet("vehicle/{vehicleId}")]
        public async Task<IActionResult> GetCommentsForVehicle(int vehicleId)
        {
            var comments = await _commentService.GetAllForVehicleAsync(vehicleId);

            return Ok(_mapper.Map<IEnumerable<CommentDto>>(comments));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetComment(int id)
        {
            var comment = await _commentService.GetByIdAsync(id);

            if (comment == null)
                return NotFound();

            return Ok(_mapper.Map<CommentDto>(comment));
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CommentToSaveDto commentToSave)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var comment = _mapper.Map<Comment>(commentToSave);

            var userid = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            comment.UserId = userid;

            await _commentService.AddAsync(comment);

            comment = await _commentService.GetByIdAsync(comment.Id);

            await _hub.Clients.All.SendAsync("transfercomments", true);

            return CreatedAtAction("GetComment", new { id = comment.Id }, comment);
        }
    }
}
