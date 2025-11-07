using Gimnasio.Core.CustomEntities;
using Gimnasio.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gimnasio.Core.Interfaces{
    public interface IClasesRepository : IBaseRepository<Clase>
    {
        //CRUD

        new Task<IEnumerable<Clase>> GetAllAsync();
        Task<IEnumerable<Clase>> GetAllClaseDapperAsync(int limit = 10);
        new Task<Clase> GetByIdAsync(int id);

        //Metodo adicional
        Task<IEnumerable<Clase>> GetClaseByInstructorAsync(int instructorId); //Lista de todas las clases con instructor dado
        Task<int> GetCapacidadMaximaAsync(int claseId); 
        Task<IEnumerable<ClaseInstructorResponse>> GetClaseInstructor();
    }
}