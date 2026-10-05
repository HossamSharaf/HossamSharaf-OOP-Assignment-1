# Refactoring the Invoice Class: Overcoming the God Class Anti-Pattern

## Overview
This document explains the architectural flaws of designing an `Invoice` class as a single, flat entity with over 20 loosely related properties. It breaks down why a massive constructor is problematic in practice and explores the deeper design issues associated with this approach.

---

## 1. The Problem with a 20-Parameter Constructor
Having a single constructor that accepts 20 parameters introduces several critical issues in day-to-day development:

### A. Call-Site Readability
When instantiating the object, passing 20 consecutive arguments creates a block of code that is incredibly difficult to read. A developer looking at `new Invoice("INV01", "John", "john@email.com", ...)` will struggle to identify what each parameter represents without constantly checking the method signature.

### B. Positional Arguments Risk (Type Blindness)
When multiple parameters share the same data type (e.g., strings for names and addresses, or decimals for financial amounts), the compiler cannot protect you from logical errors. 
* **Example:** Accidentally swapping the `CustomerName` with the `ShippingStreet`, or the `DiscountAmount` with the `TaxAmount`. The compiler will accept this gracefully, leading to corrupted data and critical system bugs.

### C. Maintenance & Fragility
If business requirements change and a new property (e.g., `ApartmentNumber`) is added, or an existing property is made optional, you must modify the constructor signature. This breaks the code everywhere the constructor was called across the entire codebase, turning a simple update into a time-consuming refactoring task.

---

## 2. The Deeper Design Issue: The "God Class"
The lengthy constructor is merely a symptom of a much deeper architectural flaw. Placing 20 loosely related properties in a single class creates what is known as a **God Class**. 

### A. Violation of the Single Responsibility Principle (SRP)
A class should have only one reason to change. The flat `Invoice` class currently manages customer identity, shipping logistics, billing locations, and financial tax calculations simultaneously. It is doing too much.

### B. Low Cohesion
Cohesion refers to how closely related the data inside a class is. Properties like `ShippingZipCode` and `DiscountAmount` have absolutely no relationship with each other, yet they are forced to live in the same object. This makes the class unstructured and harder to test.

### C. High Coupling
Because this massive class is used everywhere in the application, the entire system becomes heavily dependent on it. A bug or modification in the billing logic could inadvertently affect the shipping logic.

---

## 3. The Solution: Composition
To resolve these issues, we apply **Composition**. Instead of a flat list of properties, we group cohesive data into smaller, reusable Value Objects:
* `Address` (handles Street, City, State, ZipCode, Country)
* `CustomerInfo` (handles Name, Email, Phone)
* `PaymentDetails` (handles SubTotal, Taxes, Discounts, and computes Total)

**Benefits of this approach:**
* **Reusable:** The `Address` class can now be used for `Employee` or `Supplier` entities.
* **Safe:** Constructors are smaller, preventing positional argument mistakes.
* **Maintainable:** The `Invoice` class is now clean, readable, and strictly focused on grouping these components together.

## ------------------------------------------------------------------------------------------

## Why Is the Composed Builder Better Than a Single Large Builder?

The composed Builder design is better than a single large Builder because it makes the code simpler, more organized, reusable, and easier to maintain.

### 1. Single Responsibility Principle (SRP)

Each Builder has its own responsibility:

* **AddressBuilder:** Responsible only for creating and validating addresses.
* **OrderBuilder:** Responsible only for creating and validating order and payment information.
* **InvoiceBuilder:** Responsible for combining the different components to create the final invoice.

This follows the Single Responsibility Principle because each class has a clear and specific purpose.

### 2. Independent Validation

The `AddressBuilder` validates its own required fields, such as street, city, state, zip code, and country. The parent `InvoiceBuilder` does not need to know the validation rules for each address field. It only needs to ensure that the required address is provided.

This makes validation more organized and keeps each class independent.

### 3. Reusability

The same `AddressBuilder` is reused for both billing and shipping addresses. Without this approach, we might need to duplicate the address-building and validation logic for both addresses.

Reusing the same Builder reduces code duplication and makes future changes easier.

### 4. Readability at the Call Site

With a single large Builder, all invoice properties and validation rules are handled in one class, which can become difficult to manage as the project grows.

With the composed Builder, the code is divided into clear groups: address information and order/payment information. This makes the invoice construction process easier to read and understand.

### Conclusion

The composed Builder design improves code organization, follows the Single Responsibility Principle, supports independent validation, encourages reuse, and makes the code easier to maintain and extend. Each Builder handles its own group of data, while the main `InvoiceBuilder` combines everything to create the final invoice.
