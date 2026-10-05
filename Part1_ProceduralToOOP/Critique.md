# Design Critique: Procedural Order Management System

## 1. Primary Flaw: Complete Lack of Encapsulation
The fundamental flaw in `order_system.cpp` is that data and the operations acting on it are completely decoupled.

* **Exposed Global State:** All application state (`customerNames`, `productStock`, `orderIsPaid`) lives in global arrays accessible to any function. There are no boundaries or access restrictions. Any line of code can bypass invariants—such as marking an empty order paid or resetting stock directly—leading to silent data corruption.
* **Parallel Arrays (Broken Data Cohesion):** Rather than modeling a real-world entity, a customer is fragmented across five independent arrays synchronized only by an implicit index `i`. If one array is modified without the others, data integrity immediately collapses.
* **Externalized Business Logic:** Entities do not protect their own invariants. Standalone functions perform calculations and mutate state remotely, making business logic difficult to track and prone to validation leaks.

## 2. Secondary Flaws
* **Index-Coupling:** Orders store ephemeral array indices instead of entity references or primary keys. Sorting, filtering, or deleting records will instantly assign orders to the wrong customers or products.
* **Fixed Buffers:** Static limits (`MAX_CUSTOMERS`, `MAX_ORDERS`) cause abrupt failure upon capacity and waste memory via flat multi-dimensional arrays (`lineProductIndexes[100][20]`).
* **Unsafe Stock Mutation:** Stock is deducted immediately upon line addition with no rollback mechanism if the order is never completed.

## 3. The C# / OOP Remedy
* **Encapsulated Classes:** Model `Customer`, `Product`, `Order`, and `OrderLine` as classes with private fields and controlled properties.
* **Self-Governing Invariants:** Methods like `AddLine()` and `MarkPaid()` belong inside the `Order` class to ensure business rules cannot be bypassed.
* **Object References & Dynamic Collections:** Replace parallel arrays and index offsets with `List<OrderLine>` and direct object references.