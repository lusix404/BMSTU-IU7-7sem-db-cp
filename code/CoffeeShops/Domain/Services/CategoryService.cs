using CoffeeShops.Domain.Models;
using CoffeeShops.Domain.Interfaces.Services;
using CoffeeShops.Domain.Exceptions.CategoryServiceExceptions;
using CoffeeShops.Domain.Interfaces.Repositories;
using CoffeeShops.DataAccess.Repositories;
using CoffeeShops.Domain.Exceptions.DrinkServiceExceptions;
using Npgsql;


namespace CoffeeShops.Domain.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Category?> GetCategoryByIdAsync(Guid category_id, int id_role) 
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(category_id, id_role);
            if (category == null)
            {
                throw new CategoryNotFoundException($"Category with id={category_id} was not found");
            } 
            return category; 

        }
        public async Task<List<Category>?> GetAllCategoriesAsync(int id_role)
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync(id_role);
            if (categories == null)
            {
                throw new CategoryNotFoundException($"There is no categories in data base");
            }

            return categories;

        }
        public async Task AddCategoryAsync(Category category, int id_role)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            if (string.IsNullOrWhiteSpace(category.Name))
                throw new CategoryIncorrectAtributeException("Имя категории не должно быть пустым");

            try
            {
                await _categoryRepository.AddCategoryAsync(category, id_role);
            }
            catch (CategoryUniqueException ex)
            {
                throw;
            }
            catch (PostgresException ex) when (ex.SqlState == "22001")
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
