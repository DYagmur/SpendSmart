using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SpendSmart.Models;

namespace SpendSmart.Services
{
    public class DataSynchronizer<TSourceContext, TTargetContext>
    where TSourceContext : DbContext
    where TTargetContext : DbContext
    {
        private readonly TSourceContext _sourceDbContext;
        private readonly TTargetContext _targetDbContext;

        public DataSynchronizer(TSourceContext sourceDbContext, TTargetContext targetDbContext)
        {
            _sourceDbContext = sourceDbContext;
            _targetDbContext = targetDbContext;
        }

        public async Task SynchronizeAsync()
        {
            var sourceExpenses = await _sourceDbContext.Set<Expense>().AsNoTracking().ToListAsync();
            foreach (var expense in sourceExpenses)
            {
                var existingExpense = await _targetDbContext.Set<Expense>().FindAsync(expense.Id);
                if (existingExpense == null)
                {
                    _targetDbContext.Set<Expense>().Add(expense);
                }
                else
                {
                    _targetDbContext.Entry(existingExpense).CurrentValues.SetValues(expense);
                }
            }

            await _targetDbContext.SaveChangesAsync();
        }
    }

}
