# Microservices Testing Strategy Guide
## Comprehensive Testing Approach for Service-Oriented Architecture

## Testing Pyramid for Microservices

### 1. Unit Tests (70% of tests)
**Scope:** Individual classes and methods within a service  
**Tools:** xUnit, NUnit, Moq  
**Focus:** Business logic, data transformations, validation

```csharp
[Test]
public void CalculatePrice_WithValidInput_ReturnsCorrectPrice()
{
    // Arrange
    var calculator = new PriceCalculator();
    
    // Act
    var result = calculator.Calculate(quantity: 5, unitPrice: 10.0m);
    
    // Assert
    Assert.AreEqual(50.0m, result);
}
```

### 2. Integration Tests (20% of tests) 
**Scope:** Service interactions with databases, external APIs  
**Tools:** TestContainers, In-Memory databases  
**Focus:** Data persistence, external service integration

```csharp
[Test]
public async Task CreateOrder_WithValidData_PersistsToDatabase()
{
    // Arrange
    using var db = new TestDatabase();
    var service = new OrderService(db.Context);
    
    // Act
    await service.CreateOrderAsync(validOrder);
    
    // Assert
    var saved = await db.Context.Orders.FindAsync(orderId);
    Assert.IsNotNull(saved);
}
```

### 3. Contract Tests (5% of tests)
**Scope:** API contracts between services  
**Tools:** Pact, Spring Cloud Contract  
**Focus:** API compatibility, breaking change detection

```csharp
[Test]
public void GetOrder_ReturnsExpectedContract()
{
    // Consumer test - verifies service provides expected response format
    var response = orderService.GetOrder(orderId);
    
    Assert.AreEqual("Order", response.Type);
    Assert.IsNotNull(response.Id);
    Assert.IsNotNull(response.CustomerId);
}
```

### 4. End-to-End Tests (5% of tests)
**Scope:** Complete business workflows across services  
**Tools:** Selenium, Playwright, API testing tools  
**Focus:** Critical business paths, user journeys

## Service-Specific Testing Patterns

### Testing Event-Driven Communication
```csharp
[Test]
public async Task ProcessOrder_PublishesOrderCreatedEvent()
{
    // Arrange
    var eventBus = new TestEventBus();
    var service = new OrderService(eventBus);
    
    // Act
    await service.ProcessOrderAsync(order);
    
    // Assert
    var publishedEvent = eventBus.GetPublishedEvent<OrderCreatedEvent>();
    Assert.IsNotNull(publishedEvent);
    Assert.AreEqual(order.Id, publishedEvent.OrderId);
}
```

### Testing Async Event Handlers
```csharp
[Test]
public async Task HandlePaymentProcessed_UpdatesOrderStatus()
{
    // Arrange
    var handler = new PaymentProcessedHandler(orderRepository);
    var paymentEvent = new PaymentProcessedEvent { OrderId = orderId };
    
    // Act
    await handler.HandleAsync(paymentEvent);
    
    // Assert
    var order = await orderRepository.GetAsync(orderId);
    Assert.AreEqual(OrderStatus.Paid, order.Status);
}
```

### Testing Service Resilience
```csharp
[Test]
public async Task CallExternalService_WithRetry_HandlesTransientFailures()
{
    // Arrange
    var mockClient = new Mock<IExternalServiceClient>();
    mockClient.SetupSequence(x => x.CallAsync())
           .ThrowsAsync(new HttpRequestException())
           .ReturnsAsync(successResponse);
    
    var service = new ResilientService(mockClient.Object);
    
    // Act & Assert
    var result = await service.ProcessAsync();
    Assert.IsNotNull(result);
    mockClient.Verify(x => x.CallAsync(), Times.Exactly(2));
}
```

## Test Data Management

### Database per Test Strategy
```csharp
public class DatabaseTest : IDisposable
{
    private readonly TestDatabase _database;
    
    public DatabaseTest()
    {
        _database = new TestDatabase();
        _database.SeedTestData();
    }
    
    public void Dispose()
    {
        _database.Cleanup();
    }
}
```

### Event Store Testing
```csharp
public class EventStoreTest
{
    [Test]
    public async Task ReplayEvents_ReconstructsAggregateState()
    {
        // Arrange
        var events = new[] {
            new OrderCreatedEvent(orderId, customerId),
            new OrderItemAddedEvent(orderId, itemId),
            new OrderSubmittedEvent(orderId)
        };
        
        // Act
        var order = Order.FromEvents(events);
        
        // Assert
        Assert.AreEqual(OrderStatus.Submitted, order.Status);
        Assert.AreEqual(1, order.Items.Count);
    }
}
```

## Performance Testing

### Load Testing Individual Services
```csharp
[Test]
public async Task GetOrders_UnderLoad_MaintainsPerformance()
{
    // Arrange
    var tasks = new List<Task<OrderResponse>>();
    
    // Act - Simulate 100 concurrent requests
    for (int i = 0; i < 100; i++)
    {
        tasks.Add(orderService.GetOrderAsync(orderIds[i % orderIds.Length]));
    }
    
    var responses = await Task.WhenAll(tasks);
    
    // Assert
    Assert.AreEqual(100, responses.Length);
    Assert.IsTrue(responses.All(r => r != null));
}
```

## Test Environment Management

### Docker Compose for Integration Tests
```yaml
version: '3.8'
services:
  test-db:
    image: postgres:13
    environment:
      POSTGRES_DB: testdb
      POSTGRES_USER: test
      POSTGRES_PASSWORD: test
    ports:
      - "5433:5432"
      
  test-redis:
    image: redis:6
    ports:
      - "6380:6379"
```

### TestContainers Integration
```csharp
public class IntegrationTestBase : IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer;
    
    public IntegrationTestBase()
    {
        _dbContainer = new PostgreSqlBuilder()
            .WithDatabase("testdb")
            .WithUsername("test")
            .WithPassword("test")
            .Build();
    }
    
    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }
    
    public async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }
}
```

## Testing Checklist for Each Service

### Service Implementation Tests
- [ ] All business logic unit tested
- [ ] Data access layer integration tested  
- [ ] External API integration tested with mocks
- [ ] Event publishing tested
- [ ] Event handling tested
- [ ] Error handling and retry logic tested
- [ ] Configuration and dependency injection tested

### Service Contract Tests
- [ ] API endpoints return expected response formats
- [ ] API endpoints handle invalid input correctly
- [ ] Published events match expected schema
- [ ] Consumer contract tests pass
- [ ] Backward compatibility verified

### Service Deployment Tests
- [ ] Health checks work correctly
- [ ] Service starts up successfully
- [ ] Database migrations apply correctly
- [ ] Configuration loads properly
- [ ] Logging works as expected
- [ ] Metrics collection functional

## Common Testing Anti-Patterns to Avoid

### ❌ Testing Implementation Details
```csharp
// BAD - Testing internal method calls
Mock.Verify(x => x.InternalMethod(), Times.Once);

// GOOD - Testing behavior
var result = service.ProcessOrder(order);
Assert.AreEqual(OrderStatus.Processing, result.Status);
```

### ❌ Overly Complex Test Setup
```csharp
// BAD - Complex, brittle setup
var mock1 = new Mock<IDep1>();
var mock2 = new Mock<IDep2>();
// ... 10 more mocks
mock1.Setup(x => x.Method()).Returns(complexObject);
// ... complex setup

// GOOD - Use test builders or factory methods
var service = TestServiceBuilder.Create()
    .WithValidDependencies()
    .Build();
```

### ❌ Testing Multiple Things in One Test
```csharp
// BAD - Tests multiple behaviors
[Test]
public void ProcessOrder_DoesEverything()
{
    var result = service.ProcessOrder(order);
    Assert.IsTrue(result.IsValid);
    Assert.AreEqual(1, result.Items.Count);
    Assert.IsTrue(eventWasPublished);
    Assert.IsTrue(emailWasSent);
}

// GOOD - Separate focused tests
[Test] public void ProcessOrder_ValidatesInput() { }
[Test] public void ProcessOrder_PublishesEvent() { }
[Test] public void ProcessOrder_SendsEmail() { }
```

---

*Remember: Good tests are your safety net during microservices refactoring. Invest in comprehensive testing to enable confident architectural changes.*