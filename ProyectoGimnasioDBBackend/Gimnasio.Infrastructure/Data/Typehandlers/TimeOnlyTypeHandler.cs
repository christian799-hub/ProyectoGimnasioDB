using Dapper;
using System.Data;

namespace Gimnasio.Infrastructure.Data.TypeHandlers
{
    public class TimeOnlyTypeHandler : SqlMapper.TypeHandler<TimeOnly> // Castea para evitar problemas con el tiempo para la clase horario
    {
        public override TimeOnly Parse(object value)
        {
            if (value is TimeSpan timeSpan)
                return TimeOnly.FromTimeSpan(timeSpan);
            
            if (value is DateTime dateTime)
                return TimeOnly.FromDateTime(dateTime);
                
            if (value is string strValue && TimeSpan.TryParse(strValue, out var ts))
                return TimeOnly.FromTimeSpan(ts);
                
            throw new InvalidCastException($"No se puede convertir {value} a TimeOnly");
        }

        public override void SetValue(IDbDataParameter parameter, TimeOnly value)
        {
            parameter.Value = value.ToTimeSpan();
        }
    }
}