using System.Linq.Expressions;

namespace Core.Mappy.Configuracion;

public class MapperConfiguration<TSource, TDestination> // son genericas por esoo lleva el <TSource,TDestination>, esto es para que pueda ser utilizada para mapear cualquier tipo de objeto, es decir, cualquier clase que se quiera mapear, ya sea una clase de dominio, una clase de DTO, una clase de entidad, etc. El TSource es el tipo de objeto que se va a mapear, es decir, el tipo de objeto que se va a convertir en otro tipo de objeto, y el TDestination es el tipo de objeto al que se va a mapear, es decir, el tipo de objeto que se va a obtener después de mapear el TSource.
{

    private readonly Dictionary<string, Func<TSource, object>> _mappings = new(); // este diccionario es el que se va a utilizar para almacenar los mapeos entre los objetos, es decir, para almacenar la configuracion que se le dio al Mapper para mapear un objeto de tipo TSource a un objeto de tipo TDestination, y el Mapper va a utilizar esa configuracion para mapear los objetos cuando se le pida que lo haga, por ejemplo, cuando se le pida que mapee un objeto de tipo TSource a un objeto de tipo TDestination, el Mapper va a buscar en su configuracion si existe un mapeo entre esos dos tipos de objetos, y si existe, va a utilizar ese mapeo para mapear el objeto de tipo TSource al objeto de tipo TDestination.




    internal Dictionary<string, Func<TSource, object>> GetMappings() => _mappings; // este metodo es el que se va a utilizar para obtener los mapeos entre los objetos, es decir, para obtener la configuracion que se le dio al Mapper para mapear un objeto de tipo TSource a un objeto de tipo TDestination, y el Mapper va a utilizar esa configuracion para mapear los objetos cuando se le pida que lo haga, por ejemplo, cuando se le pida que mapee un objeto de tipo TSource a un objeto de tipo TDestination, el Mapper va a buscar en su configuracion si existe un mapeo entre esos dos tipos de objetos, y si existe, va a utilizar ese mapeo para mapear el objeto de tipo TSource al objeto de tipo TDestination.



    public void Map<TProperty>(Expression<Func<TDestination, TProperty>> destinationMember, Expression<Func<TSource, TProperty>> sourceMember)
    {
        if (sourceMember == null)
        {
            throw new ArgumentNullException(nameof(sourceMember));
        }
        var memberName = GetMemberName(destinationMember);
        var compiledSource = sourceMember.Compile();
        _mappings[memberName] = source => compiledSource(source)!;
    }


    private string GetMemberName<TProperty>(Expression<Func<TDestination, TProperty>> expression)
    {
        if (expression.Body is MemberExpression memberExpression)
        {
            return memberExpression.Member.Name;
        }
        throw new ArgumentException("La expresión debe ser un miembro de la clase de destino.", nameof(expression));
    }
}
