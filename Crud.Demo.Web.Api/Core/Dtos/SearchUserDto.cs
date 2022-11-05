namespace Core.Dtos
{
    public class SearchUserDto
    {       
        public int PageSize { get; set; }      
        public int StartIndex { get; set; }
        public string? FieldName { get; set; }
        public string? FieldValue { get; set; }
    }
}
