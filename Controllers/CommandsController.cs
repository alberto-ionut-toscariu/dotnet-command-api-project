using AutoMapper;
using Commander.Data;
using Commander.Dtos;
using Commander.Models;
using Microsoft.AspNetCore.Mvc;

namespace Commander.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommanderRepo _repository;
        private readonly IMapper _mapper;

        public CommandsController(ICommanderRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetAllCommands()
        {
            var commandItems = _repository.GetAllCommands();
            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
        }
        [HttpGet("{id}", Name = "GetCommandById")]
        public ActionResult<CommandReadDto> GetCommandById(int id)
        {
            var commandItem = _repository.GetCommandById(id);
            if (commandItem != null)
                return Ok(_mapper.Map<CommandReadDto>(commandItem));
            return NotFound();
        }

        [HttpPost(Name = "CreateCommand")]
        public ActionResult<CommandReadDto> CreateCommand(CommandCreateDto commandCreateDto)
        {
            var commandItem = _mapper.Map<Command>(commandCreateDto);
            _repository.CreateCommand(commandItem);
            _repository.SaveChanges();
            var CommandReadDto = _mapper.Map<CommandReadDto>(commandItem);
            return CreatedAtRoute(nameof(GetCommandById), new { Id = CommandReadDto.Id }, CommandReadDto);
        }

        [HttpPut("{id}", Name = "UpdateCommand")]
        public ActionResult UpdateCommand(int id, CommandCreateDto commandUpdateDto)
        {
            var commandModelFromRepo = _repository.GetCommandById(id);
            if (commandModelFromRepo == null)
            {
                return NotFound();
            }

            _mapper.Map(commandUpdateDto, commandModelFromRepo);
            _repository.UpdateCommand(commandModelFromRepo);
            _repository.SaveChanges();

            return NoContent();
        }
    }
}