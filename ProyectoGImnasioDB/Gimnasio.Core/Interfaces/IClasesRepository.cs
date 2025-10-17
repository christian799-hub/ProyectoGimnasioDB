using Gimnasio.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gimnasio.Core.Interfaces{
    public interface IClasesRepository
    {
        //CRUD

        Task<IEnumerable<Clase>> GetAllClasesAsync();
        Task<Clase> GetClaseByIdAsync(int id);
        Task InsertarClase(Clase clase);
        Task UpdateClase(Clase clase);
        Task DeleteClase(Clase clase);

        //Metodo adicional
        Task<IEnumerable<Clase>> GetClaseByInstructorAsync(int instructorId);
        Task<int> GetCapacidadMaximaAsync(int claseId);
    }
}