namespace SuperERP.PDV.Application.Dtos;

public class PagedQueryDto
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string Sort { get; set; }
}
