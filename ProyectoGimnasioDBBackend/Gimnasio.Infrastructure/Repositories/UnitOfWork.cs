
using Gimnasio.Core.Interfaces;
using Gimnasio.Core.Entities;
using Gimnasio.Infrastructure.Data;
using Gimnasio.Core.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Gimnasio.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GimnasioContext _context;
        public  IAsistenciaRepository? _asistenciaRepository;
        public  IClasesRepository? _claseRepository;
        public  IHorariosRepository? _horariosRepository;
        public   IUsuarioMembresiasRepository? _usuarioMembresiasRepository;
        public   IMembresiasRepository? _membresiaRepository;
        public   IUsuariosRepository? _usuarioRepository;
        public  IDapperContext _dapper;


        private IDbContextTransaction? _efTransaction;

        public UnitOfWork(GimnasioContext context, IDapperContext dapper)
        {
            _context = context;
            _dapper = dapper;
        }

        public IAsistenciaRepository asistenciaRepository => 
        _asistenciaRepository ??= new AsistenciaRepository(_context,_dapper);

        public IClasesRepository claseRepository => 
        _claseRepository ??= new ClasesRepository(_context,_dapper);

        public IHorariosRepository horariosRepository => 
        _horariosRepository ??= new HorariosRepository(_context,_dapper);

        public IUsuarioMembresiasRepository usuarioMembresiasRepository => 
        _usuarioMembresiasRepository ??= new UsuarioMembresiasRepository(_context,_dapper);

        public IMembresiasRepository membresiaRepository => 
        _membresiaRepository ??= new MembresiaRepository(_context, _dapper);

        public IUsuariosRepository usuarioRepository =>
        _usuarioRepository ??= new UsuariosRepository(_context, _dapper);

    public void Dispose()
    {
        if (_context != null)
        {
            _efTransaction?.Dispose();
            _context.Dispose();
        }
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        if (_efTransaction == null)
        {
            _efTransaction = await _context.Database.BeginTransactionAsync();

            //Registrar coneccion/tx DapperContext
            var conn = _context.Database.GetDbConnection();
            var tx = _efTransaction.GetDbTransaction();
            _dapper.SetAmbientConnection(conn, tx);
        }
    }

    public async Task CommitAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            if (_efTransaction != null)
            { 
                await _efTransaction.CommitAsync();
                _efTransaction.Dispose();
                _efTransaction = null;
            }
        }
        finally
        {
            _dapper.ClearAmbientConnection();
        }
    }

    public async Task RollbackAsync()
    {
        if (_efTransaction != null)
        {
            await _efTransaction.RollbackAsync();
            _efTransaction.Dispose();
            _efTransaction = null;
        }

        _dapper.ClearAmbientConnection();
    }

    public IDbConnection? GetDbConnection()
    {
        //Retornar la coneccion subyacente del DbContext
        return _context.Database.GetDbConnection();
    }

    public IDbTransaction? GetDbTransaction()
    {
        return _efTransaction?.GetDbTransaction();
    }

    }
}