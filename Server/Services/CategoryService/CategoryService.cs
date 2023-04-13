namespace MyEcommerce.Server.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext _context;

        public CategoryService(DataContext context)
        {
            _context = context;
        }
        public async Task<ServiceResponse<List<Category>>> GetCategories()
        {
           var response=new ServiceResponse<List<Category>>();  
           response.Data=await _context.Categories.ToListAsync();
            return response;
        }

        public Task<ServiceResponse<Category>> GetCategory(int categoryId)
        {
            throw new NotImplementedException();
        }
    }
}
