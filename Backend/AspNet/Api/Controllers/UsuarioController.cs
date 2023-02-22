using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        [HttpGet("ObterDataHoraAtual")]
        public IActionResult ObterDataHora(){
            var obj = new
            {
                Data = DateTime.Now.ToLongDateString(),
                Hora = DateTime.Now.ToShortTimeString()
            };
            return Ok(obj);
        }        

        [HttpGet("Apresentar/{nome}")]
        public IActionResult Apresentar(string nome)
        {
            var mensagem = $"Olá {nome}, seja bem vindo!!!";
            return Ok(new {mensagem});
        }

        [HttpGet("OperacaoSoma/{n1} {n2}")]
        public IActionResult Somar(double n1, double n2)
        {
           var soma = n1 + n2;
            var resultado = $"A soma de {n1} e {n2} é igual a {soma}.";
            return Ok(new {resultado});
        }
    }
}