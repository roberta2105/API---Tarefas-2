using Microsoft.EntityFrameworkCore;
using SistemaDeTarefas.Data;
using SistemaDeTarefas.Models;
using SistemaDeTarefas.Repositorys.Interfaces;

namespace SistemaDeTarefas.Repositorys.Classes
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly SistemasDeTarefasDBContext _dbContext;

        public UsuarioRepository(SistemasDeTarefasDBContext sistemasDeTarefasDBContext) 
        {
            _dbContext = sistemasDeTarefasDBContext;
        }

        public async Task<UsuarioModel> BuscarPorId(int id)
        {
            return await _dbContext.Usuarios.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<UsuarioModel>> BuscarTodosUsuarios()
        {
            return await _dbContext.Usuarios.ToListAsync();
        }

        public async Task<UsuarioModel> Adicionar(UsuarioModel usuario)
        {
            await _dbContext.Usuarios.AddAsync(usuario);

            await _dbContext.SaveChangesAsync();

            return usuario;
        }

        public async Task<UsuarioModel> Atualizar(UsuarioModel usuario, int id)
        {
            UsuarioModel UsuarioBanco = await BuscarPorId(id);

            if(UsuarioBanco == null)
            {
                throw new Exception($"Usuário de id {id} não encontrado no banco.");
            }

            _dbContext.Entry(UsuarioBanco).CurrentValues.SetValues(usuario.Id);

            _dbContext.Update<UsuarioModel>(UsuarioBanco);

            await _dbContext.SaveChangesAsync();

            return UsuarioBanco;

        }

        public async Task<bool> Apagar(int id)
        {
            UsuarioModel UsuarioBanco = await BuscarPorId(id);

            if (UsuarioBanco == null)
            {
                throw new Exception($"Usuário de id {id} não encontrado no banco.");
            }

            _dbContext.Usuarios.Remove(UsuarioBanco);
            _dbContext.SaveChanges();

            return true;
        }




    }
}
