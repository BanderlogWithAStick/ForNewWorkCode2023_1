using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestWorkApp1.Interfaces;

namespace TestWorkApp1.Services.SeederRunnerService
{
    public class SeederRunnerService
    {
        private readonly ICustomSeeder[] _seeders;

        public SeederRunnerService(IServiceProvider serviceProvider)
        {
            _seeders = serviceProvider.GetServices<ICustomSeeder>().ToArray();
        }

        public async Task RunSeedersAsync()
        {
            List<ICustomSeeder> orderedSeeders = _seeders.OrderBy(s => s.SeederPriority).ToList();
            for (int i = 0; i < orderedSeeders.Count; i++)
                await orderedSeeders[i].Seed();
        }
    }
}
