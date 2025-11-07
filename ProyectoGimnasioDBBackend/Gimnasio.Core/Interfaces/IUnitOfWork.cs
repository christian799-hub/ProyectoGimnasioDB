
using System.Data;
using Gimnasio.Core.Entities;

namespace Gimnasio.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAsistenciaRepository asistenciaRepository { get; }
        IClasesRepository claseRepository { get; }
        IHorariosRepository horariosRepository { get; }
        IUsuarioMembresiasRepository usuarioMembresiasRepository { get; }
        IMembresiasRepository membresiaRepository { get; }
        IUsuariosRepository usuarioRepository { get; }
        void SaveChanges();
        Task SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
        IDbConnection? GetDbConnection();
        IDbTransaction? GetDbTransaction();
    }
}