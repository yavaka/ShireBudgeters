using ShireBudgeters.Common;
using ShireBudgeters.DA.Models;

namespace ShireBudgeters.DA.Repositories.Category;

public interface ICategoryRepository : IRepository<CategoryModel, int>
{
    Task<IEnumerable<CategoryModel>> GetActiveByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CategoryModel>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CategoryModel>> GetChildCategoriesAsync(int parentCategoryId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CategoryModel>> GetRootCategoriesByUserIdAsync(string userId, CancellationToken cancellationToken = default);
}