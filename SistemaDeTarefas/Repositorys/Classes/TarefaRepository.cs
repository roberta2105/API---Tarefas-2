using Microsoft.EntityFrameworkCore;
using SistemaDeTarefas.Data;
using SistemaDeTarefas.Models;
using SistemaDeTarefas.Repositorys.Interfaces;

namespace SistemaDeTarefas.Repositorys.Classes
{
    public class TarefaRepository : ITarefaRepository
    {
        private readonly SistemasDeTarefasDBContext _dbContext;

        public TarefaRepository(SistemasDeTarefasDBContext sistemasDeTarefasDBContext) 
        {
            _dbContext = sistemasDeTarefasDBContext;
        }

        public async Task<TarefaModel> BuscarPorId(int id)
        {
            return await _dbContext.Tarefas.FirstOrDefaultAsync(x => x.Id == id);
            Include(x => x.Usuario);
        }

        public async Task<List<TarefaModel>> BuscarTodasTarefas()
        {
            return await _dbContext.Tarefas.ToListAsync();
            Include(x => x.Usuario);
        }

        public async Task<TarefaModel> Adicionar(TarefaModel tarefa)
        {
            await _dbContext.Tarefas.AddAsync(tarefa);

            await _dbContext.SaveChangesAsync();

            return tarefa;
        }

        public async Task<TarefaModel> Atualizar(TarefaModel tarefa, int id)
        {
            TarefaModel TarefaBanco = await BuscarPorId(id);

            if(TarefaBanco == null)
            {
                throw new Exception($"Tarefa de id {id} não encontrado no banco.");
            }
            TarefaBanco.Nome = tarefa.Nome;
            TarefaBanco.Descricao = tarefa.Descricao;
            TarefaBanco.Status = tarefa.Status;
            TarefaBanco.UsuarioId = tarefa.UsuarioId;

            _dbContext.Tarefas.Update(TarefaBanco);

            await _dbContext.SaveChangesAsync();

            return TarefaBanco;

        }

        public async Task<bool> Apagar(int id)
        {
            TarefaModel TarefaBanco = await BuscarPorId(id);

            if (TarefaBanco == null)
            {
                throw new Exception($"Usuário de id {id} não encontrado no banco.");
            }

            _dbContext.Tarefas.Remove(TarefaBanco);
            await _dbContext.SaveChangesAsync();

            return true;
        }




    }
}
