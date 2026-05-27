using CRUD_Dapper.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;

namespace CRUD_Dapper.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PessoasApiController : ControllerBase
    {
        private readonly string ConnectionString =
            "User ID=postgres;Password=18091995;Host=localhost;Port=5432;Database=PessoasDB;";

        // GET: api/PessoasApi
        [HttpGet]
        public IActionResult Get()
        {
            using IDbConnection con = new NpgsqlConnection(ConnectionString);

            string query = "SELECT * FROM pessoas";

            var pessoas = con.Query<Pessoas>(query).ToList();

            return Ok(pessoas);
        }

        // GET: api/PessoasApi/1
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            using IDbConnection con = new NpgsqlConnection(ConnectionString);

            string query = "SELECT * FROM pessoas WHERE pessoaid = @id";

            var pessoa = con.QueryFirstOrDefault<Pessoas>(
                query,
                new { id });

            if (pessoa == null)
            {
                return NotFound();
            }

            return Ok(pessoa);
        }

        // POST: api/PessoasApi
        [HttpPost]
        public IActionResult Post(Pessoas pessoa)
        {
            using IDbConnection con = new NpgsqlConnection(ConnectionString);

            string query = @"
                INSERT INTO pessoas (nome, idade, peso)
                VALUES (@Nome, @Idade, @Peso)";

            con.Execute(query, pessoa);

            return Ok(new
            {
                mensagem = "Pessoa cadastrada com sucesso"
            });
        }

        // PUT: api/PessoasApi/1
        [HttpPut("{id}")]
        public IActionResult Put(int id, Pessoas pessoa)
        {
            if (id != pessoa.PessoaId)
            {
                return BadRequest();
            }

            using IDbConnection con = new NpgsqlConnection(ConnectionString);

            string query = @"
                UPDATE pessoas
                SET nome = @Nome,
                    idade = @Idade,
                    peso = @Peso
                WHERE pessoaid = @PessoaId";

            int linhasAfetadas = con.Execute(query, pessoa);

            if (linhasAfetadas == 0)
            {
                return NotFound();
            }

            return Ok(new
            {
                mensagem = "Pessoa atualizada com sucesso"
            });
        }

        // DELETE: api/PessoasApi/1
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using IDbConnection con = new NpgsqlConnection(ConnectionString);

            string query = "DELETE FROM pessoas WHERE pessoaid = @id";

            int linhasAfetadas = con.Execute(query, new { id });

            if (linhasAfetadas == 0)
            {
                return NotFound();
            }

            return Ok(new
            {
                mensagem = "Pessoa excluída com sucesso"
            });
        }
    }
}