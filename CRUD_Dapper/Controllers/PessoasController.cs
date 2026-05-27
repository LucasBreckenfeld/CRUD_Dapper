using CRUD_Dapper.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;

namespace CRUD_Dapper.Controllers
{
    public class PessoasController : Controller
    {

        private readonly string ConnectionString = "User ID=postgres;Password=18091995;Host=localhost;Port=5432;Database=PessoasDB;";
        public IActionResult Index()
        {
            IDbConnection con;

            try
            {
                string selecaoQuery = "SELECT * FROM pessoas";
                con = new NpgsqlConnection(ConnectionString);
                con.Open();
                IEnumerable<Pessoas> listapessoas = con.Query<Pessoas>(selecaoQuery).ToList();
                return View(listapessoas);
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Pessoas pessoa)
        {
            if (ModelState.IsValid)
            {
                IDbConnection con;

                try
                {
                    string insercaoQuery = "INSERT INTO pessoas (nome, idade, peso) VALUES (@Nome, @Idade, @Peso)";
                    con = new NpgsqlConnection(ConnectionString);
                    con.Open();
                    con.Execute(insercaoQuery, pessoa);
                    con.Close();
                    return RedirectToAction(nameof(Index));
                }

                catch (Exception ex)
                {
                    throw ex;
                }

            }

            return View(pessoa);
        }

        [HttpGet]
        public IActionResult Edit(int pessoaid)
        {
            IDbConnection con;

            try
            {
                string selecaoQuery = "SELECT * FROM pessoas WHERE pessoaid = @pessoaid";
                con = new NpgsqlConnection(ConnectionString);
                con.Open();
                Pessoas pessoas = con.Query<Pessoas>(selecaoQuery, new { pessoaid }).FirstOrDefault(); con.Close();

                return View(pessoas);
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        public IActionResult Edit(int pessoaid, Pessoas pessoas)
        {
            if (pessoaid != pessoas.PessoaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                IDbConnection con;
                try
                {
                    string atualizacaoQuery = "UPDATE pessoas SET nome = @Nome, idade = @Idade, peso = @Peso WHERE pessoaid = @pessoaId";
                    con = new NpgsqlConnection(ConnectionString);
                    con.Open();
                    con.Execute(atualizacaoQuery, pessoas);
                    con.Close();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return View(pessoas);
        }
        [HttpPost]
        public IActionResult Delete(int pessoaid)
        {
            IDbConnection con;
            try
            {
                string excluirQuery = "DELETE FROM pessoas WHERE Pessoaid = @pessoaid";
                con = new NpgsqlConnection(ConnectionString);
                con.Open();
                con.Execute(excluirQuery, new { pessoaid = pessoaid });
                con.Close();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
