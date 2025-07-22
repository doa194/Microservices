using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaStateMachine.StateDbContext
{
    public class OrderStateDBContext : SagaDbContext
    {
        public OrderStateDBContext(DbContextOptions options) : base(options)
        {
        }

        protected override IEnumerable<ISagaClassMap> Configurations
        {
            // The OrderStateMap class is used to map the OrderStateInstance to the database table.
            get
            {
                yield return new StateMaps.OrderStateMap();
            }
        }

    }
}
