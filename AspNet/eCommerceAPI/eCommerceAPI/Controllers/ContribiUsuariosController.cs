using Microsoft.AspNetCore.Mvc;
using eCommerceAPI.Models;
using eCommerceAPI.Repositories;

namespace eCommerceAPI.Controllers
{
    [Route("api/Contribi/usuarios")]
    [ApiController]
    public class ContribiUsuariosController : ControllerBase
    {
        private IUsuarioRepository _repository;
        
        public ContribiUsuariosController()
        {
            _repository = new ContribiUsuarioRepository();
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_repository.Get());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var usuario = _repository.Get(id);
            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        [HttpPost]
        public IActionResult Insert([FromBody] Usuario usuario)
        {
            _repository.Insert(usuario);
            return Ok(usuario);
        }

        [HttpPut]
        public IActionResult Update([FromBody] Usuario usuario)
        {
            _repository.Update(usuario);
            return Ok(usuario);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _repository.Delete(id);
            return Ok();
        }
     }
   }

