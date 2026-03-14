namespace Pasteleria.Shared.Extensions
{
    public class PagedList<T> //: List<T>
    {
        public List<T> Items { get; internal set; }
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public bool Success { get; set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
        public List<string> Errors { get; set; }
        /// <summary>
        /// Sirve para crear un objeto de tipo PagedList. Util en tablas de administración
        /// </summary>
        public PagedList()
        {
            Success = false;
            Errors = new List<string>();
            Items = new List<T>();
        }
        /// <summary>
        /// Sirve para crear un objeto de tipo PagedList. Util en tablas de administración
        /// </summary>
        /// <param name="items">Corresponde al resultado esperado</param>
        /// <param name="count">Total de registros que fueron consultados</param>
        /// <param name="pageNumber">Número de página relativa a la colección total de datos</param>
        /// <param name="pageSize">Tamaño de página sobre el cual se va a paginar los datos consultados</param>
        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Errors = new List<string>();
            Items = items;
            Success = true;
        }
        /// <summary>
        /// Sirve para crear un objeto de tipo PagedList. Util en tablas de administración
        /// Devuelve todos los datos sin paginación
        /// Los valores PageSize y CurrentPage son por defecto 1
        /// Total count corresponde a todos los registros que se encuentren en base de datos
        /// </summary>
        /// <param name="items">Corresponde al resultado esperado</param>
        public PagedList(List<T>? items)
        {
            TotalCount = items == null ? 0 : items.Count;
            PageSize = 1;
            CurrentPage = 1;
            TotalPages = 1;
            Items = items ?? new List<T>();
            Errors = new List<string>();
            Success = true;
        }
    }
}
