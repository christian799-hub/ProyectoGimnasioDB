using Dapper;
using System.Data;

namespace Gimnasio.Infrastructure.Data.TypeHandlers
{
    public class DateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly> //Casteo del datetime para evitar problemas con la clase clase
    {
        public override DateOnly Parse(object value)
        {
            if (value is DateTime dateTime)
                return DateOnly.FromDateTime(dateTime);
            
            if (value is string strValue && DateTime.TryParse(strValue, out var dt))
                return DateOnly.FromDateTime(dt);
                
            throw new InvalidCastException($"No se puede convertir {value} a DateOnly");
        }

        public override void SetValue(IDbDataParameter parameter, DateOnly value)
        {
            parameter.Value = value.ToDateTime(TimeOnly.MinValue);
        }
    }
}