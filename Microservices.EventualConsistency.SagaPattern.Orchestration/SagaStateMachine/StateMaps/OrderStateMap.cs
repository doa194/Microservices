using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SagaStateMachine.StateInstance;

namespace SagaStateMachine.StateMaps
{
    // This class maps the OrderStateInstance to the database table
    // It specifies the properties of the OrderStateInstance that will be stored in the database and their  configurations.

    public class OrderStateMap : SagaClassMap<OrderStateInstance>
    {
        // This method is used to configure the model for the OrderStateInstance
        // It is called by the Entity Framework Core when the model is being created.
        // It is where you can specify the properties and relationships of the OrderStateInstance.
        // For example, you can specify that the OrderId and CustomerId properties are required.
        // It also sets a default value for the TotalAmount property.
        protected override void Configure(EntityTypeBuilder<OrderStateInstance> entity, ModelBuilder model)
        {
            entity.Property(x => x.CustomerId).IsRequired();
            entity.Property(x => x.OrderId).IsRequired();
            entity.Property(x => x.TotalAmount).HasDefaultValue(0);
        }
    }
}
